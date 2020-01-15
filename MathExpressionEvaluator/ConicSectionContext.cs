using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    public class ConicSectionContext : IContext
    {
        public float A { get; set; }
        public float B { get; set; }
        public float C { get; set; }
        public float D { get; set; }
        public float E { get; set; }
        public float F { get; set; }
        public float X { get; set; }

        public float ResolveVariable(string name) => name switch
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
