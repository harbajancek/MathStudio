using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace MathStudioWpf
{
    class ThreePointsModel : IGraphable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isGraphable = true;
        private double x1 = 0;
        private double y1 = 1;
        private double x2 = 1;
        private double y2 = 1;
        private double x3 = 1;
        private double y3 = 0;
        private ThreePointsType type = ThreePointsType.Circle;

        public double X1
        {
            get => x1;
            set
            {
                x1 = value;
                IsGraphable = ExpressionGrapher.IsThreePointsGraphable(Point1, Point2, Point3);
                NotifyPropertyChanged();
            }
        }
        public double Y1
        {
            get => y1;
            set
            {
                y1 = value;
                IsGraphable = ExpressionGrapher.IsThreePointsGraphable(Point1, Point2, Point3);
                NotifyPropertyChanged();
            }
        }
        public double X2
        {
            get => x2;
            set
            {
                x2 = value;
                IsGraphable = ExpressionGrapher.IsThreePointsGraphable(Point1, Point2, Point3);
                NotifyPropertyChanged();
            }
        }
        public double Y2
        {
            get => y2;
            set
            {
                y2 = value;
                IsGraphable = ExpressionGrapher.IsThreePointsGraphable(Point1, Point2, Point3);
                NotifyPropertyChanged();
            }
        }
        public double X3
        {
            get => x3;
            set
            {
                x3 = value;
                IsGraphable = ExpressionGrapher.IsThreePointsGraphable(Point1, Point2, Point3);
                NotifyPropertyChanged();
            }
        }
        public double Y3
        {
            get => y3;
            set
            {
                y3 = value;
                IsGraphable = ExpressionGrapher.IsThreePointsGraphable(Point1, Point2, Point3);
                NotifyPropertyChanged();
            }
        }
        public ThreePointsType ConicSectionType
        {
            get => type;
            set
            {
                type = value;
                IsGraphable = ExpressionGrapher.IsThreePointsGraphable(Point1, Point2, Point3);
                NotifyPropertyChanged();
            }
        }
        public Point Point1 { get => new Point(X1, Y1); }
        public Point Point2 { get => new Point(X2, Y2); }
        public Point Point3 { get => new Point(X3, Y3); }
        public bool IsGraphable
        {
            get => isGraphable;
            set
            {
                isGraphable = value;
                NotifyPropertyChanged();
            }
        }
        public GraphableType Type => GraphableType.ThreePoints;
        public Brush Color { get; set; }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<IEnumerable<Point>> GetGraphPoints()
        {
            foreach (var item in ExpressionGrapher.GetLinePointsFromThreePoints(Point1, Point2, Point3, ConicSectionType))
            {
                yield return item;
            }
        }
    }
}
