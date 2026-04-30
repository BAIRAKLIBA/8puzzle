using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace puzzle_8game
{
    public abstract class Solver
    {
        protected readonly int[,] directions =
        {
            {-1, 0}, // UP
            { 1, 0}, // DOWN
            { 0,-1}, // LEFT
            { 0, 1}  // RIGHT
        };
        
        protected int[,] goal =
        {
            {1,2,3},
            {4,5,6},
            {7,8,0}
        };

        public abstract List<Node>? Solve(Node start);
        protected abstract List<Node> GetNeighbors(Node current);
        protected abstract void Swap(int[,] board, int r1, int c1, int r2, int c2);
        protected abstract bool IsValid(int r, int c);
        protected abstract string ToKey(int[,] board);
        protected abstract bool IsGoal(int[,] board);
        protected abstract List<Node> ReconstructPath(Node? node);
        public abstract Node? CreateStart(int[,] board);
    }
}