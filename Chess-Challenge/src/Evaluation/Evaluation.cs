using ChessChallenge.API;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.CodeAnalysis;
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

        static int PawnMaterial(Board board, bool white) => PawnValue * board.GetPieceList(PieceType.Pawn, white).Count;  
        public static int Evaluate(Board board)
        {
            int p = (board.IsWhiteToMove ? 1 : -1);

            //Material evaulation
            int whiteEval = CountMaterial(true, board);
            int blackEval = CountMaterial(false, board);

            //Mobillity evaluation
            /*
            if (p == 1)
            {
            whiteEval += MobilityScore(board);
            board.ForceSkipTurn();
            blackEval += MobilityScore(board);
            board.UndoSkipTurn();
            }
            else
            {
                blackEval += MobilityScore(board);
                board.ForceSkipTurn();
                whiteEval += MobilityScore(board);
                board.UndoSkipTurn();s
            }
            */
            return (whiteEval - blackEval) * p;
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

        static int MobilityScore(Board board)
        {
            int score = board.GetLegalMoves().Length;

            return (int)Math.Sqrt(score) * 10;
        }

        static int EndgameForceKingToCorner(Square friendlyKingSquare, Square oppenentKingSquare)
        {
            int eval = 0;
            
            int opponentKingDistanceToCenter =  Math.Max(3 - oppenentKingSquare.File, oppenentKingSquare.File - 4) +
                                                Math.Max(3 - oppenentKingSquare.Rank, oppenentKingSquare.Rank - 4);
            eval += opponentKingDistanceToCenter * 10;
            
            int distanceBetweenKings = Math.Abs(friendlyKingSquare.File - oppenentKingSquare.File) +
                                        Math.Abs(friendlyKingSquare.Rank - oppenentKingSquare.Rank);
            eval += (14 - distanceBetweenKings) * 4;

            return eval;
        }
    }
}
