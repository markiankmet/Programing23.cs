// ReSharper disable All

using System.Globalization;
using Newtonsoft.Json;

namespace Task_4_SP
{
    static class TimeConstatns
    {
        public const int minTimeFlights = 1;
        public const int maxTimeFlights = 7;
    }
    
    class GraphCollection
    {
        public List<Flight> listOfFlights;
        public string fileName;
        public readonly Dictionary<string, HashSet<Flight>> _graph = new();
        

        public GraphCollection(string fileName)
        {
            this.listOfFlights = new List<Flight>();
            this.fileName = fileName;
            this.ReadFromFile();
            foreach (var VARIABLE in listOfFlights)
            {
                AddToGraph(VARIABLE);
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(listOfFlights, Formatting.Indented);
        }

        public void WriteInFile()
        {
            string json_data = JsonConvert.SerializeObject(listOfFlights, Formatting.Indented);
            File.WriteAllText(fileName, json_data);
        }

        public void ReadFromFile()
        {
            using (StreamReader file = new StreamReader(fileName))
            {
                string json_data = file.ReadToEnd();
                List<Flight> users = JsonConvert.DeserializeObject<List<Flight>>(json_data);
                foreach (Flight user in users)
                {
                    if (user.isValid)
                    {
                        this.listOfFlights.Add(user);
                    }
                }
            }
        }

        public void AddItem()
        {
            Flight new_flight = new Flight();
            new_flight.InputFlight();
            if (new_flight.isValid)
            {
                this.listOfFlights.Add(new_flight);
                this.WriteInFile();
                AddToGraph(new_flight);
            }
        }
        
        private void AddToGraph(Flight flight)
        {
            if (!_graph.TryGetValue(flight.DepartureCity, out var departureFlights))
            {
                departureFlights = new HashSet<Flight>();
                _graph.Add(flight.DepartureCity, departureFlights);
            }

            departureFlights.Add(flight);
        }
        
        public bool CheckID_exist(int input_id)
        {
            foreach (Flight item in this.listOfFlights)
            {
                if (input_id == item.ID)
                {
                    return true;
                }
            }
            return false;
        }
        
        public void DeleteItem()
        {
            while (true)
            {
                Console.Write("Enter an ID of car you want to delete: ");
                int id_to_Delete = Validation.input_numeric_number("ID");
                bool existID = CheckID_exist(id_to_Delete);
                if (existID)
                {
                    foreach (Flight item in this.listOfFlights)
                    {
                        if (id_to_Delete == item.ID)
                        {
                            this.listOfFlights.Remove(item);
                            if (_graph.TryGetValue(item.DepartureCity, out var departureFlights))
                            {
                                departureFlights.RemoveWhere(flight => flight.id == item.id);
                            }
                            break;
                        }
                    }
                    this.WriteInFile();
                    break;
                }
                else
                {
                    Console.WriteLine("There are no this ID! Try again!");
                    continue;
                }
            }
        }

        public void EditItem()
        {
            Console.Write("ID of element you want edit: ");
            string input_value = Console.ReadLine();
            bool id_exist;
            while (true)
            {
                bool is_valid = true;
                int edit_id = Validation.Check_ID(input_value, ref is_valid);
                id_exist = CheckID_exist(edit_id);
                if (id_exist)
                {
                    foreach (Flight item in this.listOfFlights)
                    {
                        if (edit_id == item.ID)
                        {
                            item.EditField();
                            if (item.isValid)
                            {
                                this.WriteInFile();
                            }
                            this.listOfFlights.Clear();
                            this.ReadFromFile();
                            break;
                        }                        
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("This id does not exist! Try again!");
                    Console.Write("ID of element you want edit: ");
                    input_value = Console.ReadLine();
                }
            }
        }
        

        public List<List<Flight>> GetCheapestFlights(string startCity, string endCity, string departureTime)
        {
            var distances = new Dictionary<string, double>();
            var prev = new Dictionary<string, Flight>();
            var flights = new List<Flight>();
            foreach (var city in _graph.Keys)
            {
                distances[city] = double.MaxValue;
            }

            DateTime depTime = DateTime.ParseExact(departureTime, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None);
            distances[startCity] = 0;

            for (int i = 0; i < _graph.Keys.Count - 1; i++)
            {
                foreach (var city in _graph.Keys)
                {
                    foreach (var flight in _graph[city])
                    {
                        var nextCity = flight.ArrivalCity;
                        var weight = flight.Price;
                        var nextDepartureTime = DateTime.Parse(flight.DepartureDateTime);
                        if (distances[city] != double.MaxValue && distances[city] + weight < distances[nextCity]
                                                               && depTime.Date == nextDepartureTime.Date)
                        {
                            distances[nextCity] = distances[city] + weight;
                            prev[nextCity] = flight;
                        }
                    }
                }
            }

            if (!prev.ContainsKey(endCity))
            {
                return null;
            }

            Flight curFlight = prev[endCity];
            while (curFlight != null)
            {
                flights.Insert(0, curFlight);
                curFlight = prev.ContainsKey(curFlight.DepartureCity) ? prev[curFlight.DepartureCity] : null;
            }

            var validFlightPaths = new List<List<Flight>> { flights };

            return validFlightPaths;
        }
        
        public void GetCheapestFlightsInMoths1(string startCity, string endCity, string departureTime)
        {
            DateTime depTime = DateTime.ParseExact(departureTime, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);
            int month = depTime.Month;
            int year = depTime.Year;
            int daysInMonth = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= daysInMonth; ++day)
            {
                var cheapestRoute = GetCheapestFlights(startCity, endCity, depTime.ToString("yyyy-MM-dd"));
                if (cheapestRoute != null)
                {
                    Console.WriteLine($"Cheapest flight for date {depTime.ToString("yyyy-MM-dd")}:");
                    foreach (var item in cheapestRoute)
                    {
                        foreach (var item2 in item)
                        {

                            Console.WriteLine(item2.ToString());

                        }

                        Console.WriteLine();
                    }
                }
                depTime = depTime.AddDays(1);
            }
        }

        
        public List<List<Flight>> GetCheapestFlightsInMonth(string startCity, string endCity, DateTime departureTime)
        {
            var distances = new Dictionary<string, double>();
            var prev = new Dictionary<string, Flight>();
            var flights = new List<Flight>();
            foreach (var city in _graph.Keys)
            {
                distances[city] = double.MaxValue;
            }
            
            distances[startCity] = 0;

            for (int i = 0; i < _graph.Keys.Count - 1; i++)
            {
                foreach (var city in _graph.Keys)
                {
                    foreach (var flight in _graph[city])
                    {
                        var nextCity = flight.ArrivalCity;
                        var weight = flight.Price;
                        var nextDepartureTime = DateTime.Parse(flight.DepartureDateTime);
                        if (distances[city] != double.MaxValue && distances[city] + weight < distances[nextCity]
                                                               && departureTime.Date == nextDepartureTime.Date)
                        {
                            distances[nextCity] = distances[city] + weight;
                            prev[nextCity] = flight;
                        }
                    }
                }
            }

            if (!prev.ContainsKey(endCity))
            {
                return null;
            }

            Flight curFlight = prev[endCity];
            while (curFlight != null)
            {
                flights.Insert(0, curFlight);
                curFlight = prev.ContainsKey(curFlight.DepartureCity) ? prev[curFlight.DepartureCity] : null;
            }

            var validFlightPaths = new List<List<Flight>> { flights };

            return validFlightPaths;
        }
        
        public List<List<Flight>> GetCheapestInMonth(string startCity, string endCity, DateTime depTime)
        {
            var distances = new Dictionary<string, double>();
            var prev = new Dictionary<string, Flight>();
            var flights = new List<Flight>();
            foreach (var city in _graph.Keys)
            {
                distances[city] = double.MaxValue;
            }
            
            distances[startCity] = 0;

            for (int i = 0; i < _graph.Keys.Count - 1; i++)
            {
                foreach (var city in _graph.Keys)
                {
                    foreach (var flight in _graph[city])
                    {
                        var nextCity = flight.ArrivalCity;
                        var weight = flight.Price;
                        var nextDepartureTime = DateTime.Parse(flight.DepartureDateTime);
                        if (distances[city] != double.MaxValue && distances[city] + weight < distances[nextCity]
                                                               && depTime.Date == nextDepartureTime.Date)
                        {
                            distances[nextCity] = distances[city] + weight;
                            prev[nextCity] = flight;
                        }
                    }
                }
            }

            if (!prev.ContainsKey(endCity))
            {
                return null;
            }

            Flight curFlight = prev[endCity];
            while (curFlight != null)
            {
                flights.Insert(0, curFlight);
                curFlight = prev.ContainsKey(curFlight.DepartureCity) ? prev[curFlight.DepartureCity] : null;
            }

            var validFlightPaths = new List<List<Flight>> { flights };

            return validFlightPaths;
        }

        public List<Flight> GetFlightsInCountry(string aCity, string BCountry, string departureTime)
        {
            List<Flight> flightsInCountry = new List<Flight>();
            
            foreach (var flight in listOfFlights)
            {
                DateTime depTime = DateTime.ParseExact(departureTime, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None);
                if ( aCity == flight.DepartureCity && BCountry == flight.ArrivalCountry && depTime.Day == DateTime.Parse(flight.DepartureDateTime).Day)
                {
                    flightsInCountry.Add(flight);
                }
            }
            return flightsInCountry;
        }
    }
    
    
   

}