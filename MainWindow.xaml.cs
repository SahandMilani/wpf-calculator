using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace WpfCalculator;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    Calculator calculator;
    public MainWindow()
    {
        InitializeComponent();
        calculator = new Calculator { Result = "0" };
        this.DataContext = calculator;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var operandString = button.Tag.ToString();

        calculator.AddChar(operandString);
    }

    public class Calculator : INotifyPropertyChanged
    {
        readonly HashSet<string> operators = new() { "+", "-", "*", "/", "=" };
        readonly HashSet<string> numbers = new() { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "." };
        readonly HashSet<string> deleteCommands = new() { "←", "C", "CE" };

        List<string> parts = new();

        private string? result;

        public string? Result
        {
            get { return result; }
            set
            {
                result = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddChar(string nextChar)
        {
            if (deleteCommands.Contains(nextChar))
            {
                if (nextChar == "←")
                    DeleteCharFromEnd();
                if (nextChar == "C")
                    ClearAll();
            }

            if (numbers.Contains(nextChar))
            {
                AddNumberToEnd(nextChar);
            }

            if (operators.Contains(nextChar))
            {
                if (nextChar == "=")
                    Calculate();

                else
                    AddOperatorToEnd(nextChar);
            }

            UpdateResult();
        }

        /*void AddToEnd(string nextChar)
        {
            if (Result == "0")
                Result = "";
            Result += nextChar;
        }*/

        void AddNumberToEnd(string charToAdd)
        {
            if (parts.Count == 0)
            {
                if (charToAdd == ".")
                    charToAdd = "0.";

                parts.Add(charToAdd);
                return;
            }

            string last = parts.Last();

            if (operators.Contains(last))
            {
                if (charToAdd == ".")
                    charToAdd = "0.";

                parts.Add(charToAdd);
                return;
            }

            if (charToAdd == "." && last.Contains('.'))
                return;

            if (charToAdd != "." && last == "0")
                last = "";

            parts[^1] = last + charToAdd;
        }

        void AddOperatorToEnd(string charToAdd)
        {
            if (parts.Count == 0)
                return;

            string last = parts.Last();

            // last is also operator -> replace with the new one
            if (operators.Contains(last))
            {
                parts[^1] = charToAdd;
                return;
            }

            parts.Add(charToAdd);
        }

        void DeleteCharFromEnd()
        {
            if (parts.Count == 0)
                return;

            string last = parts.Last();
            parts[^1] = last.Remove(last.Length - 1);

            if (parts[^1] == "")
                parts.RemoveAt(parts.Count - 1);

        }

        void ClearAll()
        {
            parts.Clear();
        }


        void UpdateResult()
        {
            Result = String.Join(" ", parts);

            if (Result == "")
                Result = "0";
        }

        void Calculate()
        {
            List<string> partsClone = new List<string>(parts);
            //MessageBox.Show(Result);
            Dictionary<string, Func<double, double, double>> operatorsByOrder = new() {
                {"*", (a, b) => a * b},
                {"/", (a, b) => a / b},
                {"+", (a, b) => a + b},
                {"-", (a, b) => a - b}
            };

            foreach (var (op, func) in operatorsByOrder)
            {
                for (int i = 0; i < partsClone.Count; i++)
                {
                    if (partsClone[i] == op)
                    {
                        double expressionResult = func(Double.Parse(partsClone[i - 1]), Double.Parse(partsClone[i + 1]));
                        if (Double.IsInfinity(expressionResult))
                        {
                            MessageBox.Show("ERROR");
                            return;
                        }
                        partsClone[i] = expressionResult.ToString();
                        partsClone.RemoveAt(i + 1);
                        partsClone.RemoveAt(i - 1);
                        i--;
                    }
                }
            }
            //MessageBox.Show(parts[0]);

            parts = new List<string>(partsClone);
        }


    }
}