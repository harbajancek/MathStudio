using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    public class ConicSectionContext : IContext
    {
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }
        public double D { get; set; }
        public double E { get; set; }
        public double F { get; set; }
        public double X { get; set; }

        public double ResolveVariable(string name) => name switch
        {
            _ when (name == "a" || name == "A") => A,
            _ when (name == "b" || name == "B") => B,
            _ when (name == "c" || name == "C") => C,
            _ when (name == "d" || name == "D") => D,
            _ when (name == "e" || name == "E") => E,
            _ when (name == "f" || name == "F") => F,
            _ when (name == "x" || name == "X") => X,
            _ => throw new Exception("Variable name not supported in this context.")
        };
}
}
