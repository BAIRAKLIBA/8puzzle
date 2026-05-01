using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Threading.Tasks;

namespace puzzle_8game
{
    // BFS იმპლემენტაცია
    class BFS : Solver
    {
        // რიგი (FIFO) — განსაზღვრავს BFS-ის მუშაობას
        private Queue<Node> queue = new Queue<Node>();

        // უკვე ნახული მდგომარეობები
        private HashSet<string> visited = new HashSet<string>();

        public override List<Node>? Solve(Node start)
        {
            // საწყისი მდგომარეობის დამატება რიგში
            queue.Enqueue(start);

            // მისთვის უნიკალური key-ის დამატება visited-ში
            visited.Add(ToKey(start.Board));

            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                if (IsGoal(current.Board))
                    return ReconstructPath(current);

                // ვიღებთ ყველა შესაძლო მეზობელ მდგომარეობას
                foreach (var neighbor in GetNeighbors(current))
                {
                    // ვქმნით უნიკალურ key-ს
                    string key = ToKey(neighbor.Board);

                    if (!visited.Contains(key))
                    {
                        visited.Add(key);      // ვინიშნავთ როგორც ნანახს
                        queue.Enqueue(neighbor); // ვამატებთ რიგში შემდგომი დამუშავებისთვის
                    }
                }
            }

            // თუ ვერ მოიძებნა ამოხსნა
            return null;
        }

        // აბრუნებს ყველა შესაძლო მეზობელ მდგომარეობას
        protected override List<Node> GetNeighbors(Node current)
        {
            List<Node> neighbors = new List<Node>();

            // ვცდილობთ ყველა მიმართულებას (4)
            for (int i = 0; i < 4; i++)
            {
                int newR = current.Row + directions[i, 0];
                int newC = current.Col + directions[i, 1];

                // თუ მოძრაობა ვალიდურია (არ გადის საზღვრებს გარეთ)
                if (IsValid(newR, newC))
                {
                    // ვქმნით დაფის ასლს (ძალიან მნიშვნელოვანია!)
                    int[,] newBoard = (int[,])current.Board.Clone();

                    // ვცვლით ცარიელ უჯრას (swap)
                    Swap(newBoard, current.Row, current.Col, newR, newC);

                    // ვქმნით ახალ Node-ს
                    neighbors.Add(new Node(newBoard, newR, newC, current));
                }
            }

            return neighbors;
        }

        // ორ უჯრას შორის მნიშვნელობების გაცვლა
        protected override void Swap(int[,] board, int r1, int c1, int r2, int c2)
        {
            int temp = board[r1, c1];
            board[r1, c1] = board[r2, c2];
            board[r2, c2] = temp;
        }

        // ამოწმებს, არის თუ არა ინდექსი დაფის საზღვრებში
        protected override bool IsValid(int r, int c)
        {
            return r >= 0 && r < 3 && c >= 0 && c < 3;
        }

        // დაფას გარდაქმნის სტრიქონად (უნიკალური იდენტიფიკატორი)
        protected override string ToKey(int[,] board)
        {
            string key = "";

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    key += board[i, j] + ",";

            return key;
        }

        // ამოწმებს, არის თუ არა დაფა goal მდგომარეობა
        protected override bool IsGoal(int[,] board)
        {
            for(int i = 0; i < 3; i++)
                for(int j = 0; j < 3; j++)
                    if(board[i, j] != goal[i, j])
                        return false;
            
            return true;
        }

        // აგებს გზას goal-დან start-მდე Parent კავშირების გამოყენებით
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

        // ქმნის საწყის Node-ს დაფიდან
        public override Node? CreateStart(int[,] board)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (board[i, j] == 0)
                        return new Node(board, i, j, null);

            return null; // თუ ვერ ვიპოვეთ (არასწორი დაფა)
        }
    }
}