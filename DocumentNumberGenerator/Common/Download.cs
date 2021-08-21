using ClosedXML.Excel;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Controls;

namespace DocumentNumberGenerator.Common
{
   public class Download : IDownload
    {
        //private bool CheckIfListIsEmpty(ListView listOfNumbers)
        //{
        //    if (peselListView.Items.Count == 0)
        //    {
        //        countPeselLabel.Content = "Brak peseli do zapisania.";
        //        countPeselLabel.Visibility = Visibility.Visible;
        //        return true;
        //    }

        //    return false;
        //}

        public void Json(ListView listOfNumbers)
        {
            //if (CheckIfPeselListIsEmpty(listOfNumbers))
            //{
            //    return;
            //}

            //listOfNumbers.Content = "";
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Json files (*.json)|*.json|Txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                var json = JsonConvert.SerializeObject(listOfNumbers.Items);
                File.WriteAllText(saveFileDialog.FileName, json);
            }
        }

        public void Excel(ListView listOfNumbers)
        {
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
                foreach (object item in listOfNumbers.Items)
                {
                    worksheet.Cell("A" + row.ToString()).SetValue<string>(item.ToString());
                    row++;
                }

                workbook.SaveAs(saveFileDialog.FileName);
            }
        }

        public void Txt(ListView listOfNumbers)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Txt files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            StringBuilder stringBuilder = new StringBuilder();

            if (saveFileDialog.ShowDialog() == true)
            {
                foreach (object peselItem in listOfNumbers.Items.SourceCollection)
                {
                    stringBuilder.Append(peselItem + "\r\n");
                }

                File.WriteAllText(saveFileDialog.FileName, stringBuilder.ToString());
            }
        }
    }
}
