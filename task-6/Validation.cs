using System.Globalization;

namespace Task_6_StudyPractik.Models
{
    class ListsOfEnums
    {
        public static List<string> listDepartureCity = new List<string>()
        {
            "Lviv",
            "Ternopil",
            "Krakiv",
            "London",
            "Madrid"
        };
        public static List<string> listArrivalCity = new List<string>()
        {
            "Lviv",
            "Madrid",
            "London",
            "Krakiv",
            "Ternopil"
        };
        public static List<string> listAirLine = new List<string>()
        {
            "WizzAir",
            "Ryanair",
            "LOT"
        };

        public static List<string> listCountries = new List<string>()
        {
            "Ukraine",
            "Spain",
            "Poland",
            "England"
        };
    }

    static class TimeConstatns
    {
        public const int minTimeFlights = 1;
        public const int maxTimeFlights = 7;
    }

    static class Validation
    {
        public static int Check_ID(string input_value, ref bool isValid, ref string message)
        {
            int ID;
            if (string.IsNullOrWhiteSpace(input_value))
            {
                isValid &= false;
                message += $"ID cannot be empty!\n";
            }
            bool checkInput = int.TryParse(input_value, out ID);
            if (checkInput && ID > 0)
            {
                isValid &= true;
                return ID;
            }
            else
            {
                isValid &= false;
                Console.WriteLine($"Invalid ID: {input_value}!\n");
            }
            return 0;
        }

        public static string CheckDepartureCity(string input_value, ref bool isValid, ref string message)
        {
            if (ListsOfEnums.listDepartureCity.Contains(input_value))
            {
                return input_value;
            }
            else
            {
                message = $"There are not this Departure city {input_value}!\n";
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckArrivalCity(string input_value, string departureCity, ref bool isValid, ref string message)
        {
            if (ListsOfEnums.listArrivalCity.Contains(input_value))
            {
                if (input_value != departureCity)
                {
                    return input_value;

                }
                else
                {
                    message += $"Arrival and Departure cities cannot be the same {input_value} - {departureCity}\n";
                    isValid &= false;
                }
            }
            else
            {
                message += $"There are not this Arrival city {input_value}!\n";
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckArrivalCountry(string input_value, ref bool isValid, ref string message)
        {
            if (ListsOfEnums.listCountries.Contains(input_value))
            {
                return input_value;
            }
            else
            {
                message += $"There are not this Country {input_value}!\n";
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckDepartureDateTime(string input_value, ref bool isValid, ref string message)
        {
            DateTime time;
            bool check_input = DateTime.TryParseExact(input_value, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out time);
            if (check_input)
            {
                return input_value;
            }
            else
            {
                message += $"Invalid Departure Date Time {input_value}\n";
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckArrivalDateTime(string input_value, string departureDateTime, ref bool isValid, ref string message)
        {
            DateTime arrivalTime;
            bool check_arrival = DateTime.TryParseExact(input_value, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out arrivalTime);
            if (check_arrival)
            {
                DateTime departureTime;
                bool check_departure = DateTime.TryParseExact(departureDateTime, "yyyy-MM-dd HH:mm",
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out departureTime);
                if (check_departure)
                {
                    if (((arrivalTime - departureTime).TotalHours > TimeConstatns.minTimeFlights) &&
                        ((arrivalTime - departureTime).TotalHours < TimeConstatns.maxTimeFlights))
                    {
                        return input_value;
                    }
                    else
                    {
                        isValid &= false;
                        message +=
                            $"Departure Time must be more than {TimeConstatns.minTimeFlights} and less than" +
                            $" {TimeConstatns.maxTimeFlights} hours : {departureTime} - {arrivalTime}\n";
                    }
                }
                else
                {
                    isValid &= false;
                    return input_value;
                }
            }
            else
            {
                message += $"Invalid Arrival Date Time {input_value}\n";
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckAirLine(string input_value, ref bool isValid, ref string message)
        {
            if (ListsOfEnums.listAirLine.Contains(input_value))
            {
                return input_value;
            }
            else
            {
                message += $"There are not this AirLine {input_value}!\n";
                isValid &= false;
            }
            return input_value;
        }

        public static double CheckPrice(string input_value, ref bool isValid, ref string message)
        {
            double return_value;
            bool check_price = double.TryParse(input_value, out return_value);
            if (check_price)
            {
                if (return_value > 0)
                {
                    return return_value;
                }
                else
                {
                    message += $"Price cannot be negative {return_value}!\n";
                    isValid &= false;
                }
            }
            else
            {
                message += $"Invalid price: {input_value}\n";
                isValid &= false;
            }
            return return_value;
        }

        public static int input_numeric_number(string message = "")
        {
            int n;
            bool check_input;
            do
            {
                Console.Write($"Enter a {message}: ");
                check_input = int.TryParse(Console.ReadLine(), out n);

                if (check_input && n > 0) break;
                else if (n < 0)
                {
                    Console.WriteLine($"{message} must be positive number!\n");
                }
                else
                {
                    Console.WriteLine("Invalid number!");
                }

            } while (!check_input || n <= 0);


            return n;
        }
    }
}
