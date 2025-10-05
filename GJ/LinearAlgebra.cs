using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyectos.GJ
{
    public static class LinearAlgebra
    {
        /// <summary>
        /// Copia una matriz (para no mutar el original)
        /// </summary>
        public static double[,] Copy(double[,] M)
        {
            int r = M.GetLength(0), c = M.GetLength(1);
            var R = new double[r, c];
            Array.Copy(M, R, M.Length);
            return R;
        }

        private static string FormatMatrix(double[,] M, string title = null)
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(title)) sb.AppendLine(title);
            int n = M.GetLength(0), m = M.GetLength(1);
            for (int i = 0; i < n; i++)
            {
                sb.Append("  ");
                for (int j = 0; j < m; j++)
                {
                    if (j == m - 1) sb.Append("│ "); // separa [A|b]
                    sb.Append(M[i, j].ToString("0.####", CultureInfo.InvariantCulture));
                    sb.Append(j == m - 1 ? "" : "\t");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        // ===== Gauss con pasos =====
        public static (double[] x, string log) GaussWithSteps(double[,] Ab, bool partialPivot = true, double eps = 1e-12)
        {
            int n = Ab.GetLength(0);
            int m = Ab.GetLength(1);
            if (m != n + 1) throw new ArgumentException("La matriz aumentada debe ser n×(n+1).");

            var M = Copy(Ab);
            var log = new StringBuilder();

            log.AppendLine("╔══════════════════════════════════════╗");
            log.AppendLine("║    ELIMINACIÓN DE GAUSS (paso a paso)║");
            log.AppendLine("╚══════════════════════════════════════╝");
            log.AppendLine(FormatMatrix(M, "Matriz inicial [A|b]:"));

            // Forward elimination
            for (int k = 0; k < n - 1; k++)
            {
                if (partialPivot)
                {
                    int p = k; double max = Math.Abs(M[k, k]);
                    for (int i = k + 1; i < n; i++)
                    {
                        double v = Math.Abs(M[i, k]);
                        if (v > max) { max = v; p = i; }
                    }
                    if (p != k)
                    {
                        SwapRows(M, p, k);
                        log.AppendLine($"— Pivoteo parcial: R{k + 1} ↔ R{p + 1}");
                        log.AppendLine(FormatMatrix(M, "Después del pivoteo:"));
                    }
                }

                if (Math.Abs(M[k, k]) < eps)
                    throw new ArithmeticException($"Pivote casi cero en k={k}. Sistema singular o mal condicionado.");

                for (int i = k + 1; i < n; i++)
                {
                    double mik = M[i, k] / M[k, k];
                    log.AppendLine($"— Eliminar a[{i + 1},{k + 1}] con m{i + 1}{k + 1} = a[{i + 1},{k + 1}]/a[{k + 1},{k + 1}] = {mik:0.######}");
                    log.AppendLine($"  R{i + 1} := R{i + 1} − ({mik:0.######})·R{k + 1}");

                    for (int j = k; j < m; j++)
                        M[i, j] -= mik * M[k, j];

                    log.AppendLine(FormatMatrix(M, "Estado tras la operación:"));
                }
            }

            // Back substitution
            var x = new double[n];
            log.AppendLine("— Sustitución regresiva:");
            for (int i = n - 1; i >= 0; i--)
            {
                double sum = M[i, n];
                for (int j = i + 1; j <= n - 1; j++)
                    sum -= M[i, j] * x[j];

                x[i] = sum / M[i, i];
                log.AppendLine($"  x{i + 1} = (b{i + 1} − Σ a[{i + 1},j]·xj)/a[{i + 1},{i + 1}] = {x[i]:G10}");
            }

            log.AppendLine();
            log.AppendLine("Solución:");
            for (int i = 0; i < n; i++) log.AppendLine($"  x{i + 1} = {x[i]:G10}");
            return (x, log.ToString());
        }

        // ===== Gauss-Jordan con pasos =====
        public static (double[] x, double[,] RREF, string log) GaussJordanWithSteps(double[,] Ab, bool partialPivot = true, double eps = 1e-12)
        {
            int n = Ab.GetLength(0);
            int m = Ab.GetLength(1);
            if (m != n + 1) throw new ArgumentException("La matriz aumentada debe ser n×(n+1).");

            var M = Copy(Ab);
            var log = new StringBuilder();
            log.AppendLine("╔══════════════════════════════════════╗");
            log.AppendLine("║     GAUSS–JORDAN (paso a paso)       ║");
            log.AppendLine("╚══════════════════════════════════════╝");
            log.AppendLine(FormatMatrix(M, "Matriz inicial [A|b]:"));

            int row = 0, col = 0;
            while (row < n && col < n)
            {
                // seleccionar pivote
                int p = row;
                if (partialPivot)
                {
                    double max = Math.Abs(M[row, col]);
                    for (int r = row + 1; r < n; r++)
                    {
                        double v = Math.Abs(M[r, col]);
                        if (v > max) { max = v; p = r; }
                    }
                }
                if (Math.Abs(M[p, col]) < eps) { col++; continue; }

                if (p != row)
                {
                    SwapRows(M, p, row);
                    log.AppendLine($"— Pivoteo parcial: R{row + 1} ↔ R{p + 1}");
                    log.AppendLine(FormatMatrix(M, "Después del pivoteo:"));
                }

                // normalizar fila pivote
                double piv = M[row, col];
                log.AppendLine($"— Normalizar pivote a 1: R{row + 1} := R{row + 1}/({piv:0.######})");
                for (int j = col; j < m; j++) M[row, j] /= piv;
                log.AppendLine(FormatMatrix(M, "Tras normalizar:"));

                // anular columna col en el resto
                for (int r = 0; r < n; r++)
                {
                    if (r == row) continue;
                    double factor = M[r, col];
                    if (Math.Abs(factor) > eps)
                    {
                        log.AppendLine($"— Hacer cero a[{r + 1},{col + 1}]: R{r + 1} := R{r + 1} − ({factor:0.######})·R{row + 1}");
                        for (int j = col; j < m; j++) M[r, j] -= factor * M[row, j];
                        log.AppendLine(FormatMatrix(M, "Estado:"));
                    }
                }

                row++; col++;
            }

            // verificar consistencia / única
            for (int i = 0; i < n; i++)
            {
                bool allZero = true;
                for (int j = 0; j < n; j++) if (Math.Abs(M[i, j]) > eps) { allZero = false; break; }
                if (allZero && Math.Abs(M[i, n]) > eps)
                    throw new ArithmeticException("Sistema incompatible (fila 0…0 | c≠0).");
            }

            var x = new double[n];
            for (int i = 0; i < n; i++)
            {
                int pivotCol = -1;
                for (int j = 0; j < n; j++)
                {
                    if (Math.Abs(M[i, j] - 1) < 1e-9)
                    {
                        bool ok = true;
                        for (int r = 0; r < n; r++) if (r != i && Math.Abs(M[r, j]) > 1e-9) { ok = false; break; }
                        if (ok) { pivotCol = j; break; }
                    }
                }
                if (pivotCol == -1)
                    throw new ArithmeticException("No hay solución única (existen parámetros libres).");
                x[pivotCol] = M[i, n];
            }

            log.AppendLine();
            log.AppendLine("RREF final [I|x]:");
            log.AppendLine(FormatMatrix(M));
            log.AppendLine("Solución:");
            for (int i = 0; i < n; i++) log.AppendLine($"  x{i + 1} = {x[i]:G10}");

            return (x, M, log.ToString());
        }

        /// <summary>
        /// Eliminación de Gauss con pivoteo parcial opcional.
        /// Recibe matriz aumentada [A|b] de tamaño n x (n+1).
        /// Devuelve vector solución x (long n).
        /// </summary>
        public static double[] Gauss(double[,] Ab, bool partialPivot = true, double eps = 1e-12)
        {
            int n = Ab.GetLength(0);
            int m = Ab.GetLength(1); // m = n + 1 esperado

            if (m != n + 1) 
                throw new ArgumentException("La matriz aumentada debe ser de tamaño n x (n+1).");

            var M = Copy(Ab);

            // FORWARD ELIMINATION
            for (int k = 0; k < n - 1; k++)
            {
                // Pivoteo parcial: elegir fila con mayor |M[i,k]| desde i=k..n-1
                if (partialPivot)
                {
                    int p = k;
                    double max = Math.Abs(M[k, k]);
                    for (int i = k + 1; i < n; i++)
                    {
                        double val = Math.Abs(M[i, k]);
                        if (val > max) { max = val; p = i; }
                    }
                    if (p != k)
                        SwapRows(M, p, k);
                }

                if (Math.Abs(M[k, k]) < eps)
                    throw new ArithmeticException($"Pivote casi cero en k={k}. El sistema puede ser singular o mal condicionado.");

                for (int i = k + 1; i < n; i++)
                {
                    double factor = M[i, k] / M[k, k];
                    for (int j = k; j < m; j++)
                        M[i, j] -= factor * M[k, j];
                }
            }

            if (Math.Abs(M[n - 1, n - 1]) < eps)
                throw new ArithmeticException("Pivote final casi cero. El sistema puede ser singular o tener infinitas soluciones.");

            // BACK SUBSTITUTION
            var x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                double sum = M[i, n]; // término independiente
                for (int j = i + 1; j <= n - 1; j++)
                    sum -= M[i, j] * x[j];

                x[i] = sum / M[i, i];
            }
            return x;
        }

        /// <summary>
        /// Gauss-Jordan para reducir a RREF. Devuelve:
        /// - vector solución (si hay solución única) y
        /// - la matriz reducida (n x (n+1))
        /// Lanza excepción si no hay solución o si no es única (para simplicidad).
        /// </summary>
        public static (double[] x, double[,] RREF) GaussJordan(double[,] Ab, bool partialPivot = true, double eps = 1e-12)
        {
            int n = Ab.GetLength(0);
            int m = Ab.GetLength(1);
            if (m != n + 1) throw new ArgumentException("La matriz aumentada debe ser de tamaño n x (n+1).");

            var M = Copy(Ab);
            int row = 0, col = 0;

            while (row < n && col < n)
            {
                // Pivoteo parcial: fila con mayor |M[r, col]| desde r=row..n-1
                int p = row;
                if (partialPivot)
                {
                    double max = Math.Abs(M[row, col]);
                    for (int r = row + 1; r < n; r++)
                    {
                        double val = Math.Abs(M[r, col]);
                        if (val > max) { max = val; p = r; }
                    }
                }
                if (Math.Abs(M[p, col]) < eps)
                {
                    col++;
                    continue; // no hay pivote en esta columna
                }

                if (p != row) SwapRows(M, p, row);

                // Normalizar fila pivote
                double piv = M[row, col];
                for (int j = col; j < m; j++) M[row, j] /= piv;

                // Hacer ceros en la columna col para todas las filas != row
                for (int r = 0; r < n; r++)
                {
                    if (r == row) continue;
                    double factor = M[r, col];
                    if (Math.Abs(factor) > eps)
                    {
                        for (int j = col; j < m; j++)
                            M[r, j] -= factor * M[row, j];
                    }
                }

                row++;
                col++;
            }

            // Comprobar consistencia: filas [0...n-1] con todo 0 en A y b != 0 -> sin solución
            for (int i = 0; i < n; i++)
            {
                bool allZero = true;
                for (int j = 0; j < n; j++)
                    if (Math.Abs(M[i, j]) > eps) { allZero = false; break; }
                if (allZero && Math.Abs(M[i, n]) > eps)
                    throw new ArithmeticException("El sistema es incompatible (sin solución).");
            }

            // Intentar extraer solución única
            var x = new double[n];
            for (int i = 0; i < n; i++)
            {
                // Buscar columna con 1 como pivote en esta fila
                int pivotCol = -1;
                for (int j = 0; j < n; j++)
                {
                    if (Math.Abs(M[i, j] - 1.0) < 1e-9)
                    {
                        // asegurarse de que el resto de la columna sea ~0
                        bool colOk = true;
                        for (int r = 0; r < n; r++)
                            if (r != i && Math.Abs(M[r, j]) > 1e-9) { colOk = false; break; }
                        if (colOk) { pivotCol = j; break; }
                    }
                }
                if (pivotCol == -1)
                    throw new ArithmeticException("El sistema no tiene solución única (parámetros libres).");

                x[pivotCol] = M[i, n];
            }

            return (x, M);
        }

        private static void SwapRows(double[,] M, int r1, int r2)
        {
            if (r1 == r2) return;
            int cols = M.GetLength(1);
            for (int j = 0; j < cols; j++)
            {
                double tmp = M[r1, j];
                M[r1, j] = M[r2, j];
                M[r2, j] = tmp;
            }
        }

        public static (double[,] RREF, int rankA, int rankAb, List<int> pivots) RrefGeneral(
    double[,] Ab, bool partialPivot = true, double eps = 1e-12)
        {
            int rows = Ab.GetLength(0);
            int cols = Ab.GetLength(1);
            int vars = cols - 1;

            double[,] M = Copy(Ab);
            int r = 0;
            var pivots = new List<int>();

            for (int c = 0; c < vars && r < rows; c++)
            {
                // buscar pivote en columna c
                int p = r;
                double max = Math.Abs(M[r, c]);
                if (partialPivot)
                {
                    for (int i = r + 1; i < rows; i++)
                    {
                        double val = Math.Abs(M[i, c]);
                        if (val > max) { max = val; p = i; }
                    }
                }
                if (Math.Abs(M[p, c]) < eps) continue; // no hay pivote en esta col

                if (p != r) SwapRows(M, p, r);

                // normalizar fila pivote
                double piv = M[r, c];
                for (int j = c; j < cols; j++) M[r, j] /= piv;

                // ceros arriba/abajo
                for (int i = 0; i < rows; i++)
                {
                    if (i == r) continue;
                    double factor = M[i, c];
                    if (Math.Abs(factor) > eps)
                        for (int j = c; j < cols; j++)
                            M[i, j] -= factor * M[r, j];
                }

                pivots.Add(c);
                r++;
            }

            int rankA = pivots.Count;

            // rank([A|b]): si encuentras fila [0...0 | c!=0] => inconsistente
            int rankAb = rankA;
            for (int i = 0; i < rows; i++)
            {
                bool allZeroA = true;
                for (int j = 0; j < vars; j++)
                    if (Math.Abs(M[i, j]) > eps) { allZeroA = false; break; }

                if (allZeroA && Math.Abs(M[i, vars]) > eps)
                {
                    rankAb = rankA + 1;
                    break;
                }
            }

            return (M, rankA, rankAb, pivots);
        }

    }
}
