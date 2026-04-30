using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;


namespace puzzle_8game
{
    class BFS : Solver
    {

        private Queue<Node> queue = new Queue<Node>();
        private HashSet<string> visited = new HashSet<string>();


        public override List<Node>? Solve(Node start)
        {
            
            queue.Enqueue(start);
            visited.Add(ToKey(start.Board));

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                if (IsGoal(current.Board))
                    return ReconstructPath(current);

                foreach (var neighbor in GetNeighbors(current))
                {
                    string key = ToKey(neighbor.Board);

                    if (!visited.Contains(key))
                    {
                        visited.Add(key);
                        queue.Enqueue(neighbor);
                    }
                }
            }

            return null;
        }

        
        
        protected override List<Node> GetNeighbors(Node current)
        {
            List<Node> neighbors = new List<Node>();

            for (int i = 0; i < 4; i++)
            {
                int newR = current.Row + directions[i, 0];
                int newC = current.Col + directions[i, 1];

                if (IsValid(newR, newC))
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
            int temp = board[r1, c1];
            board[r1, c1] = board[r2, c2];
            board[r2, c2] = temp;
        }


        protected override bool IsValid(int r, int c)
        {
            return r >= 0 && r < 3 && c >= 0 && c < 3;
        }


        protected override string ToKey(int[,] board)
        {
            string key = "";

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    key += board[i, j] + ",";

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
            List<Node> path = new List<Node>();

            while (node != null)
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