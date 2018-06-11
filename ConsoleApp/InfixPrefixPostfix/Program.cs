using System;

namespace InfixPrefixPostfix
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n-----------------------\nWanna Enter :");
                Console.WriteLine("\n\t1.Infix\n\t2.Prefix:\n\t3.Postfix:");

                var convertIt = new ConvertTo();
                var menu = Console.ReadLine();
                switch (menu)
                {
                    case "1":
                        convertIt.INstr = Console.ReadLine();
                        Console.WriteLine("\nInfix:");
                        Console.WriteLine(convertIt.INstr);
                        break;

                    case "2":
                        convertIt.PREstr = Console.ReadLine();
                        Console.WriteLine("\nInfix:");
                        Console.WriteLine(convertIt.PreToIn());
                        break;

                    case "3":
                        convertIt.POSTstr = Console.ReadLine();
                        Console.WriteLine("\nInfix:");
                        Console.WriteLine(convertIt.PostToIn());
                        break;
                }
                Console.WriteLine("\nPrefix:");
                Console.WriteLine(convertIt.InToPre());
                Console.WriteLine("\nPostfix:");
                Console.WriteLine(convertIt.InToPost());


                Console.WriteLine("\nEvaluate:");
                Console.WriteLine(convertIt.Evaluate());
            }
        }
    }
}