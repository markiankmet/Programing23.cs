// ReSharper disable All
namespace ModulePR
{

    static class Validation
    {
        private const double MAX_PRICE = 20000.00;
        private const string MIN_DATE = "2019-10-23";
        public static string CheckName(string input_value, ref bool isValid)
        {
            if (string.IsNullOrWhiteSpace(input_value))
            {
                Console.WriteLine("Name cannot be empty!");
                isValid &= false;
            }
            else if (input_value.All(c => Char.IsLetter(c) || Char.IsSeparator(c)))
            {
                return input_value;
            }
            else
            {
                Console.WriteLine($"Invalid Name: {input_value}. Must contain only letters!");
                isValid &= false;
            }
            return input_value;
        }

        public static double CheckPrice(double input_value, ref bool isValid)
        {
            if (input_value <= 0)
            {
                Console.WriteLine($"Price cannot be less or equal than zero: {input_value}");
                isValid &= false;
            }
            else if (input_value < MAX_PRICE)
            {
                return input_value;
            }
            else
            {
                Console.WriteLine($"Price shoud be less than MAX_PRICE({MAX_PRICE}) -> {input_value}");
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckStartDate(string input_value, ref bool isValid)
        {
            DateTime startDate;
            bool check_input = DateTime.TryParse(input_value, out startDate);
            if (check_input)
            {
                if (input_value.CompareTo(MIN_DATE) > 0)
                {
                    return input_value;
                }
                else
                {
                    Console.WriteLine($"MIN_DATE({MIN_DATE}) must be less than Start Date: {input_value}");
                    isValid &= false;
                }
            }
            else
            {
                Console.WriteLine($"Invalid Start Date: {input_value}");
                isValid &= false;
            }
            return input_value;
        }
        
    }
}
