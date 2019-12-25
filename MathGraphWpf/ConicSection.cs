using MathExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MathStudioWpf
{
    class ConicSection : IGraphable
    {
        private bool isGraphable;
        public bool IsGraphable
        {
            get
            {
                return isGraphable;
            }
            set
            {
                isGraphable = value;
                NotifyPropertyChanged();
            }
        }
        public Brush Color { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public decimal[] Coefficients { get; set; } = new decimal[5];

        public decimal Eccentricity { get; set; }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public IEnumerable<PointCollection> GetGraphPoints(decimal xmax, decimal ymax, decimal xmin, decimal ymin, decimal dx)
        {
            List<PointCollection> pointCollections = new List<PointCollection>();
            PointCollection points = new PointCollection();

            Point currentPoint;

            points = new PointCollection();

            string expressionString = "(-(b * x + e) - pow((b * x + e)^2 - 4*c*(a*x^2 + d*x + f)),1/2) / (2*c)";

            var context = new FunctionContext();
            var tokenizer = new Tokenizer(new StringReader());
            var parser = new Parser(tokenizer);
            var nodeExpression = parser.ParseExpression();

            decimal? result = default;

            bool wrong = false;
            for (decimal x = xmin; x < xmax; x += dx)
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
                        if (!currentPoint.Equals(new Point(0, 0)))
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
            return pointCollections;
        }
    }
}
