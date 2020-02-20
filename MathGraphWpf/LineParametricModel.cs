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
        private bool isGraphable;
        public event PropertyChangedEventHandler PropertyChanged;

        private double x1;
        private double x2;
        private double u1;
        private double u2;

        public double X1
        {
            get
            {
                return x1;
            }
            set
            {
                x1 = value;
                IsGraphable = ExpressionGrapher.IsLineParametricGraphable(Point, Vector);
                NotifyPropertyChanged();
            }
        }
        public double X2
        {
            get
            {
                return x2;
            }
            set
            {
                x2 = value;
                IsGraphable = ExpressionGrapher.IsLineParametricGraphable(Point, Vector);
                NotifyPropertyChanged();
            }
        }
        public double U1
        {
            get
            {
                return u1;
            }
            set
            {
                u1 = value;
                IsGraphable = ExpressionGrapher.IsLineParametricGraphable(Point, Vector);
                NotifyPropertyChanged();
            }
        }
        public double U2
        {
            get
            {
                return u2;
            }
            set
            {
                u2 = value;
                IsGraphable = ExpressionGrapher.IsLineParametricGraphable(Point, Vector);
                NotifyPropertyChanged();
            }
        }

        public Point Point
        {
            get
            {
                return new Point(X1, X2);
            }
        }

        public Point Vector
        {
            get
            {
                return new Point(U1, U2);
            }
        }

        public bool IsGraphable
        {
            get
            {
                return isGraphable;
            }
            set
            {
                isGraphable = value;
                NotifyPropertyChanged();
            }
        }
        public GraphableType Type
        {
            get
            {
                return GraphableType.LineParametric;
            }
        }
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
