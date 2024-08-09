using System.Windows;
using System.Windows.Input;
using WpfCalculator.Model;

namespace WpfCalculator.ViewModel;
public class CalculatorViewModel : BaseViewModel
{
    Calculator calculator;

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
    private ICommand _buttonPressedCommand;

    public ICommand ButtonPressedCommand
    {
        get
        {
            if (_buttonPressedCommand == null)
            {
                _buttonPressedCommand = new RelayCommand(param => ExecuteButtonPressedCommand((string)param), param => true);
            }
            return _buttonPressedCommand;
        }
    }

    public CalculatorViewModel()
    {
        calculator = new Calculator();
    }

    private void ExecuteButtonPressedCommand(string param)
    {
        // Logic to execute when the command is invoked
        MessageBox.Show("Button clicked!" + param);
    }
}