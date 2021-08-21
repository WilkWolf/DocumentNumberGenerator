using System.Windows.Controls;

namespace DocumentNumberGenerator.Common
{
    interface IDownload
    {
        void Excel(ListView listOfNumbers);
        void Json(ListView listOfNumbers);
        void Txt(ListView listOfNumbers);
    }
}