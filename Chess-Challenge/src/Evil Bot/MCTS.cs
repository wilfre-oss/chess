using ChessChallenge.API;


namespace ChessChallenge.Example
{
    using ChessChallenge.Evaluation;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp.Syntax;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.Design.Serialization;
    using System.Diagnostics.CodeAnalysis;

    public class EvilBot2 : IChessBot
    {
        Board board;
        bool isRootWhite;
        Node? root = null;
        Dictionary<ulong, Node> tt = new Dictionary<ulong, Node>();
        private class Node
        {
            public int visits = 0;
            public double value = 0;
            public Move move;
            public Node[]? children;
            public Node? parent;
            public Node(Node? parent, Move move, Node[]? children = null)
            {
                this.children = children;
                this.parent = parent;
                this.move = move;
            }
        }
        public Move Think(Board board, Timer timer)
        {
            this.board = board;
            isRootWhite = board.IsWhiteToMove;

            
            if (root != null)
            {
                if (tt.TryGetValue(board.ZobristKey, out Node newRoot))
                {
                    root = newRoot;
                    root.parent = null;
                }
                else
                {
                    root = new(null, Move.NullMove);
                    Expand(root, board.GetLegalMoves());
                }
                tt = new Dictionary<ulong, Node>();
            }
            else
            {
                root = new(null, Move.NullMove);
                Expand(root, board.GetLegalMoves());
            }
            


            while (timer.MillisecondsElapsedThisTurn < 2000)
            {
                Node leaf = Search(root);
                double eval = Rollout(-1.0, 1.0);
                Backpropagate(leaf, eval);
            }

            int mostVisits = 0;
            Move bestMove = Move.NullMove;

            foreach (Node child in root.children)
            {
                board.MakeMove(child.move);
                foreach (Node grandChild in child.children)
                {
                    board.MakeMove(grandChild.move);
                    tt.TryAdd(board.ZobristKey, grandChild);
                    board.UndoMove(grandChild.move);
                }
                board.UndoMove(child.move);
                Console.WriteLine(child.move + " visits: " + child.visits + " value: " + child.value);
                if (child.visits > mostVisits)
                {
                    mostVisits = child.visits;
                    bestMove = child.move;
                }
            }

            return bestMove;
        }

        Node Search(Node node)
        {
            while (node.children != null)
            {
                node = UCTSelction(node);
                board.MakeMove(node.move);
            }
            if (node.visits == 0) return node;

            Move[] moves = board.GetLegalMoves();
            if (moves.Length == 0) 
            {
                return node;
            }

            Expand(node, moves);
            return Search(node);
        }

        double Rollout (Node node)
        {
            if (board.GetLegalMoves().Length == 0)
            {
                if (board.IsInCheck())
                {
                    return board.IsWhiteToMove == isRootWhite ?
                        -1 : 1;
                }
                return 0;
            }

            double score = Evaluation.EvaluateMCTS(board);

            return board.IsWhiteToMove == isRootWhite ? score: -score;
        }
        double Rollout(double alpha, double beta)
        {
            if (board.GetLegalMoves().Length == 0)
            {
                if (board.IsInCheck())
                {
                    return board.IsWhiteToMove == isRootWhite ?
                        -1 : 1;
                }
                return 0;
            }

            double eval = Evaluation.EvaluateMCTS(board); 

            if (eval >= beta)
                return beta;
            if (eval >= alpha)
                alpha = eval;

            foreach (Move move in board.GetLegalMoves(true))
            {
                board.MakeMove(move);
                eval = -Rollout(-beta, -alpha);
                board.UndoMove(move);
                if (eval >= beta)
                    return beta;
                if (eval > alpha)
                    alpha = eval;
            }
            return alpha;
        }
        void Backpropagate (Node node, double value)
        {
            while (node.parent != null)
            {
                board.UndoMove(node.move);
                node.value += value;
                node.visits++;
                node = node.parent;
                value = -value;
            }
            node.value += value;
            node.visits++;
        }

        static Node UCTSelction (Node node)
        {
            double best = double.MinValue;
            Node next = node.children[0];

            foreach (Node child in node.children)
            {
                if (child.visits == 0) return child;

                double uct = (child.value / child.visits) + (2 * Math.Sqrt(Math.Log(node.visits) / child.visits));
                if (uct > best)
                {
                    best = uct;
                    next = child;
                }
            }
            return next;
        }
        
        void Expand(Node node, Move[] moves)
        {
            Node[] children = new Node[moves.Length];
            for (int i = 0; i < moves.Length; i++)
            {
                children[i] = new Node(node, moves[i], null);
            }
            node.children = children;
        }
    }
}