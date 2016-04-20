using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodukuSolver
{
    class SodukuSolver
    {
        private int _zeroCount = 0;
        private AlgorithmX _algorithmX;

        #region "Properties"
        public int Size { get; set; }

        public int BoardSize
        {
            get { return Size * Size; }
        }
        #endregion

        #region "Constructor"
        public SodukuSolver(string pSoduku)
        {
            Size = (int)Math.Sqrt(pSoduku.Length);
            //Count zeros
            _zeroCount = pSoduku.Count(pC => pC == '0' || pC == ' ');

            //Initialize map
            int[,] map = new int[(BoardSize * 4), (_zeroCount * Size) + (BoardSize - _zeroCount)];

            //Populate map
            int row = 0;
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    int num = (int)Char.GetNumericValue(pSoduku[x + (y * Size)]);
                    if (num != 0)
                    {
                        CreateMapping(x, y, num - 1, ref row, ref map);
                    }
                    else
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            CreateMapping(x, y, i, ref row, ref map);
                        }
                    }
                }
            }

            _algorithmX = new AlgorithmX(map);
        }

        public SodukuSolver(int[,] pSoduku)
        {
            Size = pSoduku.GetLength(0);
            //Count zeros
            _zeroCount = 0;
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    if (pSoduku[x, y] == 0) { _zeroCount++; }
                }
            }

            //Initialize map
            int[,] map = new int[(BoardSize * 4), (_zeroCount * Size) + (BoardSize - _zeroCount)];

            //Populate map
            int row = 0;
            for (int x = 0; x < Size; x++)
            {
                for (int y = 0; y < Size; y++)
                {
                    int num = pSoduku[x, y];
                    if (num != 0)
                    {
                        CreateMapping(x, y, num - 1, ref row, ref map);
                    }
                    else
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            CreateMapping(x, y, i, ref row, ref map);
                        }
                    }
                }
            }

            _algorithmX = new AlgorithmX(map);
        }
        #endregion

        public void Solve(Action<int[,]> pResults)
        {
            _algorithmX.SolutionFound += (pS) => pResults.Invoke(pS);
            _algorithmX.Solve();
        }

        #region "Private functions"
        private static void CreateMapping(int pX, int pY, int pNum, ref int pRow, ref int[,] pMap)
        {
            //Row/Column
            pMap[pX + (pY * 9), pRow] = 1;

            //Row/Number
            pMap[(9 * 9) + ((pY * 9) + pNum), pRow] = 1;

            //Column/Number
            pMap[(9 * 9 * 2) + ((pX * 9) + pNum), pRow] = 1;

            //Box/Number
            pMap[(9 * 9 * 3) + (BoxNum(pX, pY) * 9) + pNum, pRow] = 1;
            pRow++;
        }

        private static int BoxNum(int pX, int pY)
        {
            double d = pX / (9 / 3);
            double d2 = pY / (9 / 3);
            return (int)Math.Floor(d + Math.Floor(d2) * 3);
        }
        #endregion
    }
}
