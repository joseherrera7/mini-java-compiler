﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace mini_java_compiler
{
    public partial class Principal : Form
    {
        private Reader rdr = new Reader();

        public Principal()
        {
            InitializeComponent();
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {

            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.InitialDirectory = "c:\\";
                    openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                    openFileDialog.FilterIndex = 2;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        //Read the contents of the file into a stream
                        var fileStream = openFileDialog.OpenFile();

                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            var fileContent = string.Empty;
                            int lineCounter = 0;
                            while (!reader.EndOfStream)
                            {
                                lineCounter++;
                                fileContent = reader.ReadLine();
                                rdr.ReadProgram(fileContent, lineCounter);
                            }
                            reader.Close();
                        }
                    }
                }
                MessageBox.Show("Se leyo el archivo");
                estado.Text = "Estado: Archivo Cargado";
                estado.ForeColor = Color.Green;
            }
            catch
            {
                MessageBox.Show("No se pudo leer el archivo");
                estado.ForeColor = Color.Red;
                estado.Text = "Estado: Error";
            }
        }

        private void btn_createFile_Click(object sender, EventArgs e)
        {
            try
            {
                //SE CREA LA CARPETA RESULTADO
                string ruta = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Resultado";
                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);
                string lineas = rdr.Writer;
                if (rdr.getComentarioAbierto())
                {
                    lineas += ("\n***ERROR:  Comentario en EOF***");
                }
                else if (rdr.CadenaAbrir)
                {
                    lineas += ("\n***ERROR:  Cadena sin cerrar***");
                }
                using (StreamWriter outputFile = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Resultado\\Codigo.txt"))
                {

                    outputFile.WriteLine(lineas);
                    outputFile.Close();

                }

                MessageBox.Show("Se creó el archivo");
                creado.Text = "Estado: Archivo Creado";
                creado.ForeColor = Color.Green;
            }
            catch
            {
                MessageBox.Show("No se pudo crear el archivo");
                creado.ForeColor = Color.Red;
                creado.Text = "Estado: Error";
            }
        }
    }
}
