using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MathStudioWpf
{
    class TwoPointsModel : IGraphable
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private bool isGraphable = true;
        private double x1 = 0;
        private double y1 = 0;
        private double x2 = 1;
        private double y2 = 1;

        public double X1
        {
            get => x1;
            set
            {
                x1 = value;
                IsGraphable = ExpressionGrapher.IsTwoPointsGraphable(Point1, Point2);
                NotifyPropertyChanged();
            }
        }
        public double Y1
        {
            get => y1;
            set
            {
                y1 = value;
                IsGraphable = ExpressionGrapher.IsTwoPointsGraphable(Point1, Point2);
                NotifyPropertyChanged();
            }
        }
        public double X2
        {
            get => x2;
            set
            {
                x2 = value;
                IsGraphable = ExpressionGrapher.IsTwoPointsGraphable(Point1, Point2);
                NotifyPropertyChanged();
            }
        }
        public double Y2
        {
            get => y2;
            set
            {
                y2 = value;
                IsGraphable = ExpressionGrapher.IsTwoPointsGraphable(Point1, Point2);
                NotifyPropertyChanged();
            }
        }
        public Point Point1 { get => new Point(X1, Y1); }
        public Point Point2 { get => new Point(X2, Y2); }
        public bool IsGraphable
        {
            get => isGraphable;
            set
            {
                isGraphable = value;
                NotifyPropertyChanged();
            }
        }
        public GraphableType Type => GraphableType.TwoPoints;
        public Brush Color { get; set; }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<IEnumerable<Point>> GetGraphPoints()
        {
            foreach (var item in ExpressionGrapher.GetLinePointsFromTwoPoints(Point1, Point2))
            {
                yield return item;
            }
        }
    }
}
