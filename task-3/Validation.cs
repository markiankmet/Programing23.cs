// ReSharper disable All

using System.Globalization;

namespace Task_4_SP
{
    
    static class Validation
    {
        public static int Check_ID(string input_value, ref bool isValid)
        {
            int ID;
            if (string.IsNullOrWhiteSpace(input_value))
            {
                isValid &= false;
                Console.WriteLine($"ID cannot be empty!");
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
                Console.WriteLine($"Invalid ID: {input_value}!");
            }
            return 0;
        }
        
        public static string CheckDepartureCity(string input_value, ref bool isValid)
        {
            if (ListsOfEnums.listDepartureCity.Contains(input_value))
            {
                return input_value;
            }
            else
            {
                Console.WriteLine($"There are not this Departure city {input_value}!");
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckArrivalCity(string input_value, string departureCity, ref bool isValid)
        {
            if (ListsOfEnums.listArrivalCity.Contains(input_value))
            {
                if (input_value != departureCity)
                {
                    return input_value;

                }
                else
                {
                    Console.WriteLine($"Arrival and Departure cities cannot be the same {input_value} - {departureCity}");
                    isValid &= false;
                }
            }
            else
            {
                Console.WriteLine($"There are not this Arrival city {input_value}!");
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckDepartureDateTime(string input_value, ref bool isValid)
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
                Console.WriteLine($"Invalid Departure Date Time {input_value}");
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckArrivalDateTime(string input_value, string departureDateTime, ref bool isValid)
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
                    if ( ((arrivalTime - departureTime).TotalHours > TimeConstatns.minTimeFlights) &&
                        ( (arrivalTime - departureTime).TotalHours < TimeConstatns.maxTimeFlights) )
                    {
                        return input_value;
                    }
                    else
                    {
                        isValid &= false;
                        Console.WriteLine(
                            $"Departure Time must be more than {TimeConstatns.minTimeFlights} and less than" +
                            $" {TimeConstatns.maxTimeFlights} hours : {departureTime} - {arrivalTime}");
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
                Console.WriteLine($"Invalid Arrival Date Time {input_value}");
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckAirLine(string input_value, ref bool isValid)
        {
            if (ListsOfEnums.listAirLine.Contains(input_value))
            {
                return input_value;
            }
            else
            {
                Console.WriteLine($"There are not this AirLine {input_value}!");
                isValid &= false;
            }
            return input_value;
        }

        public static double CheckPrice(string input_value, ref bool isValid)
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
                    Console.WriteLine($"Price cannot be negative {return_value}!");
                    isValid &= false;
                }
            }
            else
            {
                Console.WriteLine($"Invalid price: {input_value}");
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
                    Console.WriteLine($"{message} must be positive number!");
                }
                else
                {
                    Console.WriteLine("Invalid number!");
                }

            } while (!check_input || n <= 0);


            return n;
        }
        
        public static string Check_File(string file)
        {
            while (true)
            {
                if (File.Exists(file) && Path.GetExtension(file).Equals(".json", StringComparison.InvariantCultureIgnoreCase))
                {
                    return file;
                }
                else
                {
                    Console.Write("File name is incorrect. Try again: ");
                    file = Console.ReadLine();
                }
            }
        }
    }
}