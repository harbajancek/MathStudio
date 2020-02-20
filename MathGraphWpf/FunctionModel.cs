using MathExpressionEvaluator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MathStudioWpf
{
    class FunctionModel : IGraphable
    {
        public GraphableType Type
        {
            get
            {
                return GraphableType.Function;
            }
        }

        private string expressionString;
        private bool isGraphable;

        public event PropertyChangedEventHandler PropertyChanged;

        public string ExpressionString
        {
            get
            {
                return expressionString;
            }
            set
            {
                expressionString = value;
                IsGraphable = ExpressionGrapher.IsExpressionGraphable(expressionString);
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
        public Brush Color { get; set; }


        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public IEnumerable<IEnumerable<Point>> GetGraphPoints()
        {
            foreach (var item in ExpressionGrapher.GetGraphPointsFromExpression(ExpressionString))
            {
                yield return item;
            }
        }
    }
}
