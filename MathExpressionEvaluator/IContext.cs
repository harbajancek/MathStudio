using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    public interface IContext
    {
        public double ResolveVariable(string name);
        public double CallFunction(string functionName, double[] arguments)
        {
            double result = functionName switch
            {
                "log" => Math.Log(arguments[0], arguments[1]),
                "sin" => Math.Sin(arguments[0]),
                "tan" => Math.Tan(arguments[0]),
                "cos" => Math.Cos(arguments[0]),
                "pow" => Math.Pow(arguments[0], arguments[1]),
                "root" => Math.Pow(arguments[0], 1/arguments[1]),
                _ => throw new Exception("Function name not recognized")
            };

            return result;
        }
    }
}
