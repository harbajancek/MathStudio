using MathExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace MathConsoleTest
{
    class Tests
    {
        public static void FunctionContextPerformanceTest(string expression, int testCount, FunctionContext context, decimal maxValue)
        {
            Parser parser = new Parser(new Tokenizer(new StringReader(expression)));
            var nodeExpression = parser.ParseExpression();
            Stopwatch stopwatch = new Stopwatch();
            List<TimeSpan> times = new List<TimeSpan>();
            List<List<decimal>> variableLists = new List<List<decimal>>();
            for (int i = 0; i < testCount; i++)
            {
                Console.WriteLine($"Starting test {i}");
                List<decimal> variables = new List<decimal>();
                stopwatch.Reset();
                stopwatch.Start();
                int step = 0;
                do
                {
                    context.Step = step;
                    try
                    {
                        variables.Add(nodeExpression.Eval(context));
                    }
                    catch (DivideByZeroException)
                    {
                    }
                    
                    //Console.WriteLine($"{context.CurrentVariableValue} * {context.CurrentVariableValue} = " + expression.Eval(context).ToString("0.############################", CultureInfo.InvariantCulture));
                    step++;

                } while (context.CurrentVariableValue <= maxValue);
                stopwatch.Stop();
                variableLists.Add(variables);
                times.Add(stopwatch.Elapsed);
                Console.WriteLine($"Test {i} results:\n\tTime elapsed: {stopwatch.Elapsed}\n");

            }

            TimeSpan combined = new TimeSpan();
            TimeSpan max = times[0];
            int maxVar = 0;
            TimeSpan min = times[0];
            int minVar = 0;
            int index = 0;
            foreach (var item in times)
            {
                if (item > max)
                {
                    max = item;
                    maxVar = index;
                }
                if (item < min)
                {
                    min = item;
                    minVar = index;
                }
                combined += item;
                index++;
            }

            TimeSpan average = combined / times.Count;

            Console.WriteLine($"Number of tests: {times.Count}\nAverage time: {average}\nMax time: {max} (Test {maxVar})\nMin time: {min} (Test {minVar})");
        }
    }
}
