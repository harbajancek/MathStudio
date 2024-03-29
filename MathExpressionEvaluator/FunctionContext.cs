﻿using System;
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

        public decimal? CallFunction(string functionName, decimal?[] arguments) 
        {
            double result = functionName switch
            {
                "log" => Math.Log((double)arguments[0], (double)arguments[1]),
                "sin" => Math.Sin((double)arguments[0]),
                "tan" => Math.Tan((double)arguments[0]),
                "cos" => Math.Cos((double)arguments[0]),
                "abs" => Math.Abs((double)arguments[0]),
                "pow" => Math.Pow((double)arguments[0], (double)arguments[1]),
                _ => throw new Exception("Function name not recognized")
            };

            if (result > (double)decimal.MaxValue)
            {
                return decimal.MaxValue;
            }

            if (result < (double)decimal.MinValue)
            {
                return decimal.MinValue;
            }

            if (double.IsNaN(result))
            {
                return null;
            }

            return (decimal)result;
        }
    }
}
