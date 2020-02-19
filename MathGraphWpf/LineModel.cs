using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MathStudioWpf
{
    class LineModel : IGraphable
    {
        public GraphableType Type
        {
            get
            {
                return GraphableType.Line;
            }
        }

        private bool isGraphable;

        public event PropertyChangedEventHandler PropertyChanged;

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
        public Brush Color { get; set; }


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<IEnumerable<Point>> GetGraphPoints(double xmax, double ymax, double xmin, double ymin, double dx)
        {
            List<Point> points = new List<Point>();

            yield return points;
        }
    }
}
