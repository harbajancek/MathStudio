using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeUnary : Node
    {
        private Node Right { get; }
        private Func<float?, float?> Operation { get; }

        public NodeUnary(Node right, Func<float?, float?> operation)
        {
            Right = right;
            Operation = operation;
        }

        public override float? Eval(IContext context)
        {
            float? rightValue = Right.Eval(context);

            return Operation(rightValue);
        }
    }
}
