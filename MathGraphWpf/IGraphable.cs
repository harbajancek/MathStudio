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
        GraphableType Type { get; }
        IEnumerable<IEnumerable<Point>> GetGraphPoints();
        Brush Color { get; set; }
    }
}
