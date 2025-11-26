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
    public partial class MatrixForm : Form
    {
        private TextBox[,] matrixTextBoxes;
        private double[,] matrixValues;
        private short rows;
        private short columns;
        
        public MatrixForm()
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
            matrixValues = new double[rows , columns];

            for (int i = 0;i<rows;i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    if (matrixTextBoxes[i, j].Text == "-")
                        matrixValues[i, j] = -1;
                    else if(matrixTextBoxes[i, j].Text == "+")
                        matrixValues[i, j] = 1;
                    else
                        matrixValues[i, j] = double.Parse(matrixTextBoxes[i, j].Text);
                }
            }
        }
     
        private void Solution()
        {
             ConvertBoxesToMatrix();

            GaussJordanSolver System = new GaussJordanSolver(matrixValues);
            System.Solve();

            string stepsText = System.GetStepsString();
            // MessageBox.Show(stepsText, "Gauss-Jordan Steps");



            double[] result = System.GetResult();
            string resultText = "";

            for (int i = 0; i < result.Length; i++)
                resultText += $"X{i + 1} = {result[i]:0.##}\n";

            if (System.SolutionType == GaussJordanSolver.enSolutionType.eOneSolution)
                richTextBoxSolution.Text = stepsText + "\n\n" + resultText;

            else if (System.SolutionType == GaussJordanSolver.enSolutionType.eManySolutions)
                richTextBoxSolution.Text = stepsText + "\nSystem has MANY solution";
            else
                richTextBoxSolution.Text = stepsText + "\nSystem has NO solution";

        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            Solution();
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
            btnSolve.Visible=false;
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
         
            
            if (textBoxRows.Text.Length == 0 && e.KeyChar == '0')
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

            btnSolve.Visible = allFilled;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBoxSolution.Clear();
        }

        
    }
   
}
/*
 اعمل فورم جديد عشان تجيب الانفرس بس
 */