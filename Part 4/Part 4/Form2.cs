using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Part_4
{
    public partial class Form2 : Form
    {
        StringBuilder inputStringBuilder;
        private Dictionary<string, double> frequency = new Dictionary<string, double>();
        private readonly List<int> posibleAlpha = new List<int>() { 1, 2, 3, 4, 5, 7, 8, 10, 13, 14, 16, 17, 19, 20, 23, 25, 26, 28, 29, 31, 32 };
        private readonly List<int> posibleBeta = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 
            16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };

        public Form2()
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
            if (string.IsNullOrEmpty(richTextBox1.Text) || string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Text is missing");
            }

            int count = 0;
            inputStringBuilder = new StringBuilder(richTextBox1.Text);
            for (int i = 0; i < inputStringBuilder.Length; i++)
            {
                count++;
                var key = inputStringBuilder[i].ToString();
                if (frequency.ContainsKey(key))
                {
                    frequency[key]++;
                    continue;
                }
                frequency.Add(key, 1);
            }

            var twoCharWithMaxFrequency = frequency.OrderByDescending(f => f.Value).Select(f => f.Key).Take(2).ToList();

            var aAndB = SolvingEquation(twoCharWithMaxFrequency);
            if (aAndB == null)
            {
                MessageBox.Show("Cannot decrypt, first and second max frequencies char are not correct");
            }

            GetDecryption(aAndB);
        }

        private Tuple<int, int> SolvingEquation(List<string> twoCharWithMaxFrequency)
        {
            int indexOfFirstChar = Form1.alphavit.IndexOf(twoCharWithMaxFrequency[0].ToCharArray()[0]);
            int indexOfSecondChar = Form1.alphavit.IndexOf(twoCharWithMaxFrequency[1].ToCharArray()[0]);

            int indexFirstMaxFreqChar = Form1.alphavit.IndexOf(textBox1.Text.ToCharArray()[0]);
            int indexSecondMaxFreqChar = Form1.alphavit.IndexOf(textBox2.Text.ToCharArray()[0]);

            for (int i = 0; i < posibleAlpha.Count; i++)
            {
                for (int j = 0; j < posibleBeta.Count; j++)
                {
                    var solution1 = (posibleAlpha[i] * indexFirstMaxFreqChar + posibleBeta[j]) % Form1.alphavit.Count == indexOfFirstChar;
                    var solution2 = (posibleAlpha[i] * indexSecondMaxFreqChar + posibleBeta[j]) % Form1.alphavit.Count == indexOfSecondChar;

                    if (solution1 && solution2)
                    {
                        return new Tuple<int, int>(posibleAlpha[i], posibleBeta[j]);
                    }
                }
            }

            return null;
        }

        private void GetDecryption(Tuple<int, int> aAndB) 
        {
            int helper = 0;
            StringBuilder decryptedText = new StringBuilder();
            for (int i = 0; i < inputStringBuilder.Length; i++)
            {
                helper = modInverse(aAndB.Item1, Form1.alphavit.Count) * (Form1.alphavit.IndexOf(inputStringBuilder[i]) + Form1.alphavit.Count - aAndB.Item2) % Form1.alphavit.Count;
                decryptedText.Append(Form1.alphavit.ElementAt(helper));
            }
            richTextBox2.Text = decryptedText.ToString();
        }

        private int modInverse(int a, int n)
        {
            int i = n, v = 0, d = 1;
            while (a > 0)
            {
                int t = i / a, x = a;
                a = i % x;
                i = x;
                x = d;
                d = v - t * x;
                v = x;
            }
            v %= n;
            if (v < 0) v = (v + n) % n;
            return v;
        }
    }
}
