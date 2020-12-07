using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DocumentNumberGenerator.Validators
{
    class Validators : IValidators
    {
        public void OnlyNumber(TextCompositionEventArgs e, TextBox textBox, Label label)
        {
            Regex regex = new Regex("[0-9]");
            e.Handled = !regex.IsMatch(e.Text);

            if (regex.IsMatch(e.Text) && textBox.Text.Length < textBox.MaxLength)
            {
                label.Visibility = Visibility.Visible;
                label.Content = "Należy podać tylko cyfry.";
            }
            else
            {
                label.Visibility = Visibility.Hidden;
            }
        }

    }
}
