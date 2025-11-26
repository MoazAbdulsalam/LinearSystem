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
    public partial class EquationsForm : Form
    {
        public EquationsForm()
        {
            InitializeComponent();
            Resize resize = new Resize(this);

        }


        void Solotion()
        {
            string Equations = textBoxEquations.Text;

            double[,] mtrx = ConvertEquationsToMatrix.Convert(Equations);

            GaussJordanSolver System = new GaussJordanSolver(mtrx);
            System.Solve();

            string stepsText = System.GetStepsString();
           // MessageBox.Show(stepsText, "Gauss-Jordan Steps");

     

            double[] result = System.GetResult();
            string resultText = "";

            for (int i = 0; i < result.Length; i++)
                resultText += $"X{i + 1} = {result[i]:0.##}\n";

            if (System.SolutionType==GaussJordanSolver.enSolutionType.eOneSolution)
                richTextBoxSolution.Text= stepsText+"\n\n" +resultText ;

            else if (System.SolutionType == GaussJordanSolver.enSolutionType.eManySolutions)
                richTextBoxSolution.Text = stepsText + "\nSystem has MANY solution";
            else
                richTextBoxSolution.Text = stepsText + "\nSystem has NO solution";
        }

        private void btnSolve_Click(object sender, EventArgs e)
        {
            if (textBoxEquations.Text.Length > 0)
            {
                richTextBoxSolution.Clear();
                Solotion();
            }
            else
                MessageBox.Show("Enter your System");
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBoxSolution.Clear();
        }

     

        private void textBoxEquations_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
