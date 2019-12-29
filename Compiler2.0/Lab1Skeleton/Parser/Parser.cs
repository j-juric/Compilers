using System;
using System.Collections.Generic;

using Lexer;

namespace Parser
{
    public class Parser
    {
        Lexer.Lexer lexer;
        Token curr;
        Token next;

        public Parser(string prg)
        {
            lexer = new Lexer.Lexer(prg);
            curr = lexer.Next();
            if (curr.type != Token.Type.EOF)
                next = lexer.Next();
            else
                next = null;
        }

        public Parser(Lexer.Lexer lexer)
        {
            this.lexer = lexer;
            curr = lexer.Next();
            if (curr.type != Token.Type.EOF)
                next = lexer.Next();
            else
                next = null;
        }



        public bool IsNext(Token.Type t)
        {
            return curr.type == t;
        }

        public bool IsNext(Token.Type t, String lexeme)
        {
            return (curr.type == t) && (curr.lexeme == lexeme);
        }

        public Token Match()
        {
            Token t = curr;
            curr = next;
            if (next.type != Token.Type.EOF)
                next = lexer.Next();
            return t;
        }

        public Token Match(Token.Type type, String lexme)
        {
            Token t = curr;
            if (curr.type == Token.Type.EOF)
                return t;
            if (curr.type == type && curr.lexeme == lexme)
            {
                //Console.Write("OK");
                curr = next;
                if (next.type != Token.Type.EOF)
                    next = lexer.Next();
                return t;
            }
            //Console.WriteLine(type.ToString() + " " + lexme);
            //Console.WriteLine(curr.type.ToString() + " " + curr.lexeme);
            //throw new Exception("MATCH\n");
            var e = "Expected " + lexme + ". Found " + curr.lexeme + " at line " + curr.line + " column " + curr.column + ".";
            throw new Exception(e);
        }

        public Statement Parse()
        {
            return ParseC();
        }

        public Statement ParseC()
        {
            var s = ParseCe();
            if (!IsNext(Token.Type.EOF))
            {
                string e = $"Expected EOF. Got {curr.lexeme} at line {curr.line} column {curr.column}\n";
                throw new Exception(e);
            }

            //if ((IsNext(Token.Type.SEP, ";")))
            //    {
            //        var s2 = ParseSp();
            //        if (s2 == null)
            //            return s1;
            //        return new SequenceStatement(s1, s2);
            //    }
            return s;
        }

        public SequenceStatement ParseCe()
        {
            return new SequenceStatement(ParseS(), ParseSp());
        }
        public Statement ParseS()
        {
            if (IsNext(Token.Type.ID))
            {
                var t = Match();
                Match(Token.Type.OP, ":=");
                var e = ParseE();

                return new AssignmentStatement(t.lexeme, e);
            }
            if (IsNext(Token.Type.KEYW, "print"))
            {
                Match();
                Match(Token.Type.SEP, "(");
                var list = ParseL();
                Match(Token.Type.SEP, ")");
                return list;
            }
            Token exToken = lexer.Peek();
            //if (exToken.type == Token.Type.EOF)
            //    return ParseEOF();
            throw new Exception("Expected NUM or ID. Found " + exToken.type.ToString() + " (" + exToken.lexeme + ")" + " at line " + curr.line + " column " + curr.column + ".");

        }

        public SequenceStatement ParseSp()
        {
            if (IsNext(Token.Type.SEP, ";"))
            {
                Match();
                var s1 = ParseS();
                var s2 = ParseSp();
                return new SequenceStatement(s1, s2);
            }
            return null;
        }

        public Expression ParseT()
        {
            if (IsNext(Token.Type.NUM))
            {
                var t = Match();
                return new NumberExpression(t.lexeme);
            }
            if (IsNext(Token.Type.ID))
            {
                var t = Match();
                return new IdentifierExpression(t.lexeme);
            }
            if (IsNext(Token.Type.SEP, "("))
            {
                Match();
                var c = ParseCe();
                Match(Token.Type.SEP, ",");
                var e = ParseE();
                Match(Token.Type.SEP, ")");

                return new LetExpression(c, e);
            }
            if (IsNext(Token.Type.EOF))
                return new EOFExpression();
            Token exToken = lexer.Peek();
            throw new Exception("Expected NUM, ID or (. Found " + exToken.type.ToString() + " " + exToken.lexeme + " at line " + curr.line + " column " + curr.column + ".");
        }

        public Expression ParseE()
        {

            var e = ParseT();
            // var op = new BinaryOperatorExpression(BinaryOperatorExpression.Type.ADD, null, e);
            var ep = ParseEp();
            if (ep == null) return e;
            // var tree = (BinaryOperatorExpression)ep;
            //tree.LeftMost().left = e;
            return new BinaryOperatorExpression(BinaryOperatorExpression.Type.ADD, e, ep);

        }

        public Expression ParseEp()
        {
            if (IsNext(Token.Type.OP, "+"))
            {
                Match();
                var t = ParseE();
                return t;//new BinaryOperatorExpression(BinaryOperatorExpression.Type.ADD, null, t);
            }
            return null;
        }

        public PrintStatement ParseL()
        {
            var e = ParseE();
            LinkedList<Expression> list = new LinkedList<Expression>();
            list.AddLast(e);
            ParseLp(list); ;
            return new PrintStatement(list); //list;
        }

        public void ParseLp(LinkedList<Expression> list)
        {
            if (IsNext(Token.Type.SEP, ","))
            {
                Match();
                var e = ParseE();
                list.AddLast(e);
                ParseLp(list);
            }
        }

    }
}
