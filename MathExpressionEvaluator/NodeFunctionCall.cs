using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeFunctionCall : Node
    {
        private string FunctionName { get; set; }
        private Node[] Arguments { get; set; }
        public NodeFunctionCall(string functionName, Node[] arguments)
        {
            FunctionName = functionName;
            Arguments = arguments;
        }

        public override decimal? Eval(IContext context)
        {
            var argumentValues = new decimal?[Arguments.Length];
            for (int i = 0; i < Arguments.Length; i++)
            {
                argumentValues[i] = Arguments[i].Eval(context);
            }
            return context.CallFunction(FunctionName, argumentValues);
        }
    }
}
