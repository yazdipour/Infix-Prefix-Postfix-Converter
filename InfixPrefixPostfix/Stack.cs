using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfixPrefixPostfixConverter
{
    class Stack
    {
        private string[] TheStack;
        private int top = -1;
        private int TheStackSize;

        public Stack(int size=20)
        {
            TheStack = new string[size];
            this.TheStackSize = size;
        }
        public string peek()
        {
            if (isEmpty()) 
                throw new Exception();
            return TheStack[top];            
        }
        public string pop(){
            string res = peek();
            top--;
            return res;
        }
        public void push(string value)
        {
            if(top!=TheStackSize-1)TheStack[++top]=(value);
            else throw new Exception();
        }
        public bool isEmpty()
        {
            return top == -1;
        }
    }
}
