using ClosedXML.Excel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace DocumentNumberGenerator
{
    /// <summary>
    /// Interaction logic for PersonelIdPage.xaml
    /// </summary>
    public partial class PersonelIdPage : Page
    {
        public PersonelIdPage()
        {
            InitializeComponent();
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
