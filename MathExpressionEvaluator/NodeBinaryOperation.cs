using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeBinaryOperation : Node
    {
        private Node Left { get; }
        private Node Right { get; }
        private Func<decimal?, decimal?, decimal?> Operation { get; }

        public NodeBinaryOperation(Node left, Node right, Func<decimal?, decimal?, decimal?> operation)
        {
            Left = left;
            Right = right;
            Operation = operation;
        }

        public override decimal? Eval(IContext context)
        {
            decimal? leftValue = Left.Eval(context);
            decimal? rightValue = Right.Eval(context);

            return Operation(leftValue, rightValue);
        }
    }
}
