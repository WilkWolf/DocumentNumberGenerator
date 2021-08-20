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

        private readonly IValidators _validators = new Validators.Validators();

        private void OnlyNumberInTextBox(object sender, TextCompositionEventArgs e)
        {
            _validators.OnlyNumber(e, countPeselTextBox, countPeselLabel);
        }

        private void Generate(object sender, RoutedEventArgs e)
        {
            IPeselGenerator pesel = new PeselGenerator();
            
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
                    List<string> peselList = pesel.Generate(settings);

                    int index = 0;
                    foreach (var item in peselList)
                    {
                        peselListView.Items.Insert(index, item);
                        index++;
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

        private void ClearPeselListWindow_Click(object sender, RoutedEventArgs e)
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
            return checkbox.IsEnabled && checkbox.IsChecked.GetValueOrDefault();
        }

        private void PeselDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
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

        private bool CheckIfPeselListIsEmpty()
        {
            if (peselListView.Items.Count == 0)
            {
                countPeselLabel.Content = "Brak peseli do zapisania.";
                countPeselLabel.Visibility = Visibility.Visible;
                return true;
            }

            return false;
        }

        private void DownloadJson_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfPeselListIsEmpty())
            {
                return;
            }

            countPeselLabel.Content = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Json files (*.json)|*.json|Txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var json = JsonConvert.SerializeObject(peselListView.Items);
                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }

        private void DownloadTxt_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfPeselListIsEmpty())
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            StringBuilder stringBuilder = new StringBuilder();

            if (saveFileDialog.ShowDialog() == true)
            {
                foreach (object peselItem in peselListView.Items.SourceCollection)
                {
                    stringBuilder.Append(peselItem + "\r\n");
                }

                File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
            }
        }

        private void DownloadXlsx_Click(object sender, RoutedEventArgs e)
        {
            if (CheckIfPeselListIsEmpty())
            {
                return;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel files|*.xlsx",
                Title = "Save an Excel File"
            };

            saveFileDialog.ShowDialog();

            if (!string.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                XLWorkbook workbook = new XLWorkbook();
                workbook.AddWorksheet("Pesel");

                IXLWorksheet worksheet = workbook.Worksheet("Pesel");

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
