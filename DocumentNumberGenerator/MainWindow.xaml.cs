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

            try
            {
                PeselSettings settings = CheckSettings();

                if (settings.Count <= 0)
                {
                    countPeselLabel.Visibility = Visibility.Visible;
                    countPeselLabel.Content = "Błąd! Wartość musi być większa od zera.";
                }
                else
                {
                    List<string> pese = pesel.Generate(settings);

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

        private PeselSettings CheckSettings()
        {
            string comboBoxTag = ((ComboBoxItem)GenderComboBox.SelectedItem).Tag.ToString();
            PeselSettings settings = new PeselSettings
            {
                UseDay = CheckIfEnableAndChecked(IfUseDayCheckBox),
                UseMonth = CheckIfEnableAndChecked(IfUseMonthCheckBox),
                UseYear = CheckIfEnableAndChecked(IfUseYearCheckBox),
                Gender = int.Parse(comboBoxTag),

                Count = int.Parse(countPeselTextBox.Text)
            };
            if (PeselDate.SelectedDate != null)
            {
                settings.Date = PeselDate.SelectedDate.Value.ToShortDateString();
            }

            return settings;
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
        public int Gender { get; set; }
        public int Count { get; set; }

    }
}
