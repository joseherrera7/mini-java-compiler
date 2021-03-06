﻿namespace mini_java_compiler
{
    partial class Principal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Principal));
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_createFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.estado = new System.Windows.Forms.Label();
            this.creado = new System.Windows.Forms.Label();
            this.txtErrores = new System.Windows.Forms.RichTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnASDR = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.rtxASDR = new System.Windows.Forms.RichTextBox();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.rtbTablaSimbolos = new System.Windows.Forms.RichTextBox();
            this.btnTablaSimbolos = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.lbTS = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.AllowDrop = true;
            this.btnLoadFile.AutoEllipsis = true;
            this.btnLoadFile.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnLoadFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLoadFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLoadFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadFile.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnLoadFile.Location = new System.Drawing.Point(183, 304);
            this.btnLoadFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(124, 38);
            this.btnLoadFile.TabIndex = 0;
            this.btnLoadFile.Text = "Load File";
            this.btnLoadFile.UseVisualStyleBackColor = false;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(107, 258);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Paso 1: Cargue el archivo";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label2.Location = new System.Drawing.Point(120, 356);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(214, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Paso 2: Crear el archivo";
            // 
            // btn_createFile
            // 
            this.btn_createFile.AllowDrop = true;
            this.btn_createFile.AutoEllipsis = true;
            this.btn_createFile.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_createFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btn_createFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btn_createFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_createFile.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btn_createFile.Location = new System.Drawing.Point(183, 391);
            this.btn_createFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_createFile.Name = "btn_createFile";
            this.btn_createFile.Size = new System.Drawing.Size(124, 38);
            this.btn_createFile.TabIndex = 4;
            this.btn_createFile.Text = "Create File";
            this.btn_createFile.UseVisualStyleBackColor = false;
            this.btn_createFile.Click += new System.EventHandler(this.btn_createFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(561, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(238, 29);
            this.label3.TabIndex = 5;
            this.label3.Text = "Mini Java Compiler";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label4.Location = new System.Drawing.Point(384, 78);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(578, 54);
            this.label4.TabIndex = 6;
            this.label4.Text = "Instrucciones: El siguiente programa leerá un archivo de texto y lo analizará\r\nlí" +
    "nea por linea, devolviendo un archivo con todo el análisis\r\ndel código Java.";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label5.Location = new System.Drawing.Point(8, 150);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 18);
            this.label5.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.DarkOrange;
            this.label6.Location = new System.Drawing.Point(17, 454);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(464, 34);
            this.label6.TabIndex = 8;
            this.label6.Text = "NOTA: El archivo resultante llevará el nombre de result \r\ny se creará en una carp" +
    "eta llamada Resultado, en el Escritorio";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.ForeColor = System.Drawing.SystemColors.Window;
            this.label7.Location = new System.Drawing.Point(492, 733);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(528, 17);
            this.label7.TabIndex = 9;
            this.label7.Text = "José Herrera y Renato Cabrera. Universidad Rafael Landívar. Compiladores 2020.";
            // 
            // estado
            // 
            this.estado.AutoSize = true;
            this.estado.BackColor = System.Drawing.Color.Transparent;
            this.estado.ForeColor = System.Drawing.Color.White;
            this.estado.Location = new System.Drawing.Point(325, 316);
            this.estado.Name = "estado";
            this.estado.Size = new System.Drawing.Size(136, 17);
            this.estado.TabIndex = 10;
            this.estado.Text = "Estado: No Cargado";
            // 
            // creado
            // 
            this.creado.AutoSize = true;
            this.creado.BackColor = System.Drawing.Color.Transparent;
            this.creado.ForeColor = System.Drawing.Color.White;
            this.creado.Location = new System.Drawing.Point(328, 404);
            this.creado.Name = "creado";
            this.creado.Size = new System.Drawing.Size(136, 17);
            this.creado.TabIndex = 11;
            this.creado.Text = "Estado: No Cargado";
            // 
            // txtErrores
            // 
            this.txtErrores.Location = new System.Drawing.Point(548, 210);
            this.txtErrores.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtErrores.Name = "txtErrores";
            this.txtErrores.Size = new System.Drawing.Size(601, 131);
            this.txtErrores.TabIndex = 12;
            this.txtErrores.Text = "";
            this.txtErrores.TextChanged += new System.EventHandler(this.txtErrores_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label8.Location = new System.Drawing.Point(133, 203);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(201, 20);
            this.label8.TabIndex = 13;
            this.label8.Text = "PROCESO DE CARGA";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label9.Location = new System.Drawing.Point(716, 169);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(247, 20);
            this.label9.TabIndex = 14;
            this.label9.Text = "ERRORES EN EL ARCHIVO";
            // 
            // btnASDR
            // 
            this.btnASDR.AllowDrop = true;
            this.btnASDR.AutoEllipsis = true;
            this.btnASDR.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnASDR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnASDR.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnASDR.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnASDR.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnASDR.Location = new System.Drawing.Point(784, 356);
            this.btnASDR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnASDR.Name = "btnASDR";
            this.btnASDR.Size = new System.Drawing.Size(124, 38);
            this.btnASDR.TabIndex = 15;
            this.btnASDR.Text = "Analizar";
            this.btnASDR.UseVisualStyleBackColor = false;
            this.btnASDR.Click += new System.EventHandler(this.btnASDR_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label10.Location = new System.Drawing.Point(1220, 169);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(223, 20);
            this.label10.TabIndex = 17;
            this.label10.Text = "ERRORES SEMANTICOS";
            this.label10.Click += new System.EventHandler(this.label10_Click);
            // 
            // rtxASDR
            // 
            this.rtxASDR.Location = new System.Drawing.Point(1155, 210);
            this.rtxASDR.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtxASDR.Name = "rtxASDR";
            this.rtxASDR.Size = new System.Drawing.Size(368, 131);
            this.rtxASDR.TabIndex = 16;
            this.rtxASDR.Text = "";
            this.rtxASDR.TextChanged += new System.EventHandler(this.rtxASDR_TextChanged);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.AllowDrop = true;
            this.btnLimpiar.AutoEllipsis = true;
            this.btnLimpiar.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnLimpiar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLimpiar.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnLimpiar.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLimpiar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnLimpiar.Location = new System.Drawing.Point(183, 524);
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(124, 38);
            this.btnLimpiar.TabIndex = 18;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = false;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // rtbTablaSimbolos
            // 
            this.rtbTablaSimbolos.Location = new System.Drawing.Point(548, 501);
            this.rtbTablaSimbolos.Name = "rtbTablaSimbolos";
            this.rtbTablaSimbolos.Size = new System.Drawing.Size(975, 144);
            this.rtbTablaSimbolos.TabIndex = 19;
            this.rtbTablaSimbolos.Text = "";
            // 
            // btnTablaSimbolos
            // 
            this.btnTablaSimbolos.AllowDrop = true;
            this.btnTablaSimbolos.AutoEllipsis = true;
            this.btnTablaSimbolos.BackColor = System.Drawing.SystemColors.HotTrack;
            this.btnTablaSimbolos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTablaSimbolos.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTablaSimbolos.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTablaSimbolos.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnTablaSimbolos.Location = new System.Drawing.Point(968, 661);
            this.btnTablaSimbolos.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnTablaSimbolos.Name = "btnTablaSimbolos";
            this.btnTablaSimbolos.Size = new System.Drawing.Size(163, 50);
            this.btnTablaSimbolos.TabIndex = 20;
            this.btnTablaSimbolos.Text = "Tabla de Simbolos";
            this.btnTablaSimbolos.UseVisualStyleBackColor = false;
            this.btnTablaSimbolos.Click += new System.EventHandler(this.btnTablaSimbolos_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label11.Location = new System.Drawing.Point(947, 468);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(202, 20);
            this.label11.TabIndex = 21;
            this.label11.Text = "TABLA DE SIMBOLOS";
            // 
            // lbTS
            // 
            this.lbTS.AutoSize = true;
            this.lbTS.BackColor = System.Drawing.Color.Transparent;
            this.lbTS.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lbTS.Location = new System.Drawing.Point(1137, 679);
            this.lbTS.Name = "lbTS";
            this.lbTS.Size = new System.Drawing.Size(143, 17);
            this.lbTS.TabIndex = 22;
            this.lbTS.Text = "Estado: NO CREADO";
            // 
            // Principal
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(1654, 776);
            this.Controls.Add(this.lbTS);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.btnTablaSimbolos);
            this.Controls.Add(this.rtbTablaSimbolos);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.rtxASDR);
            this.Controls.Add(this.btnASDR);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtErrores);
            this.Controls.Add(this.creado);
            this.Controls.Add(this.estado);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btn_createFile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnLoadFile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Principal";
            this.Text = "Mini Java Compiler";
            this.Load += new System.EventHandler(this.Principal_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_createFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label estado;
        private System.Windows.Forms.Label creado;
        private System.Windows.Forms.RichTextBox txtErrores;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnASDR;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.RichTextBox rtxASDR;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.RichTextBox rtbTablaSimbolos;
        private System.Windows.Forms.Button btnTablaSimbolos;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbTS;
    }
}

