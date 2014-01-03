using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using NCalc;

namespace Calculator
{
    /// <summary>
    /// Parses string calcualations from a display text box
    /// </summary>
    class CalculationParser
    {
        private TextBox Display;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="box">TextBox for obtaining and displaying calculations</param>
        public CalculationParser(TextBox box)
        {
            Display = box;
        }

        /// <summary>
        /// Parses calculation string and displays result in Display
        /// </summary>
        /// <returns>KeyValuePair of the string calculation and result</returns>
        public KeyValuePair<string, double> Evaluate()
        {
            string evalStr = Regex
                .Replace(parseBrackets(Display.Text), @"[A-Za-z\s]", string.Empty)
                .Replace("√","Sqrt");
            Console.WriteLine(evalStr);
            double evalNum = Double.Parse(new Expression(evalStr).Evaluate().ToString());

            Display.Text = evalNum.ToString();

            return new KeyValuePair<string, double>(evalStr, evalNum);
        }

        /// <summary>
        /// Adds multiplication symbols when needed to bracketed sections
        /// </summary>
        /// <param name="calculation">Calculation string</param>
        /// <returns>Calculation with multiplication symbols added</returns>
        private string parseBrackets(string calculation)
        {
            char previousChar;
            char nextChar;

            for (var i = 1; i < calculation.Length-1; i++ )
            {
                if (calculation[i].Equals('('))
                {
                    previousChar = calculation[i-1];
                    if (Char.IsNumber(previousChar) || previousChar.Equals(')'))
                    {
                        calculation = calculation.Insert(i, "*");
                        i++;
                    }
                }
                else if (calculation[i].Equals(')'))
                {
                    nextChar = calculation[i+1];
                    if (Char.IsNumber(nextChar) || nextChar.Equals('('))
                    {
                        calculation = calculation.Insert(i+1, "*");
                        i++;
                    }
                    
                }
            }

            return calculation;
        }
    }
}
