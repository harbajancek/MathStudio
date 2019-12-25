using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    public class FunctionContext : IContext
    {
        public decimal Variable { get; set; }

        public decimal ResolveVariable(string name)
        {
            return Variable;
        }
    }
}
