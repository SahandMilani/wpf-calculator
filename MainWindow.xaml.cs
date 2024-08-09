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
        int operand = int.Parse(operandString);

        calculator.Result += operand.ToString();
    }

    public class Calculator : INotifyPropertyChanged
    {
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
    }
}