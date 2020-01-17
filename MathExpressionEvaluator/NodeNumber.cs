using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeNumber : Node
    {
        private double Number { get; }
        public NodeNumber(double number)
        {
            Number = number;
        }

        public override double? Eval(IContext context)
        {
            return Number;
        }
    }
}
