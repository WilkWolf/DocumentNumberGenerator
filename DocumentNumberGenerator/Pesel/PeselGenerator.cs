using System;
using System.Collections.Generic;
using System.Text;

namespace DocumentNumberGenerator.Pesel
{
    class PeselGenerator : IPeselGenerator
    {
        private enum GenderEnum
        {
            Different = 0,
            Woman = 1,
            Man = 2
        }

        public List<string> Generate(PeselSettingsModel settings)
        {
            List<string> list = new List<string>();

            for (int i = 0; i < settings.Count; i++)
            {
                PeselModel peselModel = SetRandomNumber(settings);
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
            int calculateControlNumber = 0;

            foreach (char number in pesel)
            {
                int digit = int.Parse(number.ToString());
                calculateControlNumber += digit * numberWeight[iteration];
                iteration++;
            }

            calculateControlNumber %= 10;
            int controlNumber = 10 - calculateControlNumber;

            return controlNumber == 10 ? "0" : controlNumber.ToString();
        }

        private static string BuildPeselString(PeselModel peselModel)
        {
            StringBuilder randomPeselBuild = new StringBuilder();

            string randomPesel = randomPeselBuild.Append(peselModel.YearLastDigit).Append(peselModel.CodedMonth)
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
            int codedMonth;
            switch (peselModel.YearFirstDigit)
            {
                case "18":
                    codedMonth = int.Parse(peselModel.Month) + 80;
                    peselModel.CodedMonth = codedMonth.ToString();
                    break;
                case "19":
                    peselModel.CodedMonth = peselModel.Month;
                    break;
                case "20":
                    codedMonth = int.Parse(peselModel.Month) + 20;
                    peselModel.CodedMonth = codedMonth.ToString();
                    break;
                default:
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

        private static PeselModel SetRandomNumber(PeselSettingsModel settings)
        {
            Random random = new Random();

            PeselModel peselModel = new PeselModel
            {
                Year = SetYearFromSettings(settings, random),
                Month = SetMonthFromSettings(settings, random),
                Gender = SetGenderFromSettings(settings, random),
                SerialNumber = random.Next(0, 1000).ToString()
            };
            peselModel.Day = SetDayFromSettings(settings, peselModel, random);

            return peselModel;
        }

        private static string SetDayFromSettings(PeselSettingsModel settings, PeselModel peselModel, Random random)
        {
            if (settings.UseDay)
            {
                return settings.Date.Substring(0, 2);
            }

            int daysInMonth = DateTime.DaysInMonth(int.Parse(peselModel.Year), int.Parse(peselModel.Month));

            return random.Next(1, daysInMonth + 1).ToString();
        }

        private static string SetGenderFromSettings(PeselSettingsModel settings, Random random)
        {
            string gender = "";
            int number;
            switch (settings.Gender)
            {
                case (int)GenderEnum.Different:
                    gender = random.Next(0, 9).ToString();
                    break;
                case (int)GenderEnum.Woman:
                    do
                    {
                        number = random.Next(0, 9); //0,2,4,6,8
                    } while (number % 2 != 0);

                    gender = number.ToString();
                    break;
                case (int)GenderEnum.Man:
                    do
                    {
                        number = random.Next(1, 10); //1,3,5,7,9
                    } while (number % 2 == 0);

                    gender = number.ToString();
                    break;
                default:
                    break;
            }

            return gender;
        }

        private static string SetMonthFromSettings(PeselSettingsModel settings, Random random)
        {
            return settings.UseMonth ? settings.Date.Substring(3, 2) : random.Next(1, 13).ToString();
        }

        private static string SetYearFromSettings(PeselSettingsModel settings, Random random)
        {
            return settings.UseYear ? settings.Date.Substring(6) : random.Next(1800, 2100).ToString();
        }
    }
}
