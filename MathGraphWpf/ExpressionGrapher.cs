using MathExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;

namespace MathStudioWpf
{
    public static class ExpressionGrapher
    {
        public static double XMax { get; set; }
        public static double XMin { get; set; }
        public static double YMax { get; set; }
        public static double YMin { get; set; }
        public static double PixelWidth { get; set; }

        public static bool IsExpressionGraphable(string expression)
        {
            var willIsGraphable = true;
            try
            {
                var context = new FunctionContext();
                var tokenizer = new Tokenizer(new StringReader(expression));
                var parser = new Parser(tokenizer);
                var nodeExpression = parser.ParseExpression();

                for (double i = 0; i < 2; i++)
                {
                    context.Variable = i;
                    _ = nodeExpression.Eval(context);
                }
            }
            catch (DivideByZeroException)
            {

            }
            catch (Exception)
            {
                willIsGraphable = false;
            }

            return willIsGraphable;
        }

        public static bool IsLineParametricGraphable(Point point, Point vector)
        {
            return (vector.X == 0 && vector.Y == 0) ? false : true;
        }

        public static bool IsTwoPointsGraphable(Point point1, Point point2)
        {
            return (point1 == point2) ? false : true;
        }

        public static bool IsThreePointsGraphable(Point point1, Point point2, Point point3)
        {
            // TODO
            return (point1 == point2) ? false : true;
        }

        public static IEnumerable<IEnumerable<Point>> GetGraphPointsFromExpression(string expression)
        {
            List<Point> points = new List<Point>();

            Point currentPoint;

            var context = new FunctionContext();
            var tokenizer = new Tokenizer(new StringReader(expression));
            var parser = new Parser(tokenizer);
            var nodeExpression = parser.ParseExpression();

            double? result = default;

            bool wrong = false;
            for (double x = XMin; x < XMax; x += PixelWidth)
            {
                context.Variable = x;
                try
                {
                    result = nodeExpression.Eval(context);
                }
                catch (DivideByZeroException)
                {
                    double y = (currentPoint.Y > 0) ? YMax : YMin;
                    currentPoint = new Point(currentPoint.X, (double)y);
                    wrong = true;
                }
                catch (OverflowException)
                {
                    wrong = true;
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (result > YMax || result < YMin)
                {
                    currentPoint = new Point(0, 0);
                    wrong = true;
                }

                if (wrong || result == null)
                {
                    if (points.Count != 0)
                    {
                        if (!currentPoint.Equals(new Point(0, 0)))
                        {
                            points.Add(currentPoint);
                        }
                        yield return points;
                        points = new List<Point>();
                    }
                    wrong = false;
                    continue;
                }

                currentPoint = new Point((double)context.Variable, (double)result);
                points.Add(currentPoint);
            }

            if (points.Count != 0)
            {
                yield return points;
            }
        }

        public static IEnumerable<IEnumerable<Point>> GetLinePointsFromTwoPoints(Point point1, Point point2)
        {
            if (point1.X == point2.X)
            {
                double x = point1.X;
                yield return new List<Point>()
                {
                    new Point(x, YMax),
                    new Point(x, YMin)
                };
            }
            else
            {
                // y = k * x + q
                double k = (point1.Y - point2.Y) / (point1.X - point2.X);
                double q = point1.X * (point1.Y - point2.Y) / (point2.X - point1.X) + point1.Y;

                foreach (var item in GetGraphPointsFromExpression($"{k} * x + {q}"))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<IEnumerable<Point>> GetLinePointsFromThreePoints(Point point1, Point point2, Point point3)
        {
            // TODO
            if (point1.X == point2.X)
            {
                double x = point1.X;
                yield return new List<Point>()
                {
                    new Point(x, YMax),
                    new Point(x, YMin)
                };
            }
            else
            {
                // y = k * x + q
                double k = (point1.Y - point2.Y) / (point1.X - point2.X);
                double q = point1.X * k + point2.X;

                foreach (var item in GetGraphPointsFromExpression($"{k.ToString("0.#")} * x + {q.ToString("0.#")}"))
                {
                    yield return item;
                }
            }
        }

        public static IEnumerable<IEnumerable<Point>> GetLinePointsFromParametres(Point point, Point vector)
        {
            Point point1 = point;
            Point point2 = new Point(point.X + vector.X, point.Y + vector.Y);

            foreach (var item in GetLinePointsFromTwoPoints(point1, point2))
            {
                yield return item;
            }
        }
    }
}
