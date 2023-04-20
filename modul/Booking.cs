// ReSharper disable All

using Newtonsoft.Json;

namespace ModulePR
{

    class Booking
    {
        private string _name;
        private double _pricePerDay;
        private string _startDate;
        private string _endDate;
        [JsonIgnore]
        public bool _isValid = true;
        [JsonIgnore] 
        public int bookCount = 1;

        public string Name
        {
            get => _name;
            set
            {
                _name = Validation.CheckName(value, ref _isValid);
            }
        }

        public double PricePerDay
        {
            get => _pricePerDay;
            set
            {
                _pricePerDay = Validation.CheckPrice(value, ref _isValid);
            }
        }

        public string StartDate
        {
            get => _startDate;
            set
            {
                _startDate = Validation.CheckStartDate(value, ref _isValid);
            }
        }

        public string EndDate
        {
            get => _endDate;
            set
            {
                _endDate = Validation.CheckStartDate(value, ref _isValid);
                if (_endDate.CompareTo(_startDate) > 0)
                {
                    
                }
                else
                {
                    Console.WriteLine($"Start date must be earlier than End date {_startDate}-{_endDate}");
                    _isValid &= false;
                }
            }
        }

        public Booking()
        {
            Name = "Animal Farm";
            PricePerDay = 40.60;
            StartDate = "2022-11-02";
            EndDate = "2022-11-06";
        }

        public Booking(string name, double pricePerDay, string startDate, string endDate)
        {
            Name = name;
            PricePerDay = pricePerDay;
            StartDate = startDate;
            EndDate = endDate;
        }

        public override string ToString()
        {
            return $"{Name} {PricePerDay} '{StartDate}'-'{EndDate}'";
        }

        public double CountPriceForBooking()
        {
            DateTime startDate;
            DateTime endDate;
            bool check_input = DateTime.TryParse(StartDate, out startDate);
            bool check_input2 = DateTime.TryParse(EndDate, out endDate);
            double coutPrice = 0.0;
            if (check_input && check_input2)
            {
                TimeSpan difference = endDate.Subtract(startDate);
                int days = difference.Days;
                Console.WriteLine("Number of days between start and end dates: " + days);
                coutPrice = PricePerDay * days;
            }

            return coutPrice;
        }
    }
}