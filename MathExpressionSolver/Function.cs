using MathPatterns;
using System;

namespace MathExpressionSolver
{
    public class Function
    {
        private string baseString;
        public string StringExpression
        {
            get
            {
                return baseString;
            }
        }
        public Interval Domain
        {
            get;
        }
        public Interval Range { get; }
        public decimal Max { get; }
        public decimal Min { get; }
        public bool isPeriodic { get; }
        public decimal Limit { get; }
    }
}
