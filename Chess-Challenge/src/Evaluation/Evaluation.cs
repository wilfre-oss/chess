using ChessChallenge.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessChallenge.Evaluation
{
    
    public class Evaluation
    {
        public const int PawnValue = 100;
        public const int KnightValue = 300;
        public const int BishopValue = 320;
        public const int RookValue = 500;
        public const int QueenValue = 900;
        public static int Evaluate(Board board)
        {
            int whiteEval = CountMaterial(true, board);
            int blackEval = CountMaterial(false, board);

            int eval = (whiteEval - blackEval);


            return eval * (board.IsWhiteToMove ? 1 : -1);
        }

        static int CountMaterial(bool isWhite, Board board)
        {
            int material = 0;
            material += PawnValue * board.GetPieceList(PieceType.Pawn, isWhite).Count;
            material += KnightValue * board.GetPieceList(PieceType.Knight, isWhite).Count;
            material += BishopValue * board.GetPieceList(PieceType.Bishop, isWhite).Count;
            material += RookValue * board.GetPieceList(PieceType.Rook, isWhite).Count;
            material += QueenValue * board.GetPieceList(PieceType.Queen, isWhite).Count;
            return material;
        }
    }
}
