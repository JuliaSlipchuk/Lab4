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

namespace Lab1
{
    public partial class Form1 : Form
    {
        static string filePath;
        int countOfLetters = 0;
        List<char> alphavit = new List<char>() { 'а', 'б', 'в', 'г', 'ґ', 'д', 'е', 'є', 'ж', 'з', 'и', 'і', 'ї', 'й', 'к', 'л',
            'м', 'н', 'о', 'п', 'р', 'с', 'т', 'у', 'ф', 'х', 'ц', 'ч', 'ш', 'щ', 'ь', 'ю', 'я' };
        Dictionary<char, float> frequency = new Dictionary<char, float>();

        public Form1()
        {
            FillDictionary();

            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = true;
            richTextBox1.Clear();

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var filePaths = openFileDialog1.FileNames;
                foreach(var filePath in filePaths)
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
            if (richTextBox1.TextLength == 0)
                MessageBox.Show("Text is missing");
            countOfLetters = richTextBox1.Text.Count(c => char.IsLetter(c));
            for (int i = 0; i < richTextBox1.Text.Length; i++)
            {
                if (frequency.ContainsKey(Char.ToLower(richTextBox1.Text[i])))
                {
                    frequency[Char.ToLower(richTextBox1.Text[i])] += 1;
                }
            }
            foreach(char item in alphavit)
            {
                frequency[item] /= countOfLetters;
            }
            FillChart1();
            FillChart2();
            label2.Text = string.Join(", ", frequency.OrderByDescending(f => f.Value).Select(f => f.Key));
        }

        private void FillChart1()
        {
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.Series["Frequency"].Points.Clear();
            foreach (char item in alphavit)
            {
                chart1.Series["Frequency"].Points.AddXY(item.ToString(), frequency[item]);
            }
        }

        private void FillChart2()
        {
            chart2.ChartAreas[0].AxisX.Interval = 1;
            chart2.Series["Frequency"].Points.Clear();
            foreach(var item in frequency.OrderBy(f => f.Value))
            {
                chart2.Series["Frequency"].Points.AddXY(item.Key.ToString(), item.Value);
            }
        }

        private void FillDictionary()
        {
            for (int i = 0; i < alphavit.Count; i++)
            {
                frequency.Add(alphavit[i], 0);
            }
        }
    }
}
