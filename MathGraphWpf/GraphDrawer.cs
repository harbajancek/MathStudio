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

        internal double WXMax { get => CoordinatesConverter.GetWXMax(); }
        internal double WXMin { get => CoordinatesConverter.GetWXMin(); }
        internal double WYMax { get => CoordinatesConverter.GetWYMax(); }
        internal double WYMin { get => CoordinatesConverter.GetWYMin(); }
        public IEnumerable<IGraphable> Graphables { get; set; }
        public Canvas Graph { get; set; }
        private CoordinatesConverter CoordinatesConverter { get; set; } = new CoordinatesConverter();
        private int GetZoomLevel()
        {
            return CoordinatesConverter.ZoomLevel;
        }

        private void SetZoomLevel(int value)
        {
            CoordinatesConverter.ZoomLevel = value;
        }

        public void DrawGraph()
        {
            double dxmax = Graph.ActualWidth;
            double dymax = Graph.ActualHeight;
            CoordinatesConverter.PrepareMatrix(dxmax, dymax);

            ExpressionGrapher.XMax = WXMax;
            ExpressionGrapher.XMin = WXMin;
            ExpressionGrapher.YMax = WYMax;
            ExpressionGrapher.YMin = WYMin;
            ExpressionGrapher.PixelWidth = CoordinatesConverter.GetPixelWidth();

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
                while (delta <= i / 10)
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

                    if (!(!isX && currentValue == 0))
                    {
                        AddToGraph(numberBlock);
                    }

                    double blockHeight = numberBlock.LineHeight;
                    double blockWidth = numberBlock.FontSize + numberBlock.Margin.Right + numberBlock.Margin.Left;

                    Point labelPoint = WtoD(new Point(
                            (isX) ? currentValue.RealValue : (WXMin > 0) ? WXMin : (WXMax < 0) ? WXMax : 0,
                            (isX) ? (WYMin > 0) ? WYMin : (WYMax < 0) ? WYMax : 0 : currentValue.RealValue
                            ));

                    // TODO pokud maxx < 0 || maxy < 0 zmenit aby numberblock sel videt
                    //labelPoint.X -= (numberBlock.FontSize + numberBlock.Margin.Left) / 2;
                    //if (labelPoint.X < WXMin)
                    //{
                    //    labelPoint.X = WXMin;
                    //}

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
                foreach (var points in item.GetGraphPoints())
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
            SetZoomLevel(GetZoomLevel() + 1);
        }

        public void ZoomOut()
        {
            SetZoomLevel(GetZoomLevel() - 1);
        }

        public void OffsetMouse(Point dStart, Point dEnd)
        {
            var wStart = DtoW(dStart);
            var wEnd = DtoW(dEnd);

            var offsetXValue = (wStart.X - wEnd.X);
            var offsetYValue = (wStart.Y - wEnd.Y);

            CoordinatesConverter.Offset(offsetXValue, offsetYValue);
        }

        public void Offset(double x, double y)
        {
            CoordinatesConverter.Offset(x, y);
        }

        public void Reset()
        {
            CoordinatesConverter.Reset();
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
