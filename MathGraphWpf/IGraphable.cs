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
        IEnumerable<PointCollection> GetGraphPoints(float xmax, float ymax, float xmin, float ymin, float dx);
        Brush Color { get; set; }
    }
}
