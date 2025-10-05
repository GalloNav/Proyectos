using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyectos.GJ
{
    public partial class GJ : Form
    {
        public GJ()
        {
            InitializeComponent();
            ConfigurarEstiloFormulario();

            nudEcuaciones.Minimum = 1;
            nudEcuaciones.Maximum = 6;
            nudEcuaciones.Value = 3;

            nudVariables.Minimum = 1;
            nudVariables.Maximum = 6;
            nudVariables.Value = 3;
        }

        private void ConfigurarEstiloFormulario()
        {
            // Estilo general del formulario
            this.BackColor = Color.FromArgb(240, 244, 248);
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // Estilo para el NumericUpDown
            nudEcuaciones.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            nudEcuaciones.BackColor = Color.White;

            // Estilo para los botones
            EstilizarBoton(btnCrear, Color.FromArgb(59, 130, 246), Color.White);
            EstilizarBoton(btnGauss, Color.FromArgb(16, 185, 129), Color.White);
            EstilizarBoton(btnGaussJordan, Color.FromArgb(139, 92, 246), Color.White);

            // Estilo para el checkbox
            chkPivot.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
            chkPivot.ForeColor = Color.FromArgb(55, 65, 81);

            // Estilo para el DataGridView
            if (grid != null)
            {
                grid.BackgroundColor = Color.White;
                grid.BorderStyle = BorderStyle.None;
                grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
                grid.GridColor = Color.FromArgb(226, 232, 240);
                grid.EnableHeadersVisualStyles = false;

                // Encabezados
                grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 65, 85);
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
                grid.ColumnHeadersHeight = 35;

                grid.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(71, 85, 105);
                grid.RowHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

                // Celdas
                grid.DefaultCellStyle.BackColor = Color.White;
                grid.DefaultCellStyle.ForeColor = Color.FromArgb(31, 41, 55);
                grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(59, 130, 246);
                grid.DefaultCellStyle.SelectionForeColor = Color.White;
                grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
                grid.DefaultCellStyle.Padding = new Padding(3);
                grid.RowTemplate.Height = 30;

                // Celdas alternadas
                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            }

            // Estilo para el TextBox de solución
            if (txtSol != null)
            {
                txtSol.BackColor = Color.White;
                txtSol.ForeColor = Color.FromArgb(31, 41, 55);
                txtSol.Font = new Font("Consolas", 9.5F, FontStyle.Regular);
                txtSol.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        private double[,] LeerMatrizAumentada(out int ecuaciones, out int variables)
        {
            ecuaciones = grid.RowCount;
            variables = grid.ColumnCount - 1; // última columna es b

            var Ab = new double[ecuaciones, variables + 1];
            for (int i = 0; i < ecuaciones; i++)
                for (int j = 0; j < variables + 1; j++)
                {
                    string val = grid.Rows[i].Cells[j].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(val))
                        throw new FormatException($"Celda vacía en ({i + 1},{j + 1}).");
                    if (!double.TryParse(val, NumberStyles.Float, CultureInfo.InvariantCulture, out double v))
                        throw new FormatException($"Valor inválido en ({i + 1},{j + 1}).");
                    Ab[i, j] = v;
                }
            return Ab;
        }


        private void EstilizarBoton(Button btn, Color backColor, Color foreColor)
        {
            if (btn == null) return;

            btn.BackColor = backColor;
            btn.ForeColor = foreColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseOverBackColor = AjustarBrillo(backColor, -20);
            btn.FlatAppearance.MouseDownBackColor = AjustarBrillo(backColor, -40);
            btn.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(10, 5, 10, 5);
        }

        private Color AjustarBrillo(Color color, int amount)
        {
            int r = Math.Max(0, Math.Min(255, color.R + amount));
            int g = Math.Max(0, Math.Min(255, color.G + amount));
            int b = Math.Max(0, Math.Min(255, color.B + amount));
            return Color.FromArgb(color.A, r, g, b);
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            int ecuaciones = (int)nudEcuaciones.Value;
            int variables = (int)nudVariables.Value;

            grid.Enabled = true;
            grid.ReadOnly = false;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            grid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            grid.Columns.Clear();
            grid.Rows.Clear();

            // columnas a1...aN + b
            for (int j = 0; j < variables; j++)
                grid.Columns.Add($"c{j}", $"a{j + 1}");
            grid.Columns.Add($"c{variables}", "b");

            grid.Rows.Add(ecuaciones);
            for (int i = 0; i < ecuaciones; i++)
                grid.Rows[i].HeaderCell.Value = $"Eq {i + 1}";

            grid.RowHeadersWidth = 60;
            grid.CurrentCell = grid[0, 0];
            grid.BeginEdit(true);
            txtSol.Clear();
        }

        private void btnGauss_Click(object sender, EventArgs e)
        {
            txtSol.Clear();
            try
            {
                var Ab = LeerMatrizAumentada(out int ecuaciones, out int variables);
                if (ecuaciones != variables)
                {
                    MostrarError("Gauss requiere matriz cuadrada. Usa Gauss-Jordan para m×n.");
                    return;
                }

                var (x, log) = LinearAlgebra.GaussWithSteps(Ab, chkPivot.Checked);
                txtSol.ForeColor = Color.FromArgb(31, 41, 55);
                txtSol.Text = log; // ← muestra TODO el procedimiento
            }
            catch (Exception ex) { MostrarError(ex.Message); }
        }

        private void MostrarMensaje(string mensaje, string titulo, MessageBoxIcon icono)
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, icono);
        }
        //MostrarError
        private void MostrarError(string mensaje)
        {
            txtSol.ForeColor = Color.FromArgb(220, 38, 38);
            txtSol.Text = $"╔══════════════════════════════════════╗\n" +
                         $"║   ERROR                              ║\n" +
                         $"╚══════════════════════════════════════╝\n\n" +
                         $"✗ {mensaje}";

            // Restaurar color después de 3 segundos
            System.Threading.Timer timer = null;
            timer = new System.Threading.Timer((obj) =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => txtSol.ForeColor = Color.FromArgb(31, 41, 55)));
                }
                else
                {
                    txtSol.ForeColor = Color.FromArgb(31, 41, 55);
                }
                timer?.Dispose();
            }, null, 3000, System.Threading.Timeout.Infinite);
        }

        private void btnGaussJordan_Click(object sender, EventArgs e)
        {
            txtSol.Clear();
            try
            {
                var Ab = LeerMatrizAumentada(out int ecuaciones, out int variables);
                if (ecuaciones != variables)
                {
                    MostrarError("Esta versión de Gauss-Jordan asume matriz cuadrada. (Puedo darte RREF general con pasos).");
                    return;
                }

                var (x, R, log) = LinearAlgebra.GaussJordanWithSteps(Ab, chkPivot.Checked);
                txtSol.ForeColor = Color.FromArgb(31, 41, 55);
                txtSol.Text = log; // ← procedimiento completo
            }
            catch (Exception ex) { MostrarError(ex.Message); }
        }
    }
}
