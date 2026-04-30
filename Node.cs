using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace puzzle_8game
{
    public class Node
    {
        public int[,] Board;
        public int Row, Col;
        public Node? Parent;

        #region AStar

        public int H;
        public int G;
        public int F => G + H;

        #endregion

        public Node(int [,] board, int row, int col, Node? parent)
        {
            Board = board;
            Row = row;
            Col = col;
            Parent = parent;
        }
    }
}