using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    public abstract class Node
    {
        public abstract float? Eval(IContext context);
    }
}
