using System.Windows;
using System.Windows.Controls;

namespace WpfCalculator;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    //Calculator calculator;
    public MainWindow()
    {
        InitializeComponent();
        //calculator = new Calculator { Result = "0" };
        //this.DataContext = calculator;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var operandString = button.Tag.ToString();

        //calculator.AddChar(operandString);
    }


}