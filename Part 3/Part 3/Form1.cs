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

namespace Part_3
{
    public partial class Form1 : Form
    {
        private Dictionary<string, double> frequency = new Dictionary<string, double>();
        StringBuilder inputStringBuilder;
        List<char> forbiddenCharacters = new List<char>()
            { '.', ',', '!', '?', '@', '#', '$', '%', '^', '&', '*', '(', ')', '-', '=', '+', '/', '\'', '\"', ':', ';', ' ', '’', '\n' };
        public static List<char> alphavit = new List<char>() { 'а', 'б', 'в', 'г', 'ґ', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й', 'к', 'л',
            'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            richTextBox1.Clear();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filePaths = openFileDialog1.FileNames;
                foreach (var filePath in filePaths)
                {
                    using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8, true))
                    {
                        richTextBox1.Text += reader.ReadToEnd();
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(richTextBox1.Text))
            {
                MessageBox.Show("Text is missing");
            }

            inputStringBuilder = new StringBuilder(richTextBox1.Text.ToLower());
            for (int i = 0; i < inputStringBuilder.Length; i++)
            {
                if (!alphavit.Contains(inputStringBuilder[i]))
                {
                    inputStringBuilder.Remove(i, 1);
                    i--;
                }
            }

            int count = 0;
            for (int i = 0; i < inputStringBuilder.Length - 2; i++)
            {
                count++;
                var key = inputStringBuilder[i].ToString() + inputStringBuilder[i + 1] + inputStringBuilder[i + 2];
                if (frequency.ContainsKey(key))
                {
                    frequency[key]++;
                    continue;
                }
                frequency.Add(key, 1);
            }
            for (var i = 0; i < frequency.Count; i++)
            {
                var element = frequency.ElementAt(i);
                frequency[element.Key] = (double)element.Value / count;
            }

            dataGridView1.DataSource = GetResultForDataGridView();
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 80;

            richTextBox2.Text = string.Join(", ", frequency.OrderByDescending(f => f.Value).Select(f => f.Key).Take(30));

            FillChart();
        }

        private DataTable GetResultForDataGridView()
        {
            DataTable table = new DataTable();

            table.Columns.Add("Trigram", typeof(string));
            table.Columns.Add("Frequency", typeof(double));

            foreach (var item in frequency.OrderBy(f => f.Value))
            {
                table.Rows.Add(item.Key, Math.Round(item.Value, 8));
            }

            return table;
        }

        private void FillChart()
        {
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.Series["Frequency"].Points.Clear();
            foreach (var item in frequency.OrderByDescending(f => f.Value).Take(30))
            {
                chart1.Series["Frequency"].Points.AddXY(item.Key.ToString(), item.Value);
            }
        }
    }
}
