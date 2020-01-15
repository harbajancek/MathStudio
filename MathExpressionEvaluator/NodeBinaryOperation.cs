using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeBinaryOperation : Node
    {
        private Node Left { get; }
        private Node Right { get; }
        private Func<float?, float?, float?> Operation { get; }

        public NodeBinaryOperation(Node left, Node right, Func<float?, float?, float?> operation)
        {
            Left = left;
            Right = right;
            Operation = operation;
        }

        public override float? Eval(IContext context)
        {
            float? leftValue = Left.Eval(context);
            float? rightValue = Right.Eval(context);

            return Operation(leftValue, rightValue);
        }
    }
}
