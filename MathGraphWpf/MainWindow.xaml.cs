using MathExpressionEvaluator;
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

namespace MathGraphWpf
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
            Brushes.Green
        };
        private int brushesIndex = 0;

        bool window_loaded = false;

        //private int scrollIndex = 0;
        //private readonly int scrollBase = 31;
        //private readonly int scrollStep = 5;

        //private int ScrollZoom => (scrollIndex * scrollStep) + scrollBase;

        private double wxmax { get; set; }
        private double wymax { get; set; }
        private double wxmin { get; set; }
        private double wymin { get; set; }

        TestFunctionClassViewModel Tests { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Tests = new TestFunctionClassViewModel();
            ExpressionsList.DataContext = Tests;
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
            Tests.TestFunctions.Add(new TestFunctionClass()
            {
                ExpressionString = ExpressionInput.Text,
                Domain = "null",
                Range = "null"
            });

            DisplayGraphs();
        }

        public void DisplayGraphBase()
        {
            Polyline pl;
            PointCollection points;

            // y axis
            for (decimal i = 0; i <= (decimal)Math.Floor(wymax); i += (decimal)yScale.Value)
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
                Graph.Children.Add(pl);
            }
            for (decimal i = 0; i >= (decimal)Math.Ceiling(wymin); i -= (decimal)yScale.Value)
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

            // x axis
            for (decimal i = 0; i <= (decimal)Math.Floor(wxmax); i += (decimal)xScale.Value)
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
                Graph.Children.Add(pl);
            }
            for (decimal i = 0; i >= (decimal)Math.Ceiling(wxmin); i -= (decimal)xScale.Value)
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
        }

        public void DisplayGraphs()
        {
            Graph.Children.Clear();

            DisplayGraphBase();

            List<PointCollection> pointCollections = new List<PointCollection>();

            // See how big 1 pixel is in world coordinates.
            Point p0 = DtoW(new Point(0, 0));
            Point p1 = DtoW(new Point(1, 1));
            double dx = p1.X - p0.X;

            foreach (var item in Tests.TestFunctions)
            {
                pointCollections.AddRange(item.GetGraphs((decimal)wxmax + 10, (decimal)wymax + 10, (decimal)wxmin - 10, (decimal)wymin - 10, (decimal)dx));
            }
            brushesIndex = 0;
            foreach (var points in pointCollections)
            {
                DrawPolyline(points);
            }
        }

        private void DrawPolyline(PointCollection points)
        {
            PointCollection pointsD = new PointCollection(points.Count);
            foreach (var point in points)
            {
                pointsD.Add(WtoD(point));
            }

            var polyline = GetNewDefaultPolyline();
            polyline.Points = pointsD;

            polyline.Stroke = brushes[brushesIndex];
            brushesIndex++;
            if (brushesIndex >= brushes.Count)
            {
                brushesIndex = 0;
            }

            Graph.Children.Add(polyline);
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

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PrepareGraph();
            window_loaded = true;
        }

        private void PrepareGraph()
        {
            // Prepare the transformation.
            double dxmax = Graph.ActualWidth;
            double dymax = Graph.ActualHeight;
            double difference = dxmax / dymax;

            wxmax = 1 / xScale.Value * 10 * difference;
            wymax = 1 / yScale.Value * 10;
            wxmin = 1 / xScale.Value * -10 * difference;
            wymin = 1 / yScale.Value * -10;

            prepareTransformations(0, dxmax, dymax, 0);

            DisplayGraphBase();
            DisplayGraphs();
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

        private void Clear_ButtonClick(object sender, RoutedEventArgs e)
        {
            Tests.TestFunctions.Clear();
            DisplayGraphs();
        }
    }
}
