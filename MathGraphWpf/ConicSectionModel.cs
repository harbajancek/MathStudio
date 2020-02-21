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
        public GraphableType Type => GraphableType.ConicSection;
        private bool isGraphable = true;
        private ConicSectionType conicSectionType = ConicSectionType.Ellipse;

        private double a = 5;
        private double b = 10;
        private double c = 10;
        private double d = 10;
        private double e = -20;

        public double A
        {
            get => a;
            set
            {
                a = value;
                IsGraphable = ExpressionGrapher.IsConicSectionGraphable(a, b, c, d, e);
                ConicSectionType = ExpressionGrapher.GetConicSectionType(a, b, c, d, e);
                NotifyPropertyChanged();
            }
        }
        public double B
        {
            get => b;
            set
            {
                b = value;
                IsGraphable = ExpressionGrapher.IsConicSectionGraphable(a, b, c, d, e);
                ConicSectionType = ExpressionGrapher.GetConicSectionType(a, b, c, d, e);
                NotifyPropertyChanged();
            }
        }
        public double C
        {
            get => c;
            set
            {
                c = value;
                IsGraphable = ExpressionGrapher.IsConicSectionGraphable(a, b, c, d, e);
                ConicSectionType = ExpressionGrapher.GetConicSectionType(a, b, c, d, e);
                NotifyPropertyChanged();
            }
        }
        public double D
        {
            get => d;
            set
            {
                d = value;
                IsGraphable = ExpressionGrapher.IsConicSectionGraphable(a, b, c, d, e);
                ConicSectionType = ExpressionGrapher.GetConicSectionType(a, b, c, d, e);
                NotifyPropertyChanged();
            }
        }
        public double E
        {
            get => e;
            set
            {
                e = value;
                IsGraphable = ExpressionGrapher.IsConicSectionGraphable(a, b, c, d, e);
                ConicSectionType = ExpressionGrapher.GetConicSectionType(a, b, c, d, e);
                NotifyPropertyChanged();
            }
        }

        public bool IsGraphable
        {
            get => isGraphable;
            set
            {
                isGraphable = value;
                NotifyPropertyChanged();
            }
        }

        public Point Vertex
        {
            get
            {
                Point noVertex = new Point(double.NaN, double.NaN);
                if (A != 0 && B != 0)
                {
                    return noVertex;
                }

                var vertex = new Point();

                if (A == 0)
                {
                    vertex.X = -E / C + D * D / 4 / C / D;
                    vertex.Y = -D / 2 / B;

                    return (double.IsFinite(vertex.X) && double.IsFinite(vertex.Y)) ? vertex : noVertex;
                }
                else if (B == 0)
                {
                    vertex.X = C / 2 / A;
                    vertex.Y = E / D + C * C / 4 / A / D;

                    return (double.IsFinite(vertex.X) && double.IsFinite(vertex.Y)) ? vertex : noVertex;
                }

                return noVertex;
            }
        }

        public Point Center
        {
            get
            {
                Point noCenter = new Point(double.NaN, double.NaN);

                if (A == 0 || B == 0)
                {
                    return noCenter;
                }

                var center = new Point();

                center.X = -C / 2 / A;
                center.Y = -D / 2 / B;

                return (double.IsFinite(center.X) && double.IsFinite(center.Y)) ? center : noCenter;
            }
        }

        public double Eccentricity
        {
            get
            {
                return Math.Sqrt(Math.Abs(A * A - B * B));
            }
        }

        public double ParabolaParameter
        {
            get
            {
                if (A == 0)
                {
                    return Math.Abs(C / 2 / B);
                }
                else if (B == 0)
                {
                    return Math.Abs(D / A / 2);
                }
                else
                {
                    return double.NaN;
                }
            }
        }

        public Point Focus
        {
            get
            {
                var noFocus = new Point(double.NaN, double.NaN);
                if ((A != 0 && B != 0) || (A == 0 && B == 0))
                {
                    return noFocus;
                }

                var focus = new Point();

                if (A == 0)
                {
                    if (Math.Sign(B) == Math.Sign(C))
                    {
                        focus.X = Vertex.X - Math.Sqrt(ParabolaParameter) * Math.Sqrt(ParabolaParameter) / 2;
                        focus.Y = Vertex.Y;
                    }
                    else
                    {
                        focus.X = Vertex.X + Math.Sqrt(ParabolaParameter) * Math.Sqrt(ParabolaParameter) / 2;
                        focus.Y = Vertex.Y;
                    }
                }
                else if (B == 0)
                {
                    if (Math.Sign(D) == Math.Sign(A))
                    {
                        focus.X = Vertex.X;
                        focus.Y = Vertex.Y - Math.Sqrt(ParabolaParameter) * Math.Sqrt(ParabolaParameter) / 2;
                    }
                    else
                    {
                        focus.X = Vertex.X;
                        focus.Y = Vertex.Y + Math.Sqrt(ParabolaParameter) * Math.Sqrt(ParabolaParameter) / 2;
                    }
                }
                else
                {
                    return noFocus;
                }

                return focus;
            }
        }

        public Point Focus1
        {
            get
            {
                var noFocus1 = new Point(double.NaN, double.NaN);
                if (A == 0 || B == 0)
                {
                    return noFocus1;
                }

                var focus1 = new Point();

                if (A > B)
                {
                    focus1.X = Center.X;
                    focus1.Y = Center.Y - Eccentricity;
                }
                else
                {
                    focus1.X = Center.X - Eccentricity;
                    focus1.Y = Center.Y;
                }

                return focus1;
            }
        }

        public Point Focus2
        {
            get
            {
                var noFocus2 = new Point(double.NaN, double.NaN);
                if (A == 0 || B == 0)
                {
                    return noFocus2;
                }

                var focus2 = new Point();

                if (A > B)
                {
                    focus2.X = Center.X;
                    focus2.Y = Center.Y + Eccentricity;
                }
                else
                {
                    focus2.X = Center.X + Eccentricity;
                    focus2.Y = Center.Y;
                }

                return focus2;
            }
        }

        public ConicSectionType ConicSectionType
        {
            get => conicSectionType;
            set
            {
                conicSectionType = value;
                NotifyPropertyChanged();
            }
        }

        public Brush Color { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public IEnumerable<IEnumerable<Point>> GetGraphPoints()
        {
            foreach (var item in ExpressionGrapher.GetPointsFromConicSection(A, B, C, D, E))
            {
                yield return item;
            }
        }
    }

}
