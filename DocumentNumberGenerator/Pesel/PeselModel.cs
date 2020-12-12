using System;

namespace DocumentNumberGenerator.Pesel
{
    public class PeselModel
    {
        private string _lastYearDigit;
        private string _firstYearDigit;
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string CodedMonth { get; set; }
        public string SerialNumber { get; set; }
        public string ControlNumber { get; set; }
        public string Gender { get; set; }
        public string YearLastDigit { get => _lastYearDigit = SetLastYearDigit(); set => _lastYearDigit = value; }
        public string YearFirstDigit { get => _firstYearDigit = SetFirstYearDigit(); set => _firstYearDigit = value; }

        private string SetLastYearDigit()
        {
            try
            {
                string digit = Year.Substring(2, 2);
                return digit;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }


        private string SetFirstYearDigit()
        {
            try
            {
                string digit = Year.Substring(0, 2);
                return digit;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}