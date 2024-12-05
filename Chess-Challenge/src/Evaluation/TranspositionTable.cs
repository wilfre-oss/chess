using ChessChallenge.API;
using System.Collections.Generic;

namespace ChessChallenge.Evaluation
{
    public class TTEntry
    {
        public byte Depth { get; set; } // Depth of the search at which this entry was created
        public int Eval { get; set; } // The evaluation score of the position
        public Move BestMove { get; set; } // The best move found at this position
        public FlagType Flag { get; set; } // Node type (exact, lower bound, upper bound)

        public TTEntry(int depth, int eval, Move move, FlagType flag) 
        { 
            Depth = (byte)depth;
            Eval = eval;
            BestMove = move;
            Flag = flag;
        }
    }

    public enum FlagType
    {
        Exact,      // Exact score (PV node)
        LowerBound, // Lower bound score (cut node)
        UpperBound  // Upper bound score (all node)
    }
    public class TranspositionTable
    {
        private readonly Dictionary<ulong,  TTEntry> table;
        private readonly int capacity;

        public TranspositionTable(int capacity)
        {
            this.capacity = capacity * (1024 * 1024);
            this.table = new Dictionary<ulong, TTEntry>(capacity);
        }

        public void Store(ulong key, int depth, int eval, Move move, FlagType flag)
        {
            table[key] = new TTEntry(depth, eval, move, flag);
        }

        public bool TryGet(ulong key, out TTEntry entry) 
        {
            return table.TryGetValue(key, out entry);
        }
    }
}
