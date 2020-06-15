using System.Windows.Controls;
using System.Windows.Input;

namespace DocumentNumberGenerator.Validators
{
    interface IValidators
    {
        void OnlyNumber(TextCompositionEventArgs e, TextBox textBox, Label label);
    }
}
