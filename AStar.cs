using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace puzzle_8game
{
    public class AStar : Solver
    {
        // private readonly int[,] goal =
        // {
        //     {1, 2, 3},
        //     {4, 5, 6},
        //     {7, 8, 0}
        // };

        // private readonly int[,] directions =
        // {
        //     {-1, 0},
        //     {1, 0},
        //     {0, -1},
        //     {0, 1}
        // };

        public override List<Node>? Solve(Node start)
        {
            var open = new PriorityQueue<Node, int>();
            var visited  = new HashSet<string>();

            start.G = 0;
            start.H = Manhattan(start.Board);

            open.Enqueue(start, start.F);

            while(open.Count > 0)
            {
                var current = open.Dequeue();

                string key = ToKey(current.Board);
                if(visited.Contains(key)) continue;
                
                visited.Add(key);

                if(IsGoal(current.Board))
                    return ReconstructPath(current);
                
                foreach(var neighbor in GetNeighbors(current))
                {
                    neighbor.G = current.G + 1;
                    neighbor.H = Manhattan(neighbor.Board);

                    open.Enqueue(neighbor, neighbor.F);
                }
                
            }

            return null;
        }

        private int Manhattan(int[,] board)
        {
            int distance = 0;

            for(int i = 0; i < 3; i++)
            {
                for(int j = 0; j < 3; j++)
                {
                    int val = board[i, j];
                    if(val == 0) continue;

                    int targetRow = (val - 1) / 3;
                    int targetCol = (val - 1) % 3;

                    distance = Math.Abs(i - targetRow) + Math.Abs(j - targetCol);
                }
            }

            return distance;
        }


        protected override List<Node> GetNeighbors(Node current)
        {
            var neighbors = new List<Node>();

            for(int i = 0; i < 4; i++)
            {
                int newR = current.Row + directions[i, 0];
                int newC = current.Col + directions[i, 1];

                if(IsValid(newR, newC))
                {
                    int[,] newBoard = (int[,])current.Board.Clone();

                    Swap(newBoard, current.Row, current.Col, newR, newC);

                    neighbors.Add(new Node(newBoard, newR, newC, current));
                }
            }

            return neighbors;
        }

        protected override void Swap(int[,] board, int r1, int c1, int r2, int c2)
        {
            (board[r1, c1], board[r2, c2]) = (board[r2, c2], board[r1, c1]);
        }


        protected override bool IsValid(int r, int c)
        {
            return r >= 0 && r < 3 && c >= 0 && c < 3;
        }

        protected override string ToKey(int[,] board)
        {
            string key = "";

            for(int i = 0; i < 3; i++)
                for(int j = 0; j < 3; j++)
                    key += board[i, j] + ", ";
            return key;
        }

        protected override bool IsGoal(int[,] board)
        {
            for(int i = 0; i < 3; i++)
                for(int j = 0; j < 3; j++)
                    if(board[i, j] != goal[i, j])
                        return false;
            
            return true;
        }

        protected override List<Node> ReconstructPath(Node? node)
        {
            var path = new List<Node>();

            while(node != null)
            {
                path.Add(node);
                node = node.Parent;
            }

            path.Reverse();
            return path;
        }
        
        public override Node? CreateStart(int[,] board)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == 0)
                        return new Node(board, i, j, null);

            return null;
        }
    }
}