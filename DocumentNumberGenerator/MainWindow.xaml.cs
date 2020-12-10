using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using DocumentNumberGenerator.Pesel;
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

        private void Generate(object sender, RoutedEventArgs e)
        {
            PeselGenerator pesel = new PeselGenerator();
            int countOfPesel = Int32.Parse(countPeselTextBox.Text);
            List<string> pese = pesel.Generate(countOfPesel);

            int i = 0;
            foreach (var item in pese)
            {
                peselListView.Items.Insert(i, item);
                i++;
            }
        }
    }
}
