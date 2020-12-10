using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentNumberGenerator.Pesel
{
    class PeselGenerator
    {
        public List<string> Generate(int count)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < count; i++)
            {
                PeselModel peselModel = SetRandomNumber();

                CodeYearInMonth(ref peselModel);
                DateFollowingZero(ref peselModel);

                StringBuilder randomPeselBuild = new StringBuilder();

                var randomPesel = randomPeselBuild.Append(peselModel.YearLastDigit).Append(peselModel.CodedMonth).Append(peselModel.Day).ToString();
                list.Add(randomPesel);
            }

            return list;
        }

        private void DateFollowingZero(ref PeselModel peselModel)
        {
            peselModel.CodedMonth = AddFollowingZeroWhenNotTwoDigit(peselModel.CodedMonth);
            peselModel.YearLastDigit = AddFollowingZeroWhenNotTwoDigit(peselModel.YearLastDigit);
            peselModel.Day = AddFollowingZeroWhenNotTwoDigit(peselModel.Day);
        }
        private void CodeYearInMonth(ref PeselModel peselModel)
        {
            int codedTemp;
            switch (peselModel.YearFirstDigit)
            {
                case "18":
                    codedTemp = int.Parse(peselModel.Month) + 80;
                    peselModel.CodedMonth = codedTemp.ToString();
                    break;
                case "19":
                    peselModel.CodedMonth = peselModel.Month.ToString();
                    break;
                case "20":
                    codedTemp = int.Parse(peselModel.Month) + 20;
                    peselModel.CodedMonth = codedTemp.ToString();
                    break;
            }
        }

        private string AddFollowingZeroWhenNotTwoDigit(string date)
        {
            if (date.Length != 2)
            {
                date = "0" + date;
            }
            return date;
        }

        private static PeselModel SetRandomNumber()
        {
            PeselModel peselModel = new PeselModel();
            Random random = new Random();
            peselModel.Year = random.Next(1800, 2100).ToString();
            peselModel.Month = random.Next(1, 13).ToString();
            int daysInMonth = DateTime.DaysInMonth(Int32.Parse(peselModel.Year), Int32.Parse(peselModel.Month));
            peselModel.Day = random.Next(1, daysInMonth + 1).ToString();
            return peselModel;
        }
    }
}
