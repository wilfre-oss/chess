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

        public MoveGenerator()
        {
            History = new int[2,64,64];
        }
        public List<Move> GenerateMoves(Board board, bool quietMoves = false)
        {
            int colorIndex = board.IsWhiteToMove ? 0 : 1;
            Move[] legalMoves = board.GetLegalMoves(quietMoves);
            var movesWithScores = new List<(Move move, int score)>(legalMoves.Length);

            foreach (Move move in legalMoves)
            {
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
            var sortedMoves = new List<Move>(movesWithScores.Count);
            foreach (var (move, _) in movesWithScores)
            {
                sortedMoves.Add(move);
            }

            return sortedMoves;
        }

        

    }
}