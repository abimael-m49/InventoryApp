using System;
using System.Drawing;
using System.Windows.Forms;
using InventoryApp.Repositories;
using InventoryApp.Domain;

namespace InventoryApp.WinForms
{
    public partial class SalesViewForm : Form
    {
        private readonly ISaleRepository _saleRepo;
        private readonly IClientRepository _clientRepo;

        private DataGridView dgvVentas = null!;
        private DataGridView dgvDetalles = null!;
        private ComboBox cboCliente = null!;
        private DateTimePicker dtpFechaInicio = null!;
        private DateTimePicker dtpFechaFin = null!;
        private Button btnFiltrar = null!;
        private Button btnLimpiar = null!;
        private Label lblTituloVentas = null!;
        private Label lblTituloDetalles = null!;

        public SalesViewForm(ISaleRepository saleRepo, IClientRepository clientRepo)
        {
            _saleRepo = saleRepo;
            _clientRepo = clientRepo;

            InitializeComponents();
            this.Shown += async (s, e) => await LoadDataAsync();
        }

        private void InitializeComponents()
        {
            this.Text = "Visualizar Ventas";
            this.Size = new Size(1200, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            // Título principal
            var lblTituloPrincipal = new Label
            {
                Text = "Visualización de Ventas",
                Location = new Point(20, 20),
                Size = new Size(400, 35),
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 122, 204)
            };

            // Panel de filtros
            var grpFiltros = new GroupBox
            {
                Text = "Filtros",
                Location = new Point(20, 70),
                Size = new Size(1160, 100),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            var lblCliente = new Label
            {
                Text = "Cliente:",
                Location = new Point(20, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F)
            };

            cboCliente = new ComboBox
            {
                Location = new Point(20, 55),
                Size = new Size(250, 25),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F)
            };

            var lblFechaInicio = new Label
            {
                Text = "Fecha Inicio:",
                Location = new Point(300, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F)
            };

            dtpFechaInicio = new DateTimePicker
            {
                Location = new Point(300, 55),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 9F)
            };
            dtpFechaInicio.Value = DateTime.Now.AddMonths(-1);

            var lblFechaFin = new Label
            {
                Text = "Fecha Fin:",
                Location = new Point(520, 30),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F)
            };

            dtpFechaFin = new DateTimePicker
            {
                Location = new Point(520, 55),
                Size = new Size(200, 25),
                Format = DateTimePickerFormat.Short,
                Font = new Font("Segoe UI", 9F)
            };

