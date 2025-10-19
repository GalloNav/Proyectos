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
                    if (j == m - 1) sb.Append("│ ");
                    sb.Append(M[i, j].ToString("0.####", CultureInfo.InvariantCulture));
                    sb.Append(j == m - 1 ? "" : "\t");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static string FormatVector(double[] x, string title)
        {
            var sb = new StringBuilder();
            sb.AppendLine(title);
            for (int i = 0; i < x.Length; i++)
                sb.AppendLine($"  x{i + 1} = {x[i]:G10}");
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
        /// Gauss–Seidel con registro detallado del procedimiento.
        /// </summary>
        public static (double[] x, int iters, double resInf, string log) GaussSeidelWithSteps(
            double[,] Ab, double tol = 1e-8, int maxIter = 10000, double[] x0 = null, double w = 1.0)
        {
            int n = Ab.GetLength(0);
            if (Ab.GetLength(1) != n + 1) throw new ArgumentException("La matriz aumentada debe ser n×(n+1).");
            if (w <= 0 || w > 2) throw new ArgumentOutOfRangeException(nameof(w), "w debe estar en (0, 2].");

            var log = new StringBuilder();
            log.AppendLine("╔══════════════════════════════════════╗");
            log.AppendLine("║     MÉTODO DE GAUSS–SEIDEL (pasos)   ║");
            log.AppendLine("╚══════════════════════════════════════╝");
            log.AppendLine(FormatMatrix(Ab, "Matriz inicial [A|b]:"));

            var x = x0 != null ? (double[])x0.Clone() : new double[n];
            log.Append(FormatVector(x, "Vector inicial x⁽⁰⁾:"));

            double resInf = double.PositiveInfinity;

            for (int k = 0; k < maxIter; k++)
            {
                log.AppendLine();
                log.AppendLine($"— Iteración {k + 1}");

                double maxDelta = 0.0;

                for (int i = 0; i < n; i++)
                {
                    double aii = Ab[i, i];
                    if (Math.Abs(aii) < 1e-14)
                        throw new ArithmeticException($"A[{i + 1},{i + 1}]≈0. Reordena filas o usa pivoteo.");

                    double s = Ab[i, n];
                    for (int j = 0; j < n; j++)
                        if (j != i) s -= Ab[i, j] * x[j];

                    double xi_old = x[i];
                    double xi_new = (1 - w) * xi_old + w * (s / aii);

                    x[i] = xi_new;
                    maxDelta = Math.Max(maxDelta, Math.Abs(xi_new - xi_old));

                    log.AppendLine($"   i={i + 1}: s = b{i + 1} − Σ(j≠{i + 1}) a[{i + 1},j]·xj = {s:G10}");
                    if (Math.Abs(w - 1.0) < 1e-12)
                        log.AppendLine($"         x{i + 1}⁽{k + 1}⁾ = s / a[{i + 1},{i + 1}] = {xi_new:G10}");
                    else
                        log.AppendLine($"         x{i + 1}⁽{k + 1}⁾ = (1−w)·{xi_old:G10} + w·(s/a[{i + 1},{i + 1}]) = {xi_new:G10}");
                }

                // residuo infinito: max_i |(Ax - b)_i|
                resInf = 0.0;
                for (int i = 0; i < n; i++)
                {
                    double Ax = 0.0;
                    for (int j = 0; j < n; j++) Ax += Ab[i, j] * x[j];
                    resInf = Math.Max(resInf, Math.Abs(Ax - Ab[i, n]));
                }

                log.Append(FormatVector(x, "   x actualizado:"));
                log.AppendLine($"   Δmax = {maxDelta:E3},  ‖Ax−b‖∞ = {resInf:E3}");

                if (resInf <= tol || maxDelta <= tol)
                {
                    log.AppendLine();
                    log.AppendLine($"Convergencia alcanzada en {k + 1} iteraciones (tol = {tol:G}).");
                    return (x, k + 1, resInf, log.ToString());
                }
            }

            log.AppendLine();
            log.AppendLine($"No convergió en {maxIter} iteraciones. Último ‖Ax−b‖∞ = {resInf:E3}");
            return (x, maxIter, resInf, log.ToString());
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

        // --- Generador aleatorio con dominancia diagonal estricta (converge bien) ---
        public static double[,] RandomDiagonallyDominantAugmented(int n, int min = -5, int max = 5)
        {
            if (n < 1) 
                throw new ArgumentException("n debe ser > 0");
            var rnd = new Random();
            var Ab = new double[n, n + 1];

            for (int i = 0; i < n; i++)
            {
                double rowsum = 0.0;
                for (int j = 0; j < n; j++)
                {
                    if (i == j) continue;
                    int v = rnd.Next(min, max + 1);
                    Ab[i, j] = v;
                    rowsum += Math.Abs(v);
                }
                // Diagonal dominante estricta
                Ab[i, i] = rowsum + rnd.Next(1, 6);
                // termino independiente
                Ab[i, n] = rnd.Next(min * 2, max * 2 + 1);
            }
            return Ab;
        }

        // [A|b] aleatoria eqs×(vars+1). Si eqs==vars refuerza dominancia diagonal.
        public static double[,] RandomAugmented(int eqs, int vars, int min = -5, int max = 5, bool forceDiagDom = true)
        {
            if (eqs < 1 || vars < 1) throw new ArgumentException("Tamaños inválidos.");
            var rnd = new Random();
            var Ab = new double[eqs, vars + 1];

            for (int i = 0; i < eqs; i++)
            {
                for (int j = 0; j < vars; j++)
                    Ab[i, j] = rnd.Next(min, max + 1);
                Ab[i, vars] = rnd.Next(min * 2, max * 2 + 1);
            }

            if (forceDiagDom && eqs == vars)
            {
                for (int i = 0; i < eqs; i++)
                {
                    double rowsum = 0;
                    for (int j = 0; j < vars; j++) if (j != i) rowsum += Math.Abs(Ab[i, j]);
                    if (Math.Abs(Ab[i, i]) <= rowsum) Ab[i, i] = rowsum + rnd.Next(1, 6);
                }
            }
            return Ab;
        }

        // --- Gauss–Seidel (con relajación opcional w) ---
        // Ab: matriz aumentada n×(n+1); x0 opcional; tol y maxIter configurables.
        public static (double[] x, int iters, double resMax) GaussSeidel(
            double[,] Ab, double tol = 1e-8, int maxIter = 5000, double[] x0 = null, double w = 1.0)
        {
            int n = Ab.GetLength(0);
            if (Ab.GetLength(1) != n + 1) throw new ArgumentException("La matriz aumentada debe ser n×(n+1).");
            if (w <= 0 || w > 2) throw new ArgumentOutOfRangeException(nameof(w), "w debe estar en (0, 2].");

            var x = x0 != null ? (double[])x0.Clone() : new double[n];

            for (int k = 0; k < maxIter; k++)
            {
                double maxDiff = 0.0;

                for (int i = 0; i < n; i++)
                {
                    double aii = Ab[i, i];
                    if (Math.Abs(aii) < 1e-14)
                        throw new ArithmeticException($"A[{i + 1},{i + 1}]≈0. Reordena filas o usa pivoteo.");

                    double s = Ab[i, n];
                    // usa x actualizada (j<i) y anterior (j>i) — propio de GS
                    for (int j = 0; j < n; j++)
                        if (j != i) s -= Ab[i, j] * x[j];

                    double xi = (1 - w) * x[i] + w * (s / aii);
                    maxDiff = Math.Max(maxDiff, Math.Abs(xi - x[i]));
                    x[i] = xi;
                }

                // residuo infinito: max_i |(Ax - b)_i|
                double rInf = 0.0;
                for (int i = 0; i < n; i++)
                {
                    double Ax = 0.0;
                    for (int j = 0; j < n; j++) Ax += Ab[i, j] * x[j];
                    rInf = Math.Max(rInf, Math.Abs(Ax - Ab[i, n]));
                }

                if (rInf <= tol || maxDiff <= tol)
                    return (x, k + 1, rInf);
            }

            // No convergió dentro de maxIter
            return (x, maxIter, double.NaN);
        }

    }
}
