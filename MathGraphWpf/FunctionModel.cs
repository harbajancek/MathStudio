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
        public GraphableType Type => GraphableType.Function;

        private string expressionString = "1/x";
        private bool isGraphable = true;

        public event PropertyChangedEventHandler PropertyChanged;

        public string ExpressionString
        {
            get => expressionString;
            set
            {
                expressionString = value;
                IsGraphable = ExpressionGrapher.IsExpressionGraphable(expressionString);
                NotifyPropertyChanged();
            }
        }

        public bool IsGraphable
        {
            get => isGraphable;
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
