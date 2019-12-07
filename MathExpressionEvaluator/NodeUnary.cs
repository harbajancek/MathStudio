using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeUnary : Node
    {
        private Node Right { get; }
        private Func<decimal?, decimal?> Operation { get; }

        public NodeUnary(Node right, Func<decimal?, decimal?> operation)
        {
            Right = right;
            Operation = operation;
        }

        public override decimal? Eval(IContext context)
        {
            decimal? rightValue = Right.Eval(context);

            return Operation(rightValue);
        }
    }
}
