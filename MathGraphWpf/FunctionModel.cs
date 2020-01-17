using MathExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MathStudioWpf
{
    class FunctionModel : IGraphable
    {
        private string expressionString;
        private bool isGraphable;

        public event PropertyChangedEventHandler PropertyChanged;

        public string ExpressionString
        {
            get
            {
                return expressionString;
            }
            set
            {
                var willIsGraphable = true;
                try
                {
                    expressionString = value;
                    var context = new FunctionContext();
                    var tokenizer = new Tokenizer(new StringReader(expressionString));
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
                IsGraphable = willIsGraphable;
                NotifyPropertyChanged();
            }
        }
        
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


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<IEnumerable<Point>> GetGraphPoints(double xmax, double ymax, double xmin, double ymin, double dx)
        {
            List<Point> points = new List<Point>();

            Point currentPoint;

            var context = new FunctionContext();
            var tokenizer = new Tokenizer(new StringReader(ExpressionString));
            var parser = new Parser(tokenizer);
            var nodeExpression = parser.ParseExpression();

            double? result = default;

            bool wrong = false;
            for (double x = xmin; x < xmax; x += dx)
            {
                context.Variable = x;
                try
                {
                    result = nodeExpression.Eval(context);
                }
                catch (DivideByZeroException)
                {
                    double y = (currentPoint.Y > 0) ? ymax : ymin;
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


    }
}
