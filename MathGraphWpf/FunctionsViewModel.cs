using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MathGraphWpf
{
    class FunctionsViewModel
    {
        public ObservableCollection<FunctionModel> TestFunctions { get; set; } = new ObservableCollection<FunctionModel>();
    }
}
