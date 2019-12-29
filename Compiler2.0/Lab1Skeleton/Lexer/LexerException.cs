using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexer
{
    public class LexerException : Exception
    {
        public LexerException()
        {
        }
        public LexerException(string message) : base(message)
        {
        }

        public LexerException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }

    public class LexerExceptionTextNotFound : LexerException
    {
        public LexerExceptionTextNotFound()
        {
        }
        public LexerExceptionTextNotFound(string message) : base(message)
        {
        }
        public LexerExceptionTextNotFound(string message, LexerException inner)
        : base(message, inner)
        {
        }
    }

    public class LexerExceptionTerminatorTokenNotFound : LexerException
    {
        public LexerExceptionTerminatorTokenNotFound()
        {
        }
        public LexerExceptionTerminatorTokenNotFound(string message) : base(message)
        {
        }
        public LexerExceptionTerminatorTokenNotFound(string message, LexerException inner)
        : base(message, inner)
        {
        }
    }
}
