using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeBinaryOperation : Node
    {
        private Node Left { get; }
        private Node Right { get; }
        private Func<double?, double?, double?> Operation { get; }

        public NodeBinaryOperation(Node left, Node right, Func<double?, double?, double?> operation)
        {
            Left = left;
            Right = right;
            Operation = operation;
        }

        public override double? Eval(IContext context)
        {
            double? leftValue = Left.Eval(context);
            double? rightValue = Right.Eval(context);

            return Operation(leftValue, rightValue);
        }
    }
}
