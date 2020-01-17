using System;
using System.Collections.Generic;
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
        double OffsetX
        {
            get
            {
                return CoordinatesConverter.OffsetX;
            }
            set
            {
                CoordinatesConverter.OffsetX = value;
            }
        }
        double OffsetY
        {
            get
            {
                return CoordinatesConverter.OffsetY;
            }
            set
            {
                CoordinatesConverter.OffsetY = value;
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

        private void DrawBase()
        {
            
            double delta = Math.Ceiling(WXMax / 6);
            DrawBaseX(delta);
            DrawBaseY(delta);
        }
        private void DrawBaseY(double delta)
        {
            PointCollection points;
            Polyline pl;
            for (double i = delta; i <= Math.Floor(WYMax); i += delta)
            {
                points = new PointCollection
                {
                    WtoD(new Point(WXMin, i)),
                    WtoD(new Point(WXMax, i))
                };
                pl = new Polyline
                {
                    Points = points,
                    StrokeThickness = 1,
                    Stroke = Brushes.Gray
                };

                TextBlock numberBlock = new TextBlock()
                {
                    Text = i.ToString(),
                    Background = Brushes.WhiteSmoke,
                    Margin = new Thickness(5),
                    Padding = new Thickness(0),
                    FontWeight = FontWeights.Bold
                };

                Thickness padding = new Thickness(0);
                numberBlock.Padding = padding;


                AddToGraph(numberBlock);
                Point labelPoint = WtoD(new Point(0, i));
                Canvas.SetLeft(numberBlock, labelPoint.X);
                Canvas.SetTop(numberBlock, labelPoint.Y);
                Canvas.SetZIndex(numberBlock, 7);

                AddToGraph(pl);
            }
            for (double i = -delta; i >= Math.Ceiling(WYMin); i -= delta)
            {
                points = new PointCollection
                {
                    WtoD(new Point(WXMin, i)),
                    WtoD(new Point(WXMax, i))
                };
                pl = new Polyline
                {
                    Points = points,
                    StrokeThickness = 1,
                    Stroke = Brushes.Gray
                };

                TextBlock numberBlock = new TextBlock()
                {
                    Text = i.ToString(),
                    Background = Brushes.WhiteSmoke,
                    Margin = new Thickness(5),
                    Padding = new Thickness(0),
                    FontWeight = FontWeights.Bold
                };

                Thickness padding = new Thickness(0);
                numberBlock.Padding = padding;

                AddToGraph(numberBlock);
                Point labelPoint = WtoD(new Point(0, i));
                Canvas.SetLeft(numberBlock, labelPoint.X);
                Canvas.SetTop(numberBlock, labelPoint.Y);
                Canvas.SetZIndex(numberBlock, 7);

                AddToGraph(pl);
            }
            points = new PointCollection
            {
                WtoD(new Point(WXMin,0)),
                WtoD(new Point(WXMax,0))
            };
            pl = new Polyline
            {
                Points = points,
                StrokeThickness = 1,
                Stroke = Brushes.Black
            };
            AddToGraph(pl);
            Canvas.SetZIndex(pl, 5);
        }
        private void DrawBaseX(double delta)
        {
            if (IsPiEnabled)
            {
                delta = Math.PI;
            }
            PointCollection points;
            Polyline pl;
            for (double i = 1; i <= Math.Floor(WXMax)/delta; i += 1)
            {
                points = new PointCollection
                {
                    WtoD(new Point(i*delta, WYMin)),
                    WtoD(new Point(i*delta, WYMax))
                };
                pl = new Polyline
                {
                    Points = points,
                    StrokeThickness = 1,
                    Stroke = Brushes.Gray
                };

                TextBlock numberBlock = new TextBlock()
                {
                    Text = (IsPiEnabled)? ((i > 1)? $"{i}π": "π"):$"{i*delta}",
                    Background = Brushes.WhiteSmoke,
                    Margin = new Thickness(5),
                    Padding = new Thickness(0),
                    FontWeight = FontWeights.Bold
                };

                AddToGraph(numberBlock);
                Point labelPoint = WtoD(new Point(i*delta, 0));
                labelPoint.X -= (numberBlock.FontSize + numberBlock.Margin.Left) / 2;
                Canvas.SetLeft(numberBlock, labelPoint.X);
                Canvas.SetTop(numberBlock, labelPoint.Y);
                Canvas.SetZIndex(numberBlock, 7);

                AddToGraph(pl);
            }
            for (double i = -1; i >= Math.Ceiling(WXMin)/delta; i -= 1)
            {
                points = new PointCollection
                {
                    WtoD(new Point(i*delta, WYMin)),
                    WtoD(new Point(i*delta, WYMax))
                };
                pl = new Polyline
                {
                    Points = points,
                    StrokeThickness = 1,
                    Stroke = Brushes.Gray
                };

                TextBlock numberBlock = new TextBlock()
                {
                    Text = (IsPiEnabled) ? ((i < -1) ? $"{i}π" : "π") : $"{i * delta}",
                    Background = Brushes.WhiteSmoke,
                    Margin = new Thickness(5),
                    Padding = new Thickness(0),
                    FontWeight = FontWeights.Bold
                };

                Thickness padding = new Thickness(0);
                numberBlock.Padding = padding;

                AddToGraph(numberBlock);
                Point labelPoint = WtoD(new Point(i*delta, 0));
                labelPoint.X -= (numberBlock.FontSize + numberBlock.Margin.Left) / 2;
                Canvas.SetLeft(numberBlock, labelPoint.X);
                Canvas.SetTop(numberBlock, labelPoint.Y);
                Canvas.SetZIndex(numberBlock, 7);

                AddToGraph(pl);
            }
            points = new PointCollection
            {
                WtoD(new Point(0,WYMin)),
                WtoD(new Point(0,WYMax))
            };
            pl = new Polyline
            {
                Points = points,
                StrokeThickness = 1,
                Stroke = Brushes.Black
            };
            AddToGraph(pl);
            Canvas.SetZIndex(pl, 5);
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
                Stroke = color
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

        public void Offset(double x, double y)
        {
            OffsetX += x;
            OffsetY -= y;
        }

        private Point WtoD(Point point) => CoordinatesConverter.WtoD(point);
        private Point DtoW(Point point) => CoordinatesConverter.DtoW(point);

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
