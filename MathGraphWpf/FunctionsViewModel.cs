using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MathStudioWpf
{
    class FunctionsViewModel
    {
        public ObservableCollection<IGraphable> TestFunctions { get; set; } = new ObservableCollection<IGraphable>();
    }
}
