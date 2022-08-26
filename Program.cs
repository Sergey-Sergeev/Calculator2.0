using System;

namespace Programm
{
    class Programm
    {
        static void Main()
        {
            MathExample mathExample = new MathExample();
            String rules;
            String allSigns;
            String title;


            title = "Calculator v2.0";
            allSigns = "+ - * / ()";
            rules = "1.There must be sing near brackets;";

            Console.Title = title;

            ConsoleWrite("Rules: \n");
            ConsoleWrite(rules + "\n", ConsoleColor.Green);
            ConsoleWrite("All signs: ");
            ConsoleWrite(allSigns + "\n", ConsoleColor.Green);

            for (int i = 0; i < Console.BufferWidth; i++) ConsoleWrite("#", ConsoleColor.Red);

            for (; ; )
            {
                String exampleWithoutSpaces = String.Empty;
                String example;


                ConsoleWrite("Enter the equation: ");

                example = Console.ReadLine();

                //Delete spaces
                foreach (Char symbol in example)
                    if (symbol != ' ') exampleWithoutSpaces += symbol;

                ConsoleWrite(">>>");

                try { ConsoleWrite(mathExample.Calculate(exampleWithoutSpaces) + "\n", ConsoleColor.Green); }
                catch { ConsoleWrite("Error\n"); }
            }
        }
        static void ConsoleWrite(String str, ConsoleColor consoleColor = ConsoleColor.Cyan)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(str);
            Console.ResetColor();
        }
    }
}
