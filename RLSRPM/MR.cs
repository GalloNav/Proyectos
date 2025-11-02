using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Proyectos.RLSRPM
{
    public partial class MR : Form
    {
        readonly Random _rnd = new Random();

        public MR()
        {
            InitializeComponent();
            // UI inicial
            cmbMetodo.Items.AddRange(new[] { "Lineal simple", "Polinomial", "Lineal múltiple" });
            cmbMetodo.SelectedIndex = 0;
            nudFilas.Value = 10;
            nudGrado.Minimum = 1; nudGrado.Maximum = 10; nudGrado.Value = 2;
            nudVars.Minimum = 1; nudVars.Maximum = 10; nudVars.Value = 3;

            ConfigurarChart();
            ActualizarVisibilidad();
            // columnas iniciales
            CrearColumnasParaMetodo();
            ConfigurarEstiloFormulario();
        }

        // ===== PALETA (Tailwind-like) =====
        // Azul:   #3B82F6  | Verde: #10B981 | Violeta: #8B5CF6 | Ámbar: #EAB308
        // Grises: base #F0F4F8, headers #334155, rowheader #475569, texto #1F2937

        private void ConfigurarEstiloFormulario()
        {
            this.BackColor = Color.FromArgb(240, 244, 248); // base
            this.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);

            // NumericUpDown / Combo
            foreach (var ctl in new Control[] { nudFilas, nudGrado, nudVars })
                if (ctl != null) ctl.Font = new Font("Segoe UI", 10F);

            if (cmbMetodo != null) cmbMetodo.Font = new Font("Segoe UI", 10F);

            // Botones
            EstilizarBoton(btnGenerar, Color.FromArgb(59, 130, 246), Color.White);   // azul
            EstilizarBoton(btnAjustar, Color.FromArgb(16, 185, 129), Color.White);   // verde

            // DataGridView
            EstilizarGrid(dgvDatos);

            // Chart
            EstilizarChart(chartReg);

            
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

        private void EstilizarGrid(DataGridView grid)
        {
            if (grid == null) return;

            // Doblebúfer para evitar parpadeo
            var pi = typeof(DataGridView).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            pi?.SetValue(grid, true, null);

            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            grid.GridColor = Color.FromArgb(226, 232, 240);
            grid.EnableHeadersVisualStyles = false;
            grid.SelectionMode = DataGridViewSelectionMode.CellSelect;
            grid.EditMode = DataGridViewEditMode.EditOnKeystrokeOrF2;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Encabezados
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(51, 65, 85);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(5);
            grid.ColumnHeadersHeight = 35;

            grid.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(71, 85, 105);
            grid.RowHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.RowHeadersWidth = 60;

            // Celdas
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(31, 41, 55);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(59, 130, 246);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9.5F);
            grid.DefaultCellStyle.Padding = new Padding(3);
            grid.RowTemplate.Height = 30;

            // Alternadas
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
        }

        private void EstilizarChart(System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            if (chart == null) return;

            var area = chart.ChartAreas.Count > 0 ? chart.ChartAreas[0] : chart.ChartAreas.Add("ChartArea1");
            // Fondo blanco y rejilla gris claro
            area.BackColor = Color.White;
            area.AxisX.MajorGrid.Enabled = true;
            area.AxisY.MajorGrid.Enabled = true;
            area.AxisX.MajorGrid.LineColor = Color.FromArgb(226, 232, 240);
            area.AxisY.MajorGrid.LineColor = Color.FromArgb(226, 232, 240);
            area.AxisX.LabelStyle.ForeColor = Color.FromArgb(55, 65, 81);
            area.AxisY.LabelStyle.ForeColor = Color.FromArgb(55, 65, 81);

            // Series "Datos" y "Modelo" si no existen
            var sDatos = chart.Series.FindByName("Datos") ??
                         chart.Series.Add("Datos");
            sDatos.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            sDatos.MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Square;
            sDatos.MarkerSize = 8;
            sDatos.Color = Color.FromArgb(234, 88, 12); // naranja

            var sModelo = chart.Series.FindByName("Modelo") ??
                          chart.Series.Add("Modelo");
            sModelo.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            sModelo.BorderWidth = 3;
            sModelo.Color = Color.FromArgb(59, 130, 246); // azul
        }

        private void ConfigurarChart()
        {
            chartReg.Series.Clear();
            chartReg.ChartAreas.Clear();
            chartReg.ChartAreas.Add(new ChartArea("area"));

            var sDatos = new Series("Datos") { ChartType = SeriesChartType.Point, MarkerSize = 7 };
            var sModelo = new Series("Modelo") { ChartType = SeriesChartType.Line, BorderWidth = 2 };

            chartReg.Series.Add(sDatos);
            chartReg.Series.Add(sModelo);
        }

        private void ActualizarVisibilidad()
        {
            string m = cmbMetodo.SelectedItem?.ToString() ?? "";
            nudGrado.Visible = m == "Polinomial";
            nudVars.Visible = m == "Lineal múltiple";
        }

        private void CrearColumnasParaMetodo()
        {
            dgvDatos.ReadOnly = false;
            dgvDatos.AllowUserToAddRows = false;
            dgvDatos.AllowUserToDeleteRows = true;
            dgvDatos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDatos.Columns.Clear();
            dgvDatos.Rows.Clear();

            var metodo = cmbMetodo.SelectedItem?.ToString();

            if (metodo == "Lineal simple" || metodo == "Polinomial")
            {
                dgvDatos.Columns.Add("X", "X");
                dgvDatos.Columns.Add("Y", "Y");
            }
            else // Lineal múltiple
            {
                int p = (int)nudVars.Value;
                for (int j = 1; j <= p; j++)
                    dgvDatos.Columns.Add("X" + j, "X" + j);
                dgvDatos.Columns.Add("Y", "Y");
            }
        }

        private (double[] x, double[] y) LeerXY()
        {
            var xs = new List<double>();
            var ys = new List<double>();
            foreach (DataGridViewRow r in dgvDatos.Rows)
            {
                if (r.IsNewRow) continue;
                if (double.TryParse(Convert.ToString(r.Cells[0].Value), out double x) &&
                    double.TryParse(Convert.ToString(r.Cells[1].Value), out double y))
                { xs.Add(x); ys.Add(y); }
            }
            if (xs.Count < 2) throw new InvalidOperationException("Ingresa al menos 2 filas X,Y.");
            return (xs.ToArray(), ys.ToArray());
        }

        private (double[,] X, double[] y) LeerXyMultiple()
        {
            int p = dgvDatos.Columns.Count - 1;
            var X = new List<double[]>();
            var y = new List<double>();
            foreach (DataGridViewRow r in dgvDatos.Rows)
            {
                if (r.IsNewRow) continue;
                var fila = new double[p];
                bool ok = true;
                for (int j = 0; j < p; j++)
                    ok &= double.TryParse(Convert.ToString(r.Cells[j].Value), out fila[j]);
                ok &= double.TryParse(Convert.ToString(r.Cells[p].Value), out double yy);
                if (ok) { X.Add(fila); y.Add(yy); }
            }
            if (X.Count < p + 1) throw new InvalidOperationException("Datos insuficientes en múltiple.");
            var Xm = new double[X.Count, p];
            for (int i = 0; i < X.Count; i++)
                for (int j = 0; j < p; j++) Xm[i, j] = X[i][j];
            return (Xm, y.ToArray());
        }

        private void PintarPuntos(double[] x, double[] y)
        {
            var sDatos = chartReg.Series["Datos"];
            sDatos.Points.Clear();
            for (int i = 0; i < x.Length; i++)
                sDatos.Points.AddXY(x[i], y[i]);
        }

        private void PintarModelo(Func<double, double> f, double xmin, double xmax, int pts = 200)
        {
            var sModelo = chartReg.Series["Modelo"];
            sModelo.Points.Clear();
            if (xmin == xmax) { xmin -= 1; xmax += 1; }
            double h = (xmax - xmin) / (pts - 1);
            for (int i = 0; i < pts; i++)
            {
                double xx = xmin + i * h;
                sModelo.Points.AddXY(xx, f(xx));
            }
        }

        private void AjustarLinealSimple()
        {
            var (x, y) = LeerXY();
            var (a, b, R2, RMSE) = RegressionService.FitLinealSimple(x, y);

            PintarPuntos(x, y);
            PintarModelo(xx => RegressionService.EvalLineal(a, b, xx), x.Min(), x.Max());

            // Si tienes cajas de texto para métricas, así:
            // txtR2.Text = R2.ToString("G6"); txtRMSE.Text = RMSE.ToString("G6");
        }

        private void AjustarPolinomial()
        {
            var (x, y) = LeerXY();
            int grado = (int)nudGrado.Value;
            var (coef, R2, RMSE) = RegressionService.FitPolinomial(x, y, grado);

            PintarPuntos(x, y);
            PintarModelo(xx => RegressionService.EvalPoli(coef, xx), x.Min(), x.Max());
        }

        private void AjustarLinealMultiple()
        {
            var (X, y) = LeerXyMultiple();
            var (beta, R2, RMSE) = RegressionService.FitLinealMultiple(X, y);

            // En múltiple no se puede graficar el modelo sobre el plano X–Y (hay >1 X).
            // Para dar claridad similar a la imagen, graficamos Y real vs índice y Y ajustada vs índice.
            var sDatos = chartReg.Series["Datos"]; sDatos.Points.Clear(); sDatos.ChartType = SeriesChartType.Point;
            var sModelo = chartReg.Series["Modelo"]; sModelo.Points.Clear(); sModelo.ChartType = SeriesChartType.Line;

            // ŷ = beta0 + sum beta_j * x_ij
            int n = X.GetLength(0), p = X.GetLength(1);
            for (int i = 0; i < n; i++)
            {
                double yhat = beta[0];
                for (int j = 0; j < p; j++) yhat += beta[j + 1] * X[i, j];
                sDatos.Points.AddXY(i + 1, y[i]);     // puntos reales
                sModelo.Points.AddXY(i + 1, yhat);    // línea del modelo
            }
            // txtR2.Text = R2.ToString("G6"); txtRMSE.Text = RMSE.ToString("G6");
        }


        private void btnGenerar_Click(object sender, EventArgs e)
        {
            CrearColumnasParaMetodo();

            int n = (int)nudFilas.Value;
            dgvDatos.Rows.Add(n);

            string metodo = cmbMetodo.SelectedItem.ToString();
            if (metodo == "Lineal simple" || metodo == "Polinomial")
            {
                // X ~ [1..10], Y ~ recta con ruido leve (editable)
                double a = 0.5 + _rnd.NextDouble();     // intercepto aprox
                double b = 0.5 + 1.5 * _rnd.NextDouble(); // pendiente aprox
                for (int i = 0; i < n; i++)
                {
                    double x = 1 + 9 * _rnd.NextDouble();
                    double ruido = _rnd.NextDouble() * 0.8 - 0.4;
                    double y = a + b * x + ruido;
                    dgvDatos.Rows[i].Cells[0].Value = Math.Round(x, 3);
                    dgvDatos.Rows[i].Cells[1].Value = Math.Round(y, 3);
                }
            }
            else // Lineal múltiple
            {
                int p = dgvDatos.Columns.Count - 1; // últimas es Y
                                                    // Beta reales "ocultas" para generar: beta0=1, beta_j ~ [-2,2]
                var beta = new double[p + 1];
                beta[0] = 1;
                for (int j = 1; j <= p; j++) beta[j] = -2 + 4 * _rnd.NextDouble();

                for (int i = 0; i < n; i++)
                {
                    var xs = new double[p];
                    for (int j = 0; j < p; j++)
                    {
                        xs[j] = Math.Round(-3 + 6 * _rnd.NextDouble(), 3);
                        dgvDatos.Rows[i].Cells[j].Value = xs[j];
                    }
                    double ruido = _rnd.NextDouble() * 0.8 - 0.4;
                    double y = beta[0] + xs.Select((v, j) => v * beta[j + 1]).Sum() + ruido;
                    dgvDatos.Rows[i].Cells[p].Value = Math.Round(y, 3);
                }
            }
        }

        private void btnAjustar_Click(object sender, EventArgs e)
        {
            try
            {
                string metodo = cmbMetodo.SelectedItem.ToString();
                if (metodo == "Lineal simple")
                    AjustarLinealSimple();
                else if (metodo == "Polinomial")
                    AjustarPolinomial();
                else
                    AjustarLinealMultiple();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbMetodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarVisibilidad();
            CrearColumnasParaMetodo();
        }
    }
}
