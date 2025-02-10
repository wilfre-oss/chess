using ChessChallenge.API;


using Raylib_cs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public static int[] PieceSquarePawnsWhite = {
                0,  0,  0,  0,  0,  0,  0,  0,
                50, 50, 50, 50, 50, 50, 50, 50,
                10, 10, 20, 30, 30, 20, 10, 10,
                5,  5, 10, 25, 25, 10,  5,  5,
                0,  0,  0, 20, 20,  0,  0,  0,
                5, -5,-10,  0,  0,-10, -5,  5,
                5, 10, 10,-20,-20, 10, 10,  5,
                0,  0,  0,  0,  0,  0,  0,  0
        };


        public static int[] PieceSquarePawnsBlack = {
    0,  0,  0,  0,  0,  0,  0,  0,
    5, 10, 10,-20,-20, 10, 10,  5,
    5, -5,-10,  0,  0,-10, -5,  5,
    0,  0,  0, 20, 20,  0,  0,  0,
    5,  5, 10, 25, 25, 10,  5,  5,
    10, 10, 20, 30, 30, 20, 10, 10,
    50, 50, 50, 50, 50, 50, 50, 50,
    0,  0,  0,  0,  0,  0,  0,  0
};

        public static readonly int[] PawnsEnd = {
             0,   0,   0,   0,   0,   0,   0,   0,
            80,  80,  80,  80,  80,  80,  80,  80,
            50,  50,  50,  50,  50,  50,  50,  50,
            30,  30,  30,  30,  30,  30,  30,  30,
            20,  20,  20,  20,  20,  20,  20,  20,
            10,  10,  10,  10,  10,  10,  10,  10,
            10,  10,  10,  10,  10,  10,  10,  10,
             0,   0,   0,   0,   0,   0,   0,   0
        };

        public static int[] PieceSquareKnightsWhite = {
            -50,-40,-30,-30,-30,-30,-40,-50,
            -40,-20,  0,  0,  0,  0,-20,-40,
            -30,  0, 10, 15, 15, 10,  0,-30,
            -30,  5, 15, 20, 20, 15,  5,-30,
            -30,  0, 15, 20, 20, 15,  0,-30,
            -30,  5, 10, 15, 15, 10,  5,-30,
            -40,-20,  0,  5,  5,  0,-20,-40,
            -50,-40,-30,-30,-30,-30,-40,-50
        };

        public static int[] PieceSquareKnightsBlack = {
    -50,-40,-30,-30,-30,-30,-40,-50,
    -40,-20,  0,  5,  5,  0,-20,-40,
    -30,  5, 10, 15, 15, 10,  5,-30,
    -30,  0, 15, 20, 20, 15,  0,-30,
    -30,  5, 15, 20, 20, 15,  5,-30,
    -30,  0, 10, 15, 15, 10,  0,-30,
    -40,-20,  0,  0,  0,  0,-20,-40,
    -50,-40,-30,-30,-30,-30,-40,-50
};

        public static int[] PieceSquareBishopsWhite = {
            -20,-10,-10,-10,-10,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  0,  5, 10, 10,  5,  0,-10,
            -10,  5,  5, 10, 10,  5,  5,-10,
            -10,  0, 10, 10, 10, 10,  0,-10,
            -10, 10, 10, 10, 10, 10, 10,-10,
            -10,  5,  0,  0,  0,  0,  5,-10,
            -20,-10,-10,-10,-10,-10,-10,-20
        };

        public static int[] PieceSquareBishopsBlack = {
    -20,-10,-10,-10,-10,-10,-10,-20,
    -10,  5,  0,  0,  0,  0,  5,-10,
    -10, 10, 10, 10, 10, 10, 10,-10,
    -10,  0, 10, 10, 10, 10,  0,-10,
    -10,  5,  5, 10, 10,  5,  5,-10,
    -10,  0,  5, 10, 10,  5,  0,-10,
    -10,  0,  0,  0,  0,  0,  0,-10,
    -20,-10,-10,-10,-10,-10,-10,-20
};

        public static int[] PieceSquareRooksWhite = {
            0,  0,  0,  0,  0,  0,  0,  0,
            5, 10, 10, 10, 10, 10, 10,  5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            -5,  0,  0,  0,  0,  0,  0, -5,
            0,  0,  0,  5,  5,  0,  0,  0
        };


        public static int[] PieceSquareRooksBlack = {
    0,  0,  0,  5,  5,  0,  0,  0,
   -5,  0,  0,  0,  0,  0,  0, -5,
   -5,  0,  0,  0,  0,  0,  0, -5,
   -5,  0,  0,  0,  0,  0,  0, -5,
   -5,  0,  0,  0,  0,  0,  0, -5,
   -5,  0,  0,  0,  0,  0,  0, -5,
    5, 10, 10, 10, 10, 10, 10,  5,
    0,  0,  0,  0,  0,  0,  0,  0
};

        public static int[] PieceSquareQueenWhite = {
            -20,-10,-10, -5, -5,-10,-10,-20,
            -10,  0,  0,  0,  0,  0,  0,-10,
            -10,  0,  5,  5,  5,  5,  0,-10,
            -5,  0,  5,  5,  5,  5,  0, -5,
            0,  0,  5,  5,  5,  5,  0, -5,
            -10,  5,  5,  5,  5,  5,  0,-10,
            -10,  0,  5,  0,  0,  0,  0,-10,
            -20,-10,-10, -5, -5,-10,-10,-20
        };
        public static int[] PieceSquareQueenBlack = {
   -20,-10,-10, -5, -5,-10,-10,-20,
   -10,  0,  5,  0,  0,  0,  0,-10,
   -10,  5,  5,  5,  5,  5,  0,-10,
    0,  0,  5,  5,  5,  5,  0, -5,
   -5,  0,  5,  5,  5,  5,  0, -5,
   -10,  0,  5,  5,  5,  5,  0,-10,
   -10,  0,  0,  0,  0,  0,  0,-10,
   -20,-10,-10, -5, -5,-10,-10,-20
};


        public static int[] PieceSquareKingMidWhite = {
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -30,-40,-40,-50,-50,-40,-40,-30,
            -20,-30,-30,-40,-40,-30,-30,-20,
            -10,-20,-20,-20,-20,-20,-20,-10,
            20, 20,  0,  0,  0,  0, 20, 20,
            20, 30, 10,  0,  0, 10, 30, 20
        };

        public static int[] PieceSquareKingMidBlack = {
    20, 30, 10,  0,  0, 10, 30, 20,
    20, 20,  0,  0,  0,  0, 20, 20,
   -10,-20,-20,-20,-20,-20,-20,-10,
   -20,-30,-30,-40,-40,-30,-30,-20,
   -30,-40,-40,-50,-50,-40,-40,-30,
   -30,-40,-40,-50,-50,-40,-40,-30,
   -30,-40,-40,-50,-50,-40,-40,-30,
   -30,-40,-40,-50,-50,-40,-40,-30
};

        public static int[] PieceSquareKingEndWhite = {
            -50,-40,-30,-20,-20,-30,-40,-50,
            -30,-20,-10,  0,  0,-10,-20,-30,
            -30,-10, 20, 30, 30, 20,-10,-30,
            -30,-10, 30, 40, 40, 30,-10,-30,
            -30,-10, 30, 40, 40, 30,-10,-30,
            -30,-10, 20, 30, 30, 20,-10,-30,
            -30,-30,  0,  0,  0,  0,-30,-30,
            -50,-30,-30,-30,-30,-30,-30,-50
        };

        public static int[] PieceSquareKingEndBlack = {
   -50,-30,-30,-30,-30,-30,-30,-50,
   -30,-30,  0,  0,  0,  0,-30,-30,
   -30,-10, 20, 30, 30, 20,-10,-30,
   -30,-10, 30, 40, 40, 30,-10,-30,
   -30,-10, 30, 40, 40, 30,-10,-30,
   -30,-10, 20, 30, 30, 20,-10,-30,
   -30,-20,-10,  0,  0,-10,-20,-30,
   -50,-40,-30,-20,-20,-30,-40,-50
};

        const float endgameMaterialStart = RookValue * 2 + BishopValue + KnightValue;

        public static int Evaluate(API.Board board)
        {
            bool isWhite = board.IsWhiteToMove;


            //Material evaulation
            int myEval = CountMaterial(isWhite, board);
            int opponentEval = CountMaterial(!isWhite, board);

            //Pawn scores
            int myPawnValue = PawnValue * board.GetPieceList(PieceType.Pawn, isWhite).Count;
            int opponentPawnValue = PawnValue * board.GetPieceList(PieceType.Pawn, !isWhite).Count;
            myEval += myPawnValue;
            opponentEval += opponentPawnValue;

            //Endgame wieght
            int myMaterial = myEval - myPawnValue;
            int opponentMaterial = opponentEval - myPawnValue;
            float myEndgameWeight = EndgamePhaseWeight(myMaterial);
            float opponentEndgameWeight = EndgamePhaseWeight(opponentMaterial);

            //Piece position score
            int myPieceSquareScore = CountPieceSquare(isWhite, board, myEndgameWeight);
            int opponentPieceSquareScore = CountPieceSquare(isWhite, board, opponentEndgameWeight);
            myEval += myPieceSquareScore;
            opponentEval += opponentPieceSquareScore;

            //Mobillity evaluation
            myEval += MobilityScore(board);
            board.ForceSkipTurn();
            opponentEval += MobilityScore(board);
            board.UndoSkipTurn();

            if (opponentMaterial + 200 < myMaterial)
            {
                myEval += ForceKingToCorner(board.GetKingSquare(isWhite), board.GetKingSquare(!isWhite), opponentEndgameWeight);
            }
            else if (myMaterial + 200 < opponentMaterial)
            {
                opponentEval += ForceKingToCorner(board.GetKingSquare(!isWhite), board.GetKingSquare(isWhite), myEndgameWeight);
            }

            myEval -= UncastledPenalty(board.GetKingSquare(isWhite), opponentPieceSquareScore, opponentMaterial);
            opponentEval -= UncastledPenalty(board.GetKingSquare(!isWhite), myPieceSquareScore, myMaterial);

            return myEval - opponentEval;
        }

        public static int Evaluatemid(API.Board board)
        {
            bool isWhite = board.IsWhiteToMove;


            //Material evaulation
            int myEval = CountMaterial(isWhite, board);
            int opponentEval = CountMaterial(!isWhite, board);

            //Pawn scores
            int myPawnValue = PawnValue * board.GetPieceList(PieceType.Pawn, isWhite).Count;
            int opponentPawnValue = PawnValue * board.GetPieceList(PieceType.Pawn, !isWhite).Count;
            myEval += myPawnValue;
            opponentEval += opponentPawnValue;

            ////Endgame wieght
            //int myMaterial = myEval - myPawnValue;
            //int opponentMaterial = opponentEval - myPawnValue;
            //float myEndgameWeight = EndgamePhaseWeight(myMaterial);
            //float opponentEndgameWeight = EndgamePhaseWeight(opponentMaterial);

            ////Piece position score
            //int myPieceSquareScore = CountPieceSquare(isWhite, board, myEndgameWeight);
            //int opponentPieceSquareScore = CountPieceSquare(isWhite, board, opponentEndgameWeight);
            //myEval += myPieceSquareScore;
            //opponentEval += opponentPieceSquareScore;

            ////Mobillity evaluation
            //myEval += MobilityScore(board);
            //board.ForceSkipTurn();
            //opponentEval += MobilityScore(board);
            //board.UndoSkipTurn();

            //if (opponentMaterial + 200 < myMaterial)
            //{
            //    myEval += ForceKingToCorner(board.GetKingSquare(isWhite), board.GetKingSquare(!isWhite), opponentEndgameWeight);
            //}
            //else if (myMaterial + 200 < opponentMaterial)
            //{
            //    opponentEval += ForceKingToCorner(board.GetKingSquare(!isWhite), board.GetKingSquare(isWhite), myEndgameWeight);
            //}

            //myEval -= UncastledPenalty(board.GetKingSquare(isWhite), opponentPieceSquareScore, opponentMaterial);
            //opponentEval -= UncastledPenalty(board.GetKingSquare(!isWhite), myPieceSquareScore, myMaterial);

            return myEval - opponentEval;
        }

        public static double EvaluateMCTS(API.Board board)
        {

            double evalScore = (double)Evaluate(board);
            return Math.Tanh(evalScore / 300.0);
        }

        static int CountMaterial(bool isWhite, API.Board board)
        {
            int material = 0;
            material += KnightValue * board.GetPieceList(PieceType.Knight, isWhite).Count;
            material += BishopValue * board.GetPieceList(PieceType.Bishop, isWhite).Count;
            material += RookValue * board.GetPieceList(PieceType.Rook, isWhite).Count;
            material += QueenValue * board.GetPieceList(PieceType.Queen, isWhite).Count;
            return material;
        }

        static int UncastledPenalty(Square kingSquare, int opponentPieceSquareScore, int opponentMaterial)
        {
            if (kingSquare.File < 2 || kingSquare.File > 5 ||
                opponentMaterial <= endgameMaterialStart)
            {
                return 0;
            }

            float opponentDevelopmentScore = System.Math.Clamp((opponentPieceSquareScore + 10) / 130f, 0, 1);
            return (int)(50 * opponentDevelopmentScore);
        }


        static int CountPieceSquare(bool isWhite, Board board, float endgameWeight)
        {
            int pieceSquareValue = 0;

            if (isWhite)
            {
                pieceSquareValue += LoopPieceList(board.GetPieceList(PieceType.Knight, isWhite), PieceSquareKnightsWhite);
                pieceSquareValue += LoopPieceList(board.GetPieceList(PieceType.Bishop, isWhite), PieceSquareBishopsWhite);
                pieceSquareValue += LoopPieceList(board.GetPieceList(PieceType.Rook, isWhite), PieceSquareRooksWhite);
                pieceSquareValue += LoopPieceList(board.GetPieceList(PieceType.Queen, isWhite), PieceSquareQueenWhite);

                int midPawn = LoopPieceList(board.GetPieceList(PieceType.Pawn, isWhite), PieceSquarePawnsWhite);
                int endPawn = LoopPieceList(board.GetPieceList(PieceType.Pawn, isWhite), PawnsEnd);
                pieceSquareValue += (int)((midPawn * (1 - endgameWeight)) + (endPawn * endgameWeight));

                int midKing = PieceSquareKingMidWhite[board.GetKingSquare(isWhite).Index];
                int endKing = PieceSquareKingEndWhite[board.GetKingSquare(isWhite).Index];
                pieceSquareValue += (int)((midKing * (1 - endgameWeight)) + (endPawn * endgameWeight));
            }
            else
            {
                pieceSquareValue += LoopPieceList(board.GetPieceList(PieceType.Knight, isWhite), PieceSquareKnightsBlack);
                pieceSquareValue += LoopPieceList(board.GetPieceList(PieceType.Bishop, isWhite), PieceSquareBishopsBlack);
                pieceSquareValue += LoopPieceList(board.GetPieceList(PieceType.Rook, isWhite), PieceSquareRooksBlack);
                pieceSquareValue += LoopPieceList(board.GetPieceList(PieceType.Queen, isWhite), PieceSquareQueenBlack);

                int midPawn = LoopPieceList(board.GetPieceList(PieceType.Pawn, isWhite), PieceSquarePawnsBlack);
                int endPawn = LoopPieceList(board.GetPieceList(PieceType.Pawn, isWhite), GetFlippedTable(PawnsEnd));
                pieceSquareValue += (int)((midPawn * (1 - endgameWeight)) + (endPawn * endgameWeight));

                int midKing = PieceSquareKingMidBlack[board.GetKingSquare(isWhite).Index];
                int endKing = PieceSquareKingEndBlack[board.GetKingSquare(isWhite).Index];
                pieceSquareValue += (int)((midKing * (1 - endgameWeight)) + (endKing * endgameWeight));
            }
            return pieceSquareValue;
        }

        static int LoopPieceList(PieceList plist, int[] pieceSquare)
        {
            int value = 0;
            foreach (Piece piece in plist)
            {
                value += pieceSquare[piece.Square.Index];
            }
            return value;

        }


        static int MobilityScore(Board board)
        {
            Move[] moves = board.GetLegalMoves();

            int score = 0;

            for (int i = 0; i < moves.Length; i++)
            {
                if (moves[i].MovePieceType == PieceType.Pawn ||
                    moves[i].MovePieceType == PieceType.Queen ||
                    moves[i].MovePieceType == PieceType.King)
                    continue;
                score++;
            }

            return (int)Math.Sqrt(score) * 10;
        }

        static float EndgamePhaseWeight(int materialCountWithoutPawns)
        {
            const float multiplier = 1 / endgameMaterialStart;
            return 1 - Math.Min(1, materialCountWithoutPawns * multiplier);
        }

        static int ForceKingToCorner(Square friendlyKingSquare, Square oppenentKingSquare, float endgameWeight)
        {

            int eval = 0;

            int opponentKingDistanceToCenter = Math.Max(3 - oppenentKingSquare.File, oppenentKingSquare.File - 4) +
                                                Math.Max(3 - oppenentKingSquare.Rank, oppenentKingSquare.Rank - 4);
            eval += opponentKingDistanceToCenter * 10;

            int distanceBetweenKings = Math.Abs(friendlyKingSquare.File - oppenentKingSquare.File) +
                                        Math.Abs(friendlyKingSquare.Rank - oppenentKingSquare.Rank);
            eval += (14 - distanceBetweenKings) * 4;

            return (int)(eval * endgameWeight);
        }
        static int[] GetFlippedTable(int[] table)
        {
            int[] flippedTable = new int[table.Length];

            for (int i = 0; i < table.Length; i++)
            {
                Chess.Coord coord = new Chess.Coord(i);
                Chess.Coord flippedCoord = new Chess.Coord(coord.fileIndex, 7 - coord.rankIndex);
                flippedTable[flippedCoord.SquareIndex] = table[i];
            }
            return flippedTable;
        }
    }
}
