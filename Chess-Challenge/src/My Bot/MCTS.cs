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

    public class MyBot : IChessBot
    {
        Board board;
        bool isRootWhite;
        Node? root = null;
        Dictionary<ulong, Node> tt = new Dictionary<ulong, Node>();
        private class Node
        {
            public int visits;
            public double value;
            public Move move;
            public Node[]? children;
            public Node? parent;
            public Node(Node? parent, Move move, Node[]? children = null)
            {
                this.children = children;
                this.parent = parent;
                this.move = move;
                this.value = 0;
                this.visits = 0;
            }
            public Node(Node? parent, Move move, double value, Node[]? children = null)
            {
                this.children = children;
                this.parent = parent;
                this.move = move;
                this.value = value;
                this.visits = 0;
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

            while (timer.MillisecondsElapsedThisTurn < 250)
            {
                Search(root);
            }

            int mostVisits = 0;
            Move bestMove = Move.NullMove;

            foreach (Node child in root.children)
            {
                board.MakeMove(child.move);
                if (child.children != null)
                {
                    foreach (Node grandChild in child.children)
                    {
                        board.MakeMove(grandChild.move);
                        tt.TryAdd(board.ZobristKey, grandChild);
                        board.UndoMove(grandChild.move);
                    }
                }
                board.UndoMove(child.move);
                if (child.visits > mostVisits)
                {
                    mostVisits = child.visits;
                    bestMove = child.move;
                }
            }
            Console.WriteLine(board.PlyCount / 2 + " " + root.visits);
            return bestMove;
        }

        
        void Search(Node node)
        {
            double eval = 0.0;
            Move[] moves = new Move[128];
           
            while (node.children != null)
            {
                node = UCTSelction(node);
                board.MakeMove(node.move);
            }
            moves = board.GetLegalMoves();
            if (moves.Length == 0)
            {
                eval = board.IsInCheck() ? 1 : 0;

            }
            else 
            {
                if (node.visits == 0)
                {
                    eval = Rollout(-1.0, 1.0);
                }
                else 
                { 
                    Expand(node, moves);
                    int visits = node.children != null ? node.children.Length :  1;
                    foreach(Node child in node.children)
                    {
                        board.MakeMove(child.move);

                        if (board.GetLegalMoves().Length > 0)
                        {
                            eval += Rollout(-1.0, 1.0);
                        }
                        else { eval += board.IsInCheck() ? 1 : 0; }
                        board.UndoMove(child.move);
                    }
                    Backpropagate(node, -eval, visits);
                    return;
                }
            }
                
            
            
            Backpropagate(node, -eval);
        }

        double Rollout(double alpha, double beta)
        {

            double eval = Evaluation.EvaluateMCTS(board);

            if (eval >= beta)
                return beta;
            if (eval >= alpha)
                alpha = eval;

            Move[] moves = board.GetLegalMoves(true);
            for (int i = 0; i < moves.Length; i++)
            {
                board.MakeMove(moves[i]);
                eval = -Rollout(-beta, -alpha);
                board.UndoMove(moves[i]);
                if (eval >= beta)
                    return beta;
                if (eval > alpha)
                    alpha = eval;
            }
            return alpha;
        }
        void Backpropagate(Node node, double value)
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

        void Backpropagate(Node node, double value, int visits)
        {
            while (node.parent != null)
            {
                board.UndoMove(node.move);
                node.value += value;
                node.visits += visits;
                node = node.parent;
                value = -value;
            }
            node.value += value;
            node.visits += visits;
        }

        static Node UCTSelction(Node node)
        {
            double best = double.MinValue;
            Node next = node.children[0];

            foreach (Node child in node.children)
            {
                if (child.visits == 0) return child;

                double uct = (child.value / child.visits) + (Math.Sqrt(Math.Log(node.visits) / child.visits));
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
                children[i] = new Node(node, moves[i]);
            }
            node.children = children;
        }
    }
}