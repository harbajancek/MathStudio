using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeUnary : Node
    {
        private Node Right { get; }
        private Func<double?, double?> Operation { get; }

        public NodeUnary(Node right, Func<double?, double?> operation)
        {
            Right = right;
            Operation = operation;
        }

        public override double? Eval(IContext context)
        {
            double? rightValue = Right.Eval(context);

            return Operation(rightValue);
        }
    }
}
