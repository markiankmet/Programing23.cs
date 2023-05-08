namespace Task_6_PR_2._0.Model
{
    public class AddCarRequest
    {
        public bool _isValid = true;
        public string message = "";

        public string _brand;
        public string Brand
        {
            get => _brand;
            set
            {
                _brand = Validation.Check_brand(value, ref _isValid, ref message);
            }
        }

        public string _model;
        public string Model
        {
            get => _model;
            set
            {
                _model = Validation.Check_model(value, ref _isValid, ref message);
            }
        }

        public string _registrationNumber;
        public string RegistrationNumber
        {
            get => _registrationNumber;
            set
            {
                _registrationNumber = Validation.Check_registration_number(value, ref _isValid, ref message);
            }
        }

        public string _lastRepairedAt;
        public string LastRepairedAt
        {
            get => _lastRepairedAt;
            set
            {
                _lastRepairedAt = Validation.Check_last_repaired(value, ref _isValid, ref message);
            }
        }

        public string _boughtAt;
        public string BoughtAt
        {
            get => _boughtAt;
            set
            {
                _boughtAt = Validation.Check_bought_at(value, LastRepairedAt, ref _isValid, ref message);
            }
        }

        public int _carMileage;
        public int CarMileage
        {
            get => _carMileage;
            set
            {
                _carMileage = Validation.Check_car_mileage(value.ToString(), ref _isValid, ref message);
            }
        }
    }
}
