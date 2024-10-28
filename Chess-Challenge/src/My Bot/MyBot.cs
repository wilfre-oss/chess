using ChessChallenge.API;
using Microsoft.CodeAnalysis.Diagnostics;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;


public class MyBot : IChessBot
{
    public const int PawnValue = 100;
    public const int KnightValue = 300;
    public const int BishopValue = 320;
    public const int RookValue = 500;
    public const int QueenValue = 900;

    Board board;
    

    public Move Think(Board boardIn, Timer timer)
    {
        int depth = 5;
        board = boardIn;
        Move[] moves = boardIn.GetLegalMoves();
        Move bestMove = moves[0];
        int alpha = -int.MaxValue;
        int beta = int.MaxValue;

        foreach (Move move in moves)
        {
            board.MakeMove(move);
            int eval = -Search(depth - 1, alpha, beta);
            board.UndoMove(move);
            if (eval > alpha)
            {
                alpha = eval;
                bestMove = move;
            }
        }

        return bestMove;
    }

    static int Evaluate(Board board)
    {
        int whiteEval = CountMaterial(true, board);
        int blackEval = CountMaterial(false, board);

        int eval = (whiteEval - blackEval) * (board.IsWhiteToMove ? 1 : -1);
        

        return eval;  
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

    int Search(int depth)
    {
        if (depth == 0) return Evaluate(board);

        Move[] moves = board.GetLegalMoves();
        if (moves.Length == 0) return board.IsInCheck() ? int.MinValue : 0;

        int bestEval = int.MinValue;

        foreach (Move move in moves)
        {
            board.MakeMove(move);
            int eval = -Search(depth - 1);
            board.UndoMove(move);

            bestEval = Math.Max(bestEval, eval);
        }

        return bestEval;
    }

    int Search(int depth, int alpha, int beta)
    {
        
        if (depth == 0) return Evaluate(board);

        if (board.IsInCheckmate()) return -(100_000 + depth);
        

        foreach (Move move in board.GetLegalMoves())
        {
            board.MakeMove(move);
            int eval = -Search(depth - 1, -beta, -alpha);
            board.UndoMove(move);
            if(eval >= beta) {
                return beta;
            };

            alpha = Math.Max(alpha, eval);
        }
        return alpha;
    }
}
