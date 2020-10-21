using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Part2
{
    public partial class Form2 : Form
    {
        private Dictionary<string, double> frequency { get; set; }

        public Form2(Dictionary<string, double> frequency)
        {
            this.frequency = frequency;
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            CreateMatrix();
        }

        private void CreateMatrix()
        {
            for (int i = 0; i < Form1.alphavit.Count; i++)
            {
                dataGridView2.Columns.Add(Form1.alphavit[i].ToString(), Form1.alphavit[i].ToString());
                dataGridView2.Columns[i].Width = 20;
                dataGridView2.Rows.Add(new DataGridViewRow());
                dataGridView2.Rows[i].HeaderCell = new DataGridViewRowHeaderCell();
                dataGridView2.Rows[i].HeaderCell.Value = Form1.alphavit[i].ToString();
            }
            var maxFrequency = frequency.Max(f => f.Value);
            foreach (var item in frequency)
            {
                int rowIndex = Form1.alphavit.IndexOf(item.Key[0]);
                int columnIndex = Form1.alphavit.IndexOf(item.Key[1]);

                dataGridView2.Rows[rowIndex].Cells[columnIndex].Style.BackColor = GradientPick(item.Value/maxFrequency, Color.Blue, Color.Green, Color.Red);
            }
        }

        private int LinearInterp(int start, int end, double percentage) => start + (int)Math.Round(percentage * (end - start));
        private Color ColorInterp(Color start, Color end, double percentage) =>
            Color.FromArgb(LinearInterp(start.A, end.A, percentage),
                           LinearInterp(start.R, end.R, percentage),
                           LinearInterp(start.G, end.G, percentage),
                           LinearInterp(start.B, end.B, percentage));
        private Color GradientPick(double percentage, Color Start, Color Center, Color End)
        {
            if (percentage < 0.5)
                return ColorInterp(Start, Center, percentage / 0.5);
            else if (percentage == 0.5)
                return Center;
            else
                return ColorInterp(Center, End, (percentage - 0.5) / 0.5);
        }
    }
}
