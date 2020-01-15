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
    class ConicSectionModel : IGraphable
    {
        public bool IsGraphable
        {
            get
            {
                return true;
            }
            set
            {
            }
        }
        public Brush Color { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public float[] Coefficients { get; set; } = new float[5];

        public float Eccentricity { get; set; }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public IEnumerable<PointCollection> GetGraphPoints(float xmax, float ymax, float xmin, float ymin, float dx)
        {
            List<PointCollection> pointCollections = new List<PointCollection>();
            PointCollection points = new PointCollection();
            List<Node> nodeExpressions = new List<Node>(); 

            Point currentPoint;

            points = new PointCollection();


            float a = 4;
            float b = 9;
            float c = 8;
            float d = -90;
            float e = 193;

            float stredX = -c / 2 / a;
            float stredY = -d / 2 / b;

            float ra = (c * c * b + d * d * a - 4 * a * b * e) / 4 / a / a / b;
            float rb = (c * c * b + d * d * a - 4 * a * b * e) / 4 / a / b / b;


            string expressionString = $"(({ra}-(x-{stredX})^2)*{rb}/{ra})^(1/2)+{stredY}";

            var context = new ConicSectionContext();
            context.A = -4;
            context.B = 0;
            context.C = 1;
            context.D = -32;
            context.E = 4;
            context.F = -96;

            var tokenizer = new Tokenizer(new StringReader(expressionString));
            var parser = new Parser(tokenizer);
            nodeExpressions.Add(parser.ParseExpression());

            expressionString = $"-(({ra}-(x-{stredX})^2)*{rb}/{ra})^(1/2)+{stredY}";

            tokenizer = new Tokenizer(new StringReader(expressionString));
            parser = new Parser(tokenizer);
            nodeExpressions.Add(parser.ParseExpression());
            
            foreach (var nodeExpression in nodeExpressions)
            {
                float? result = default;
                bool wrong = false;

                for (float x = xmin; x < xmax; x += dx)
                {
                    context.X = x;
                    try
                    {
                        result = nodeExpression.Eval(context);
                    }
                    catch (DivideByZeroException)
                    {
                        float y = (currentPoint.Y > 0) ? ymax : ymin;
                        currentPoint = new Point(currentPoint.X, (double)y);
                        wrong = true;
                    }
                    catch (OverflowException)
                    {
                        continue;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
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

                    currentPoint = new Point((double)context.X, (double)result);
                    points.Add(currentPoint);
                }

                if (points.Count != 0)
                {
                    pointCollections.Add(points);
                }
            }
            
            return pointCollections;
        }
    }
}
