using MathExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MathGraphWpf
{
    class TestFunctionClass
    {
        private IEnumerable<PointCollection> pointsCache;
        private decimal xmaxCache;
        private decimal ymaxCache;
        private decimal xminCache;
        private decimal yminCache;
        private decimal dxCache;
        private bool expressionChanged = false;
        private string expressionString;
        public string ExpressionString
        {
            get
            {
                return expressionString;
            }
            set
            {
                expressionChanged = (expressionString != value) ? true : false;
                expressionString = value;
            }
        }
        public string Range { get; set; }
        public string Domain { get; set; }
        public decimal? Asymptote { get; set; } = null;

        public TestFunctionClass(string expression)
        {
            var context = new FunctionContext();
            var tokenizer = new Tokenizer(new StringReader(expression));
            var parser = new Parser(tokenizer);
            var nodeExpression = parser.ParseExpression();

            // test
            for (decimal i = 0; i < 2; i++)
            {
                context.Variable = i;
                try
                {
                    _ = nodeExpression.Eval(context);
                }
                catch (DivideByZeroException)
                {

                }
            }

            expressionString = expression;
        }

        public IEnumerable<PointCollection> GetGraphs(decimal xmax, decimal ymax, decimal xmin, decimal ymin, decimal dx)
        {
            if (xmaxCache == xmax
                && ymaxCache == ymax
                && xminCache == xmin
                && yminCache == ymin
                && dxCache == dx
                && !expressionChanged)
            {
                expressionChanged = false;
                return pointsCache;
            }

            xmaxCache = xmax;
            ymaxCache = ymax;
            xminCache = xmin;
            yminCache = ymin;
            dxCache = dx;

            List<PointCollection> pointCollections = new List<PointCollection>();
            PointCollection points = new PointCollection();

            Point currentPoint;

            points = new PointCollection();

            var context = new FunctionContext();
            var tokenizer = new Tokenizer(new StringReader(ExpressionString));
            var parser = new Parser(tokenizer);
            var nodeExpression = parser.ParseExpression();

            decimal? result = default;

            bool wrong = false;
            for (decimal x = xmin; x < xmax; x += (decimal)0.02)
            {
                context.Variable = x;
                try
                {
                    result = nodeExpression.Eval(context);
                }
                catch (DivideByZeroException)
                {
                    decimal y = (currentPoint.Y > 0) ? ymax : ymin;
                    currentPoint = new Point(currentPoint.X, (double)y);
                    wrong = true;
                }
                catch (Exception e)
                {
                    throw e;
                }

                if (result > ymax || result < ymin)
                {
                    currentPoint = new Point(0, 0);
                    wrong = true;
                }

                if (wrong || result == null)
                {
                    if (points.Count != 0)
                    {
                        if (!currentPoint.Equals(new Point(0,0)))
                        {
                            points.Add(currentPoint);
                        }
                        pointCollections.Add(points);
                        points = new PointCollection();
                    }
                    wrong = false;
                    continue;
                }

                currentPoint = new Point((double)context.Variable, (double)result);
                points.Add(currentPoint);
            }

            if (points.Count != 0)
            {
                pointCollections.Add(points);
            }
            pointsCache = pointCollections;
            return pointCollections;
        }

        private Polyline GetNewDefaultPolyline()
        {
            return new Polyline
            {
                Stroke = Brushes.Black,
                Tag = "funPolGraph",
                StrokeThickness = 1
            };
        }
    }
}
