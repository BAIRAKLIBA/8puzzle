using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Threading.Tasks;

namespace puzzle_8game
{
    public class PuzzleGenerator
    {
        private static readonly Random rand = new Random();
        private static readonly int[,] directions =
        {
            {-1, 0},
            {1, 0},
            {0, -1},
            {0, 1}
        };

        public static int[,] GenerateRandomBoard(int steps = 30)
        {
            int[,] board =
            {
                {1, 2, 3},
                {4, 5, 6},
                {7, 8, 0}
            };

            int row = 2, col = 2;

            (int r, int c)? previous = null;

            for(int i = 0; i < steps; i++)
            {
                var validMoves = new List<(int r, int c)>();

                for(int d = 0; d < 4; d++)
                {
                    int newR = row + directions[d, 0];
                    int newC = col + directions[d, 1];

                    if(newR >= 0 && newR < 3 && newC >= 0 && newC < 3)
                    {
                        if(previous != null && newR == previous.Value.r && newC == previous.Value.c)
                            continue;
                        
                        validMoves.Add((newR, newC));
                    }
                }

                var move = validMoves[rand.Next(validMoves.Count)];
                

                previous = (row, col);

                (board[row, col], board[move.r, move.c]) = (board[move.r, move.c],  board[row, col]);

                row = move.r;
                col = move.c;
            }

            return board;
        }

        public static bool IsSolvable(int[,] board)
        {
            int[] arr = new int[9];
            int k = 0;

            for(int i = 0; i < 3; i++)
                for(int j = 0; j < 3; j++)
                    arr[k++] = board[i, j];
            
            int inversions = 0;

            for(int i = 0; i < 9; i++)
                for(int j = i + 1; j < 9; j++)
                    if(arr[i] != 0 && arr[j] != 0 && arr[i] > arr[j])
                        inversions++;
            
            return inversions % 2 == 0;
        }
    }
}