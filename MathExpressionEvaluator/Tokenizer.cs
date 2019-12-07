using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace MathExpressionEvaluator
{
    public class Tokenizer
    {
        TextReader Reader { get; }
        public Token Token { get; private set; }
        public decimal Number { get; private set; }
        public string Identifier { get; private set; }
        private char CurrentChar { get; set; }
        public Tokenizer(TextReader reader)
        {
            Reader = reader;
            nextChar();
            NextToken();
        }

        private void nextChar()
        {
            int i = Reader.Read();
            CurrentChar = i >= 0 ? (char)i : '\0';
        }

        public void NextToken()
        {
            while (char.IsWhiteSpace(CurrentChar))
            {
                nextChar();
            }

            Token = CurrentChar switch
            {
                '\0' => Token.EndOfExpression,
                '+' => Token.Add,
                '-' => Token.Subtract,
                '*' => Token.Multiply,
                '/' => Token.Divide,
                '(' => Token.OpenParenthesis,
                ')' => Token.CloseParenthesis,
                ',' => Token.Comma,
                _ when char.IsDigit(CurrentChar) => Token.Number,
                _ when char.IsLetter(CurrentChar) => Token.Identifier,
                _ => throw new InvalidDataException($"Unexpected character: {CurrentChar}")
            };

            if (Token == Token.Number)
            {
                StringBuilder sb = new StringBuilder();
                bool hasDecimalPoint = false;
                while (char.IsDigit(CurrentChar) || (!hasDecimalPoint && CurrentChar == '.'))
                {
                    sb.Append(CurrentChar);
                    hasDecimalPoint = CurrentChar == '.';
                    nextChar();
                }

                Number = decimal.Parse(sb.ToString(), CultureInfo.InvariantCulture);
            }
            else if (Token == Token.Identifier)
            {
                StringBuilder sb = new StringBuilder();
                while (char.IsLetter(CurrentChar))
                {
                    sb.Append(CurrentChar);
                    nextChar();
                }

                Identifier = sb.ToString();
            }
            else
            {
                nextChar();
            }
        }
    }
}
