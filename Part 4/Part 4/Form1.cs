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

namespace Part_4
{
    public partial class Form1 : Form
    {
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

            richTextBox2.Clear();
            if (checkBox1.Checked)
            {
                GetDecryption();
            }
            else
            {
                GetEncryption();
            }
        }

        private void GetEncryption()
        {
            int helper;
            StringBuilder encryptedText = new StringBuilder();
            for (int i = 0; i < inputStringBuilder.Length; i++)
            {
                helper = (alphavit.IndexOf(inputStringBuilder[i]) * 2 + 4) % alphavit.Count;
                encryptedText.Append(alphavit.ElementAt(helper));
            }
            richTextBox2.Text = encryptedText.ToString();
        }

        private void GetDecryption()
        {
            int helper = 0;
            StringBuilder decryptedText = new StringBuilder();
            for (int i = 0; i < inputStringBuilder.Length; i++)
            {
                helper = 17 * (alphavit.IndexOf(inputStringBuilder[i]) + alphavit.Count - 4) % alphavit.Count;
                decryptedText.Append(alphavit.ElementAt(helper));
            }
            richTextBox2.Text = decryptedText.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
}
