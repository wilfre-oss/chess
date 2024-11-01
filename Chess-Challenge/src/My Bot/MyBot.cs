using ChessChallenge.API;
using ChessChallenge.Evaluation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;



public class MyBot : IChessBot
{
    
    
    Board board;
    MoveGenerator moveGenerator;

    public Move Think(Board boardIn, Timer timer)
    {
        board = boardIn;
        Move bestMove = Move.NullMove;
        moveGenerator = new MoveGenerator();
        int alpha = -int.MaxValue;
        int beta = int.MaxValue;
        
        for(int depth = 1; depth < 7; depth++)
        {
            foreach (Move move in GenerateMoves(board))
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
            alpha = -int.MaxValue;
        }

        return bestMove;
    }

    int Search(int depth, int alpha, int beta)
    {
        
        if (depth == 0) return Evaluate(board);

        
        if (board.IsInCheckmate()) return -(100_000 + depth);

        if (board.IsDraw()) return 0;
        
        foreach (Move move in GenerateMoves(board))
        {
            board.MakeMove(move);
            int eval = -Search(depth - 1, -beta, -alpha);
            board.UndoMove(move);
            if(eval >= beta) {
                moveGenerator.History[ColorIndex, move.StartSquare.Index, move.TargetSquare.Index] = depth * depth;
                return beta;
            };

            alpha = Math.Max(alpha, eval);
        }
        return alpha;
    }
    public static int Evaluate(Board board) => Evaluation.Evaluate(board);
    IEnumerable<Move> GenerateMoves(Board board) => moveGenerator.GenerateMoves(board);

    int ColorIndex => board.IsWhiteToMove ? 0 : 1;
}
