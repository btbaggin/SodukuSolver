using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodukuSolver
{
    class DancingLinks
    {
        public HeaderNode Root;

        private int _width;
        private int _height;

#region "Node Class"
        public class Node
        {
            public Node Left;         //* Node to the left
            public Node Right;        //* Node to the right
            public Node Up;           //* Node above
            public Node Down;         //* Node below
            public HeaderNode Header; //* Header node for the column this node is in
            public int X { get; private set; }             //* Unique X value indentifing the location of each node
            public int Y { get; private set; }             //* Unique Y value indentifing the location of each node

            /// <summary>
            /// Node constructor
            /// </summary>
            /// <param name="pX">Unique X value to indentify the node</param>
            /// <param name="pY">Unique Y value to indentify the node</param>
            /// <param name="pHeader">The header node for the column this node is contained in</param>
            public Node(int pX, int pY , HeaderNode pHeader)
            {
                Left = this;
                Right = this;
                Up = this;
                Down = this;
                X = pX;
                Y = pY;
                Header = pHeader;
            }

#region "Public Functions"
            /// <summary>
            /// Removes this node from the row
            /// </summary>
            public void UnlinkRow()
            {
                Right.Left = Left;
                Left.Right = Right;
            }

            /// <summary>
            /// Adds this node to the row
            /// </summary>
            public void LinkRow()
            {
                Right.Left = this;
                Left.Right = this;
            }

            /// <summary>
            /// Removes this node from the column
            /// </summary>
            public void UnlinkColumn()
            {
                Up.Down = Down;
                Down.Up = Up;
                Header.Count--;
            }

            /// <summary>
            /// Adds this node to the column
            /// </summary>
            public void LinkColumn()
            {
                Up.Down = this;
                Down.Up = this;
                Header.Count++;
            }

            public override bool Equals(object obj)
            {
                Node n = obj as Node;
                if (n != null)
                {
                    return X == n.X && Y == n.Y;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return X.GetHashCode() + Y.GetHashCode();
            }

            public override string ToString()
            {
                return X + ", " + Y;
            }
#endregion
        }
#endregion

#region "HeaderNode Class"
        public class HeaderNode : Node
        {
            public int Count;

            /// <summary>
            /// Header node construtor
            /// </summary>
            /// <param name="pX">Unique X value indentifing each node</param>
            /// <param name="pY">Unique Y value indentyfing each node</param>
            public HeaderNode(int pX , int pY ) : base(pX, pY, null)
            {
                Header = this;
                Count = 0;
            }

            /// <summary>
            /// Covers the header node.  This removes the header node from the header node list.
            /// It also removes any row with a node present in this header node's column.
            /// </summary>
            /// <remarks></remarks>
            public void Cover()
            {
                UnlinkRow();

                Node column = Down;
                while(column != this)
                {
                    Node row = column.Right;
                    while(row != column)
                    {
                        row.UnlinkColumn();
                        row = row.Right;
                    }
                                
                    column = column.Down;
                }
            }

            /// <summary>
            /// Uncovers the header node.  Adds the header node back to the header node list.
            /// It also adds any rows with a node present in this header node's column.
            /// </summary>
            public void Uncover()
            {
                Node column = Up;

                while(column != this)
                {
                    Node row = column.Left;
                    while(row != column)
                    {
                        row.LinkColumn();
                        row = row.Left;
                    }
                                
                    column = column.Up;
                }

                LinkRow();
            }
        }
#endregion

#region "Constructor"
    /// <summary>
    /// DancingLinks construtor
    /// </summary>
    /// <param name="pMap">Binary map to intialize the links.  Any spot with non-zero in the map will be initialized with a node.</param>
    public DancingLinks(int[,] pMap)
    {
        _width = pMap.GetLength(0);
        _height = pMap.GetLength(1);
        //Initialize our matrix with an extra column for the header nodes
        Node[,] matrix = new Node[_width, _height + 1];

        //Cycle through the matrix
        for (int X = 0; X < _width; X++)
        {
            HeaderNode header = new HeaderNode(X, 0);
            matrix[X, 0] = header;

            for (int Y = 0; Y < _height; Y++)
            {
                //If the map has a 1
                if(pMap[X, Y] != 0)
                {
                    //Initialize a node in the position
                    matrix[X, Y + 1] = new Node(X, Y, header);

                    //Mark the count of nodes for this column
                    header.Count++;
                }
            }
        }

        //Create the links between each node
        LinkNodes(matrix);

        //Insert noot node
        Root = new HeaderNode(-1, -1);

        //Link root node
        Root.Left = matrix[0, 0].Left;
        Root.Right = matrix[0, 0];
        Root.Left.Right = Root;
        matrix[0, 0].Left = Root;
    }
#endregion

    /// <summary>
    /// Establishes the 4-way doubly linked cirular links between all nodes
    /// </summary>
    /// <param name="pMatrix">Matrix of nodes to link to each other</param>
    private void LinkNodes(Node[,] pMatrix)
    {
        //Link nodes
        for(int X = 0; X < _width; X++)
        {
            for(int Y = 0; Y < _height; Y++)
            {
                if(pMatrix[X, Y] != null)
                {
                    int i, j;

                    //Left nodes
                    i = X;
                    j = Y;
                    do
                    {
                        i = (i - 1 < 0) ? _width - 1 : i - 1;
                    } while(pMatrix[i, j] == null);
                    pMatrix[X, Y].Left = pMatrix[i, j];

                    //Right nodes
                    i = X;
                    j = Y;
                    do
                    {
                        i = (i + 1 >= _width) ? 0 : i + 1;
                    } while(pMatrix[i,j] == null);
                    pMatrix[X, Y].Right = pMatrix[i, j];

                    //Up nodes
                    i = X;
                    j = Y;
                    do
                    {
                        j = (j - 1 < 0) ? _height - 1 : j - 1;
                    } while(pMatrix[i,j] == null);
                    pMatrix[X, Y].Up = pMatrix[i, j];

                    //Down nodes
                    i = X;
                    j = Y;
                    do
                    {
                        j = (j + 1 >= _height) ? 0 : j + 1;
                    } while(pMatrix[i,j] == null);
                    pMatrix[X, Y].Down = pMatrix[i, j];
                  }            
                }
            }
        }
    }
}
