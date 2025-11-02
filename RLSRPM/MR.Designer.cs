namespace Proyectos.RLSRPM
{
    partial class MR
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.cmbMetodo = new System.Windows.Forms.ComboBox();
            this.nudFilas = new System.Windows.Forms.NumericUpDown();
            this.nudGrado = new System.Windows.Forms.NumericUpDown();
            this.nudVars = new System.Windows.Forms.NumericUpDown();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.btnAjustar = new System.Windows.Forms.Button();
            this.dgvDatos = new System.Windows.Forms.DataGridView();
            this.chartReg = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrado)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVars)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartReg)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbMetodo
            // 
            this.cmbMetodo.FormattingEnabled = true;
            this.cmbMetodo.Location = new System.Drawing.Point(40, 56);
            this.cmbMetodo.Name = "cmbMetodo";
            this.cmbMetodo.Size = new System.Drawing.Size(179, 24);
            this.cmbMetodo.TabIndex = 0;
            this.cmbMetodo.SelectedIndexChanged += new System.EventHandler(this.cmbMetodo_SelectedIndexChanged);
            // 
            // nudFilas
            // 
            this.nudFilas.Location = new System.Drawing.Point(40, 134);
            this.nudFilas.Name = "nudFilas";
            this.nudFilas.Size = new System.Drawing.Size(146, 22);
            this.nudFilas.TabIndex = 1;
            // 
            // nudGrado
            // 
            this.nudGrado.Location = new System.Drawing.Point(41, 187);
            this.nudGrado.Name = "nudGrado";
            this.nudGrado.Size = new System.Drawing.Size(146, 22);
            this.nudGrado.TabIndex = 2;
            // 
            // nudVars
            // 
            this.nudVars.Location = new System.Drawing.Point(40, 246);
            this.nudVars.Name = "nudVars";
            this.nudVars.Size = new System.Drawing.Size(146, 22);
            this.nudVars.TabIndex = 3;
            // 
            // btnGenerar
            // 
            this.btnGenerar.Location = new System.Drawing.Point(40, 299);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(208, 64);
            this.btnGenerar.TabIndex = 4;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // btnAjustar
            // 
            this.btnAjustar.Location = new System.Drawing.Point(40, 402);
            this.btnAjustar.Name = "btnAjustar";
            this.btnAjustar.Size = new System.Drawing.Size(208, 64);
            this.btnAjustar.TabIndex = 5;
            this.btnAjustar.Text = "Ajustar";
            this.btnAjustar.UseVisualStyleBackColor = true;
            this.btnAjustar.Click += new System.EventHandler(this.btnAjustar_Click);
            // 
            // dgvDatos
            // 
            this.dgvDatos.AllowUserToAddRows = false;
            this.dgvDatos.AllowUserToDeleteRows = false;
            this.dgvDatos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDatos.Location = new System.Drawing.Point(331, 51);
            this.dgvDatos.Name = "dgvDatos";
            this.dgvDatos.RowHeadersWidth = 51;
            this.dgvDatos.RowTemplate.Height = 24;
            this.dgvDatos.Size = new System.Drawing.Size(559, 610);
            this.dgvDatos.TabIndex = 6;
            // 
            // chartReg
            // 
            chartArea1.Name = "ChartArea1";
            this.chartReg.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartReg.Legends.Add(legend1);
            this.chartReg.Location = new System.Drawing.Point(938, 56);
            this.chartReg.Name = "chartReg";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point;
            series1.Legend = "Legend1";
            series1.MarkerSize = 7;
            series1.Name = "Datos";
            series1.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series1.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.BorderWidth = 3;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Modelo";
            series2.XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            series2.YValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.Double;
            this.chartReg.Series.Add(series1);
            this.chartReg.Series.Add(series2);
            this.chartReg.Size = new System.Drawing.Size(435, 605);
            this.chartReg.TabIndex = 7;
            this.chartReg.Text = "chart1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Metodo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(38, 115);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(97, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Filas a generar";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 168);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(264, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Grado del polinomio (Visible en polinomial)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 227);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(287, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Número de variables (Visible en lineal multiple)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 383);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(222, 16);
            this.label5.TabIndex = 12;
            this.label5.Text = "Generar grafica despues de la tabla";
            // 
            // MR
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1408, 758);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chartReg);
            this.Controls.Add(this.dgvDatos);
            this.Controls.Add(this.btnAjustar);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.nudVars);
            this.Controls.Add(this.nudGrado);
            this.Controls.Add(this.nudFilas);
            this.Controls.Add(this.cmbMetodo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MR";
            this.Text = "MR";
            ((System.ComponentModel.ISupportInitialize)(this.nudFilas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGrado)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVars)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDatos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartReg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbMetodo;
        private System.Windows.Forms.NumericUpDown nudFilas;
        private System.Windows.Forms.NumericUpDown nudGrado;
        private System.Windows.Forms.NumericUpDown nudVars;
        private System.Windows.Forms.Button btnGenerar;
        private System.Windows.Forms.Button btnAjustar;
        private System.Windows.Forms.DataGridView dgvDatos;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartReg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}