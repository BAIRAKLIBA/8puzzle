namespace puzzle_8game;
class Program
{

    static void PrintBoard(int[,] board, int step = 1)
    {
        
        for(int i = 0; i < board.GetLength(0); i++)
        {
            //Console.WriteLine($"=======Step #{step++} =====");
            for(int j = 0; j < board.GetLength(1); j++)
                Console.Write($"{board[i, j]} ");
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    public static Solver? Choice(int choice)
    {
        if(choice == 1)
        {
            Console.WriteLine("=== BFS ===\n");
            return new BFS();
        }
        else if(choice == 2){
            Console.WriteLine("=== A* ===\n");
            return new AStar();
        }
        return null;
    }

    
    static void Main()
    {
        Console.WriteLine("8 puzzle game.\n");
        while (true)
        {
            int[,] board = PuzzleGenerator.GenerateRandomBoard(40);

            PrintBoard(board);

            Console.WriteLine($"Solvable: {PuzzleGenerator.IsSolvable(board)}\n");
            
            
            Console.Write("1. BFS\n2. A*\n3. Exit\nChoice: ");
            int choice = Convert.ToInt16(Console.ReadLine());
            Solver? solver = Choice(choice);
            
            if(solver == null)
            {
                Console.WriteLine("Exiting the program...\n");
                break;
            }
                


            Node? start = solver?.CreateStart(board);

            if(start == null)
            {
                Console.WriteLine("Invalid start state");
                return;
            }

            var sw = System.Diagnostics.Stopwatch.StartNew();

            var solution = solver?.Solve(start);

            sw.Stop();

            
            
            if(solution != null)
            {
                foreach(Node step in solution)
                    PrintBoard(step.Board);
                
                Console.WriteLine($"Steps: {solution.Count - 1}");
                Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms\n");
            }
            else 
                Console.WriteLine("No solution\n");

            
            string? input;

            do
            {
                Console.Write("Continue? (y/n): ");
                input = Console.ReadLine()?.Trim().ToLower();
            }
            while(input != "y" && input != "yes" && input != "n" && input != "no");

            if(input == "n" || input == "no")
            {
                Console.WriteLine("Exiting the program...\n");
                break;
            }

        }
    }
}