            btnFiltrar = new Button
            {
                Text = "Filtrar",
                Location = new Point(750, 50),
                Size = new Size(100, 35),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnFiltrar.FlatAppearance.BorderSize = 0;
            btnFiltrar.Click += btnFiltrar_Click;

            btnLimpiar = new Button
            {
                Text = "Limpiar",
                Location = new Point(870, 50),
                Size = new Size(100, 35),
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btnLimpiar.Click += btnLimpiar_Click;

            grpFiltros.Controls.Add(lblCliente);
            grpFiltros.Controls.Add(cboCliente);
            grpFiltros.Controls.Add(lblFechaInicio);
            grpFiltros.Controls.Add(dtpFechaInicio);
            grpFiltros.Controls.Add(lblFechaFin);
            grpFiltros.Controls.Add(dtpFechaFin);
            grpFiltros.Controls.Add(btnFiltrar);
            grpFiltros.Controls.Add(btnLimpiar);

            // Título ventas
            lblTituloVentas = new Label
            {
                Text = "Ventas Registradas",
                Location = new Point(20, 190),
                Size = new Size(300, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };

            // Grid de ventas
            dgvVentas = new DataGridView
            {
                Location = new Point(20, 220),
                Size = new Size(1160, 200),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvVentas.SelectionChanged += dgvVentas_SelectionChanged;

            // Título detalles
            lblTituloDetalles = new Label
            {
                Text = "Detalle de la Venta Seleccionada",
                Location = new Point(20, 440),
                Size = new Size(400, 25),
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };

            // Grid de detalles
            dgvDetalles = new DataGridView
            {
                Location = new Point(20, 470),
                Size = new Size(1160, 180),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };

            // Agregar controles al formulario
            this.Controls.Add(lblTituloPrincipal);
            this.Controls.Add(grpFiltros);
            this.Controls.Add(lblTituloVentas);
            this.Controls.Add(dgvVentas);
            this.Controls.Add(lblTituloDetalles);
            this.Controls.Add(dgvDetalles);
        }

        private async System.Threading.Tasks.Task LoadDataAsync()
        {
            try
            {
                // Cargar clientes para el filtro
                var clientes = await _clientRepo.GetAllAsync();
                clientes.Insert(0, new Client { Id = 0, Nombre = "Todos los clientes" });
                cboCliente.DataSource = clientes;
                cboCliente.DisplayMember = "Nombre";
                cboCliente.ValueMember = "Id";

                // Cargar todas las ventas
                await CargarVentasAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async System.Threading.Tasks.Task CargarVentasAsync()
        {
            try
            {
                var ventas = await _saleRepo.GetAllAsync();
                dgvVentas.DataSource = null;
                dgvVentas.DataSource = ventas;

                if (dgvVentas.Columns.Count > 0)
                {
                    dgvVentas.Columns["Id"].HeaderText = "ID";
                    dgvVentas.Columns["Id"].Width = 50;
                    dgvVentas.Columns["ClienteId"].Visible = false;
                    dgvVentas.Columns["ClienteNombre"].HeaderText = "Cliente";
                    dgvVentas.Columns["ClienteNombre"].Width = 250;
                    dgvVentas.Columns["Fecha"].HeaderText = "Fecha";
                    dgvVentas.Columns["Fecha"].Width = 150;
                    dgvVentas.Columns["Fecha"].DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
                    dgvVentas.Columns["Total"].HeaderText = "Total (Q)";
                    dgvVentas.Columns["Total"].Width = 120;
                    dgvVentas.Columns["Total"].DefaultCellStyle.Format = "N2";

                    // Ocultar columna Detalles si existe
                    if (dgvVentas.Columns.Contains("Detalles"))
                    {
                        dgvVentas.Columns["Detalles"].Visible = false;
                    }
                }

                dgvDetalles.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar ventas: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void dgvVentas_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvVentas.SelectedRows.Count == 0) return;

            try
            {
                int saleId = Convert.ToInt32(dgvVentas.SelectedRows[0].Cells["Id"].Value);
                var detalles = await _saleRepo.GetSaleDetailsAsync(saleId);

                dgvDetalles.DataSource = null;
                dgvDetalles.DataSource = detalles;

                if (dgvDetalles.Columns.Count > 0)
                {
                    dgvDetalles.Columns["Id"].Visible = false;
                    dgvDetalles.Columns["VentaId"].Visible = false;
                    dgvDetalles.Columns["ProductoId"].Visible = false;
                    dgvDetalles.Columns["ProductoNombre"].HeaderText = "Producto";
                    dgvDetalles.Columns["ProductoNombre"].Width = 300;
                    dgvDetalles.Columns["Cantidad"].HeaderText = "Cantidad";
                    dgvDetalles.Columns["Cantidad"].Width = 100;
                    dgvDetalles.Columns["PrecioUnit"].HeaderText = "Precio Unit.";
                    dgvDetalles.Columns["PrecioUnit"].Width = 120;
                    dgvDetalles.Columns["PrecioUnit"].DefaultCellStyle.Format = "N2";
                    dgvDetalles.Columns["Subtotal"].HeaderText = "Subtotal";
                    dgvDetalles.Columns["Subtotal"].Width = 120;
                    dgvDetalles.Columns["Subtotal"].DefaultCellStyle.Format = "N2";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar detalles: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnFiltrar_Click(object? sender, EventArgs e)
        {
            try
            {
                int clienteId = cboCliente.SelectedValue != null ? (int)cboCliente.SelectedValue : 0;
                DateTime fechaInicio = dtpFechaInicio.Value.Date;
                DateTime fechaFin = dtpFechaFin.Value.Date.AddDays(1).AddSeconds(-1);

                System.Collections.Generic.List<Sale> ventas;

                if (clienteId > 0)
                {
                    ventas = await _saleRepo.GetByClientAsync(clienteId);
                }
                else
                {
                    ventas = await _saleRepo.GetByDateRangeAsync(fechaInicio, fechaFin);
                }

                dgvVentas.DataSource = null;
                dgvVentas.DataSource = ventas;
                dgvDetalles.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al filtrar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnLimpiar_Click(object? sender, EventArgs e)
        {
            cboCliente.SelectedIndex = 0;
            dtpFechaInicio.Value = DateTime.Now.AddMonths(-1);
            dtpFechaFin.Value = DateTime.Now;
            await CargarVentasAsync();
        }
    }
}