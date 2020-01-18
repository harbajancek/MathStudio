using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeNumber : Node
    {
        public double Number { get; private set; }
        public NodeNumber(double number)
        {
            Number = number;
        }

        public override double Eval(IContext context)
        {
            return Number;
        }

        public override Node Simplify()
        {
            return this;
        }
    }
}
