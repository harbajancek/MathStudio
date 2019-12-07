using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeNumber : Node
    {
        private decimal Number { get; }
        public NodeNumber(decimal number)
        {
            Number = number;
        }

        public override decimal? Eval(IContext context)
        {
            return Number;
        }
    }
}
