using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Task_5_StudyPractik.Data;


namespace Task_5_StudyPractik.Models
{
    static class FindCheapestFlights
    {
        public static List<List<Flight>> GetCheapestFlights(string DepartureCity, string ArrivalCity,string departureTime, List<Flight> listFilght)
        {
            Dictionary<string, HashSet<Flight>> _graph = new();

            foreach (var flight in listFilght)
            {
                if (!_graph.TryGetValue(flight.DepartureCity, out var departureFlights))
                {
                    departureFlights = new HashSet<Flight>();
                    _graph.Add(flight.DepartureCity, departureFlights);
                }
                departureFlights.Add(flight);
            }
            var distances = new Dictionary<string, double>();
            var prev = new Dictionary<string, Flight>();
            var flights = new List<Flight>();
            foreach (var city in _graph.Keys)
            {
                distances[city] = double.MaxValue;
            }

            DateTime depTime = DateTime.ParseExact(departureTime, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None);
            distances[DepartureCity] = 0;

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

            if (!prev.ContainsKey(ArrivalCity))
            {
                return null;
            }

            Flight curFlight = prev[ArrivalCity];
            while (curFlight != null)
            {
                flights.Insert(0, curFlight);
                curFlight = prev.ContainsKey(curFlight.DepartureCity) ? prev[curFlight.DepartureCity] : null;
            }



            List<List<Flight>> validFlightPaths = new List<List<Flight>> { flights };


            return validFlightPaths;

        }
    }
}
