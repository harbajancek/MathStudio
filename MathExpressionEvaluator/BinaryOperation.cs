using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class BinaryOperation
    {
        public Func<double,double,double> Action { get; }
        public OperationType Type { get; }

        public BinaryOperation(Func<double, double, double> operation, OperationType type)
        {
            Action = operation;
            Type = type;
        }
    }
}
