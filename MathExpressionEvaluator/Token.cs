using System;

namespace MathExpressionEvaluator
{
    public enum Token
    {
        EndOfExpression,
        Subtract,
        Add,
        Number,
        Multiply,
        Divide,
        OpenParenthesis,
        CloseParenthesis,
        Identifier,
        Comma,
        Raise,
        AbsoluteParenthesis
    }
}
