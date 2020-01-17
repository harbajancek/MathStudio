using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MathStudioWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GraphDrawer GraphDrawer { get; set; } = new GraphDrawer();

        bool window_loaded { get; set; } = false;

        GraphablesViewModel GraphablesViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            GraphablesViewModel = new GraphablesViewModel();
            GraphDrawer.Graph = Graph;
            GraphDrawer.Graphables = GraphablesViewModel.Graphables;
            
            ExpressionsList.DataContext = GraphablesViewModel;
            DisableGridCheckBox.DataContext = this;
        }

        private void Add_ButtonClick(object sender, RoutedEventArgs e)
        {
            FunctionModel function = new FunctionModel();
            function.PropertyChanged += FunctionChange_NotifyEvent;
            GraphablesViewModel.Graphables.Add(function);

            GraphDrawer.DrawGraph();
        }

        private void FunctionChange_NotifyEvent(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsGraphable")
            {
                GraphDrawer.DrawGraph();
            }
        }

        /*
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
            */

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                GraphDrawer.ZoomOut();
            }
            else if (e.Delta < 0)
            {
                GraphDrawer.ZoomIn();
            }
            GraphDrawer.DrawGraph();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //GraphablesViewModel.Graphables.Add(new ConicSectionModel() { Color = Brushes.Black });
            GraphDrawer.DrawGraph();
            window_loaded = true;
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            GraphDrawer.DrawGraph();
        }

        private void Clear_ButtonClick(object sender, RoutedEventArgs e)
        {
            GraphablesViewModel.Graphables.Clear();
            GraphDrawer.DrawGraph();
        }

        private void RemoveItem_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            FunctionModel function = (FunctionModel)button.DataContext;

            GraphablesViewModel.Graphables.Remove(function);
            GraphDrawer.DrawGraph();
        }

        public static IEnumerable<Point> GetIntersectionPoints(PointCollection points1, PointCollection points2)
        {
            return points1.Intersect(points2);
        }

        private Point MousePosition { get; set; }
        private void Graph_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                return;
            }
            var currentPosition = e.GetPosition(Graph);

            GraphDrawer.Offset(MousePosition.X - currentPosition.X, MousePosition.Y - currentPosition.Y);
            MousePosition = currentPosition;
            GraphDrawer.DrawGraph();
        }

        private void Graph_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MousePosition = e.GetPosition(Graph);
        }
    }
}
