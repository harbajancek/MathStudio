using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class ConicSectionContext : IContext
    {
        public decimal A { get; set; }
        public decimal B { get; set; }
        public decimal C { get; set; }
        public decimal D { get; set; }
        public decimal E { get; set; }
        public decimal F { get; set; }
        public decimal X { get; set; }

        public decimal ResolveVariable(string name) => name switch
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
