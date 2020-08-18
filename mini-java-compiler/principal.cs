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

        public Principal()
        {
            InitializeComponent();
        }

        private void btnLoadFile_Click(object sender, EventArgs e)
        {
            var fileContent = string.Empty;
            var filePath = string.Empty;

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        while (!reader.EndOfStream)
                        {
                            fileContent = reader.ReadLine();
                            rdr.ReadProgram(fileContent);
                        }
                        
                    }
                }
            }

            if (!fileContent.Equals(""))
            {
                MessageBox.Show(fileContent, "File Content at path: " + filePath, MessageBoxButtons.OK);
            }
        }

   

        private void btn_createFile_Click(object sender, EventArgs e)
        {
            string[] codigo = {/*Colocar aqui lo que se recibira*/};
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
