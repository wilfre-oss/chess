using ChessChallenge.API;

namespace ChessChallenge.Evaluation
{
    public class Material
    {
        public const PieceType
            PAWN = PieceType.Pawn,
            KNIGHT = PieceType.Knight,
            BISHOP = PieceType.Bishop,
            ROOK = PieceType.Rook,
            QUEEN = PieceType.Queen;

        public static int MaterialValue(PieceType pieceType) => pieceType switch
        {
            PAWN => 100,
            KNIGHT => 300,
            BISHOP => 320,
            ROOK => 500,
            QUEEN => 900,
            _ => 0,
        };
        
    }
}
