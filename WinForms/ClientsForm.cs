using System;
using System.Drawing;
using System.Windows.Forms;
using InventoryApp.Repositories;
using InventoryApp.Domain;

namespace InventoryApp.WinForms
{
    public class ClientsForm : Form
    {
        private readonly IClientRepository _clientRepo;
        private DataGridView dgvClients = null!;
        private GroupBox grpForm = null!;
        private TextBox txtNombre = null!;
        private TextBox txtNit = null!;
        private Label lblNombre = null!;
        private Label lblNit = null!;
        private Button btnSave = null!;
        private Button btnNew = null!;
        private Button btnDelete = null!;
        private int _selectedId = 0;

        public ClientsForm(IClientRepository clientRepo)
        {
            _clientRepo = clientRepo;
            InitializeComponents();
            this.Shown += (s, e) => LoadClients();
        }

        private void InitializeComponents()
        {
            this.Text = "Gestión de Clientes";
            this.Size = new Size(1000, 600);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.White;

            dgvClients = new DataGridView
            {
                Location = new Point(400, 20),
                Size = new Size(580, 520),
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.Fixed3D
            };
            dgvClients.CellDoubleClick += dgvClients_CellDoubleClick;

            grpForm = new GroupBox
            {
                Text = "Datos del Cliente",
                Location = new Point(20, 20),
                Size = new Size(360, 280),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };

            lblNombre = new Label
            {
                Text = "Nombre Completo:",
                Location = new Point(20, 40),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F)
            };

            txtNombre = new TextBox
            {
                Location = new Point(20, 65),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 10F)
            };

            lblNit = new Label
            {
                Text = "NIT:",
                Location = new Point(20, 110),
                AutoSize = true,
                Font = new Font("Segoe UI", 9F)
            };

            txtNit = new TextBox
            {
                Location = new Point(20, 135),
                Size = new Size(320, 25),
                Font = new Font("Segoe UI", 10F)
            };

            btnSave = new Button
            {
                Text = "Guardar",
                Location = new Point(20, 220),
                Size = new Size(150, 40),
                BackColor = Color.FromArgb(0, 122, 204),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += btnSave_Click;

            btnNew = new Button
            {
                Text = "Nuevo",
                Location = new Point(190, 220),
                Size = new Size(150, 40),
                Font = new Font("Segoe UI", 10F),
                Cursor = Cursors.Hand
            };
            btnNew.Click += btnNew_Click;

            btnDelete = new Button
            {
                Text = "Eliminar Seleccionado",
                Location = new Point(20, 320),
                Size = new Size(360, 40),
                BackColor = Color.FromArgb(192, 0, 0),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Click += btnDelete_Click;

            grpForm.Controls.Add(lblNombre);
            grpForm.Controls.Add(txtNombre);
            grpForm.Controls.Add(lblNit);
            grpForm.Controls.Add(txtNit);
            grpForm.Controls.Add(btnSave);
            grpForm.Controls.Add(btnNew);

            this.Controls.Add(grpForm);
            this.Controls.Add(dgvClients);
            this.Controls.Add(btnDelete);
        }

        private async void LoadClients()
        {
            try
            {
                var clients = await _clientRepo.GetAllAsync();
                dgvClients.DataSource = null;
                dgvClients.DataSource = clients;

                if (dgvClients.Columns.Count > 0)
                {
                    dgvClients.Columns["Id"].HeaderText = "ID";
                    dgvClients.Columns["Id"].Width = 50;
                    dgvClients.Columns["Nombre"].HeaderText = "Nombre Completo";
                    dgvClients.Columns["Nombre"].Width = 300;
                    dgvClients.Columns["Nit"].HeaderText = "NIT";
                    dgvClients.Columns["Nit"].Width = 150;
                }

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar clientes: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearForm()
        {
            _selectedId = 0;
            txtNombre.Clear();
            txtNit.Clear();
            txtNombre.Focus();
        }

        private async void btnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtNit.Text))
            {
                MessageBox.Show("El NIT es obligatorio", "Validación",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNit.Focus();
                return;
            }

            try
            {
                var client = new Client
                {
                    Id = _selectedId,
                    Nombre = txtNombre.Text.Trim(),
                    Nit = txtNit.Text.Trim()
                };

                if (_selectedId == 0)
                {
                    await _clientRepo.InsertAsync(client);
                    MessageBox.Show("Cliente creado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    await _clientRepo.UpdateAsync(client);
                    MessageBox.Show("Cliente actualizado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                LoadClients();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnNew_Click(object? sender, EventArgs e)
        {
            ClearForm();
        }

        private void dgvClients_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvClients.Rows.Count)
            {
                var row = dgvClients.Rows[e.RowIndex];
                if (row.Cells["Id"].Value != null)
                {
                    _selectedId = Convert.ToInt32(row.Cells["Id"].Value);
                    txtNombre.Text = row.Cells["Nombre"].Value?.ToString() ?? "";
                    txtNit.Text = row.Cells["Nit"].Value?.ToString() ?? "";
                }
            }
        }

        private async void btnDelete_Click(object? sender, EventArgs e)
        {
            if (dgvClients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione un cliente para eliminar", "Información",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show(
                "¿Está seguro de eliminar este cliente?\n\nNota: No se puede eliminar un cliente con ventas asociadas.",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    int clientId = Convert.ToInt32(dgvClients.SelectedRows[0].Cells["Id"].Value);
                    await _clientRepo.DeleteAsync(clientId);
                    MessageBox.Show("Cliente eliminado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadClients();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        $"Error al eliminar: {ex.Message}\n\nPosiblemente el cliente tiene ventas asociadas.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
    }
}