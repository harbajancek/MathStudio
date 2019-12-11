using MathExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.IO;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            string expression = "x^2/1 + y^2/1";
            decimal equalTo = 1;
            decimal maxValue = 10;
            Tests.EquationPerformanceTestDecimal(expression, maxValue, equalTo);
        }

        
    }
}
