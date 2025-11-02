using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyectos.RLSRPM
{
    public static class RegressionService
    {
        // ===== Utilidades de algebra lineal =====
        private static double[,] Transpose(double[,] A)
        {
            int n = A.GetLength(0), m = A.GetLength(1);
            var T = new double[m, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    T[j, i] = A[i, j];
            return T;
        }

        private static double[,] Multiply(double[,] A, double[,] B)
        {
            int n = A.GetLength(0), m = A.GetLength(1), p = B.GetLength(1);
            if (m != B.GetLength(0)) throw new ArgumentException("Dimensiones incompatibles en Multiply.");
            var C = new double[n, p];
            for (int i = 0; i < n; i++)
                for (int k = 0; k < m; k++)
                {
                    double aik = A[i, k];
                    for (int j = 0; j < p; j++)
                        C[i, j] += aik * B[k, j];
                }
            return C;
        }

        private static double[] Multiply(double[,] A, double[] x)
        {
            int n = A.GetLength(0), m = A.GetLength(1);
            if (m != x.Length) throw new ArgumentException("Dimensiones incompatibles en Multiply(A,x).");
            var y = new double[n];
            for (int i = 0; i < n; i++)
            {
                double s = 0;
                for (int j = 0; j < m; j++) s += A[i, j] * x[j];
                y[i] = s;
            }
            return y;
        }

        // Resuelve A * x = b por Eliminación Gaussiana con pivoteo parcial
        private static double[] Solve(double[,] A, double[] b)
        {
            int n = A.GetLength(0);
            if (A.GetLength(1) != n || b.Length != n)
                throw new ArgumentException("A debe ser cuadrada y b compatible.");

            var M = (double[,])A.Clone();
            var bb = (double[])b.Clone();

            for (int k = 0; k < n - 1; k++)
            {
                // Pivoteo parcial
                int piv = k;
                double max = Math.Abs(M[k, k]);
                for (int i = k + 1; i < n; i++)
                {
                    double val = Math.Abs(M[i, k]);
                    if (val > max) { max = val; piv = i; }
                }
                if (max == 0) throw new InvalidOperationException("Matriz singular.");

                if (piv != k)
                {
                    // swap filas k y piv
                    for (int j = k; j < n; j++)
                    {
                        double tmp = M[k, j]; M[k, j] = M[piv, j]; M[piv, j] = tmp;
                    }
                    double t2 = bb[k]; bb[k] = bb[piv]; bb[piv] = t2;
                }

                // Eliminación
                for (int i = k + 1; i < n; i++)
                {
                    double factor = M[i, k] / M[k, k];
                    if (factor == 0) continue;
                    for (int j = k; j < n; j++)
                        M[i, j] -= factor * M[k, j];
                    bb[i] -= factor * bb[k];
                }
            }

            // Sustitución hacia atrás
            var x = new double[n];
            for (int i = n - 1; i >= 0; i--)
            {
                double s = bb[i];
                for (int j = i + 1; j < n; j++) s -= M[i, j] * x[j];
                x[i] = s / M[i, i];
            }
            return x;
        }

        // Construye matriz de diseño para polinomio grado d: [1, x, x^2, ...]
        private static double[,] Vandermonde(double[] x, int degree)
        {
            int n = x.Length, p = degree + 1;
            var X = new double[n, p];
            for (int i = 0; i < n; i++)
            {
                double val = 1.0;
                for (int j = 0; j < p; j++)
                {
                    X[i, j] = val;
                    val *= x[i];
                }
            }
            return X;
        }

        private static (double[] beta, double sse, double sst) NormalEquations(double[,] X, double[] y)
        {
            int n = X.GetLength(0);
            int p = X.GetLength(1);
            if (y.Length != n) throw new ArgumentException("X y y incompatibles.");

            var XT = Transpose(X);
            var XTX = Multiply(XT, X);
            var XTy = Multiply(XT, y);
            var beta = Solve(XTX, XTy);

            var yhat = Multiply(X, beta);
            double ybar = y.Average();
            double sse = 0, sst = 0;
            for (int i = 0; i < n; i++)
            {
                double e = y[i] - yhat[i];
                sse += e * e;
                double d = y[i] - ybar;
                sst += d * d;
            }
            return (beta, sse, sst);
        }

        public static (double a, double b, double R2, double RMSE) FitLinealSimple(double[] x, double[] y)
        {
            if (x.Length != y.Length || x.Length < 2) throw new ArgumentException("Se requieren al menos 2 pares (x,y).");

            double xbar = x.Average();
            double ybar = y.Average();
            double Sxx = 0, Sxy = 0, Syy = 0;
            for (int i = 0; i < x.Length; i++)
            {
                double dx = x[i] - xbar;
                double dy = y[i] - ybar;
                Sxx += dx * dx;
                Sxy += dx * dy;
                Syy += dy * dy;
            }
            double b = Sxy / Sxx;
            double a = ybar - b * xbar;
            double R2 = (Sxy * Sxy) / (Sxx * Syy);
            double RMSE = Math.Sqrt((y.Select((yi, i) => (yi - (a + b * x[i])) * (yi - (a + b * x[i]))).Sum()) / (x.Length - 2));
            return (a, b, R2, RMSE);
        }

        public static (double[] coef, double R2, double RMSE) FitPolinomial(double[] x, double[] y, int degree)
        {
            if (degree < 1) throw new ArgumentException("El grado debe ser >= 1.");
            if (x.Length != y.Length || x.Length < degree + 1) throw new ArgumentException("Datos insuficientes para el grado solicitado.");

            var X = Vandermonde(x, degree);
            var (beta, sse, sst) = NormalEquations(X, y);
            double R2 = sst == 0 ? 1.0 : 1.0 - sse / sst;
            double RMSE = Math.Sqrt(sse / (x.Length - (degree + 1)));
            return (beta, R2, RMSE);
        }

        public static (double[] beta, double R2, double RMSE) FitLinealMultiple(double[,] X, double[] y)
        {
            int n = X.GetLength(0);
            int p = X.GetLength(1);
            if (y.Length != n) throw new ArgumentException("Dimensiones incompatibles.");
            // Agregar término de sesgo (columna de 1s)
            var Xd = new double[n, p + 1];
            for (int i = 0; i < n; i++)
            {
                Xd[i, 0] = 1.0;
                for (int j = 0; j < p; j++) Xd[i, j + 1] = X[i, j];
            }
            var (beta, sse, sst) = NormalEquations(Xd, y);
            double R2 = sst == 0 ? 1.0 : 1.0 - sse / sst;
            double RMSE = Math.Sqrt(sse / (n - (p + 1)));
            return (beta, R2, RMSE);
        }

        // Helpers de evaluación
        public static double EvalLineal(double a, double b, double x) => a + b * x;

        public static double EvalPoli(double[] coef, double x)
        {
            // coef[0] + coef[1] x + ... + coef[d] x^d
            double y = 0, pot = 1;
            for (int k = 0; k < coef.Length; k++) { y += coef[k] * pot; pot *= x; }
            return y;
        }

        public static double EvalMultiple(double[] beta, double[] x) // beta[0] = intercepto
        {
            double y = beta[0];
            for (int j = 0; j < x.Length; j++) y += beta[j + 1] * x[j];
            return y;
        }
    }
}
