using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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

            CheckSettings();

            try
            {
                int countOfPesel = int.Parse(countPeselTextBox.Text);
                if (countOfPesel <= 0)
                {
                    countPeselLabel.Visibility = Visibility.Visible;
                    countPeselLabel.Content = "Błąd! Wartość musi być większa od zera.";
                }
                else
                {
                    List<string> pese = pesel.Generate(countOfPesel);

                    int i = 0;
                    foreach (var item in pese)
                    {
                        peselListView.Items.Insert(i, item);
                        i++;
                    }

                    countPeselLabel.Visibility = Visibility.Hidden;
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                countPeselLabel.Content = "Błąd! Podaj prawidłową wartość.";
                countPeselLabel.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            peselListView.Items.Clear();
        }

        private void CheckSettings()
        {
            PeselSettings settings = new PeselSettings
            {
                UseDay = CheckIfEnableAndChecked(IfUseDayCheckBox),
                UseMonth = CheckIfEnableAndChecked(IfUseMonthCheckBox),
                UseYear = CheckIfEnableAndChecked(IfUseYearCheckBox),
                Gender = GenderComboBox.Text,
                Count = int.Parse(countPeselTextBox.Text)
            };
            if (PeselDate.SelectedDate != null)
            {
                settings.Date = PeselDate.SelectedDate.Value.ToShortDateString();
            }
        }

        private bool CheckIfEnableAndChecked(CheckBox checkbox)
        {
            if (checkbox.IsEnabled && checkbox.IsChecked.GetValueOrDefault())
            {
                return true;
            }
            return false;
        }

        private void PeselDate_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (PeselDate.SelectedDate.HasValue)
            {
                IfUseDayCheckBox.IsEnabled = true;
                IfUseMonthCheckBox.IsEnabled = true;
                IfUseYearCheckBox.IsEnabled = true;
            }
            else
            {
                IfUseDayCheckBox.IsEnabled = false;
                IfUseMonthCheckBox.IsEnabled = false;
                IfUseYearCheckBox.IsEnabled = false;
            }
        }
    }

    class PeselSettings
    {
        public string Date { get; set; }
        public bool UseMonth { get; set; }
        public bool UseDay { get; set; }
        public bool UseYear { get; set; }
        public string Gender { get; set; }
        public int Count { get; set; }

    }
}
