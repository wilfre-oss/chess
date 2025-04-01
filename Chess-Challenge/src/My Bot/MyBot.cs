using ChessChallenge.API;
using ChessChallenge.Evaluation;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.CodeAnalysis;



public class MyBot : IChessBot
{
    Board board;
    Timer timer;
    MoveGenerator moveGenerator;
    TranspositionTable transpositionTable;

    Move bestMove;
    Move bestMoveInIteration;
    int iterationDepth;
    int timeToThink = 1000;

    public Move Think(Board boardIn, Timer timerIn)
    {
        board = boardIn;
        timer = timerIn;
        bestMove = Move.NullMove;
        bestMoveInIteration = Move.NullMove;
        transpositionTable = new TranspositionTable(64);
        moveGenerator = new MoveGenerator(transpositionTable);
        int alpha = -int.MaxValue;
        int beta = int.MaxValue;
        int eval = 0;

        try
        {
            for (iterationDepth = 1; ; iterationDepth++)
            {
                eval = Search(iterationDepth, alpha, beta);
                bestMove = bestMoveInIteration;
            }
        } catch
        {   
            bestMove = bestMoveInIteration;

            Console.WriteLine("depth: " + (iterationDepth - 1));
            Console.WriteLine(bestMove + " eval: " + eval);
            Console.WriteLine("Time to get move: " + timer.MillisecondsElapsedThisTurn + " ms");

            return bestMove;
        }
    }

    int Search(int depth, int alpha, int beta)
    {
        if (transpositionTable.TryGet(board.ZobristKey, out TTEntry entry) && 
            entry.Depth >= depth)
        {
            if (entry.Flag == FlagType.Exact)
                return entry.Eval;
            if (entry.Flag == FlagType.LowerBound)
                alpha = entry.Eval;
            else if (entry.Flag == FlagType.UpperBound)
                beta = entry.Eval;
            if (alpha >= beta)
                return entry.Eval;
        }

        if (depth == 0) return QuiscenceSearch(alpha, beta);

        Span<Move> moves = stackalloc Move[218];
        int moveCount = GenerateMoves(board, moves);
        if (moveCount == 0)
        {
            return (board.IsInCheck()) ? 
                -(100_000 + depth) : 0;
        }

        if (iterationDepth - depth > 3 &&
            timer.MillisecondsElapsedThisTurn > timeToThink)
        {
            throw new Exception();
        }

        FlagType flag = FlagType.UpperBound;
        Move bestMoveThisPosition = Move.NullMove;

        for (int i = 0; i < moveCount; i++)
        {
            Move move = moves[i];
            board.MakeMove(move);
            int eval = -Search(depth - 1, -beta, -alpha);
            board.UndoMove(move);

            if(eval >= beta) {
                if (!move.IsCapture)
                    moveGenerator.History[ColorIndex, move.StartSquare.Index, move.TargetSquare.Index] += depth * depth;

                transpositionTable.Store(board.ZobristKey, depth, eval, move, FlagType.LowerBound);

                return beta;
            };

            if(eval > alpha)
            {
                alpha = eval;
                flag = FlagType.Exact;
                bestMoveThisPosition = move;
                
                if (depth == iterationDepth)
                {
                    bestMoveInIteration = move;
                }
            }
        }

        transpositionTable.Store(board.ZobristKey, depth, alpha, bestMoveThisPosition, flag);

        return alpha;
    }

    int QuiscenceSearch(int alpha, int beta)
    {
        int eval = Evaluate(board);

        if (eval >= beta)
            return beta;
        if (eval > alpha)
            alpha = eval;

        Span<Move> moves = stackalloc Move[64];
        int moveCount = GenerateMoves(board, moves, true);

        for(int i = 0; i < moveCount; i++)
        {
            Move move = moves[i];
            board.MakeMove(move);
            eval = -QuiscenceSearch(-beta, -alpha);
            board.UndoMove(move);
            if (eval >= beta)
            {
                return beta;
            }
            if (eval > alpha)
                alpha = eval;
        }
        return alpha;
    }
    public static int Evaluate(Board board) => Evaluation.Evaluate(board);
    int GenerateMoves(Board board, Span<Move> moveBuffer, bool capturesOnly = false) => moveGenerator.GenerateMoves(board, moveBuffer, capturesOnly);

    int ColorIndex => board.IsWhiteToMove ? 0 : 1;
}
