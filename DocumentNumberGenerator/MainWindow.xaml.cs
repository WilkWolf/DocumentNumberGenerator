using System.Windows;
using System.Windows.Input;
using DocumentNumberGenerator.Validators;

namespace DocumentNumberGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IValidators validators = new Validators.Validators();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnlyNumberInTextBox(object sender, TextCompositionEventArgs e)
        {
            validators.OnlyNumber(e, countPeselTextBox, countPeselLabel);
        }

    }
}
