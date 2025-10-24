using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InventoryApp.Domain;
using MySql.Data.MySqlClient;

namespace InventoryApp.Repositories
{
    public interface ISaleRepository
    {
        // Métodos para transacciones (ya los tienes)
        Task<int> InsertSaleAsync(MySqlConnection con, MySqlTransaction tx, Sale sale);
        Task InsertSaleDetailAsync(MySqlConnection con, MySqlTransaction tx, SaleDetail detail);

        // Métodos para visualización (agregar estos)
        Task<List<Sale>> GetAllAsync();
        Task<List<SaleDetail>> GetSaleDetailsAsync(int saleId);
        Task<List<Sale>> GetByClientAsync(int clientId);
        Task<List<Sale>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}