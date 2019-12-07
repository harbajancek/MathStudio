using MathExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.IO;

namespace MathConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            int testCount = 1;


            var context = new FunctionContext(-200, (decimal).001);
            var expression = "2 / (n + 2) + 2";
            decimal maxValue = 200;

            FunctionContextPerformanceTest(expression, testCount, context, maxValue);
            */

            var expression = "1 / x";
            var context = new FunctionContext(-768, (decimal).1);

            Tests.FunctionContextPerformanceTest(expression, 20, context, 768);
            /*
            var tokenizer = new Tokenizer(new StringReader(expression));
            var tokens = new List<Token>();
            while (true)
            {
                tokens.Add(tokenizer.Token);
                if (tokenizer.Token == Token.EndOfExpression)
                {
                    break;
                }
                tokenizer.NextToken();
            }

            foreach (var item in tokens)
            {
                Console.WriteLine(item.ToString());
            }

            var context = new FunctionContext(0);
            tokenizer = new Tokenizer(new StringReader(expression));
            var parser = new Parser(tokenizer);
            var nodeExpression = parser.ParseExpression();
            var result = nodeExpression.Eval(context);
            Console.WriteLine($"Result: {result}");
            */
        }

        
    }
}
