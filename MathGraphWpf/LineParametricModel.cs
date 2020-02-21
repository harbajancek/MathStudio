using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MathStudioWpf
{
    class LineParametricModel : IGraphable
    {
        private bool isGraphable = true;
        public event PropertyChangedEventHandler PropertyChanged;

        private double x1 = 0;
        private double x2 = 0;
        private double u1 = 1;
        private double u2 = 1;

        public double X1
        {
            get => x1;
            set
            {
                x1 = value;
                IsGraphable = ExpressionGrapher.IsLineParametricGraphable(Point, Vector);
                NotifyPropertyChanged();
            }
        }
        public double X2
        {
            get => x2;
            set
            {
                x2 = value;
                IsGraphable = ExpressionGrapher.IsLineParametricGraphable(Point, Vector);
                NotifyPropertyChanged();
            }
        }
        public double U1
        {
            get => u1;
            set
            {
                u1 = value;
                IsGraphable = ExpressionGrapher.IsLineParametricGraphable(Point, Vector);
                NotifyPropertyChanged();
            }
        }
        public double U2
        {
            get => u2;
            set
            {
                u2 = value;
                IsGraphable = ExpressionGrapher.IsLineParametricGraphable(Point, Vector);
                NotifyPropertyChanged();
            }
        }

        public Point Point => new Point(X1, X2);

        public Point Vector => new Point(U1, U2);

        public bool IsGraphable
        {
            get => isGraphable;
            set
            {
                isGraphable = value;
                NotifyPropertyChanged();
            }
        }
        public GraphableType Type => GraphableType.LineParametric;
        public Brush Color { get; set; }


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<IEnumerable<Point>> GetGraphPoints()
        {
            foreach (var item in ExpressionGrapher.GetLinePointsFromParametres(Point, Vector))
            {
                yield return item;
            }
        }
    }
}
