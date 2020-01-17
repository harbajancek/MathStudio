using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace MathStudioWpf
{
    interface IGraphable : INotifyPropertyChanged
    {
        bool IsGraphable { get; set; }
        IEnumerable<IEnumerable<Point>> GetGraphPoints(double xmax, double ymax, double xmin, double ymin, double dx);
        Brush Color { get; set; }
    }
}
