using Task_6_StudyPractik.Data;
using Task_6_StudyPractik.Models;
using Task_6_StudyPractik.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace TestProject1
{

    public class UnitTest1
    {
        private readonly DbContextOptions<FlightAPIDbContext> _options;

        public UnitTest1()
        {
            _options = new DbContextOptionsBuilder<FlightAPIDbContext>()
            .UseNpgsql("Server=localhost;Database=FlightsTesting;Port=5432;User Id=postgres;Password=MARIK2016")
            .Options;
        }

        [Fact]
        public void TestFindCheapestFlightMethod1()
        {
            using (var dbContext = new FlightAPIDbContext(_options))
            {
                List<List<Flight>> cheapestRoute = FindCheapestFlights.GetCheapestFlights("Lviv", "Krakiv", "2023-07-07", dbContext.Flights.ToList());
                double checkPrice = cheapestRoute.Sum(x => x.Sum(y => y.Price));
                Assert.Equal(101, checkPrice);

                List<Flight> checkFlight1 = cheapestRoute.FirstOrDefault(new List<Flight>());
                Flight checkFlight2 = checkFlight1.FirstOrDefault(new Flight());
                Assert.Equal("LOT", checkFlight2?.Airline);
            }
        }

        [Fact]
        public void TestFindCheapestFlightMethod2()
        {
            using (var dbContext = new FlightAPIDbContext(_options))
            {
                List<List<Flight>> cheapestRoute = FindCheapestFlights.GetCheapestFlights("Madrid", "Ternopil", "2023-07-19", dbContext.Flights.ToList());
                double checkPrice = cheapestRoute.Sum(x => x.Sum(y => y.Price));
                Assert.Equal(327, checkPrice);

                List<Flight> checkFlight1 = cheapestRoute.FirstOrDefault(new List<Flight>());
                Flight checkFlight2 = checkFlight1.FirstOrDefault(new Flight());
                Flight ckeckFlight3 = checkFlight1.Skip(1).FirstOrDefault(new Flight());
                Assert.Equal("Ryanair", checkFlight2?.Airline);
                Assert.Equal("Lviv", checkFlight2?.ArrivalCity);
                Assert.Equal("Ukraine", checkFlight2?.ArrivalCountry);

                Assert.Equal("Ryanair", ckeckFlight3?.Airline);
                Assert.Equal("Ternopil", ckeckFlight3?.ArrivalCity);
                Assert.Equal("Ukraine", ckeckFlight3?.ArrivalCountry);
                Assert.Equal(120, ckeckFlight3?.Price);

                Assert.True((DateTime.Parse(ckeckFlight3?.DepartureDateTime) - DateTime.Parse(checkFlight2?.ArrivalDateTime)).Hours > 1);
                Assert.True((DateTime.Parse(ckeckFlight3?.DepartureDateTime) - DateTime.Parse(checkFlight2?.ArrivalDateTime)).Hours < 7);


            }
        }

        [Fact]
        public void TestGetCheapestInMonthMethod()
        {
            using (var dbContext = new FlightAPIDbContext(_options))
            {

                DateTime inputDepTime;
                bool check_time = DateTime.TryParseExact("2023-08-01", "yyyy-MM-dd", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out inputDepTime);
                List<List<List<Flight>>> listFlight = new List<List<List<Flight>>>();
                int month = inputDepTime.Month;
                int year = inputDepTime.Year;
                int daysInMonth = DateTime.DaysInMonth(year, month);
                for (int day = 1; day <= daysInMonth; ++day)
                {
                    var cheapestRoute = FindCheapestFlights.GetCheapestFlights("Krakiv", "London", inputDepTime.ToString("yyyy-MM-dd"), dbContext.Flights.ToList());
                    if (cheapestRoute != null)
                    {
                        listFlight.Add(cheapestRoute);
                    }
                    inputDepTime = inputDepTime.AddDays(1);
                }
                int totalFlights = listFlight.Count;

                Assert.Equal(10, totalFlights);
                List<List<Flight>> Allflights = listFlight.FirstOrDefault(new List<List<Flight>>());
                List<Flight> Allflights2 = Allflights.FirstOrDefault(new List<Flight>());
                Flight checkFlight2 = Allflights2.FirstOrDefault(new Flight());
                Assert.Equal("2023-08-01 00:02", checkFlight2.DepartureDateTime);
                Assert.True(364 == checkFlight2.Price);
                Assert.Equal("WizzAir", checkFlight2.Airline);

                List<List<Flight>> Allflights2_2 = listFlight.Skip(1).FirstOrDefault(new List<List<Flight>>());
                List<Flight> Allflights2_2_2 = Allflights2_2.FirstOrDefault(new List<Flight>());
                Flight flight2_2_2 = Allflights2_2_2.FirstOrDefault(new Flight());
                Assert.True(250 == flight2_2_2.Price);
                Assert.Equal("2023-08-03 18:00", flight2_2_2.DepartureDateTime);
                Assert.Equal("WizzAir", flight2_2_2.Airline);

                List<List<Flight>> Allflights2_3 = listFlight.Skip(3).FirstOrDefault(new List<List<Flight>>());
                List<Flight> Allflights2_3_3 = Allflights2_3.FirstOrDefault(new List<Flight>());
                Flight flight2_3_1 = Allflights2_3_3.FirstOrDefault(new Flight());
                Flight flight2_3_2 = Allflights2_3_3.Skip(1).FirstOrDefault(new Flight());
                Assert.Equal(721, Allflights2_3_3.Sum(x => x.Price));
                Assert.Equal("Krakiv", flight2_3_1.DepartureCity);
                Assert.Equal("WizzAir", flight2_3_1.Airline);
                Assert.Equal("England", flight2_3_2.ArrivalCountry);

                Assert.True((DateTime.Parse(flight2_3_2?.DepartureDateTime) - DateTime.Parse(flight2_3_1?.ArrivalDateTime)).Hours > 1);
                Assert.True((DateTime.Parse(flight2_3_2?.DepartureDateTime) - DateTime.Parse(flight2_3_1?.ArrivalDateTime)).Hours < 7);

            }
        }

        [Fact]
        public void TestGetInCountryMethod()
        {
            using (var dbContext = new FlightAPIDbContext(_options))
            {
                List<Flight> flightsInCountry = new List<Flight>();

                foreach (var flight in dbContext.Flights.ToList())
                {
                    DateTime depTime = DateTime.ParseExact("2023-07-01", "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None);
                    if ("Madrid" == flight.DepartureCity && "Ukraine" == flight.ArrivalCountry && depTime.Day == DateTime.Parse(flight.DepartureDateTime).Day && depTime.Month == DateTime.Parse(flight.DepartureDateTime).Month)
                    {
                        flightsInCountry.Add(flight);
                    }
                }

                Flight checkFlight1 = flightsInCountry.FirstOrDefault(new Flight());
                Assert.Equal("Ryanair", checkFlight1?.Airline);

                Flight checkFlight2 = flightsInCountry.Skip(1).FirstOrDefault(new Flight());
                Assert.Equal("WizzAir", checkFlight2.Airline);
            }
        }
    }
}