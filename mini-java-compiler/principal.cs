using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Windows.Forms;

namespace mini_java_compiler
{
    public partial class Principal : Form
    {
        private string lineas = string.Empty;
        private Reader rdr = new Reader();
        private Parser prs = new Parser();
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
                
                MessageBox.Show("¡Se leyó el archivo correctamente! Proceda a crear el archivo de salida.");
                estado.Text = "Estado: Archivo Cargado";
                estado.ForeColor = Color.Green;
                string lineasError = rdr.Errores;

                lineas = rdr.Writer;
                if (rdr.getComentarioAbierto())
                {
                    lineasError += ("\n***ERROR:  Comentario en EOF***");
                    lineas += ("\n***ERROR:  Comentario en EOF***");
                }
                txtErrores.Text = lineasError;
            }
            catch
            {
               
                MessageBox.Show("No se pudo leer el archivo, por favor revisar que su archivo de entrada sea válido o que el archivo de entrada no esté corrupto.");
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

                using (StreamWriter outputFile = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Resultado\\result.out"))
                {

                    outputFile.WriteLine(lineas);
                    outputFile.Close();

                }
                
                MessageBox.Show("¡El archivo de creó exitosamente! Podrá encontrarlo en la carpeta Resultado ubicada en el Escritorio de su computadora.");
                creado.Text = "Estado: Archivo Creado";
                creado.ForeColor = Color.Green;
            }
            catch
            {
                
                MessageBox.Show("No se pudo crear el archivo, por favor revisar que su archivo de entrada sea válido o que el archivo de entrada no esté corrupto.");
                creado.ForeColor = Color.Red;
                creado.Text = "Estado: Error";
            }
        }

        private void txtErrores_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnASDR_Click(object sender, EventArgs e)
        {
            prs.Empezar(rdr.GetListaTokens());
            MessageBox.Show("Se realizó el proceso correctamente.");

            string lineasError = prs.Error;
            string lineasError2 = prs.Error2;
            rtxASDR.Text = lineasError;
            rtxASDR.Text = lineasError + lineasError2;
        }
    }
}
