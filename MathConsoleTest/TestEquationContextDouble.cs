using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleTest
{
    class TestEquationContextDouble
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }
        public TestEquationContextDecimal(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }
        public decimal? CallFunction(string functionName, decimal?[] arguments)
        {
            double result = functionName switch
            {
                "log" => Math.Log((double)arguments[0], (double)arguments[1]),
                "sin" => Math.Sin((double)arguments[0]),
                "tan" => Math.Tan((double)arguments[0]),
                "cos" => Math.Cos((double)arguments[0]),
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

        public decimal ResolveVariable(string name)
        {
            if (name == "x")
            {
                return X;
            }
            else if (name == "y")
            {
                return Y;
            }
            else
            {
                throw new Exception("Variable name not recognized");
            }
        }
    }
}
