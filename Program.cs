namespace puzzle_8game;
class Program
{
    // ბეჭდავს დაფას კონსოლში
    static void PrintBoard(int[,] board, int step = 1)
    {
        for(int i = 0; i < board.GetLength(0); i++)
        {
            for(int j = 0; j < board.GetLength(1); j++)
                Console.Write($"{board[i, j]} ");
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    // მომხმარებლის არჩევანის მიხედვით აბრუნებს შესაბამის solver-ს
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
        return null; // 3 ან სხვა მნიშვნელობა → exit
    }

    static void Main()
    {
        Console.WriteLine("8 puzzle game.\n");

        // მთავარი ციკლი — პროგრამა მუშაობს სანამ მომხმარებელი არ გამოვა
        while (true)
        {
            // გენერირდება შემთხვევითი დაფა, რომელიც იქნება ამოხსნადი
            int[,] board = PuzzleGenerator.GenerateRandomBoard(40);

            PrintBoard(board);

            // დამატებითი შემოწმება 
            Console.WriteLine($"Solvable: {PuzzleGenerator.IsSolvable(board)}\n");
            
            Console.Write("1. BFS\n2. A*\n3. Exit\nChoice: ");
            int choice = Convert.ToInt16(Console.ReadLine());

            Solver? solver = Choice(choice);
            
            if(solver == null)
            {
                Console.WriteLine("Exiting the program...\n");
                break;
            }

            // საწყისი მდგომარეობის შექმნა (იპოვის 0-ის პოზიციას)
            Node? start = solver.CreateStart(board);

            if(start == null)
            {
                Console.WriteLine("Invalid start state");
                return;
            }

            // დროის გაზომვა
            var sw = System.Diagnostics.Stopwatch.StartNew();

            // ძირითადი ნაწილი
            var solution = solver.Solve(start);

            sw.Stop();

            // თუ ამოხსნა მოიძებნა ბეჭდავს ნაბიჯებს
            if(solution != null)
            {
                foreach(Node step in solution)
                    PrintBoard(step.Board);
                
                Console.WriteLine($"Steps: {solution.Count - 1}");
                Console.WriteLine($"Time: {sw.ElapsedMilliseconds} ms\n");
            }
            else 
                Console.WriteLine("No solution\n");

            // გაგრძელების კითხვა
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