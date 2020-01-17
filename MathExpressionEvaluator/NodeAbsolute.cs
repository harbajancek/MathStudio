using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeAbsolute : Node
    {
        private Node Argument { get; }
        public NodeAbsolute(Node argument)
        {
            Argument = argument;
        }

        public override double? Eval(IContext context)
        {
            return Math.Abs((double)Argument.Eval(context));
        }
    }
}
