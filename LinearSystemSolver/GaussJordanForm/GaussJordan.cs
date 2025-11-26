using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace مشروع_جبر_خطي
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Text;
    using System.Windows.Forms;

    public class GaussJordanSolver
    {
        public enum enSolutionType { eNoSolution ,eOneSolution,eManySolutions}
        public double[,] matrix;
        private int rows, cols;
        private List<double[,]> steps; 
        private List<string> stepDescriptions = new List<string>();
        public enSolutionType SolutionType;

        public GaussJordanSolver(double[,] inputMatrix)
        {
            rows = inputMatrix.GetLength(0);
            cols = inputMatrix.GetLength(1);
            matrix = (double[,])inputMatrix.Clone(); // بنسخ المصفوفة عشان مغيرش الأصلية
            steps = new List<double[,]>();
           // SaveStep("The matrix given"); // بحفظ أول خطوة قبل أي تعديل
        }

        public void SaveStep(string description="")
        {
            double[,] copy = (double[,])matrix.Clone();
            steps.Add(copy);
            stepDescriptions.Add(description);
        }

        public void Solve()
        {
            if (rows > cols)
            {
                SaveStep($"ملاحظة: عدد الصفوف" +
                    $" ({rows}) أكبر من عدد الأعمدة" +
                    $" ({cols})،" +
                    $" وبالتالي لن يوجدعنصر رئيسي لكل صف. سيتم العمل فقط حتى العامود رقم " +
                    $"{cols}.");
            }

            int leadingCol = 0;

            for (int i = 0; i < rows && leadingCol < cols-1; i++)
            {

               

                //  لو العمود الحالي كله أصفار، ننتقل للي بعده
                bool allZeroColumn = true;
                for (int k = i; k < rows; k++)
                {
                    if (matrix[k, leadingCol] != 0)
                    {
                        allZeroColumn = false;
                        break;
                    }
                }

                if (allZeroColumn)
                {
                    leadingCol++;
                    i--;
                    continue;
                }
                SaveStep("تحديد اول عامود  غير صفري على يسار المصفوفه");

                // لو العنصر الرئيسي = 0  بدل الصف لو اول عنصر في الصف يساوي صفر بدله
                if (matrix[i, leadingCol] == 0)
                {
                    for (int k = i + 1; k < rows; k++)
                    {
                        if (matrix[k, leadingCol] != 0)
                        {
                            SaveStep("  جعل اول عنصر في اول عامود غير صفري على يسار المصفوفه لا يساوي صفر في الخطوه القادمه");
                            SwapRows(i, k);
                            SaveStep($"R{i + 1} <-> R{k + 1}  (تبديل صف مع صف)");
                            break;
                        }
                    }
                }

                double leading = matrix[i, leadingCol];

               
           

               
                SaveStep($"chosing the leading : matrix :a[{i + 1},{leadingCol + 1}], and Make it =1 if its not ");
                if (leading != 1)
                {
                    for (int j = 0; j < cols; j++)
                        matrix[i, j] /= leading;
                    SaveStep($" R{i + 1} / {leading:0.###} -> R{i + 1} (ضرب صف في ثابت غير صفري) ");
                }

                

                // تصفير باقي الصفوف في نفس العمود
                //for (int k = i + 1; k < rows; k++)   دي جاوسيان بس
                //بيعدي على كل الصفوف في العامود الواحد وبعدين بيصفر العناصر غير الليدنج
                for (int k = 0; k < rows; k++)
                {
                    //بتجنب الصف بتاع الليدنج 
                    if (k != i)
                    {
                        //الفاكتور ده هيبقى فيه العناصر اللي فوق الليدنج واللي تحته عشان انا اهو مثبت العمود اللي هو الليدنج وبلف على الصفوف اللي في العمود ده ومتجنب اني اجي ناحيه الليدنج
                        double factor = matrix[k, leadingCol];
                        if (factor != 0)
                        {
                            SaveStep("جعل باقي العناصر بالعامود اللي به \n \tleading \n\t في الخطوه القادمه يساوي صفر");

                            //لو بقا مش بيساوي صفر هخليه صفر هضرف الصف اللي فيه الليدنج في الفاكتور واطرحه من الصف اللي فيه الفاكتور
                            for (int j = 0; j < cols; j++)
                                matrix[k, j] -= factor * matrix[i, j];

                            string sign = factor > 0 ? "-" : "+";
                            SaveStep($" R{k + 1} {sign} {Math.Abs(factor):0.###} R{i + 1} -> R{k + 1}  (ضرب صف في ثابت  غير صفري وجمعه على صف اخر) ");
                        }
                    }
                    //بكرر هنا على كل عامود لاجظ اني مثبت العامود في كل لفه كده هصفر اللي فوق الليدنج واللي تحته وبتجنب الليدنج
                }
                ZeroRows();

                SaveStep($@" الان خلصنا 
                         الصف رقم: {i + 1}
                         العامود رقم: {leadingCol + 1}
                        نشطبهم في خيالنا ونكمل شغل على باقي المصفوفه");
                leadingCol++;

            }

            GetSolutionType();
        }
        private void ZeroRows()
        {
            int i = 0;
            while (i < rows)
            {
                // تحقق من الصف الحالي
                bool currentIsZero = true;
                for (int j = 0; j < cols ; j++)
                    if (matrix[i, j] != 0)
                    { 
                        currentIsZero = false; 
                        break; 
                    }

                if (currentIsZero)
                {
                    // ابحث عن صف غير صفري تحته
                    int swapIndex = -1;
                    for (int k = i + 1; k < rows; k++)
                    {
                        bool nextIsZero = true;
                        for (int j = 0; j < cols; j++)
                            if (matrix[k, j] != 0)
                            {
                                nextIsZero = false;
                                break; 
                            }

                        if (!nextIsZero) { swapIndex = k; break; }
                    }

                    if (swapIndex != -1)
                    {
                        SaveStep(" في الخطوه القادمه جعل الصفوف الصفريه في اخر المصفوفه");
                        SwapRows(i, swapIndex);
                        SaveStep($"تحريك الصف  \nR{i + 1}\n الى اخر المصفوفه \nتبديل مع R{swapIndex + 1}\n");
                        
                        continue;
                    }
                }

                i++; // انتقل للصف التالي
            }
        }
       
        private void SwapRows(int r1, int r2)
        {
            for (int j = 0; j < cols; j++)
            {
                double temp = matrix[r1, j];
                matrix[r1, j] = matrix[r2, j];
                matrix[r2, j] = temp;
            }
            
        }

        public string GetStepsString()
        {
            StringBuilder sb = new StringBuilder();

            for (int s = 0; s < steps.Count; s++)
            {
                sb.AppendLine($"Step {s+1}:      {stepDescriptions[s]} \n");
                double[,] step = steps[s];

                for (int i = 0; i < rows; i++)
                {
                    
                    for (int j = 0; j < cols; j++)
                    {
                        sb.Append(step[i, j].ToString("0.##") + "  \t");
                    }
                    sb.AppendLine();
                }

                sb.AppendLine(new string('-', 50));
            }

            return sb.ToString();
        }

        public double[] GetResult()
        {
            double[] result = new double[rows];
            for (int i = 0; i < rows; i++)
                result[i] = matrix[i, cols - 1];
            return result;
        }

        private void GetSolutionType()
        {
            bool hasContradiction = false;
            //لو الرانك اقل عدد المتغيرات عدد لا نهائي من الحلول
            //لو الرانك بيساوي عدد المتغيرات حل وحيد
            int rank = 0;

            for (int i = 0; i < rows; i++)
            {
                bool allZeroVariables = true;

                for (int j = 0; j < cols - 1; j++)
                {
                    if (matrix[i, j] != 0)
                    {
                        allZeroVariables = false;
                        break;
                    }
                }

                if (allZeroVariables)
                {
                    // لو المعاملات كلها صفر لكن الناتج مش صفر هيبقى تناقض
                    if (matrix[i, cols - 1] != 0)
                        hasContradiction = true;
                }
                else
                {
                    rank++;
                }
            }

            if (hasContradiction)
                SolutionType = enSolutionType.eNoSolution;
            else if (rank < cols - 1)
                SolutionType = enSolutionType.eManySolutions;
            else
                SolutionType = enSolutionType.eOneSolution;
        }

        //private void GetSolutionType()
        //{
        //    int lastRow = rows -1 ;
        //    bool IslastRowVariableZero =true;

        //    for(int i = 0;i < cols-1;i++)
        //    {
        //        if(matrix[lastRow, i] != 0)
        //            IslastRowVariableZero = false;
        //    }
        //    bool IslastRowConstantZero = (matrix[rows-1,cols-1] == 0);



        //    if (IslastRowConstantZero && IslastRowVariableZero)
        //        SolutionType = enSolutionType.eManySolutions;
        //    else if (!IslastRowConstantZero && IslastRowVariableZero)
        //        SolutionType = enSolutionType.eNoSolution;
        //    else
        //    {
        //       //المتغيرات اكبر من المعادلات
        //        if (rows < cols - 1)
        //            SolutionType = enSolutionType.eManySolutions;
        //        else
        //            SolutionType = enSolutionType.eOneSolution;
        //    }





        //}
    }

}







