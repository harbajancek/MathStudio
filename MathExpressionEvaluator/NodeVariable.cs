using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeVariable : Node
    {
        private string Variable { get; }
        public NodeVariable(string variable)
        {
            Variable = variable;
        }
        public override decimal? Eval(IContext context)
        {
            return context.ResolveVariable(Variable);
        }
    }
}
