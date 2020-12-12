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
                string randomPesel = BuildPeselString(peselModel);
                peselModel.ControlNumber = CreateControlNumber(randomPesel);
                randomPesel += peselModel.ControlNumber;
                list.Add(randomPesel);
            }

            return list;
        }

        private string CreateControlNumber(string pesel)
        {
            int[] numberWeight = new[] { 1, 3, 7, 9, 1, 3, 7, 9, 1, 3, 1 };
            int iteration = 0;
            int controlNumber = 0;
            int temp;
            foreach (var number in pesel)
            {
                var digit = int.Parse(number.ToString());
                controlNumber += digit * numberWeight[iteration];
                iteration++;
            }

            controlNumber = controlNumber % 10;
            temp = 10 - controlNumber;
            if (temp == 10)
            {
                return "0";
            }
            return temp.ToString();
        }

        private static string BuildPeselString(PeselModel peselModel)
        {
            StringBuilder randomPeselBuild = new StringBuilder();

            var randomPesel = randomPeselBuild.Append(peselModel.YearLastDigit).Append(peselModel.CodedMonth)
                .Append(peselModel.Day).Append(peselModel.SerialNumber).Append(peselModel.Gender).ToString();
            return randomPesel;
        }

        private void DateFollowingZero(ref PeselModel peselModel)
        {
            peselModel.CodedMonth = AddFollowingZeroWhenNotTwoDigit(peselModel.CodedMonth);
            peselModel.YearLastDigit = AddFollowingZeroWhenNotTwoDigit(peselModel.YearLastDigit);
            peselModel.Day = AddFollowingZeroWhenNotTwoDigit(peselModel.Day);
            peselModel.SerialNumber = AddFollowingZerosWhenNotFourDigits(peselModel.SerialNumber);
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
                    peselModel.CodedMonth = peselModel.Month;
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

        private string AddFollowingZerosWhenNotFourDigits(string date)
        {
            while (date.Length < 3)
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
            int daysInMonth = DateTime.DaysInMonth(int.Parse(peselModel.Year), int.Parse(peselModel.Month));
            peselModel.Day = random.Next(1, daysInMonth + 1).ToString();
            peselModel.Gender = random.Next(0, 9).ToString();
            peselModel.SerialNumber = random.Next(0, 1000).ToString();

            return peselModel;
        }
    }
}
