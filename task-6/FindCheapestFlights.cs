using System.Globalization;

namespace Task_6_StudyPractik.Models
{
    public static class FindCheapestFlights
    {
        public static List<List<Flight>> GetCheapestFlights(string departureCity, string arrivalCity, string departureTime, List<Flight> flightList)
        {
            Dictionary<string, HashSet<Flight>> graph = new();

            // Створення графу
            foreach (var flight in flightList)
            {
                if (!graph.TryGetValue(flight.DepartureCity, out var departureFlights))
                {
                    departureFlights = new HashSet<Flight>();
                    graph.Add(flight.DepartureCity, departureFlights);
                }
                departureFlights.Add(flight);
            }

            var distances = new Dictionary<string, double>();
            var previousFlight = new Dictionary<string, Flight>();
            var flights = new List<Flight>();

            // Ініціалізація відстаней
            foreach (var city in graph.Keys)
            {
                distances[city] = double.MaxValue;
            }

            // Перетворення дати в об'єкт DateTime
            DateTime departureDateTime = DateTime.ParseExact(departureTime, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None);

            distances[departureCity] = 0;

            // Шукаємо найдешевший шлях
            for (int i = 0; i < graph.Keys.Count - 1; i++)
            {
                foreach (var city in graph.Keys)
                {
                    foreach (var flight in graph[city])
                    {
                        // Перевірка на час між перельотами
                        if (previousFlight.TryGetValue(city, out var previous))
                        {
                            DateTime previousArrivalDateTime = DateTime.Parse(previous.ArrivalDateTime);
                            DateTime currentDepartureDateTime = DateTime.Parse(flight.DepartureDateTime);
                            TimeSpan timeDifference = currentDepartureDateTime - previousArrivalDateTime;

                            if (timeDifference.TotalHours < 1 || timeDifference.TotalHours > 7)
                            {
                                continue;
                            }
                        }

                        var nextCity = flight.ArrivalCity;
                        var weight = flight.Price;
                        var nextDepartureDateTime = DateTime.Parse(flight.DepartureDateTime);

                        if (distances[city] != double.MaxValue && distances[city] + weight < distances[nextCity] && departureDateTime.Date == nextDepartureDateTime.Date)
                        {
                            distances[nextCity] = distances[city] + weight;
                            previousFlight[nextCity] = flight;
                        }
                    }
                }
            }

            // Побудова шляху
            if (!previousFlight.TryGetValue(arrivalCity, out var currentFlight))
            {
                return null;
            }

            while (currentFlight != null)
            {
                flights.Insert(0, currentFlight);
                currentFlight = previousFlight.TryGetValue(currentFlight.DepartureCity, out var previous) ? previous : null;
            }

            List<List<Flight>> validFlightPaths = new List<List<Flight>> { flights };

            return validFlightPaths;
        }

    }

    public class GenerateFlights
    {
        public Dictionary<string, string> CityCountry = new Dictionary<string, string>()
        {
            {"Lviv", "Ukraine" },
            {"Ternopil", "Ukraine" },
            {"Krakiv", "Poland"},
            {"London", "England" },
            {"Madrid", "Spain" }
        };

        public static List<string> listAirLine = new List<string>()
        {
            "WizzAir",
            "Ryanair",
            "LOT"
        };

        public List<Flight> GenerateFlightsCount(int count)
        {
            var flights = new List<Flight>();
            var rand = new Random();

            for (int i = 0; i < count; i++)
            {
                var departureCity = CityCountry.Keys.ElementAt(rand.Next(0, CityCountry.Count));
                var arrivalCity = CityCountry.Keys.ElementAt(rand.Next(0, CityCountry.Count));

                // Ensure that the departure and arrival cities are different
                while (arrivalCity == departureCity)
                {
                    arrivalCity = CityCountry.Keys.ElementAt(rand.Next(0, CityCountry.Count));
                }

                var departureDateTime = DateTime.Now.AddDays(rand.Next(1, 100)).AddHours(rand.Next(0, 24)).AddMinutes(rand.Next(0, 60));
                var arrivalDateTime = departureDateTime.AddHours(rand.Next(2, 7));

                while (arrivalDateTime.Day != departureDateTime.Day)
                {
                    departureDateTime = DateTime.Now.AddDays(rand.Next(1, 100)).AddHours(rand.Next(0, 24)).AddMinutes(rand.Next(0, 60));
                    arrivalDateTime = departureDateTime.AddHours(rand.Next(2, 7));
                }

                var airline = listAirLine[rand.Next(0, listAirLine.Count)];
                var price = rand.Next(100, 1000);

                var flight = new Flight
                {
                    Id = Guid.NewGuid(),
                    DepartureCity = departureCity,
                    ArrivalCity = arrivalCity,
                    ArrivalCountry = CityCountry[arrivalCity],
                    DepartureDateTime = departureDateTime.ToString("yyyy-MM-dd HH:mm"),
                    ArrivalDateTime = arrivalDateTime.ToString("yyyy-MM-dd HH:mm"),
                    Airline = airline,
                    Price = price
                };

                flights.Add(flight);
            }

            // Sort the flights by departure date/time
            flights.Sort((a, b) => DateTime.Compare(DateTime.Parse(a.DepartureDateTime), DateTime.Parse(b.DepartureDateTime)));

            return flights;
        }
    }
}
