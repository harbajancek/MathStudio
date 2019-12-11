using MathExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace ConsoleTest
{
    struct PointDecimal
    {
        public decimal X { get; set; }
        public decimal Y { get; set; }

        public PointDecimal(decimal x, decimal y)
        {
            X = x;
            Y = y;
        }
    }
    struct PointDouble
    {
        public double X { get; set; }
        public double Y { get; set; }

        public PointDouble(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
    class Tests
    {
        public static Point Point[] GetIntersectionPoints(Geometry g1, Geometry g2)
        {
            Geometry og1 = g1.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0));
            Geometry og2 = g2.GetWidenedPathGeometry(new Pen(Brushes.Black, 1.0));
            CombinedGeometry cg = new CombinedGeometry(GeometryCombineMode.Intersect, og1, og2);
            PathGeometry pg = cg.GetFlattenedPathGeometry();
            Point[] result = new Point[pg.Figures.Count];
            for (int i = 0; i < pg.Figures.Count; i++)
            {
                Rect fig = new PathGeometry(new PathFigure[] { pg.Figures[i] }).Bounds;
                result[i] = new Point(fig.Left + fig.Width / 2.0, fig.Top + fig.Height / 2.0);
            }
            return result;
        }

        public static void EquationPerformanceTestDecimal(string expression, decimal maxValue, decimal equalTo)
        {
            Parser parser = new Parser(new Tokenizer(new StringReader(expression)));
            var expressionNode = parser.ParseExpression();
            Stopwatch stopwatch = new Stopwatch();
            List<PointDecimal> points = new List<PointDecimal>();
            TestEquationContextDecimal context = new TestEquationContextDecimal(0, 0);
            PointDecimal point = new PointDecimal();

            stopwatch.Start();
            for (decimal x = -maxValue; x <= maxValue; x += 0.1m)
            {
                for (decimal y = -maxValue; y <= maxValue; y += 0.1m)
                {
                    context.X = x;
                    context.Y = y;

                    decimal? result = null;
                    result = expressionNode.Eval(context);

                    Console.WriteLine($"Testing X:{x} Y:{y}");

                    if (result != null && result == equalTo)
                    {
                        point = new PointDecimal(x, y);
                        points.Add(point);
                    }
                }
            }
            stopwatch.Stop();

            for (int i = 0; i < points.Count; i++)
            {
                Console.WriteLine($"POINT {i}:\n\tX: {points[i].X}\n\tY: {points[i].Y}");
            }

            Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
        }

        public static void EquationPerformanceTestDouble(string expression, double maxValue, double equalTo)
        {
            Parser parser = new Parser(new Tokenizer(new StringReader(expression)));
            var expressionNode = parser.ParseExpression();
            Stopwatch stopwatch = new Stopwatch();
            List<PointDouble> points = new List<PointDouble>();
            TestEquationContextDecimal context = new TestEquationContextDecimal(0, 0);
            PointDouble point = new PointDouble();

            stopwatch.Start();
            for (double x = -maxValue; x <= maxValue; x += 0.1)
            {
                for (double y = -maxValue; y <= maxValue; y += 0.1)
                {
                    context.X = x;
                    context.Y = y;

                    decimal? result = null;
                    result = expressionNode.Eval(context);

                    Console.WriteLine($"Testing X:{x} Y:{y}");

                    if (result != null && result == equalTo)
                    {
                        point = new PointDouble(x, y);
                        points.Add(point);
                    }
                }
            }
            stopwatch.Stop();

            for (int i = 0; i < points.Count; i++)
            {
                Console.WriteLine($"POINT {i}:\n\tX: {points[i].X}\n\tY: {points[i].Y}");
            }

            Console.WriteLine($"Time elapsed: {stopwatch.Elapsed}");
        }

        public static void FunctionContextPerformanceTest(string expression, int testCount, FunctionContext context, decimal maxValue)
        {
            /*
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
            */
        }
    }
}
