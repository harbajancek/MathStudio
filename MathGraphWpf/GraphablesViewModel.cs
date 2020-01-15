using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MathStudioWpf
{
    class GraphablesViewModel
    {
        public ObservableCollection<IGraphable> Graphables { get; set; } = new ObservableCollection<IGraphable>();
    }
}
