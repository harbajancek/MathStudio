using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeAbsolute : Node
    {
        public Node Argument { get; private set; }
        public NodeAbsolute(Node argument)
        {
            Argument = argument;
        }

        public override double Eval(IContext context)
        {
            return Math.Abs(Argument.Eval(context));
        }

        public override Node Simplify()
        {
            Argument = Argument.Simplify();
            return this;
        }
    }
}
