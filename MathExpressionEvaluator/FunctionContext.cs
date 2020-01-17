using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    public class FunctionContext : IContext
    {
        public double Variable { get; set; }

        public double ResolveVariable(string name)
        {
            return Variable;
        }
    }
}
