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
            bool isWhite = board.IsWhiteToMove;

            //Material evaulation
            int myEval = CountMaterial(isWhite, board);
            int opponentEval = CountMaterial(!isWhite, board);

            //Mobillity evaluation
            
            myEval += MobilityScore(board);
            board.ForceSkipTurn();
            opponentEval += MobilityScore(board);
            board.UndoSkipTurn();
            
            
            return myEval - opponentEval;
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
            Move[] moves = board.GetLegalMoves();

            int score = 0;

            for(int i = 0; i < moves.Length; i++)
            {
                if (moves[i].MovePieceType == PieceType.Pawn ||
                    moves[i].MovePieceType == PieceType.Queen)
                    continue;
                score++;
            }

            return (int)Math.Sqrt(score) * 10;
        }

        static int ForceKingToCorner(Square friendlyKingSquare, Square oppenentKingSquare)
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
