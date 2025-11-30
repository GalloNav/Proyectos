using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyectos.Lagrange
{
    public partial class Lagrange : Form
    {
        private Random rnd = new Random();

        public Lagrange()
        {
            InitializeComponent();

            // ============ ESTILO DEL FORMULARIO ============
            EstilizarFormulario();

            dgvPuntos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            if (dgvPuntos.ColumnCount == 0)
            {
                dgvPuntos.ColumnCount = 2;
            }
            dgvPuntos.Columns[0].HeaderText = "x";
            dgvPuntos.Columns[1].HeaderText = "y";

            // Evento del NumericUpDown
            nudGrado.Minimum = 1;
            nudGrado.Maximum = 10;
            nudGrado.Value = 1;
            nudGrado.ValueChanged += NudGrado_ValueChanged;

            // Eventos de botones
            BtnAleatorio.Click += BtnAleatorio_Click;
            btnCalcular.Click += BtnCalcular_Click;

            // Inicializar tabla según el grado actual
            AjustarTabla();
            LlenarAleatorio();
        }

        // ============ MÉTODO DE ESTILIZACIÓN ============
        private void EstilizarFormulario()
        {
            // Colores del tema
            Color colorFondo = ColorTranslator.FromHtml("#1a1a2e");
            Color colorSecundario = ColorTranslator.FromHtml("#16213e");
            Color colorAccento = ColorTranslator.FromHtml("#0f4c75");
            Color colorDestaque = ColorTranslator.FromHtml("#3282b8");
            Color colorTexto = ColorTranslator.FromHtml("#bbe1fa");
            Color colorTextoClaro = Color.White;

            // Estilo del formulario principal
            this.BackColor = colorFondo;
            this.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            this.ForeColor = colorTextoClaro;

            // Estilizar todos los Labels
            foreach (Control ctrl in this.Controls)
            {
                EstilizarControlRecursivo(ctrl, colorFondo, colorSecundario, colorAccento,
                                         colorDestaque, colorTexto, colorTextoClaro);
            }

            // Estilo específico del DataGridView
            EstilizarDataGridView(dgvPuntos, colorSecundario, colorAccento, colorDestaque, colorTextoClaro);

            // Estilo del NumericUpDown
            EstilizarNumericUpDown(nudGrado, colorSecundario, colorTextoClaro);

            // Estilo de TextBox
            EstilizarTextBox(txtX, colorSecundario, colorTextoClaro);

            // Estilo de botones
            EstilizarBoton(BtnAleatorio, colorDestaque, colorTextoClaro);
            EstilizarBoton(btnCalcular, colorAccento, colorTextoClaro);

            // Estilo especial para el label de resultado
            if (lblResultado != null)
            {
                lblResultado.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
                lblResultado.ForeColor = colorDestaque;
                lblResultado.BackColor = colorSecundario;
                lblResultado.BorderStyle = BorderStyle.FixedSingle;
                lblResultado.TextAlign = ContentAlignment.MiddleCenter;
                lblResultado.Padding = new Padding(10);
            }
        }

        private void EstilizarControlRecursivo(Control ctrl, Color colorFondo, Color colorSecundario,
                                               Color colorAccento, Color colorDestaque,
                                               Color colorTexto, Color colorTextoClaro)
        {
            if (ctrl is Label lbl)
            {
                lbl.ForeColor = colorTextoClaro;
                lbl.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            }
            else if (ctrl is Panel panel)
            {
                panel.BackColor = colorSecundario;
                panel.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (ctrl is GroupBox gb)
            {
                gb.ForeColor = colorTextoClaro;
                gb.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            }

            // Recursión para controles contenedores
            foreach (Control child in ctrl.Controls)
            {
                EstilizarControlRecursivo(child, colorFondo, colorSecundario, colorAccento,
                                         colorDestaque, colorTexto, colorTextoClaro);
            }
        }

        private void EstilizarDataGridView(DataGridView dgv, Color colorFondo, Color colorAccento,
                                          Color colorDestaque, Color colorTexto)
        {
            dgv.BackgroundColor = colorFondo;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = colorAccento;

            // Estilo de las celdas
            dgv.DefaultCellStyle.BackColor = colorFondo;
            dgv.DefaultCellStyle.ForeColor = colorTexto;
            dgv.DefaultCellStyle.SelectionBackColor = colorDestaque;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgv.DefaultCellStyle.Padding = new Padding(5);

            // Estilo de las cabeceras
            dgv.ColumnHeadersDefaultCellStyle.BackColor = colorAccento;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 40;
            dgv.EnableHeadersVisualStyles = false;

            // Estilo de las filas
            dgv.RowTemplate.Height = 35;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#0e1621");

            // Sin bordes externos
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        }

        private void EstilizarNumericUpDown(NumericUpDown nud, Color colorFondo, Color colorTexto)
        {
            nud.BackColor = colorFondo;
            nud.ForeColor = colorTexto;
            nud.Font = new Font("Segoe UI", 11F);
            nud.BorderStyle = BorderStyle.FixedSingle;
        }

        private void EstilizarTextBox(TextBox txt, Color colorFondo, Color colorTexto)
        {
            txt.BackColor = colorFondo;
            txt.ForeColor = colorTexto;
            txt.Font = new Font("Segoe UI", 11F);
            txt.BorderStyle = BorderStyle.FixedSingle;
        }

        private void EstilizarBoton(Button btn, Color colorFondo, Color colorTexto)
        {
            btn.BackColor = colorFondo;
            btn.ForeColor = colorTexto;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(10, 5, 10, 5);

            // Efectos hover
            btn.MouseEnter += (s, e) => {
                btn.BackColor = ControlPaint.Light(colorFondo, 0.2f);
            };
            btn.MouseLeave += (s, e) => {
                btn.BackColor = colorFondo;
            };
        }

        // Cuando cambia el grado n, la tabla debe tener n+1 filas
        private void NudGrado_ValueChanged(object sender, EventArgs e)
        {
            AjustarTabla();
            LlenarAleatorio();
        }

        private void AjustarTabla()
        {
            int n = (int)nudGrado.Value;   // grado
            int numPuntos = n + 1;         // n+1 puntos

            dgvPuntos.Rows.Clear();
            dgvPuntos.Rows.Add(numPuntos);
        }

        // Llena la tabla con valores aleatorios para empezar
        private void LlenarAleatorio()
        {
            int numFilas = dgvPuntos.Rows.Count;

            for (int i = 0; i < numFilas; i++)
            {
                // x: 0, 1, 2, ... (para que no se repitan y sea más estable)
                double x = i;

                // y aleatorio entre 0 y 10
                double y = Math.Round(rnd.NextDouble() * 10, 2);

                dgvPuntos.Rows[i].Cells[0].Value = x.ToString("F2");
                dgvPuntos.Rows[i].Cells[1].Value = y.ToString("F2");
            }
        }

        // Lee los puntos (x, y) del DataGridView a un arreglo [numPuntos, 2]
        private bool TryLeerPuntos(out double[,] puntos)
        {
            int filas = dgvPuntos.Rows.Count;
            puntos = new double[filas, 2];

            for (int i = 0; i < filas; i++)
            {
                object valX = dgvPuntos.Rows[i].Cells[0].Value;
                object valY = dgvPuntos.Rows[i].Cells[1].Value;

                if (valX == null || valY == null)
                {
                    MessageBox.Show("Faltan datos en la fila " + (i + 1),
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!double.TryParse(valX.ToString(), out double x) ||
                    !double.TryParse(valY.ToString(), out double y))
                {
                    MessageBox.Show("Valores no numéricos en la fila " + (i + 1),
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                puntos[i, 0] = x;
                puntos[i, 1] = y;
            }

            return true;
        }

        // ---------------- MÉTODO DE LAGRANGE ----------------

        /// <summary>
        /// Interpola el punto de abscisa xInterp usando el método de Lagrange.
        /// puntos: arreglo [n+1, 2] con (x_i, y_i).
        /// n: grado del polinomio.
        /// </summary>
        private double InterpolacionLagrange(double[,] puntos, int n, double xInterp)
        {
            double suma = 0.0;

            for (int i = 0; i <= n; i++)
            {
                double Li = Multiplicatoria(i, puntos, n, xInterp);
                suma += puntos[i, 1] * Li;  // y_i * L_i(x)
            }

            return suma;
        }

        /// <summary>
        /// Calcula la multiplicatoria del término i del polinomio de Lagrange:
        /// L_i(x) = Π_{j != i} (x - x_j) / (x_i - x_j)
        /// </summary>
        private double Multiplicatoria(int i, double[,] puntos, int n, double xInterp)
        {
            double prod = 1.0;
            double xi = puntos[i, 0];

            for (int j = 0; j <= n; j++)
            {
                if (j == i) continue;

                double xj = puntos[j, 0];
                double num = xInterp - xj;
                double den = xi - xj;

                prod *= (num / den);
            }

            return prod;
        }

        private void BtnAleatorio_Click(object sender, EventArgs e)
        {
            LlenarAleatorio();
        }

        private void BtnCalcular_Click(object sender, EventArgs e)
        {
            // Leer x a interpolar
            if (!double.TryParse(txtX.Text, out double xInterp))
            {
                MessageBox.Show("Ingresa un valor numérico válido para x a interpolar.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Leer puntos desde la tabla
            if (!TryLeerPuntos(out double[,] puntos))
            {
                // El método ya muestra el mensaje de error
                return;
            }

            int n = puntos.GetLength(0) - 1; // grado n = numeroPuntos - 1

            double yInterp = InterpolacionLagrange(puntos, n, xInterp);

            lblResultado.Text = yInterp.ToString("F6");
        }
    }
}