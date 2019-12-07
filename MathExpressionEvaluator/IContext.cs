using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    public interface IContext
    {
        public decimal ResolveVariable(string name);
        public decimal? CallFunction(string functionName, decimal?[] arguments);
    }
}
