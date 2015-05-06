using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfixPrefixPostfix
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
            //    Console.WriteLine("\n-----------------------\nEnter your Infix:");
            //    InfixPrefixPostfixConverter.ConvertTo ConvertIt = new InfixPrefixPostfixConverter.ConvertTo(Console.ReadLine());
            //    Console.WriteLine("\nPostfix:"); Console.WriteLine(ConvertIt.InToPost());
            //    Console.WriteLine("\nPrefix:"); Console.WriteLine(ConvertIt.InToPre());
            //    Console.WriteLine("\nEvaluate:"); Console.WriteLine(ConvertIt.Evaluate());
                InfixPrefixPostfixConverter.ConvertTo ConvertIt = new InfixPrefixPostfixConverter.ConvertTo();
                ConvertIt.POSTstr = Console.ReadLine();
                Console.WriteLine("\nInfix:"); Console.WriteLine(ConvertIt.PostToIn());
                Console.WriteLine("\nPrefix:"); Console.WriteLine(ConvertIt.PostToPre());
                Console.WriteLine("\nPostfix:"); Console.WriteLine(ConvertIt.PreToPost());



                Console.WriteLine("\nEvaluate:"); Console.WriteLine(ConvertIt.Evaluate());
            }
        }
    }
}
