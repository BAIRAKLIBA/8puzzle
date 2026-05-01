using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;

namespace puzzle_8game
{
    public class PuzzleGenerator
    {
        // ერთჯერადი Random ობიექტი, ახალი ბიჯების შესაქმნელად
        private static readonly Random rand = new Random();

        // შესაძლო მიმართულებები 
        private static readonly int[,] directions =
        {
            {-1, 0}, // UP
            {1, 0},  // DOWN
            {0, -1}, // LEFT
            {0, 1}   // RIGHT
        };

        // შემთხვევითი (მაგრამ ამოხსნადი) დაფის გენერაცია

        public static int[,] GenerateRandomBoard(int steps = 30)
        {
            // ვიწყებთ goal მდგომარეობიდან
            int[,] board =
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 0} // 0 = ცარიელი უჯრა
            };

            // ცარიელი უჯრის საწყისი პოზიცია
            int row = 2, col = 2;

            // წინა პოზიცია, იმისთვის რომ უკან არ დავბრუნდეთ
            (int r, int c)? previous = null;

            // ვაკეთებთ steps რაოდენობის შემთხვევით მოძრაობას
            for(int i = 0; i < steps; i++)
            {
                // ყველა შესაძლო ვალიდური მოძრაობის სია
                var validMoves = new List<(int r, int c)>();

                // ვამოწმებთ 4 მიმართულებას
                for(int d = 0; d < 4; d++)
                {
                    int newR = row + directions[d, 0];
                    int newC = col + directions[d, 1];

                    // თუ ახალი პოზიცია დაფის საზღვრებშია
                    if(newR >= 0 && newR < 3 && newC >= 0 && newC < 3)
                    {
                        // თუ ეს არის წინა პოზიცია → გამოვტოვებთ (არ დავაბრუნოთ უკან)
                        if(previous != null && newR == previous.Value.r && newC == previous.Value.c)
                            continue;
                        
                        // ვამატებთ შესაძლო მოძრაობებში
                        validMoves.Add((newR, newC));
                    }
                }

                // ვირჩევთ შემთხვევით ერთ მოძრაობას
                var move = validMoves[rand.Next(validMoves.Count)];
                
                // ვინახავთ წინა პოზიციას (მომდევნო iteration-სთვის)
                previous = (row, col);

                // ვცვლით ცარიელ უჯრას არჩეულ უჯრასთან (swap)
                (board[row, col], board[move.r, move.c]) =
                    (board[move.r, move.c], board[row, col]);

                // ვაახლებთ ცარიელი უჯრის პოზიციას
                row = move.r;
                col = move.c;
            }

            // საბოლოოდ ვაბრუნებთ მიღებულ დაფას
            return board;
        }


        // ამოწმებს ამოხსნადია თუ არა დაფა
        public static bool IsSolvable(int[,] board)
        {
            // 2D დაფას ვაქცევთ 1D მასივად
            int[] arr = new int[9];
            int k = 0;

            for(int i = 0; i < 3; i++)
                for(int j = 0; j < 3; j++)
                    arr[k++] = board[i, j];
            
            int inversions = 0;

            // ვითვლით ინვერსიებს
            for(int i = 0; i < 9; i++)
                for(int j = i + 1; j < 9; j++)
                    if(arr[i] != 0 && arr[j] != 0 && arr[i] > arr[j])
                        inversions++;
            
            // თუ ინვერსიების რიცხვი ლუწია - true, ანუ, ამოხსნადია
            // წინააღმდეგ შემთხვევაში - ამოუხსნელი
            return inversions % 2 == 0;
        }
    }
}