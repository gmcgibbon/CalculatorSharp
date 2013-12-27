using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const String ERROR_MESSAGE = "Error";

        private bool reset;
        private List<KeyValuePair<string,double>> history;
        private CalculationParser parser;
        private ObservableCollection<string> observableHistory;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            parser = new CalculationParser(Calculation);
            history = new List<KeyValuePair<string,double>>();
            observableHistory = new ObservableCollection<string>();
            observableHistory.Add("_Reset");
            reset = true;

            Button_Click(null, null);

            foreach (Button btn in ButtonGrid.Children.OfType<Button>() )
            {
                btn.Click += Button_Click;
            }

            HistoryMenu.DataContext = observableHistory;
            Info.Text = String.Format("{0} v{1}", App.NAME, App.VERSION);
        }

        /// <summary>
        /// Equals button click handler that evaluates a calculation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Equal_Click(object sender, RoutedEventArgs e)
        {
            if (!Calculation.Text.Equals("0"))
            {
                try
                {
                    updateCalcHistory(parser.Evaluate());
                }
                catch
                {
                    Calculation.Text = "Error";
                    reset = true;
                }
            }
            
        }

        /// <summary>
        /// Number button click handler that prints the corresponding number out to the calculation box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Number_Click(object sender, RoutedEventArgs e)
        {
            if(reset)
            {
                Calculation.Clear();
                reset = false;
            }
            Calculation.Text += ((Button)sender).Content;
        }

        /// <summary>
        /// Clear button click handler that clears a single entry or all history
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ClearOperation_Click(object sender, RoutedEventArgs e)
        {
            if(((Button)sender).Content.Equals("C"))
            {
                for (var i = 0; i < history.Count; i++)
                {
                    history.RemoveAt(i);
                    observableHistory.RemoveAt((++i));
                }
            }
            resetCalc();
        }

        /// <summary>
        /// History menu item click handler that updates the calculation box to the 
        /// clicked history entry and removes previous items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HistoryMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var sourceItem = ((MenuItem)e.OriginalSource).DataContext.ToString();
            var sourceIndex = observableHistory.IndexOf(sourceItem)-1;

            if (sourceIndex == -1)
            {
                sourceIndex = 0;
                resetCalc();
            }
            else
            {
                Calculation.Text = history[sourceIndex].Value.ToString();
            }

            for (var i = sourceIndex; i < history.Count; i++)
            {
                history.RemoveAt(i);
                observableHistory.RemoveAt((++i));
            }
        }

        /// <summary>
        /// Info menu item click handler that displays a message box with app information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InfoMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(App.AUTHOR, "Author");
        }

        /// <summary>
        /// Web menu item clcik handler that opens the app website in the default browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(App.WEBSITE);
        }

        /// <summary>
        /// Resets the calculation box
        /// </summary>
        private void resetCalc()
        {
            reset = true;
            Calculation.Text = "0";
        }

        /// <summary>
        /// Updates the calculator history lists
        /// </summary>
        /// <param name="pair">KeyValuePair returned from the evaluation</param>
        private void updateCalcHistory(KeyValuePair<string, double> pair)
        {
            history.Add(pair);
            observableHistory.Add(String.Format("{0} = {1}", pair.Key, pair.Value));
        }

        /// <summary>
        /// Button click handler for all calculator buttons
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Calculation.Focus();
            Calculation.SelectionStart = Calculation.Text.Length;
        }

        /// <summary>
        /// Operation button click handler that prints the corresponding operation out to calculation box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Operation_Click(object sender, RoutedEventArgs e)
        {
            if (reset)
            {
                reset = false;
            }
            if (Calculation.Text == ERROR_MESSAGE)
            {
                Calculation.Text = "0";
            }
            Calculation.Text += ((Button)sender).Content;
        }

        /// <summary>
        /// SwitchSign button click handler that reverses the sign of the current calculation box number
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchSign_Click(object sender, RoutedEventArgs e)
        {
            Calculation.Text = String.Format("{0}({1})","-1*",Calculation.Text);
            Equal_Click(null, null);
        }

        /// <summary>
        /// Backspace button click handler that removes the character at the end of the calculation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Backspace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Calculation.Text = Calculation.Text.Remove(Calculation.Text.Length - 1);
            }
            catch { }
        }

    }
}
