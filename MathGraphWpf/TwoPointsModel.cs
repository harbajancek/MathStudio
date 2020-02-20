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

        private bool isGraphable;
        private double x1;
        private double y1;
        private double x2;
        private double y2;

        public double X1
        {
            get
            {
                return x1;
            }
            set
            {
                x1 = value;
                //IsGraphable = 
                NotifyPropertyChanged();
            }
        }
        public double Y1
        {
            get
            {
                return y1;
            }
            set
            {
                y1 = value;
                //IsGraphable = 
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
                //IsGraphable = 
                NotifyPropertyChanged();
            }
        }
        public double Y2
        {
            get
            {
                return y2;
            }
            set
            {
                y2 = value;
                //IsGraphable = 
                NotifyPropertyChanged();
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
                return GraphableType.TwoPoints;
            }
        }
        public Brush Color { get; set; }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<IEnumerable<Point>> GetGraphPoints()
        {
            Point point1 = new Point(X1, Y2);
            Point point2 = new Point(X2, Y2);
            foreach (var item in ExpressionGrapher.GetLinePointsFromTwoPoints(point1, point2))
            {
                yield return item;
            }
        }
    }
}
