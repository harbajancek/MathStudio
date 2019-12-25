using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Media;

namespace MathStudioWpf
{
    interface IGraphable : INotifyPropertyChanged
    {
        bool IsGraphable { get; set; }
        IEnumerable<PointCollection> GetGraphPoints(decimal xmax, decimal ymax, decimal xmin, decimal ymin, decimal dx);
        Brush Color { get; set; }
    }
}
