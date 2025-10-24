using InventoryApp.Domain;
using InventoryApp.Infrastructure;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace InventoryApp.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        // Métodos para transacciones (insertar ventas)
        public async Task<int> InsertSaleAsync(MySqlConnection con, MySqlTransaction tx, Sale sale)
        {
            using var cmd = new MySqlCommand(
                "INSERT INTO venta (cliente_id, fecha, total) VALUES (@c, @f, @t); SELECT LAST_INSERT_ID();",
                con, tx);
            cmd.Parameters.AddWithValue("@c", sale.ClienteId);
            cmd.Parameters.AddWithValue("@f", sale.Fecha);
            cmd.Parameters.AddWithValue("@t", sale.Total);
            return Convert.ToInt32(await cmd.ExecuteScalarAsync());
        }

        public async Task InsertSaleDetailAsync(MySqlConnection con, MySqlTransaction tx, SaleDetail d)
        {
            using var cmd = new MySqlCommand(
                @"INSERT INTO detalle_venta (venta_id, producto_id, cantidad, precio_unit, subtotal)
                  VALUES (@v, @p, @c, @pu, @s)", con, tx);
            cmd.Parameters.AddWithValue("@v", d.VentaId);
            cmd.Parameters.AddWithValue("@p", d.ProductoId);
            cmd.Parameters.AddWithValue("@c", d.Cantidad);
            cmd.Parameters.AddWithValue("@pu", d.PrecioUnit);
            cmd.Parameters.AddWithValue("@s", d.Subtotal);
            await cmd.ExecuteNonQueryAsync();
        }

        // Métodos para visualización
        public async Task<List<Sale>> GetAllAsync()
        {
            var sales = new List<Sale>();

            try
            {
                using (var conn = DbConnectionFactory.Instance.CreateOpen())
                {
                    string query = @"SELECT v.id, v.cliente_id, c.nombre as ClienteNombre, v.fecha, v.total 
                                    FROM venta v 
                                    INNER JOIN cliente c ON v.cliente_id = c.id 
                                    ORDER BY v.fecha DESC";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                sales.Add(new Sale
                                {
                                    Id = reader.GetInt32("id"),
                                    ClienteId = reader.GetInt32("cliente_id"),
                                    ClienteNombre = reader.GetString("ClienteNombre"),
                                    Fecha = reader.GetDateTime("fecha"),
                                    Total = reader.GetDecimal("total")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener ventas: " + ex.Message);
            }

            return sales;
        }

        public async Task<List<SaleDetail>> GetSaleDetailsAsync(int saleId)
        {
            var details = new List<SaleDetail>();

            try
            {
                using (var conn = DbConnectionFactory.Instance.CreateOpen())
                {
                    string query = @"SELECT dv.id, dv.venta_id, dv.producto_id, p.nombre as ProductoNombre, 
                                    dv.cantidad, dv.precio_unit, dv.subtotal 
                                    FROM detalle_venta dv 
                                    INNER JOIN producto p ON dv.producto_id = p.id 
                                    WHERE dv.venta_id = @saleId";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@saleId", saleId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                details.Add(new SaleDetail
                                {
                                    Id = reader.GetInt32("id"),
                                    VentaId = reader.GetInt32("venta_id"),
                                    ProductoId = reader.GetInt32("producto_id"),
                                    ProductoNombre = reader.GetString("ProductoNombre"),
                                    Cantidad = reader.GetInt32("cantidad"),
                                    PrecioUnit = reader.GetDecimal("precio_unit"),
                                    Subtotal = reader.GetDecimal("subtotal")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener detalles de venta: " + ex.Message);
            }

            return details;
        }

        public async Task<List<Sale>> GetByClientAsync(int clientId)
        {
            var sales = new List<Sale>();

            try
            {
                using (var conn = DbConnectionFactory.Instance.CreateOpen())
                {
                    string query = @"SELECT v.id, v.cliente_id, c.nombre as ClienteNombre, v.fecha, v.total 
                                    FROM venta v 
                                    INNER JOIN cliente c ON v.cliente_id = c.id 
                                    WHERE v.cliente_id = @clientId 
                                    ORDER BY v.fecha DESC";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@clientId", clientId);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                sales.Add(new Sale
                                {
                                    Id = reader.GetInt32("id"),
                                    ClienteId = reader.GetInt32("cliente_id"),
                                    ClienteNombre = reader.GetString("ClienteNombre"),
                                    Fecha = reader.GetDateTime("fecha"),
                                    Total = reader.GetDecimal("total")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar ventas por cliente: " + ex.Message);
            }

            return sales;
        }

        public async Task<List<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var sales = new List<Sale>();

            try
            {
                using (var conn = DbConnectionFactory.Instance.CreateOpen())
                {
                    string query = @"SELECT v.id, v.cliente_id, c.nombre as ClienteNombre, v.fecha, v.total 
                                    FROM venta v 
                                    INNER JOIN cliente c ON v.cliente_id = c.id 
                                    WHERE v.fecha BETWEEN @startDate AND @endDate 
                                    ORDER BY v.fecha DESC";

                    using (var cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@startDate", startDate);
                        cmd.Parameters.AddWithValue("@endDate", endDate);

                        using (var reader = await cmd.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                sales.Add(new Sale
                                {
                                    Id = reader.GetInt32("id"),
                                    ClienteId = reader.GetInt32("cliente_id"),
                                    ClienteNombre = reader.GetString("ClienteNombre"),
                                    Fecha = reader.GetDateTime("fecha"),
                                    Total = reader.GetDecimal("total")
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al filtrar ventas por fechas: " + ex.Message);
            }

            return sales;
        }
    }
}