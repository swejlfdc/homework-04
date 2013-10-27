using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WordSearch
{
    public partial class Form1 : Form
    {
        int cellSize=30;
        public Form1(int[,] matrix)
        {
            InitializeComponent();
            int n = matrix.GetLength(0);
            int m = matrix.GetLength(1);
            int offset = 1;
            Label[,] labMatrix = new Label[n, m];
            for (int j=0;j!=n;++j)
                for (int i = 0; i != m; ++i)
                {
                    labMatrix[i, j] = new Label();
                    this.Controls.Add(labMatrix[i, j]);
                    labMatrix[i, j].BorderStyle = BorderStyle.FixedSingle;
                    labMatrix[i, j].Text = ((char)matrix[i, j]).ToString();
                    labMatrix[i, j].Height = cellSize;
                    labMatrix[i, j].Width = cellSize;
                    Point labLoca = new Point();
                    labLoca.X = (i+offset) * cellSize;
                    labLoca.Y = (j+offset) * cellSize;
                    labMatrix[i, j].Location = labLoca;
                    labMatrix[i, j].Visible = true;
                    labMatrix[i, j].TextAlign = ContentAlignment.MiddleCenter;
                    labMatrix[i, j].Font = new Font("Calibri", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    if (matrix[i, j] == -1) labMatrix[i, j].BackColor = Color.Yellow;
                    if (matrix[i, j] == 1) labMatrix[i, j].BackColor = Color.LightBlue;
                }
            this.Height = (n+3) * cellSize;
            this.Width = (m+2) * cellSize;
            this.Visible = true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
