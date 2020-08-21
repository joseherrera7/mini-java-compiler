using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mini_java_compiler
{
    public partial class Principal : Form
    {
        Reader rdr = new Reader();
        List<String> codigo = new List<string>();

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
                                rdr.ReadProgram(fileContent);
                                string texto = rdr.Writer;
                                rdr.Writer = "";
                                codigo.Add(texto + " linea numero " + lineCounter.ToString());
                            }

                        }
                    }
                }
                MessageBox.Show("Se leyo el archivo");
            }
            catch
            {
                MessageBox.Show("No se pudo leer el archivo");
            }
        }

        private void btn_createFile_Click(object sender, EventArgs e)
        {
            
            using (StreamWriter outputFile = new StreamWriter(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Resultado\\Codigo.out"))
            {
                foreach (string fuente in codigo )
                {
                    outputFile.WriteLine(fuente);
                }
            }

        }
    }
}
