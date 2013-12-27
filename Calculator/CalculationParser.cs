using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
            string evalStr = Display.Text;
            double evalNum = Double.Parse(new DataTable().Compute(evalStr, null).ToString());

            Display.Text = evalNum.ToString();

            return new KeyValuePair<string, double>(evalStr, evalNum);
        }
    }
}
