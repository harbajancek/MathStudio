using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    public interface IContext
    {
        public float ResolveVariable(string name);
        public float? CallFunction(string functionName, float?[] arguments)
        {
            double result = functionName switch
            {
                "log" => Math.Log((double)arguments[0], (double)arguments[1]),
                "sin" => Math.Sin((double)arguments[0]),
                "tan" => Math.Tan((double)arguments[0]),
                "cos" => Math.Cos((double)arguments[0]),
                "pow" => Math.Pow((double)arguments[0], (double)arguments[1]),
                _ => throw new Exception("Function name not recognized")
            };

            if (result > (double)float.MaxValue)
            {
                return float.MaxValue;
            }

            if (result < (double)float.MinValue)
            {
                return float.MinValue;
            }

            if (double.IsNaN(result))
            {
                return null;
            }

            return (float)result;
        }
    }
}
