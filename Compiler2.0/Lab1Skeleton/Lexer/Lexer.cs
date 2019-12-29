using System;
using System.IO;



namespace Lexer
{

    public class Lexer
    {
        #region Attributes
        private string stream;
        private int curr;//current 
        private int line;
        private int column;
        private Token buf;
        #endregion

        #region Constructors
        public Lexer(string text)
        {
            this.stream = text;

            if (text.Length == 0)
                throw new LexerExceptionTextNotFound("Text source not found!");
            if (!stream.Contains(string.Empty))
                throw new LexerExceptionTerminatorTokenNotFound("Terminator token not found!");

            this.curr = 0;
            this.line = this.column = 1;
        }
        #endregion

        #region Functions
        public Token Peek()
        {
            if (this.buf != null)
                return this.buf;
            if (curr >= stream.Length)
                return new Token(Token.Type.EOF, string.Empty, this.line, this.column);
            int tmp = curr;

            string nl = "" + stream[tmp];
            if (tmp < stream.Length - 1)
                nl += stream[tmp + 1];

            if (nl.Length > 0 && tmp < stream.Length)
                while (stream[tmp] == ' ' || nl[0] == '\r' || nl[0] == '\n' || stream[tmp] == '\t')
                {
                    if (stream[tmp] == ' ')
                    {
                        tmp++;
                        this.column++;
                    }
                    else if (stream[tmp] == '\t')
                    {
                        tmp++;
                        this.column += 8;
                    }
                    else
                    {
                        this.line++;
                        this.column = 1;
                        if (nl.Length == 2 && nl[0] == '\r' && nl[1] == '\n')
                        {
                            tmp++;
                        }
                        tmp++;
                    }
                    if (tmp < stream.Length)
                    {
                        nl = "" + stream[tmp];
                        if (tmp < stream.Length - 1)
                            nl += stream[tmp + 1];
                    }

                    if (tmp >= this.stream.Length)
                        return new Token(Token.Type.EOF, string.Empty, this.line, this.column);
                }
            Token.Type t = Token.Type.EOF;
            string chk = "" + stream[tmp];
            if (Environment.NewLine.Length == 2 && tmp < stream.Length - 1)
                chk += stream[tmp + 1];
            if (stream[tmp] >= 'a' && stream[tmp] <= 'z' || stream[tmp] >= 'A' && stream[tmp] <= 'Z')
                t = Token.Type.ID;
            else if (stream[tmp] >= '0' && stream[tmp] <= '9')
                t = Token.Type.NUM;
            else if (stream[tmp] == ';' || stream[tmp] == '(' || stream[tmp] == ')' || stream[tmp] == ',')
                t = Token.Type.SEP;
            else if (stream[tmp] == '+' || (stream.Length >= tmp + 1) && (stream[tmp] == ':' && stream[tmp + 1] == '='))
                t = Token.Type.OP;
            else if ((stream.Length >= tmp + 1) && (chk == string.Empty))
                t = Token.Type.EOF;

            int tmpLine = this.line, tmpColumn = this.column;

            Token token;
            string input = "";

            switch (t)
            {
                case Token.Type.ID:
                    {
                        CreateTokenLex(ref tmp, ref input, ref tmpColumn, Token.Type.ID);

                        if (input == "print")
                            t = Token.Type.KEYW;
                    }
                    break;
                case Token.Type.NUM:
                    {
                        CreateTokenLex(ref tmp, ref input, ref tmpColumn, Token.Type.NUM);
                    }
                    break;
                case Token.Type.SEP:
                    {
                        input += stream[tmp];
                        tmp++;
                        tmpColumn++;
                    }
                    break;
                case Token.Type.OP:
                    {
                        if (stream[tmp] == '+')
                        {
                            input += '+';
                            tmp++;
                            tmpColumn++;
                        }
                        else
                        {
                            input += ":=";
                            tmp += 2;
                            tmpColumn += 2;
                            //Exception ako je zadnje sto postoji
                        }
                    }
                    break;
                case Token.Type.EOF:
                    {
                        input = string.Empty;
                        tmp++;
                        tmpColumn++;
                    }
                    break;
                default: break;
            }
            token = new Token(t, input, this.line, this.column);

            this.line = tmpLine;
            this.column = tmpColumn;
            this.curr = tmp;

            return token;

        }
        public Token Next()
        {
            Token t = Peek();
            buf = null;
            return t;
        }
        private bool IsNotEndOfToken(int i, Token.Type t)
        {
            if (i >= stream.Length)
                return false;
            if (t == Token.Type.ID)
            {
                return (stream[i] >= 'a' && stream[i] <= 'z') || (stream[i] >= 'A' && stream[i] <= 'Z') || (stream[i] >= '0' && stream[i] <= '9');
            }
            if (t == Token.Type.NUM)
            {
                return (stream[i] >= '0' && stream[i] <= '9');
            }
            return false;

        }
        private void CreateTokenLex(ref int tmp, ref string input, ref int tmpColumn, Token.Type t)
        {

            while (tmp < stream.Length && IsNotEndOfToken(tmp, t))
            {
                input += this.stream[tmp];
                tmp++;
                tmpColumn++;
            }

        }
        #endregion
    }
}
