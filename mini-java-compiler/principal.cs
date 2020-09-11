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
            SoundPlayer audio5 = new SoundPlayer(mini_java_compiler.Properties.Resources.Abrir);
            //audio5.Play();
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
                SoundPlayer audio3 = new SoundPlayer(mini_java_compiler.Properties.Resources.Aceptado);
                //audio3.Play();
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
                SoundPlayer audio4 = new SoundPlayer(mini_java_compiler.Properties.Resources.Error2);
                //audio4.Play();
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
                SoundPlayer audio1 = new SoundPlayer(mini_java_compiler.Properties.Resources.Creado);
                //audio1.Play();
                MessageBox.Show("¡El archivo de creó exitosamente! Podrá encontrarlo en la carpeta Resultado ubicada en el Escritorio de su computadora.");
                creado.Text = "Estado: Archivo Creado";
                creado.ForeColor = Color.Green;
            }
            catch
            {
                SoundPlayer audio2 = new SoundPlayer(mini_java_compiler.Properties.Resources.Error2);
                //audio2.Play();
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
            MessageBox.Show("Se asigno la lista de tokens a metodo empezar.");

        }
    }
}
