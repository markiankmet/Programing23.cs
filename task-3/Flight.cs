// ReSharper disable All

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Task_4_SP
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
    }
    
    class Flight
    {
        [JsonIgnore]
        public int id;
        [JsonIgnore]
        public string deparCity;
        [JsonIgnore]
        public string arrivCity;
        [JsonIgnore]
        public string timeDeparture;
        [JsonIgnore]
        public string timeArrival;
        [JsonIgnore]
        public string airLine;
        [JsonIgnore]
        public double price;
        [JsonIgnore]
        public bool isValid = true;

        public int ID
        {
            get => id;
            set
            {
                id = Validation.Check_ID(value.ToString(), ref isValid);
            }
        }
        
        public string DepartureCity
        {
            get => deparCity;
            set
            {
                deparCity = Validation.CheckDepartureCity(value, ref isValid);
            }
        }
        
        public string ArrivalCity
        {
            get => arrivCity;
            set
            {
                arrivCity = Validation.CheckArrivalCity(value, this.DepartureCity, ref isValid);
            }
        }

        public string DepartureDateTime
        {
            get => timeDeparture;
            set
            {
                timeDeparture = Validation.CheckDepartureDateTime(value, ref isValid);
            }
        }

        public string ArrivalDateTime
        {
            get => timeArrival;
            set
            {
                timeArrival = Validation.CheckArrivalDateTime(value, this.DepartureDateTime, ref isValid);
            }
        }
        
        public string Airline
        {
            get => airLine;
            set
            {
                airLine = Validation.CheckAirLine(value, ref isValid);
            }
        }

        public double Price
        {
            get => price;
            set
            {
                price = Validation.CheckPrice(value.ToString(), ref isValid);
            }
        }

        public Flight()
        {
            ID = 1;
            DepartureCity = "Lviv";
            ArrivalCity = "Madrid";
            DepartureDateTime = "2023-03-04 14:00";
            ArrivalDateTime = "2023-03-04 17:00";
            Airline = "WizzAir";
            Price = 95;
        }

        public Flight(int _id, string ACity, string BCity, string Atime, string Btime, string _airline,
            double _price)
        {
            ID = _id;
            DepartureCity = ACity;
            ArrivalCity = BCity;
            DepartureDateTime = Atime;
            ArrivalDateTime = Btime;
            Airline = _airline;
            Price = _price;
        }

        public override string ToString()
        {
            string return_str = JsonConvert.SerializeObject(this, Formatting.Indented);
            return return_str;
        }

        public void InputFlight()
        {
            var setters = typeof(Flight).GetProperties()
                .Where(p => p.GetSetMethod() != null)
                .ToList();
            
            foreach (var setter in setters)
            {
                while (true)
                {
                    Console.Write($"Input {setter.Name}: ");
                    string input = Console.ReadLine();
                    try
                    {
                        var convertedInput = Convert.ChangeType(input, setter.PropertyType);
                        setter.SetValue(this, convertedInput);
                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Invalid input: {input}");
                    }
                }
            }
        }

        public void EditField()
        {
            var setters = typeof(Flight).GetProperties()
                .Where(p => p.GetSetMethod() != null)
                .ToList();
            Console.Write("All fields: ");
            foreach (var item in setters)
            {
                Console.Write(item.Name + " ");
            }
            Console.Write("\nInput what field you want to edit: ");
            string key = Console.ReadLine();
            int index_key = 0;
            while (true)
            {
                index_key = 0;
                bool check_field = false;
                foreach (var setter in setters)
                {
                    if (key == setter.Name)
                    {
                        check_field = true;
                        break;

                    }
                    ++index_key;
                }
                if (check_field)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("There aren't this field!");
                    Console.Write("Enter field again: ");
                    key = Console.ReadLine();
                }
            }
            object parameterType = setters[index_key].PropertyType;
            while (true)
            {
                Console.Write($"\nEnter value for {key}: ");
                string value = Console.ReadLine();
                try
                {
                    var convertedInput = Convert.ChangeType(value, setters[index_key].PropertyType);
                    if (key == "DepartureDateTime")
                    {
                        DateTime departureTime;
                        bool check_departure = DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out departureTime);
                        if (check_departure)
                        {
                            DateTime arrivalTime;
                            bool check_arrival = DateTime.TryParseExact(this.ArrivalDateTime, "yyyy-MM-dd HH:mm",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None, out arrivalTime);
                            if (check_arrival)
                            {
                                if (((arrivalTime - departureTime).TotalHours > TimeConstatns.minTimeFlights) &&
                                    ((arrivalTime - departureTime).TotalHours < TimeConstatns.maxTimeFlights))
                                {
                                    GetType().GetProperty(key).SetValue(this, convertedInput);
                                    this.isValid = true;
                                    break;
                                }
                                else
                                {
                                    isValid = false;
                                    Console.WriteLine(
                                        $"Departure Time must be more than {TimeConstatns.minTimeFlights} and less than" +
                                        $" {TimeConstatns.maxTimeFlights} hours : {departureTime} - {arrivalTime}");
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Invalid Arrival Date Time {value}");
                        }
                    }
                    else
                    {
                        GetType().GetProperty(key).SetValue(this, convertedInput);
                        break;
                    }
                }
                catch (Exception ex) { Console.WriteLine($"Invalid input: {value}"); }
            }
        }
    }
}