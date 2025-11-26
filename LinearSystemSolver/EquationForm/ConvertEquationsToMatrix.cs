using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace مشروع_جبر_خطي
{
    static class ConvertEquationsToMatrix
    {
        public static double[,] Convert(string text)
        {
                                                                           //لو فيه أسطر فاضية، تجاهلها ما تحطهاش في النتيجة
            string[] lines = text.Split(new string[] { "\n", "\r", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            int row = lines.Length;
            int col = 1;

            for (int i = 0; i < lines[0].Length; i++)
            {
                if (char.IsLetter(lines[0][i]))
                    col++;
            }

            double[,] mtrx = new double[row, col];

            for (int i = 0; i < row; i++)
            {
                string line = lines[i].Replace(" ", "");
                string colector = "";
                int j = 0;
                bool afterEqual = false;

                for (int n = 0; n < line.Length; n++)
                {
                    char ch = line[n];

                    if (char.IsDigit(ch) || ch == '+' || ch == '-')
                        colector += ch;

                    else if (char.IsLetter(ch) && !afterEqual)
                    {
                        if (string.IsNullOrEmpty(colector) || colector == "+" || colector == "-")
                            colector += "1";

                        mtrx[i, j] = double.Parse(colector);
                        colector = "";
                        j++;
                    }

                    else if (ch == '=')
                    {
                        afterEqual = true;
                        colector = "";
                    }
                }

                // بعد ما نخلص السطر كله اللي هيبقى في متغير الكوليكتور هو اللي بعد علامه اليساوي
                if (afterEqual && !string.IsNullOrEmpty(colector))
                    mtrx[i, col - 1] = double.Parse(colector);
            }

            return mtrx;
        }

    }
}
