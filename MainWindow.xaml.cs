using System.Windows;
using System.Windows.Controls;

namespace WpfCalculator;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        var button = (Button)sender;
        var operandString = button.Tag.ToString();
        int operand = int.Parse(operandString);
        MessageBox.Show(operand.ToString());
    }
}