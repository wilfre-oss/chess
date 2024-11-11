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

    Move bestMove;
    Move bestMoveInIteration;
    int iterationDepth;
    int timeToThink = 100;

    public Move Think(Board boardIn, Timer timerIn)
    {
        board = boardIn;
        timer = timerIn;
        bestMove = Move.NullMove;
        bestMoveInIteration = Move.NullMove;
        moveGenerator = new MoveGenerator();
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
            Console.WriteLine("depth: " + (iterationDepth - 1));
            Console.WriteLine(bestMove + " eval: " + eval);
            Console.WriteLine("Time to get move: " + timer.MillisecondsElapsedThisTurn + " ms");

            return bestMove;
        }
        
    }

    int Search(int depth, int alpha, int beta)
    {
        
        if (depth == 0) return QuiscenceSearch(alpha, beta);

        List<Move> moves = GenerateMoves(board);
        if (moves.Count == 0)
        {
            return (board.IsInCheck()) ? 
                -(100_000 + depth) : 0;
        }

        if (iterationDepth - depth > 3 &&
            timer.MillisecondsElapsedThisTurn > 1000)
        {
            throw new Exception();
        }
        
        foreach (Move move in moves)
        {

            board.MakeMove(move);
            int eval = -Search(depth - 1, -beta, -alpha);
            board.UndoMove(move);
            if(eval >= beta) {
                if (!move.IsCapture)
                    moveGenerator.History[ColorIndex, move.StartSquare.Index, move.TargetSquare.Index] += depth * depth;
                return beta;
            };

            if(eval > alpha)
            {
                alpha = eval;
                
                if (depth == iterationDepth)
                {
                    bestMoveInIteration = move;
                }
            }
        }
        return alpha;
    }

    int QuiscenceSearch(int alpha, int beta)
    {
        int eval = Evaluate(board);

        if (eval >= beta)
            return beta;
        if (eval > alpha)
            alpha = eval;


        foreach (Move move in GenerateMoves(board, true))
        {
            board.MakeMove(move);
            eval = -QuiscenceSearch(-beta, -alpha);
            board.UndoMove(move);
            if (eval >= beta)
                return beta;
            if (eval > alpha)
                alpha = eval;
        }
        return alpha;
    }
    public static int Evaluate(Board board) => Evaluation.Evaluate(board);
    List<Move> GenerateMoves(Board board, bool quietMoves = false) => moveGenerator.GenerateMoves(board, quietMoves);

    int ColorIndex => board.IsWhiteToMove ? 0 : 1;
}
