using System;
using System.Collections.Generic;

namespace Programm
{
    internal class MathExample
    {
        private Char[] allSymbols, numberSymbols;

        public MathExample()
        {
            allSymbols = new Char[] { '*', '/', '+', '-', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '(' };
            numberSymbols = new Char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        }
        /// <summary>
        /// Calculate full math example.
        /// Return result.
        /// </summary>
        /// <param name="equation"></param>
        /// <returns></returns>
        public Double Calculate(String equation)
        {
            List<String> newNumbers = new List<String>();
            List<Double> resultsOfMultiplicationAndDivision = new List<Double>();
            String[] numbers;
            String backNumber = String.Empty;
            Double result = 0;

            //lastWasMulty is necessary for chains of multiplication or division operations.
            //start needs for first operation.
            bool lastWasMulty = false, start = true;
            const int FIRST = 0, SECOND = 1, THIRD = 2;


            numbers = FindAllNumbers(equation);

            //Сalculate all under math examples.
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i].Length > 1 && numbers[i][SECOND] == '(')
                {
                    //Take the sign from parenthesis.
                    Char sign = numbers[i][FIRST];
                    Double underEquationNumber = Calculate(numbers[i].Substring(THIRD, numbers[i].Length - 3));

                    //The plus symbol will not need, if it is a negative number.
                    if (underEquationNumber < 0 && sign == '+') numbers[i] = underEquationNumber.ToString();
                    else numbers[i] = sign.ToString() + underEquationNumber.ToString();
                }
            }

            //Calculate multiplication and division first.
            foreach (String number in numbers)
            {
                switch (number[FIRST])
                {
                    case '*':
                    case '/':

                        //Save and reset result.
                        if (!lastWasMulty && !start)
                        {
                            resultsOfMultiplicationAndDivision.Add(result);
                            result = 0;
                        }

                        //If lastWasMulty is true, backNumber = last result.
                        //Delete sign of multiplication or of division.
                        if (lastWasMulty) backNumber = backNumber.Substring(SECOND);

                        Double.TryParse(number.Substring(SECOND), out Double localParseNumber);
                        Double.TryParse(backNumber, out Double localParseBackNumber);

                        if (!lastWasMulty || start)
                        {
                            if (number[FIRST] == '/')
                                result += localParseBackNumber / localParseNumber;
                            else result += localParseBackNumber * localParseNumber;

                            newNumbers.RemoveAt(newNumbers.Count - 1);
                        }
                        else
                        {
                            if (number[FIRST] == '/') result /= localParseNumber;
                            else result *= localParseNumber;
                        }

                        start = false;
                        lastWasMulty = true;
                        break;
                    default:
                        lastWasMulty = false;
                        newNumbers.Add(number);
                        break;
                }

                backNumber = number;
            }

            //Calculate results of all multiplication and division.
            foreach (Double number in resultsOfMultiplicationAndDivision)
                result += number;

            //Then calculate subtraction and addition.
            foreach (String number in newNumbers)
            {
                switch (number[FIRST])
                {
                    case '+':
                    case '-':
                        Double.TryParse(number, out Double localParseNumber);
                        result += localParseNumber;
                        break;
                    default:
                        foreach (Char symbol in numberSymbols)
                            if (number[FIRST] == symbol) goto case '-';
                        break;
                }
            }

            return result;
        }
        private String[] FindAllNumbers(String equation)
        {
            List<String> numbers = new List<String>();
            bool isTrueSymbol = false;
            bool isUnderEquation = false;


            for (int index = 0; index < equation.Length;)
            {
                //Find start char of number or of math example in parentheses.
                foreach (Char symbol in allSymbols)
                {
                    if (equation[index] == symbol)
                    {
                        //Check, if it is math example in parentheses.
                        if (symbol == '(') isUnderEquation = true;

                        isTrueSymbol = true;
                        break;
                    }
                }

                if (isTrueSymbol)
                {
                    String number = GetNextNumber(equation.Substring(index).ToCharArray());
                    numbers.Add(number);

                    if (isUnderEquation)
                    {
                        //We must subtract two symbols of parentheses.
                        index += number.Length - 2;
                        isUnderEquation = false;
                    }
                    else index += number.Length;

                    isTrueSymbol = false;
                }
                else index++;
            }

            return numbers.ToArray();
        }
        //Return next number or math example in parentheses with sign.
        private String GetNextNumber(Char[] equation)
        {
            //Check, if it is one symbol.
            if (equation.Length == 1)
                return new String(equation);


            List<Char> number = new List<Char>();
            int underEquationCount = 0;
            bool isUnderEquation = false;


            //Check parenthesis. Two chars because the parentheses can have a sign.
            for (int localIndex = 0; localIndex < 2; localIndex++)
            {
                if (equation[localIndex] == '(')
                {
                    //If localIndex = 0, it will mean the parentheses haven't sing.
                    if (localIndex == 0) number.Add('+');

                    isUnderEquation = true;
                    break;
                }
            }

            //Collect number or full math example with parentheses .
            for (int next = 1, index = 0; index < equation.Length;)
            {
                if (!isUnderEquation)
                {
                    switch (equation[next])
                    {
                        case '+':
                        case '-':
                            //Check, if it is a negative or positive number with division or multiplication symbols.
                            if (equation[index] == '*' || equation[index] == '/') break;
                            else goto case '/';
                        case '/':
                        case '*':
                            number.Add(equation[index]);
                            goto exit;
                    }
                }
                else
                {
                    switch (equation[index])
                    {
                        case '(':
                            underEquationCount++;
                            break;
                        case ')':
                            if (--underEquationCount == 0)
                            {
                                number.Add(equation[index]);
                                goto exit;
                            }
                            break;
                    }
                }

                number.Add(equation[index]);
                index++;
                next = next + 1 < equation.Length ? next + 1 : next;
            }
        exit:
            return new String(number.ToArray());
        }
    }
}
