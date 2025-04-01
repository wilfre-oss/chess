namespace ChessChallenge.Evaluation
{
    using ChessChallenge.API;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    public class MoveGenerator
    {
        static int MaterialValue(PieceType pieceType) => Material.MaterialValue(pieceType);

        public int[,,] History;
        TranspositionTable TranspositionTable;
        public MoveGenerator(TranspositionTable tt)
        {
            History = new int[2,64,64];
            TranspositionTable = tt;
        }
        public int GenerateMoves(Board board, Span<Move> movesBuffer, bool capturesOnly = false)
        {
            int colorIndex = board.IsWhiteToMove ? 0 : 1;
            Move[] legalMoves = board.GetLegalMoves(capturesOnly);
            var movesWithScores = new List<(Move move, int score)>(legalMoves.Length);

            Move pvMove = Move.NullMove;
            if (TranspositionTable.TryGet(board.ZobristKey, out TTEntry entry) &&
                entry.Flag == FlagType.Exact)
            {
                pvMove = entry.BestMove; 
            }

            foreach (Move move in legalMoves)
            {
                
                if (move ==  pvMove)
                {
                    movesWithScores.Add((move, int.MaxValue));
                    continue;
                }

                int score = History[colorIndex, move.StartSquare.Index, move.TargetSquare.Index];

                if (move.IsCapture)
                {
                    // Adjust score based on capture value
                    score += 10 * MaterialValue(move.CapturePieceType) - MaterialValue(move.MovePieceType);
                }

                // Penalize moves that place the piece on an attacked square
                if (score > 0 && board.SquareIsAttackedByOpponent(move.TargetSquare))
                {
                    score -= 50;
                }

                movesWithScores.Add((move, score));
            }

            // Sort moves based on score (descending)
            movesWithScores.Sort((a, b) => b.score.CompareTo(a.score));

            // Return moves after sorting
            for (int i = 0; i < legalMoves.Length; i++)
            {
                movesBuffer[i] = movesWithScores[i].move;
            }
            return legalMoves.Length;
        }

        

    }
}