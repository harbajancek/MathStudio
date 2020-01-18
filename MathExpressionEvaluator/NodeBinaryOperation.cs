using System;
using System.Collections.Generic;
using System.Text;

namespace MathExpressionEvaluator
{
    class NodeBinaryOperation : Node
    {
        protected Node Left { get; private set; }
        protected Node Right { get; private set; }
        private BinaryOperation Operation { get; }

        public NodeBinaryOperation(Node left, Node right, BinaryOperation operation)
        {
            Left = left;
            Right = right;
            Operation = operation;
        }

        public override double Eval(IContext context)
        {
            double leftValue = Left.Eval(context);
            double rightValue = Right.Eval(context);

            return Operation.Action(leftValue, rightValue);
        }

        public override Node Simplify()
        {
            Left = Left.Simplify();
            Right = Right.Simplify();
            if (Left is NodeNumber && Right is NodeNumber)
            {
                double left = (Left as NodeNumber).Number;
                double right = (Right as NodeNumber).Number;
                return new NodeNumber(Operation.Action(left, right));
            }
            else if (Left is NodeVariable && Right is NodeVariable)
            {
                string left = (Left as NodeVariable).Variable;
                string right = (Right as NodeVariable).Variable;

                if (left == right)
                {
                    if (Operation.Type == OperationType.Subtraction)
                    {
                        return new NodeNumber(0);
                    }
                }
            }
            return this;
        }
    }
}
