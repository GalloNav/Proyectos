namespace Proyectos.GJ
{
    partial class GJ
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
            this.label1 = new System.Windows.Forms.Label();
            this.nudEcuaciones = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudVariables = new System.Windows.Forms.NumericUpDown();
            this.btnCrear = new System.Windows.Forms.Button();
            this.btnGauss = new System.Windows.Forms.Button();
            this.btnGaussJordan = new System.Windows.Forms.Button();
            this.chkPivot = new System.Windows.Forms.CheckBox();
            this.txtSol = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grid = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudEcuaciones)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVariables)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "Numero de ecuaciones (filas)";
            // 
            // nudEcuaciones
            // 
            this.nudEcuaciones.Location = new System.Drawing.Point(15, 28);
            this.nudEcuaciones.Name = "nudEcuaciones";
            this.nudEcuaciones.Size = new System.Drawing.Size(203, 22);
            this.nudEcuaciones.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 82);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(202, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "Numero de variables (columnas)";
            // 
            // nudVariables
            // 
            this.nudVariables.Location = new System.Drawing.Point(15, 101);
            this.nudVariables.Name = "nudVariables";
            this.nudVariables.Size = new System.Drawing.Size(203, 22);
            this.nudVariables.TabIndex = 13;
            // 
            // btnCrear
            // 
            this.btnCrear.Location = new System.Drawing.Point(15, 207);
            this.btnCrear.Name = "btnCrear";
            this.btnCrear.Size = new System.Drawing.Size(178, 47);
            this.btnCrear.TabIndex = 14;
            this.btnCrear.Text = "Crear";
            this.btnCrear.UseVisualStyleBackColor = true;
            this.btnCrear.Click += new System.EventHandler(this.btnCrear_Click);
            // 
            // btnGauss
            // 
            this.btnGauss.Location = new System.Drawing.Point(16, 260);
            this.btnGauss.Name = "btnGauss";
            this.btnGauss.Size = new System.Drawing.Size(178, 47);
            this.btnGauss.TabIndex = 15;
            this.btnGauss.Text = "Gauss";
            this.btnGauss.UseVisualStyleBackColor = true;
            this.btnGauss.Click += new System.EventHandler(this.btnGauss_Click);
            // 
            // btnGaussJordan
            // 
            this.btnGaussJordan.Location = new System.Drawing.Point(16, 313);
            this.btnGaussJordan.Name = "btnGaussJordan";
            this.btnGaussJordan.Size = new System.Drawing.Size(178, 47);
            this.btnGaussJordan.TabIndex = 16;
            this.btnGaussJordan.Text = "Gauss Jordan";
            this.btnGaussJordan.UseVisualStyleBackColor = true;
            this.btnGaussJordan.Click += new System.EventHandler(this.btnGaussJordan_Click);
            // 
            // chkPivot
            // 
            this.chkPivot.AutoSize = true;
            this.chkPivot.Checked = true;
            this.chkPivot.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPivot.Location = new System.Drawing.Point(16, 386);
            this.chkPivot.Name = "chkPivot";
            this.chkPivot.Size = new System.Drawing.Size(75, 20);
            this.chkPivot.TabIndex = 17;
            this.chkPivot.Text = "Pivoteo";
            this.chkPivot.UseVisualStyleBackColor = true;
            // 
            // txtSol
            // 
            this.txtSol.Font = new System.Drawing.Font("Consolas", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSol.Location = new System.Drawing.Point(250, 269);
            this.txtSol.Multiline = true;
            this.txtSol.Name = "txtSol";
            this.txtSol.ReadOnly = true;
            this.txtSol.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSol.Size = new System.Drawing.Size(416, 342);
            this.txtSol.TabIndex = 18;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(247, 250);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 16);
            this.label2.TabIndex = 19;
            this.label2.Text = "Resultados";
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Location = new System.Drawing.Point(250, 47);
            this.grid.Name = "grid";
            this.grid.RowHeadersWidth = 51;
            this.grid.RowTemplate.Height = 24;
            this.grid.Size = new System.Drawing.Size(416, 167);
            this.grid.TabIndex = 20;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(247, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 16);
            this.label3.TabIndex = 21;
            this.label3.Text = "Matriz";
            // 
            // GJ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(801, 657);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSol);
            this.Controls.Add(this.chkPivot);
            this.Controls.Add(this.btnGaussJordan);
            this.Controls.Add(this.btnGauss);
            this.Controls.Add(this.btnCrear);
            this.Controls.Add(this.nudVariables);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudEcuaciones);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GJ";
            this.Text = "GJ";
            ((System.ComponentModel.ISupportInitialize)(this.nudEcuaciones)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudVariables)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudEcuaciones;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudVariables;
        private System.Windows.Forms.Button btnCrear;
        private System.Windows.Forms.Button btnGauss;
        private System.Windows.Forms.Button btnGaussJordan;
        private System.Windows.Forms.CheckBox chkPivot;
        private System.Windows.Forms.TextBox txtSol;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Label label3;
    }
}