using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MathStudioWpf
{
    class GraphDrawer
    {
        public bool IsPiEnabled { get; set; }

        private int ZoomLevel
        {
            get
            {
                return CoordinatesConverter.ZoomLevel;
            }
            set
            {
                CoordinatesConverter.ZoomLevel = value;
            }
        }
        double WXMax { get => CoordinatesConverter.GetWXMax(); }
        double WXMin { get => CoordinatesConverter.GetWXMin(); }
        double WYMax { get => CoordinatesConverter.GetWYMax(); }
        double WYMin { get => CoordinatesConverter.GetWYMin(); }
        public IEnumerable<IGraphable> Graphables { get; set; }
        public Canvas Graph { get; set; }
        CoordinatesConverter CoordinatesConverter { get; set; } = new CoordinatesConverter();

        public void DrawGraph()
        {
            double dxmax = Graph.ActualWidth;
            double dymax = Graph.ActualHeight;
            CoordinatesConverter.PrepareMatrix(dxmax, dymax);

            ClearGraph();

            DrawBase();
            DrawGraphables();
        }

        private Fraction GetIter(out Fraction start, bool isX)
        {
            Fraction iterF;
            decimal decimalPoint = 1m;
            double delta = WXMax - WXMin;
            double iter;
            decimal roundedIter;
            start = new Fraction((isX) ? WXMin : WYMin);

            double i = (IsPiEnabled && isX) ? Math.PI : 1;
            i *= 10;

            if (delta >= i)
            {
                while (delta >= i)
                {
                    delta /= i;
                    decimalPoint *= 10;
                }

                decimalPoint /= 10;
            }
            else if (delta < i)
            {
                while (delta <= i/10)
                {
                    delta *= i;
                    decimalPoint /= 10;
                }

                decimalPoint /= 10;
            }


            if (isX && IsPiEnabled)
            {
                if (delta > 5 * Math.PI)
                {
                    roundedIter = 5;
                }
                else if (delta > 2 * Math.PI)
                {
                    roundedIter = 2;
                }
                else
                {
                    roundedIter = 1;
                }
            }
            else
            {
                if (delta > 5)
                {
                    roundedIter = 5;
                }
                else if (delta > 2)
                {
                    roundedIter = 2;
                }
                else
                {
                    roundedIter = 1;
                }
            }

            iter = (double)(roundedIter * decimalPoint);
            if (IsPiEnabled && isX)
            {
                iterF = new Fraction(iter, true);
            }
            else
            {
                iterF = new Fraction(iter);
            }

            Fraction startRounded = new Fraction(0, true);

            while (Math.Abs(start.RealValue) > Math.Abs(startRounded.RealValue))
            {
                if (start > 0)
                {
                    startRounded += iterF;
                }
                else
                {
                    startRounded -= iterF;
                }
            }
            start = startRounded;

            return iterF;
        }

        private void DrawBase()
        {
            Polyline polyline;
            TextBlock numberBlock;

            for (int coor = 0; coor < 2; coor++)
            {
                bool isX = (coor == 0);

                double end = Math.Ceiling((isX) ? WXMax : WYMax);

                Fraction iter = GetIter(out Fraction start, isX);

                for (double i = 0; iter * i + start < end; i++)
                {
                    Fraction currentValue = ((iter * i) + start);
                    polyline = new Polyline
                    {
                        Points = new PointCollection()
                        {
                            WtoD(new Point(
                                (isX) ? currentValue.RealValue : WXMin,
                                (isX) ? WYMin : currentValue.RealValue
                                )),
                            WtoD(new Point(
                                (isX) ? currentValue.RealValue : WXMax,
                                (isX) ? WYMax : currentValue.RealValue
                                ))
                        },
                        StrokeThickness = 1,
                        Stroke = (currentValue == 0) ? Brushes.Black : Brushes.Gray
                    };

                    string numberText;
                    numberText = $"{currentValue.ToString("0.#")}";

                    numberBlock = new TextBlock()
                    {
                        Text = numberText,
                        Background = Brushes.WhiteSmoke,
                        Margin = new Thickness(5),
                        Padding = new Thickness(0),
                        FontWeight = FontWeights.Bold
                    };

                    Point labelPoint = WtoD(new Point(
                            (isX) ? currentValue.RealValue : 0,
                            (isX) ? 0 : currentValue.RealValue
                            ));
                    if (isX)
                    {
                        labelPoint.X -= (numberBlock.FontSize + numberBlock.Margin.Left) / 2;
                    }

                    if (!(!isX && currentValue == 0))
                    {
                        AddToGraph(numberBlock);
                    }

                    Canvas.SetLeft(numberBlock, labelPoint.X);
                    Canvas.SetTop(numberBlock, labelPoint.Y);
                    Canvas.SetZIndex(numberBlock, 7);

                    AddToGraph(polyline);
                }
            }
        }
        private void DrawGraphables()
        {
            ColorControl.Reset();

            foreach (var item in Graphables)
            {
                var color = ColorControl.GetBrush();
                if (!item.IsGraphable)
                {
                    continue;
                }
                foreach (var points in item.GetGraphPoints(
                    CoordinatesConverter.GetWXMax(),
                    CoordinatesConverter.GetWYMax(),
                    CoordinatesConverter.GetWXMin(),
                    CoordinatesConverter.GetWYMin(),
                    CoordinatesConverter.GetPixelWidth()))
                {
                    var polyline = GetNewPolyline(color);
                    polyline.Points = new PointCollection(CoordinatesConverter.WtoDRange(points));

                    AddToGraph(polyline);
                    Canvas.SetZIndex(polyline, 10);
                }
            }
        }

        private Polyline GetNewPolyline(Brush color)
        {
            return new Polyline
            {
                StrokeThickness = 3,
                Tag = "funPolGraph",
                Stroke = color,
                Opacity = .65
            };
        }

        public void ZoomIn()
        {
            ZoomLevel++;
        }

        public void ZoomOut()
        {
            ZoomLevel--;
        }

        public void Offset(Point dStart, Point dEnd)
        {
            var wStart = DtoW(dStart);
            var wEnd = DtoW(dEnd);

            var offsetXValue = (wStart.X - wEnd.X) * 40;
            var offsetYValue = (wStart.Y - wEnd.Y) * 40;

            Debug.Write($"{offsetXValue},\n{offsetYValue}\n\n");

            CoordinatesConverter.Offset(offsetXValue, offsetYValue);
        }

        internal Point WtoD(Point point) => CoordinatesConverter.WtoD(point);
        internal Point DtoW(Point point) => CoordinatesConverter.DtoW(point);

        private void AddToGraph(FrameworkElement element)
        {
            Graph.Children.Add(element);
        }
        private void ClearGraph()
        {
            Graph.Children.Clear();
        }
    }
}
