using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeVariable : Node
    {
        public string Variable { get; private set; }
        public NodeVariable(string variable)
        {
            Variable = variable;
        }
        public override double Eval(IContext context)
        {
            return context.ResolveVariable(Variable);
        }

        public override Node Simplify()
        {
            return this;
        }
    }
}
