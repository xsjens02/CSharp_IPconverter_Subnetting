using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SubnettingProgram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ViewModels.ViewModel();
        }

        public void ValidateOctet(Object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"^([0-9]{1,3})?(\.[0-9]{0,3}){0,3}$");

            TextBox? textBox = sender as TextBox;
            string newText = textBox?.Text + e.Text;

            if (!regex.IsMatch(newText))
            {
                e.Handled = true;
                return;
            }

            var parts = newText.Split('.');
            if (parts.Length > 4)
            {
                e.Handled = true;
                return;
            }

            foreach (var part in parts)
            {
                if (!string.IsNullOrEmpty(part) && int.TryParse(part, out int value))
                {
                    if (value < 0 || value > 255)
                    {
                        e.Handled = true;
                        return;
                    }
                }
            }

            e.Handled = false;
        }
    }
}