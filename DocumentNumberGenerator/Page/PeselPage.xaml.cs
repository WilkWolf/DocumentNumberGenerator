using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ClosedXML.Excel;
using DocumentNumberGenerator.Pesel;
using DocumentNumberGenerator.Validators;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace DocumentNumberGenerator
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class PeselPage : Page
    {
        public PeselPage()
        {
            InitializeComponent();
        }
        private IValidators _validators = new Validators.Validators();
        private void OnlyNumberInTextBox(object sender, TextCompositionEventArgs e)
        {
            _validators.OnlyNumber(e, countPeselTextBox, countPeselLabel);
        }

        private void Generate(object sender, RoutedEventArgs e)
        {
            PeselGenerator pesel = new PeselGenerator();

            try
            {
                PeselSettingsModel settings = CheckSettings();

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

        private PeselSettingsModel CheckSettings()
        {
            string comboBoxTag = ((ComboBoxItem)GenderComboBox.SelectedItem).Tag.ToString();
            PeselSettingsModel settings = new PeselSettingsModel
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

        private void ButtonDownloadJson_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Txt files (*.txt)|*.txt|Json files (*.json)|*.json|All files (*.*)|*.*";

            if (saveFileDialog.ShowDialog() == true)
            {
                var json = JsonConvert.SerializeObject(peselListView.Items);
                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }

        private void ButtonDownloadTxt_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Txt files (*.txt)|*.txt|All files (*.*)|*.*";
            StringBuilder stringBuilder = new StringBuilder();
            if (saveFileDialog.ShowDialog() == true)
            {
                foreach (var s in peselListView.Items.SourceCollection)
                {
                    stringBuilder.Append(s + "\r\n");
                }

                File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
            }
        }

        private void ButtonDownloadXlsx_Click(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files|*.xlsx",
                Title = "Save an Excel File"
            };

            saveFileDialog.ShowDialog();

            if (!String.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                var workbook = new XLWorkbook();
                workbook.AddWorksheet("Pesel");
                var worksheet = workbook.Worksheet("Pesel");

                int row = 1;
                foreach (object item in peselListView.Items)
                {
                    worksheet.Cell("A" + row.ToString()).SetValue<string>(item.ToString());
                    row++;
                }

                workbook.SaveAs(saveFileDialog.FileName);
            }
        }
    }
}
