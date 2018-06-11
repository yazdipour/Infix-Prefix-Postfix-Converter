using System;

namespace InfixPostfixPrefix
{
    internal class Stack
    {
        private readonly string[] TheStack;
        private readonly int TheStackSize;
        private int top = -1;

        public Stack(int size = 20)
        {
            TheStack = new string[size];
            TheStackSize = size;
        }

        public string peek()
        {
            if (isEmpty())
                throw new Exception();
            return TheStack[top];
        }

        public string pop()
        {
            var res = peek();
            top--;
            return res;
        }

        public void push(string value)
        {
            if (top != TheStackSize - 1) TheStack[++top] = value;
            else throw new Exception();
        }

        public bool isEmpty()
        {
            return top == -1;
        }
    }
}