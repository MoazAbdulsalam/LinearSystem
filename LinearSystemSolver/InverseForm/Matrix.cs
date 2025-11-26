using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace مشروع_جبر_خطي
{
    internal class Matrix
    {
         int rows { get; set; }
         int cols { get; set; }
         double[,] matrix { get; set; }
        
         public bool Invertible { get; set; }
         public double[,] InverseMatrix { get; set; }
         public string InverseSteps { get; set; } 

        public Matrix(double[,] matrix)
        {
            this.rows = matrix.GetLength(0);
            this.cols = matrix.GetLength(1);
            this.InverseSteps = "";
            this.matrix = matrix;
            

        }

        public void Inverse ()
        {
            if (cols!= rows)
                throw new ArgumentException("Not Square Matrix");
            
            double[,] AugmentedMatrix = new double[rows,cols*2];

            for (int i = 0; i< this.rows;i++)
                for (int j = 0; j< cols;j++)
                    AugmentedMatrix[i,j] = matrix[i,j];


            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    AugmentedMatrix[i, j + cols] = (i == j) ? 1 : 0;
                
            

            GaussJordanSolver Solution = new GaussJordanSolver(AugmentedMatrix);
            Solution.SaveStep(" Inverse by [A | I] -->  [I | A^-1]");
            Solution.Solve();

            InverseMatrix = new double[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    InverseMatrix[i, j] = Solution.matrix[i, j + cols];

            Invertible = IsInvertible(Solution.matrix);

            InverseSteps = Solution.GetStepsString();

             
        }
        private bool IsInvertible(double[,] Aug)
        {
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                   if( (i == j && Aug[i,j]!= 1) || (i != j && Aug[i, j] != 0))
                        return false;
            return true;
        }
    }
}
