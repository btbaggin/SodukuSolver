using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodukuSolver
{
    public class AlgorithmX
    {
        private DancingLinks _links;
        private List<DancingLinks.Node> _solutionList;

        public delegate void OnSolutionFound(int[,] pSolution);
        public event OnSolutionFound SolutionFound;

#region "AlgorithmX Constructor"
    public AlgorithmX(int[,] pMap)
    {
        _links = new DancingLinks(pMap);
        _solutionList = new List<DancingLinks.Node>();
    }
#endregion

    public void Solve()
    {
        Search(ChooseHeaderNode());
    }
    

#region "Private Functions"
    private void Search(DancingLinks.HeaderNode pHeader)
    {
        //If our matrix is empty we have our solution
        if(pHeader == _links.Root)
        {
            if (SolutionFound != null)
            {
                SolutionFound(BuildSolution());
            }
        }
        else
        {
            //Cover this header
            pHeader.Cover();

            //Cycle through the column
            DancingLinks.Node column = pHeader.Down;
            while(column != pHeader)
            {
                //Add the row to our solution
                _solutionList.Add(column);

                //Add the row to our solution and cover the columns in this row
                DancingLinks.Node row = column.Right;
                while(row != column)
                {
                    row.Header.Cover();
                    _solutionList.Add(row);
                    row = row.Right;
                }

                //Recurse with our reduced matrix
                Search(ChooseHeaderNode());
                _solutionList.Remove(column);

                //Uncover the columns in the row to return our matrix to normal so recursion doesn't break
                row = column.Left;
                while (row != column)
                {
                    row.Header.Uncover();
                    _solutionList.Remove(row);
                    row = row.Left;
                }
                column = column.Down;
            }
            pHeader.Uncover();
        }
    }

    /// <summary>
    /// Chooses the header node with the least amount of nodes connected to it
    /// </summary>
    private DancingLinks.HeaderNode ChooseHeaderNode() 
    {
        DancingLinks.HeaderNode node = (DancingLinks.HeaderNode)_links.Root.Right;
        DancingLinks.HeaderNode retval = node;

        while(node != _links.Root)
        {
            if(node.Count < retval.Count) { retval = node; }
            node = (DancingLinks.HeaderNode)node.Right;
        }

        return retval;
    }

    private struct Vector
    {
        public int X, Y;
        public Vector(int pX, int pY)
        {
            X = pX; Y = pY;
        }
    }

    private int[,] BuildSolution()
    {
        var size = 9 * 9;
        var solution = new int[9, 9];
        Dictionary<int, Vector> d = new Dictionary<int, Vector>();
        var nodes = _solutionList.OrderBy(pN => pN.X);
        foreach(DancingLinks.Node n in nodes)
        {
            if (n.X < size)
            {
                d.Add(n.Y, new Vector(n.X / 9, n.X % 9));
            }
            else if(n.X < size * 2)
            {
                if (d.ContainsKey(n.Y))
                {
                    var p = d[n.Y];
                    solution[p.X, p.Y] = n.X % 9 + 1;
                }
            }
        }

        return solution;
    }
#endregion
    }
}
