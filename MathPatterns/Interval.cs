using System;
using System.Collections.Generic;

namespace MathPatterns
{
    public class Interval
    {
        public IntervalSet Set { get; }
        public List<decimal> IntervalEdges { get; }
        public bool IsComplement { get; }

        public Interval(decimal number1, decimal number2, IntervalSet set, bool isComplement)
        {
            IntervalEdges = new List<decimal>() { number1, number2 };
            IsComplement = isComplement;
            Set = set;
        }
        public Interval(decimal number, IntervalSet set)
        {
            IntervalEdges = new List<decimal>() { number };
            IsComplement = true;
            Set = set;
        }
    }
}
