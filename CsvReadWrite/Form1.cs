using Csv;
using System.Data;
using System.Text;


namespace CsvReadWrite
{
    public partial class Form1 : Form
    {
        DataTable dataTable = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonCsvRead_Click(object sender, EventArgs e)
        {
            if (openFileDialogCsv.ShowDialog() == DialogResult.OK)
            {
                textBoxInputCSVFileName.Text = openFileDialogCsv.FileName;

                string csv = File.ReadAllText(openFileDialogCsv.FileName, Encoding.GetEncoding("utf-16"));
                dataTable.TableName = "CSVTable";
                dataTable.Columns.Clear();

                dataTable.Clear();

                foreach (ICsvLine line in CsvReader.ReadFromText(csv))
                {
                    foreach (var item in line.Headers)
                    {
                        dataTable.Columns.Add(item);
                    }
                    break;
                }

                foreach (ICsvLine line in CsvReader.ReadFromText(csv))
                {
                    dataTable.Rows.Add(line.Values);
                }
                dataGridViewCsv.DataSource = dataTable;
            }

        }

        private void buttonCsvWrite_Click(object sender, EventArgs e)
        {
            if (saveFileDialogCsv.ShowDialog() == DialogResult.OK)
            {
                textBoxOutputCSVFileName.Text = saveFileDialogCsv.FileName;

                string[] header = new string[dataTable.Columns.Count];

                for (int i = 0; i < dataTable.Columns.Count; i++)
                {
                    header[i] = dataTable.Columns[i].ColumnName;
                }
                string[][] newLine = new string[dataTable.Rows.Count][];
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    newLine[i] = new string[dataTable.Columns.Count];
                for (int j = 0; j < dataTable.Columns.Count; j++)
                    {
                        newLine[i][j] = (dataTable.Rows[i][j] ?? "").ToString();
                    }
                }

                string outcsv = CsvWriter.WriteToText(header, newLine);
                File.WriteAllText(saveFileDialogCsv.FileName, outcsv, Encoding.GetEncoding("utf-16"));
            }
        }
    }
}
