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
        private bool window_loaded { get; set; } = false;

        GraphablesViewModel GraphablesViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            GraphablesViewModel = new GraphablesViewModel();
            GraphDrawer.Graph = Graph;
            GraphDrawer.Graphables = GraphablesViewModel.Graphables;

            ExpressionsList.DataContext = GraphablesViewModel;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            window_loaded = true;
            //GraphablesViewModel.Graphables.Add(new ConicSectionModel() { Color = Brushes.Black });
            PreviewKeyDown += MainWindow_KeyDown;

            Draw();
        }

        private void Add_ButtonClick(object sender, RoutedEventArgs e)
        {
            FunctionModel function = new FunctionModel();
            function.PropertyChanged += FunctionChange_NotifyEvent;
            GraphablesViewModel.Graphables.Add(function);

            Draw();
        }

        private void FunctionChange_NotifyEvent(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsGraphable")
            {
                Draw();
            }
        }

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
            Draw();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw();
        }

        private void Clear_ButtonClick(object sender, RoutedEventArgs e)
        {
            GraphablesViewModel.Graphables.Clear();
            Draw();
        }

        private void RemoveItem_ButtonClick(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            FunctionModel function = (FunctionModel)button.DataContext;

            GraphablesViewModel.Graphables.Remove(function);
            Draw();
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

            GraphDrawer.Offset(MousePosition, currentPosition);
            MousePosition = currentPosition;
            Draw();
        }

        private void Graph_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MousePosition = e.GetPosition(Graph);
        }

        private void PICheckbox_Checked(object sender, RoutedEventArgs e)
        {
            GraphDrawer.IsPiEnabled = (bool)PICheckbox.IsChecked;
            Draw();
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                GraphDrawer.Offset(new Point(0, 0), new Point(1000, 0));
            }
            if (e.Key == Key.Right)
            {
                GraphDrawer.Offset(new Point(0, 0), new Point(-1000, 0));
            }
            if (e.Key == Key.Up)
            {
                GraphDrawer.Offset(new Point(0, 0), new Point(0, 1000));
            }
            if (e.Key == Key.Down)
            {
                GraphDrawer.Offset(new Point(0, 0), new Point(0, -1000));
            }
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();
                Focus();
                e.Handled = true;
            }

            Draw();
        }

        private void Draw()
        {
            if (window_loaded)
            {
                GraphDrawer.DrawGraph();
            }
        }
    }
}
