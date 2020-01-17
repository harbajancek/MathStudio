using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    public interface IContext
    {
        public double ResolveVariable(string name);
        public double? CallFunction(string functionName, double?[] arguments)
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

            if (result > (double)double.MaxValue)
            {
                return double.MaxValue;
            }

            if (result < (double)double.MinValue)
            {
                return double.MinValue;
            }

            if (double.IsNaN(result))
            {
                return null;
            }

            return (double)result;
        }
    }
}
