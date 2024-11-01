namespace ChessChallenge.Evaluation
{
    using ChessChallenge.API;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    public class MoveGenerator
    {
        static int MaterialValue(PieceType pieceType) => Material.MaterialValue(pieceType);
        public static IEnumerable<Move> GenerateMoves(Board board)
        {


            return board.GetLegalMoves().Select(move =>
            {
                int score = 0;

                if (move.IsCapture)
                {
                    score += 10 * MaterialValue(move.CapturePieceType) - MaterialValue(move.MovePieceType);
                }

                if (board.SquareIsAttackedByOpponent(move.TargetSquare))
                {
                    score -= MaterialValue(move.MovePieceType);
                }

                return (move, score);
            })
                .OrderByDescending(x => x.score)
                .Select(x => x.move);
        }

    }
}