using MathExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Linq;
using static MathStudioWpf.ThreePointsModel;
using System.Diagnostics;

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
            if (point1 == point2 || point1 == point3 || point2 == point3)
            {
                return false;
            }

            if (((point2.Y - point1.Y) / (point2.X - point1.X)) == ((point3.Y - point1.Y) / (point3.X - point1.X)))
            {
                return false;
            }

            return true;
        }

        public static bool IsConicSectionGraphable(double a, double b, double c, double d, double e)
        {
            if (c == 0 && c == d && c == e)
            {
                return false;
            }

            if (b == 0 && d == 0)
            {
                return false;
            }

            if (a == 0 && b == 0)
            {
                return false;
            }

            return true;
        }

        public static ConicSectionType GetConicSectionType(double a, double b, double c, double d, double e)
        {
            if (!IsConicSectionGraphable(a, b, c, d, e))
            {
                return ConicSectionType.None;
            }

            if ((a == 0 || b == 0) && a != b)
            {
                return ConicSectionType.Parabola;
            }

            if (Math.Sign(a) != Math.Sign(b))
            {
                return ConicSectionType.Hyperbola;
            }
            else
            {
                if (a == b)
                {
                    return ConicSectionType.Circle;
                }
                else
                {
                    return ConicSectionType.Ellipse;
                }
            }
        }

        public static IEnumerable<IEnumerable<Point>> GetGraphPointsFromExpression(string expression)
        {
            List<Point> points = new List<Point>();

            Point currentPoint;

            var context = new FunctionContext();
            var tokenizer = new Tokenizer(new StringReader(expression));
            var parser = new Parser(tokenizer);
            var nodeExpression = parser.ParseExpression();

            double result = default;

            bool overflow = true;
            double YTemp = double.NaN;
            for (double x = XMin; x < XMax; x += PixelWidth)
            {
                context.Variable = x;
                try
                {
                    result = nodeExpression.Eval(context);
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (double.IsInfinity(result))
                {
                    Debug.Write("infinityalert");
                }

                if (double.IsNaN(result))
                {
                    if (points.Count > 0)
                    {
                        yield return points;
                        points = new List<Point>();
                    }
                    continue;
                }

                if (result > YMax || result < YMin)
                {
                    bool overflowTemp = true;

                    if (!overflow)
                    {
                        if (result > YMax)
                        {
                            currentPoint = new Point(context.Variable, YMax);
                        }
                        else
                        {
                            currentPoint = new Point(context.Variable, YMin);
                        }

                        points.Add(currentPoint);
                        yield return points;
                        points = new List<Point>();
                    }

                    YTemp = (result > YMax) ? YMax : YMin;
                    overflow = overflowTemp;
                }
                else
                {
                    bool overflowTemp = false;

                    if (overflow && !double.IsNaN(YTemp))
                    {
                        points = new List<Point>();
                        currentPoint = new Point(context.Variable, YTemp);
                        points.Add(currentPoint);
                    }

                    overflow = overflowTemp;
                }

                if (!overflow)
                {
                    currentPoint = new Point(context.Variable, result);
                    points.Add(currentPoint);
                }
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

        public static IEnumerable<IEnumerable<Point>> GetLinePointsFromThreePoints(Point point1, Point point2, Point point3, ThreePointsType type)
        {
            if (type == ThreePointsType.Circle)
            {
                double osa1x = point2.X - point1.X;
                double osa1y = point2.Y - point1.Y;
                double osa1P = (osa1x * (point2.X + point1.X)) / 2 + (osa1y * (point2.Y + point1.Y)) / 2;

                double osa2x = point3.X - point1.X;
                double osa2y = point3.Y - point1.Y;
                double osa2P = (osa2x * (point1.X + point3.X) / 2) + (osa2y * (point3.Y + point1.Y) / 2);

                double detA = osa1x * osa2y - osa1y * osa2x;
                double detx = osa1P * osa2y - osa1y * osa2P;
                double dety = osa1x * osa2P - osa2x * osa1P;

                double m = detx / detA;
                double n = dety / detA;
                double r2 = Math.Pow(point1.X - m, 2) + Math.Pow(point1.Y - n, 2);
                double r = Math.Pow(r2, .5);

                double a = 1;
                double b = 1;
                double c = -2 * m;
                double d = -2 * n;
                double e = -r2 + Math.Pow(m, 2) + Math.Pow(n, 2);

                foreach (var item in GetPointsFromConicSection(a, b, c, d, e))
                {
                    yield return item;
                }
            }
            else if (type == ThreePointsType.Parabola)
            {

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

        public static IEnumerable<IEnumerable<Point>> GetPointsFromConicSection(double a, double b, double c, double d, double e, double f = 0)
        {
            if (b == 0)
            {
                string y = $"(-{a} * (x^2) - {c} * x - {e})/({d} + {f} * x)";

                foreach (var item in GetGraphPointsFromExpression(y))
                {
                    yield return item;
                }
            }
            else
            {
                string discriminant = $"({f} * x + {d})^2 - 4 * {b} * ({e} + ({a} * (x^2)) + ({c} * x))";

                string y1 = $"(-({f} * x + {d}) + root(({discriminant}), 2))/(2*{b})";
                string y2 = $"(-({f} * x + {d}) - root(({discriminant}), 2))/(2*{b})";

                List<IEnumerable<Point>> return1 = new List<IEnumerable<Point>>();
                List<IEnumerable<Point>> return2 = new List<IEnumerable<Point>>();

                foreach (var item in GetGraphPointsFromExpression(y1))
                {
                    return1.Add(item);
                }

                foreach (var item in GetGraphPointsFromExpression(y2))
                {
                    return2.Add(item);
                }

                List<IEnumerable<Point>> fillreturn = new List<IEnumerable<Point>>();

                foreach (var points1 in return1)
                {
                    foreach (var points2 in return2)
                    {
                        Point first1 = points1.First();
                        Point first2 = points2.First();
                        Vector subtr = Point.Subtract(first1, first2);
                        if (subtr.Length / PixelWidth < 100 && subtr.Length != 0)
                        {
                            List<Point> fill = new List<Point>()
                            {
                                first1, first2
                            };
                            fillreturn.Add(fill);
                        }

                        Point last1 = points1.Last();
                        Point last2 = points2.Last();
                        subtr = Point.Subtract(last1, last2);
                        if (subtr.Length / PixelWidth < 100 && subtr.Length != 0)
                        {
                            List<Point> fill = new List<Point>()
                            {
                                last1, last2
                            };
                            fillreturn.Add(fill);
                        }
                    }
                }

                foreach (var item in return1)
                {
                    yield return item;
                }
                foreach (var item in return2)
                {
                    yield return item;
                }
                foreach (var item in fillreturn)
                {
                    yield return item;
                }
            }

            //static IEnumerable<IEnumerable<Point>> ImproveConicSectionGraphing(List<IEnumerable<Point>> return1)
            //{
            //    if (return1.Count == 2)
            //    {
            //        if (b != 0 && !(a < 0 && b > 0))
            //        {
            //            if (a == 0)
            //            {
            //                double distanceY1 = Math.Abs(return1[0].First().Y - return1[1].First().Y);
            //                double distanceY2 = Math.Abs(return1[0].Last().Y - return1[1].Last().Y);

            //                if (distanceY1 < distanceY2)
            //                {
            //                    return1[1] = return1[1].Reverse();
            //                    return1[1] = return1[1].Concat(return1[0]);
            //                    return1.RemoveAt(0);
            //                }
            //                else
            //                {
            //                    return1[1] = return1[1].Reverse();
            //                    return1[0] = return1[0].Concat(return1[1]);
            //                    return1.RemoveAt(1);
            //                }
            //            }
            //            else
            //            {
            //                double distanceX1 = Math.Abs(return1[0].First().X - return1[1].First().X);
            //                double distanceX2 = Math.Abs(return1[0].Last().X - return1[1].Last().X);

            //                bool is1TooFar = distanceX1 / PixelWidth > 2;
            //                bool is2TooFar = distanceX2 / PixelWidth > 2;

            //                if (is1TooFar != is2TooFar)
            //                {
            //                    if (is1TooFar)
            //                    {
            //                        return1[1] = return1[1].Reverse();
            //                        return1[0] = return1[0].Concat(return1[1]);
            //                        return1.RemoveAt(1);
            //                    }
            //                    else
            //                    {
            //                        return1[1] = return1[1].Reverse();
            //                        return1[0] = return1[1].Concat(return1[0]);
            //                        return1.RemoveAt(1);
            //                    }
            //                }
            //                else if (!is1TooFar && !is2TooFar)
            //                {
            //                    return1[1] = return1[1].Reverse();
            //                    return1[1] = return1[1].Concat(return1[0]);
            //                    return1.RemoveAt(0);
            //                    return1[0] = return1[0].Append(return1[0].First());
            //                }
            //            }

            //        }
            //    }
            //    else if (return1.Count == 4)
            //    {
            //        double distance1 = Math.Abs(return1[0].First().Y - return1[2].First().Y);
            //        double distance2 = Math.Abs(return1[0].Last().Y - return1[2].Last().Y);

            //        if (distance1 < distance2)
            //        {
            //            return1[2] = return1[2].Reverse();
            //            return1[0] = return1[2].Concat(return1[0]);
            //            return1.RemoveAt(2);
            //        }
            //        else
            //        {
            //            return1[2] = return1[2].Reverse();
            //            return1[0] = return1[0].Concat(return1[2]);
            //            return1.RemoveAt(2);
            //        }

            //        distance1 = Math.Abs(return1[1].First().Y - return1[2].First().Y);
            //        distance2 = Math.Abs(return1[1].Last().Y - return1[2].Last().Y);

            //        if (distance1 < distance2)
            //        {
            //            return1[2] = return1[2].Reverse();
            //            return1[2] = return1[2].Concat(return1[1]);
            //            return1.RemoveAt(1);
            //        }
            //        else
            //        {
            //            return1[2] = return1[2].Reverse();
            //            return1[1] = return1[1].Concat(return1[2]);
            //            return1.RemoveAt(2);
            //        }
            //    }
            //    else if (return1.Count == 3)
            //    {
            //        double distanceX0 = Math.Abs(return1[0].First().X - return1[0].Last().X);
            //        double distanceX2 = Math.Abs(return1[2].First().X - return1[2].Last().X);

            //        double distanceY0;
            //        double distanceY2;

            //        if (distanceX0 > distanceX2)
            //        {
            //            distanceY0 = Math.Abs(return1[0].First().Y - return1[1].First().Y);
            //            distanceY2 = Math.Abs(return1[0].Last().Y - return1[2].Last().Y);
            //        }
            //        else
            //        {
            //            distanceY0 = Math.Abs(return1[0].First().Y - return1[2].First().Y);
            //            distanceY2 = Math.Abs(return1[1].Last().Y - return1[2].Last().Y);
            //        }

            //        bool is1TooFar = distanceY0 * PixelWidth > 5;
            //        bool is2TooFar = distanceY2 * PixelWidth > 5;

            //        if (!is1TooFar || !is2TooFar)
            //        {
            //            if (distanceX0 > distanceX2)
            //            {
            //                return1[1] = return1[1].Reverse();
            //                return1[2] = return1[2].Reverse();

            //                return1[0] = return1[1].Concat(return1[0]).Concat(return1[2]);
            //                return1.RemoveRange(1, 2);
            //            }
            //            else
            //            {
            //                return1[0] = return1[0].Reverse();
            //                return1[1] = return1[1].Reverse();

            //                return1[0] = return1[0].Concat(return1[2]).Concat(return1[1]);
            //                return1.RemoveRange(1, 2);
            //            }
            //        }
            //    }
            //}
        }
    }
}
