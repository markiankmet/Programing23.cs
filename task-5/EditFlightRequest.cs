namespace Task_5_StudyPractik.Models
{
    public class EditFlightRequest
    {
        public bool _isValid = true;
        public string message = "";
        public string _departureCity;

        public string DepartureCity
        {
            get => _departureCity;
            set
            {
                _departureCity = Validation.CheckDepartureCity(value, ref _isValid, ref message);
            }
        }
        public string _arrivalCity;
        public string ArrivalCity
        {
            get => _arrivalCity;
            set
            {
                _arrivalCity = Validation.CheckArrivalCity(value, DepartureCity, ref _isValid, ref message);
            }
        }
        public string _arrivalCountry;
        public string ArrivalCountry
        {
            get => _arrivalCountry;
            set
            {
                _arrivalCountry = Validation.CheckArrivalCountry(value,
                    ref _isValid, ref message);
            }
        }
        public string _departureDateTime;
        public string DepartureDateTime
        {
            get => _departureDateTime;
            set
            {
                _departureDateTime = Validation.CheckDepartureDateTime(value,
                    ref _isValid, ref message);
            }
        }
        public string _arrivalDateTime;
        public string ArrivalDateTime
        {
            get => _arrivalDateTime;
            set
            {
                _arrivalDateTime = Validation.CheckArrivalDateTime(value, DepartureDateTime, ref _isValid, ref message);
            }
        }

        public string _airline;
        public string Airline
        {
            get => _airline;
            set
            {
                _airline = Validation.CheckAirLine
                    (value, ref _isValid, ref message);
            }
        }
        public double _price;
        public double Price
        {
            get => _price;
            set
            {
                _price = Validation.CheckPrice(value.ToString(), ref _isValid, ref message);
            }
        }
    }
}
