using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodukuSolver
{
    class SodukuSolverTest
    {
        static int Size = 0;
        public static void Main()
        {
            //Some still not working...

            //string s = "3 72    6" +
            //           " 85 6    " +
            //           "       45" +
            //           "     18  " +
            //           "9       7" +
            //           "  34     " +
            //           "84       " +
            //           "    9 27 " +
            //           "5    36 8";
            //string s2 = "000901732073500040002040860904000028000030000530000407059070600040005290368209000";          
            string s ="005902000279008040800300002300091057020000080580420006100009003090200571000605800";

            SodukuSolver ss = new SodukuSolver(s);

            Size = ss.Size;
            ss.Solve(Print);
            Console.WriteLine("Unable to find solution");
            return;       
        }

        private static void Print(int[,] pSolution)
        {
            Console.WriteLine("-------------------");
            for (int i = 0; i < Size; i++)
            {
                Console.Write("| ");
                for (int j = 0; j < Size; j++)
                {
                    Console.Write(pSolution[i, j].ToString());
                    if (j == Size / 3 - 1 || j == (Size / 3) * 2 - 1)
                    {
                        Console.Write(" | ");
                    }
                }
                Console.WriteLine(" |");
                if (i == Size / 3 - 1 || i == (Size / 3) * 2 - 1)
                {
                    Console.WriteLine("-------------------");
                }
            }
            Console.WriteLine("-------------------");
        }
    }
}
