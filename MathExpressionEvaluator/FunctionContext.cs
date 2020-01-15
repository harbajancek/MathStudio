using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    public class FunctionContext : IContext
    {
        public float Variable { get; set; }

        public float ResolveVariable(string name)
        {
            return Variable;
        }
    }
}
