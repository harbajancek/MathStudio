using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeNumber : Node
    {
        private float Number { get; }
        public NodeNumber(float number)
        {
            Number = number;
        }

        public override float? Eval(IContext context)
        {
            return Number;
        }
    }
}
