using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    public class Parser
    {
        private Tokenizer Tokenizer { get; }
        private bool isAbsolute = false;
        public Parser(Tokenizer tokenizer)
        {
            Tokenizer = tokenizer;
        }

        public Node ParseExpression()
        {
            var expression = parseAddSubtract();

            if (Tokenizer.Token != Token.EndOfExpression)
            {
                throw new Exception($"Unexpected characters at the end of an expression.");
            }

            return expression;
        }

        private Node parseAddSubtract()
        {
            var left = parseMultiplyDivide();

            while (true)
            {
                var operation = FromToken(Tokenizer.Token);

                if (operation == null)
                {
                    return left;
                }

                Tokenizer.NextToken();

                var right = parseMultiplyDivide();

                left = new NodeBinaryOperation(left, right, operation);
            }
        }

        private Node parseMultiplyDivide()
        {
            var left = parseUnary();

            while (true)
            {
                if (Tokenizer.Token != Token.Multiply && Tokenizer.Token != Token.Divide && Tokenizer.Token != Token.Raise)
                {
                    return left;
                }

                var operation = FromToken(Tokenizer.Token);

                Tokenizer.NextToken();

                var right = parseUnary();

                left = new NodeBinaryOperation(left, right, operation);
            }
        }

        private Node parseUnary()
        {
            if (Tokenizer.Token == Token.Add)
            {
                Tokenizer.NextToken();
                return parseUnary();
            }
            else if (Tokenizer.Token == Token.Subtract)
            {
                Tokenizer.NextToken();

                var right = parseUnary();

                return new NodeUnary(right, (a) => -a);
            }
            else
            {
                return parseLeaf();
            }
        }

        private Node parseLeaf()
        {
            if (Tokenizer.Token == Token.Number)
            {
                var node = new NodeNumber(Tokenizer.Number);
                Tokenizer.NextToken();
                return node;
            }
            else if (Tokenizer.Token == Token.OpenParenthesis)
            {
                Tokenizer.NextToken();

                var node = parseAddSubtract();

                if (Tokenizer.Token != Token.CloseParenthesis)
                {
                    throw new Exception("Missing close parenthesis");
                }

                Tokenizer.NextToken();
                return node;
            }
            else if (Tokenizer.Token == Token.AbsoluteParenthesis)
            {
                Tokenizer.NextToken();

                isAbsolute = true;

                var node = parseAddSubtract();

                if (Tokenizer.Token != Token.AbsoluteParenthesis)
                {
                    throw new Exception("Missing closing absolute parenthesis");
                }

                Tokenizer.NextToken();
                return new NodeAbsolute(node);
                
            }
            else if (Tokenizer.Token == Token.Identifier)
            {
                var name = Tokenizer.Identifier;
                Tokenizer.NextToken();

                if (Tokenizer.Token != Token.OpenParenthesis)
                {
                    return new NodeVariable(Tokenizer.Identifier);
                }
                else
                {
                    Tokenizer.NextToken();

                    var arguments = new List<Node>();

                    while (true)
                    {
                        arguments.Add(parseAddSubtract());

                        if (Tokenizer.Token == Token.Comma)
                        {
                            Tokenizer.NextToken();
                            continue;
                        }

                        break;
                    }

                    if (Tokenizer.Token == Token.CloseParenthesis)
                    {
                        Tokenizer.NextToken();
                        return new NodeFunctionCall(name, arguments.ToArray());
                    }
                    else
                    {
                        throw new Exception("Missing close parenthesis in function call.");
                    }
                }
            }
            else
            {
                throw new Exception($"Unexpected token: {Tokenizer.Token}");
            }
        }

        

        private static Func<double?, double?, double?> FromToken(Token token) => token switch
        {
            Token.Add => (a, b) => a + b,
            Token.Subtract => (a, b) => a - b,
            Token.Multiply => (a, b) => a * b,
            Token.Divide => (a, b) =>
            {
                if (b == 0)
                {
                    throw new DivideByZeroException();
                }
                return a / b;
            },
            Token.Raise => (a,b) => (double)Math.Pow((double)a,(double)b),
            _ => null
        };
    }
}
