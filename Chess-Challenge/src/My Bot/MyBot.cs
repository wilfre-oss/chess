using ChessChallenge.API;
using ChessChallenge.Evaluation;
using System;
using System.Collections.Generic;
using System.Data;



public class MyBot : IChessBot
{
    public static int Evaluate(Board board) => Evaluation.Evaluate(board);
    static IEnumerable<Move> GenerateMoves(Board board) => MoveGenerator.GenerateMoves(board);
    
    Board board;

    public Move Think(Board boardIn, Timer timer)
    {
        int depth = 6;
        board = boardIn;
        Move[] moves = boardIn.GetLegalMoves();
        Move bestMove = moves[0];
        int alpha = -int.MaxValue;
        int beta = int.MaxValue;
        
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

        return bestMove;
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

        if (board.IsDraw()) return 0;
        
        foreach (Move move in GenerateMoves(board))
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
