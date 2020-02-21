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

            return expression.Simplify();
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



        private static BinaryOperation FromToken(Token token) => token switch
        {
            Token.Add => new BinaryOperation((a, b) => a + b, OperationType.Addition),
            Token.Subtract => new BinaryOperation((a, b) => a - b, OperationType.Subtraction),
            Token.Multiply => new BinaryOperation((a, b) => a * b, OperationType.Multiplication),
            Token.Divide => new BinaryOperation((a, b) => a / b, OperationType.Division),
            Token.Raise => new BinaryOperation((a, b) => Math.Pow(a, b), OperationType.Exponentiaton),
            _ => null
        };
    }
}
