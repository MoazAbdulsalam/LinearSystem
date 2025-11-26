using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace مشروع_جبر_خطي
{
    public partial class InverseForm : Form
    {
        private TextBox[,] matrixTextBoxes;
        private double[,] matrixValues;
        private short rows;
        private short columns;
        public InverseForm()
        {

            InitializeComponent();
            Resize resize = new Resize(this);

        }

        private void GenerteMatrix()
        {
            panelMatrix.Controls.Clear();
            matrixTextBoxes = new TextBox[rows, columns];
            int startX = 10;
            int startY = 10;
            int boxWidth = 60;
            int boxHeight = 30;
            int spacing = 5;


            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    TextBox txt = new TextBox();
                    txt.Width = boxWidth;
                    txt.Height = boxHeight;
                    txt.Left = startX + (boxWidth + spacing) * j;
                    txt.Top = startY + (boxHeight + spacing) * i;
                    txt.TextAlign = HorizontalAlignment.Center;
                    txt.Font = new Font("Segoe UI", 10, FontStyle.Regular);


                    txt.KeyPress += MatrixBox_KeyPress;
                    txt.TextChanged += MatrixBox_TextChanged;

                    panelMatrix.Controls.Add(txt);
                    matrixTextBoxes[i, j] = txt;
                }
            }
        }

        private void ConvertBoxesToMatrix()
        {
            matrixValues = new double[rows, columns];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (matrixTextBoxes[i, j].Text == "-")
                        matrixValues[i, j] = -1;
                    else if (matrixTextBoxes[i, j].Text == "+")
                        matrixValues[i, j] = 1;
                    else
                        matrixValues[i, j] = double.Parse(matrixTextBoxes[i, j].Text);
                }
            }
        }
        private void Inverse()
        {
            ConvertBoxesToMatrix();
            if (rows != columns)
            {
                MessageBox.Show("Matrix Must Be square");
                return;
            }

            Matrix matrix = new Matrix(this.matrixValues);
            matrix.Inverse();
            StringBuilder sb = new StringBuilder();

            if (matrix.Invertible)
            {
                sb.Append("Inverse Of The Matrix : \n");
                sb.AppendLine(new string('-', 50));

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        sb.Append(matrix.InverseMatrix[i, j].ToString("0.##") + "  \t");
                    }
                    sb.AppendLine();
                }
                sb.AppendLine(new string('-', 50));
            }
            else
            {
                sb.Append("\nMatrix Has No Inverse");
            }



            richTextBoxSolution.Text = matrix.InverseSteps + "\n\n" + sb.ToString();
        }


        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxColomns.Text) || string.IsNullOrEmpty(textBoxRows.Text))
            {
                MessageBox.Show("Enter Rows And Colomns");

                return;
            }
            if (int.Parse(textBoxColomns.Text) > 41 || int.Parse(textBoxColomns.Text) > 40)
            {
                MessageBox.Show("Cant Be Done Cuze It Will Be OutOfMemory");

                return;

            }
            rows = Convert.ToInt16(textBoxRows.Text.ToString());
            columns = Convert.ToInt16(textBoxColomns.Text.ToString());
            GenerteMatrix();
            btnInverse.Visible = false;
        }

        private void textBoxRows_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (e.KeyChar == (char)8)
                return;


            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }


            if ( textBoxColomns.Text.Length == 0 && e.KeyChar == '0')
            {
                e.Handled = true;
                return;
            }
        }
        private void textBoxColomns_KeyPress(object sender, KeyPressEventArgs e)
        {


            if (e.KeyChar == (char)8)
                return;


            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }


            if (textBoxColomns.Text.Length == 0 && e.KeyChar == '0')
            {
                e.Handled = true;
                return;
            }
        }



        private void MatrixBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != '-' && e.KeyChar != '+' && e.KeyChar != '.' && e.KeyChar != 8)
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == '.' && txt.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            if ((e.KeyChar == '-' || e.KeyChar == '+') && txt.SelectionStart != 0)
            {
                e.Handled = true;
                return;
            }

            if ((e.KeyChar == '-' && txt.Text.Contains("-")) || (e.KeyChar == '+' && txt.Text.Contains("+")))
            {
                e.Handled = true;
                return;
            }
        }

        private void MatrixBox_TextChanged(object sender, EventArgs e)
        {
            bool allFilled = true;

            foreach (Control ctrl in panelMatrix.Controls)
            {
                if (ctrl is TextBox txt)
                {
                    if (string.IsNullOrWhiteSpace(txt.Text))
                    {
                        allFilled = false;
                        break;
                    }
                }
            }

            btnInverse.Visible = allFilled;

        }

        private void btnInverse_Click(object sender, EventArgs e)
        {
            Inverse();
        }
    }
}
