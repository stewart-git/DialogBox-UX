using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
// Stewart, 10/08/2022
// Dialog box example
namespace DialogUX
{
    public partial class FormDialogUX : Form
    {
        public FormDialogUX()
        {
            InitializeComponent();
        }
        static int rowSize = 8;
        static int columnSize = 3;
        string[,] employees = new string[rowSize, columnSize];
        string defaultFileName = "default.bin";

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "BIN FILES|*.bin";
            openFileDialog.Title = "Open a BIN file";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenRecord(openFileDialog.FileName);
            }
        }
        private void OpenRecord(string openFileName)
        {
            try
            {
                using (Stream stream = File.Open(openFileName, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        {
                            int x = 0;
                            Array.Clear(employees, 0, employees.Length);
                            while(stream.Position < stream.Length)
                            {
                                for(int y = 0; y < columnSize; y++)
                                {
                                    employees[x, y] = reader.ReadString();
                                }
                                x++;
                            }
                        }
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            DisplayEmployees();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "bin file|*.bin";
            saveFileDialog.Title = "Save A BIN file";
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.DefaultExt = "bin";
            saveFileDialog.ShowDialog();
            string fileName = saveFileDialog.FileName;
            if(saveFileDialog.FileName != "")
            {
                SaveRecord(fileName);
            }
            else
            {
                SaveRecord(defaultFileName);
            }
        }
        private void SaveRecord(string saveFileName)
        {
            try 
            {
                using (Stream stream = File.Open(saveFileName, FileMode.Create))
                {
                    //BinaryFormatter bin = new BinaryFormatter();
                    using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        for (int x = 0; x < rowSize; x++)
                        {                      
                            for (int y = 0; y < columnSize; y++)
                            {                                                    
                                writer.Write(employees[x, y]);
                            }
                        }
                    }
                }
            }
            catch(IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            // No error trapping
            for(int x = 0; x < rowSize; x++)
            {
                if (employees[x, 0] == "~")
                {
                    employees[x, 0] = textBoxName.Text;
                    employees[x, 1] = textBoxTitle.Text;
                    employees[x, 2] = textBoxTelephone.Text;

                    var result = MessageBox.Show("Proceed with new Record?", "Add New Record",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        break;
                    else
                    {
                        employees[x, 0] = "~"; // Name
                        employees[x, 1] = "Sales"; // Job Title
                        employees[x, 2] = "12344321"; // Telephone Number
                        break;
                    }
                }
                if(x == rowSize - 1)
                {
                    MessageBox.Show("The array is full", "Array is FULL");
                }
            }
            DisplayEmployees();
            // clear textboxes
        }
        private void DisplayEmployees()
        {
            listBoxOutput.Items.Clear();
            for (int x = 0; x < rowSize; x++)
            {
                listBoxOutput.Items.Add(employees[x, 0] + 
                    " " + employees[x, 1] + 
                    " " + employees[x,2]);
            }
        }
        private void InitaliseArray()
        {
            for (int x = 0; x < rowSize; x++)
            {
                employees[x, 0] = "~"; // Name
                employees[x, 1] = ""; // Job Title
                employees[x, 2] = ""; // Telephone Number
            }
            DisplayEmployees();
        }
        private void FormDialogUX_Load(object sender, EventArgs e)
        {
            InitaliseArray();
        }
    }
}