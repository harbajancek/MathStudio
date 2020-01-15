using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathStudioWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly object RenderLock = new object();

        private List<Brush> brushes = new List<Brush>()
        {
            Brushes.Red,
            Brushes.Blue,
            Brushes.Green,
            Brushes.Purple,
            Brushes.Orange,
            Brushes.Pink,
            Brushes.Brown
        };
        private int brushesIndex = 0;

        bool window_loaded = false;

        private double wxmax { get; set; }
        private double wymax { get; set; }
        private double wxmin { get; set; }
        private double wymin { get; set; }
        private bool disableGrid;
        public bool DisableGrid
        {
            get
            {
                return disableGrid;
            }
            set
            {
                disableGrid = value;
                PrepareGraph();
            }
        }
        private double mouseDelta = 10;
        public double MouseDelta
        {
            get
            {
                return mouseDelta;
            }
            set
            {
                mouseDelta = value;
                mouseDeltaText.Text = mouseDelta.ToString();
                PrepareGraph();
            }
        }

        GraphablesViewModel GraphablesViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            mouseDeltaText.Text = mouseDelta.ToString();
            GraphablesViewModel = new GraphablesViewModel();
            ExpressionsList.DataContext = GraphablesViewModel;
            DisableGridCheckBox.DataContext = this;
        }

        // Prepare values for perform transformations.
        private Matrix WtoDMatrix, DtoWMatrix;
        private void prepareTransformations(double dxmin, double dxmax, double dymin, double dymax)
        {
            // Make WtoD.
            WtoDMatrix = Matrix.Identity;

            WtoDMatrix.Translate(-wxmin, -wymin);


            double xscale = (dxmax - dxmin) / (wxmax - wxmin);
            double yscale = (dymax - dymin) / (wymax - wymin);

            WtoDMatrix.Scale(xscale, yscale);

            WtoDMatrix.Translate(dxmin, dymin);


            // Make DtoW.
            DtoWMatrix = WtoDMatrix;
            DtoWMatrix.Invert();
        }

        // Transform a point from world to device coordinates.
        private Point WtoD(Point point)
        {
            Point pointT = WtoDMatrix.Transform(point);

            return pointT;
        }

        // Transform a point from device to world coordinates.
        private Point DtoW(Point point)
        {
            return DtoWMatrix.Transform(point);
        }

        private void Add_ButtonClick(object sender, RoutedEventArgs e)
        {
            FunctionModel function = new FunctionModel();
            function.Color = brushes[brushesIndex];
            brushesIndex++;
            if (brushesIndex >= brushes.Count)
            {
                brushesIndex = 0;
            }
            function.PropertyChanged += FunctionChange_NotifyEvent;
            GraphablesViewModel.Graphables.Add(function);

            PrepareGraph();
        }

        private void FunctionChange_NotifyEvent(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsGraphable")
            {
                PrepareGraph();
            }
        }

        public void DisplayGraphBase()
        {
            if (disableGrid)
            {
                return;
            }
            Polyline pl;
            PointCollection points;

            float delta = (float)Math.Ceiling(MouseDelta / 5);
            mouseDeltaDeltaText.Text = delta.ToString();

            // y axis
            for (float i = delta; i <= (float)Math.Floor(wymax); i += delta)
            {

                points = new PointCollection
                {
                    WtoD(new Point(wxmin, (double)i)),
                    WtoD(new Point(wxmax, (double)i))
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

                Graph.Children.Add(numberBlock);
                Point labelPoint = WtoD(new Point(0, (double)i));
                Canvas.SetLeft(numberBlock, labelPoint.X);
                Canvas.SetTop(numberBlock, labelPoint.Y);
                Canvas.SetZIndex(numberBlock, 7);

                Graph.Children.Add(pl);
            }
            for (float i = -delta; i >= (float)Math.Ceiling(wymin); i -= delta)
            {

                points = new PointCollection
                {
                    WtoD(new Point(wxmin, (double)i)),
                    WtoD(new Point(wxmax, (double)i))
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

                Graph.Children.Add(numberBlock);
                Point labelPoint = WtoD(new Point(0, (double)i));
                Canvas.SetLeft(numberBlock, labelPoint.X);
                Canvas.SetTop(numberBlock, labelPoint.Y);
                Canvas.SetZIndex(numberBlock, 7);

                Graph.Children.Add(pl);
            }
            points = new PointCollection
            {
                WtoD(new Point(wxmin,0)),
                WtoD(new Point(wxmax,0))
            };
            pl = new Polyline
            {
                Points = points,
                StrokeThickness = 1,
                Stroke = Brushes.Black
            };
            Graph.Children.Add(pl);
            Canvas.SetZIndex(pl, 5);

            // x axis
            for (float i = delta; i <= (float)Math.Floor(wxmax); i += delta)
            {
                points = new PointCollection
                {
                    WtoD(new Point((double)i, wymin)),
                    WtoD(new Point((double)i, wymax))
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

                Graph.Children.Add(numberBlock);
                Point labelPoint = WtoD(new Point((double)i, 0));
                labelPoint.X -= (numberBlock.FontSize + numberBlock.Margin.Left) / 2;
                Canvas.SetLeft(numberBlock, labelPoint.X);
                Canvas.SetTop(numberBlock, labelPoint.Y);
                Canvas.SetZIndex(numberBlock, 7);

                Graph.Children.Add(pl);
            }
            for (float i = 0; i >= (float)Math.Ceiling(wxmin); i -= delta)
            {
                points = new PointCollection
                {
                    WtoD(new Point((double)i, wymin)),
                    WtoD(new Point((double)i, wymax))
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

                Graph.Children.Add(numberBlock);
                Point labelPoint = WtoD(new Point((double)i, 0));
                labelPoint.X -= (numberBlock.FontSize + numberBlock.Margin.Left) / 2;
                Canvas.SetLeft(numberBlock, labelPoint.X);
                Canvas.SetTop(numberBlock, labelPoint.Y);
                Canvas.SetZIndex(numberBlock, 7);

                Graph.Children.Add(pl);
            }
            points = new PointCollection
            {
                WtoD(new Point(0,wymin)),
                WtoD(new Point(0,wymax))
            };
            pl = new Polyline
            {
                Points = points,
                StrokeThickness = 1,
                Stroke = Brushes.Black
            };
            Graph.Children.Add(pl);
            Canvas.SetZIndex(pl, 5);
        }

        public void DisplayGraphs()
        {
            Graph.Children.Clear();

            DisplayGraphBase();

            // See how big 1 pixel is in world coordinates.
            Point p0 = DtoW(new Point(0, 0));
            Point p1 = DtoW(new Point(1, 1));
            double dx = p1.X - p0.X;

            foreach (var function in GraphablesViewModel.Graphables)
            {
                if (function.IsGraphable)
                {
                    List<PointCollection> pointCollections = new List<PointCollection>(function.GetGraphPoints((float)wxmax + 10, (float)wymax + 10, (float)wxmin - 10, (float)wymin - 10, (float)dx));
                    foreach (var points in pointCollections)
                    {
                        DrawPolyline(points, function.Color);
                    }
                }
            }
        }

        private void DrawPolyline(PointCollection points, Brush color)
        {
            PointCollection pointsD = new PointCollection(points.Count);
            foreach (var point in points)
            {
                pointsD.Add(WtoD(point));
            }

            var polyline = GetNewDefaultPolyline();
            polyline.Points = pointsD;

            polyline.Stroke = color;
            polyline.Opacity = .5;

            Graph.Children.Add(polyline);
            Canvas.SetZIndex(polyline, 10);
        }

        private Polyline GetNewDefaultPolyline()
        {
            return new Polyline
            {
                Stroke = Brushes.Black,
                Tag = "funPolGraph",
                StrokeThickness = 3
            };
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                MouseDelta /= 1.1;
            }
            else if (e.Delta < 0)
            {
                MouseDelta *= 1.1;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GraphablesViewModel.Graphables.Add(new ConicSectionModel() { Color = Brushes.Black });
            PrepareGraph();
            window_loaded = true;
        }

        private void PrepareGraph()
        {
            // Prepare the transformation.
            double dxmax = Graph.ActualWidth;
            double dymax = Graph.ActualHeight;
            double difference = dxmax / dymax;

            wxmax = MouseDelta * difference;
            wymax = MouseDelta;
            wxmin = -MouseDelta * difference;
            wymin = -MouseDelta;

            prepareTransformations(0, dxmax, dymax, 0);

            DisplayGraphBase();
            DisplayGraphs();

            List<Polyline> functions = new List<Polyline>();

            foreach (var item in Graph.Children)
            {
                if (item is Polyline)
                {
                    Polyline pl = (Polyline)item;
                    if (pl.Tag is string && (string)pl.Tag == "funPolGraph")
                    {
                        functions.Add(pl);
                    }
                }
            }

            List<Point> interPoints;

            if (functions.Count == 2)
            {
                interPoints = new List<Point>(GetIntersectionPoints(functions[0].Points, functions[1].Points));
                foreach (var point in interPoints)
                {
                    Ellipse ellipse = new Ellipse()
                    {
                        Stroke = Brushes.Black,
                        StrokeThickness = 2,
                        Width = 2,
                        Height = 2,
                    };
                    Graph.Children.Add(ellipse);
                }
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            PrepareGraph();
        }

        private void ScaleChange_ButtonClick(object sender, RoutedEventArgs e)
        {
            PrepareGraph();
        }

        private void Scale_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (window_loaded)
            {
                PrepareGraph();
            }
        }

        private void ShowGrid_CheckBoxClick(object sender, RoutedEventArgs e)
        {

        }

        private void Clear_ButtonClick(object sender, RoutedEventArgs e)
        {
            GraphablesViewModel.Graphables.Clear();
            DisplayGraphs();
        }

        private void RemoveItem_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            FunctionModel function = (FunctionModel)button.DataContext;

            GraphablesViewModel.Graphables.Remove(function);
            PrepareGraph();
        }

        public static IEnumerable<Point> GetIntersectionPoints(PointCollection points1, PointCollection points2)
        {
            return points1.Intersect(points2);
        }
    }
}
