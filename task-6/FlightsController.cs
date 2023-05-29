using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Task_6_StudyPractik.Data;
using Task_6_StudyPractik.Models;

namespace Task_6_StudyPractik.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlightsController : Controller
    {
        private readonly FlightAPIDbContext dbContext;

        public FlightsController(FlightAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetFlights(string? DepartureCity, string? ArrivalCity, string? departureTime, string? departureMonth, string? ArrivalCountry)
        {
            if (DepartureCity != null && ArrivalCity != null && departureTime != null)
            {
                DateTime depTime2;
                bool check_time = DateTime.TryParseExact(departureTime, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out depTime2);
                if (check_time && ListsOfEnums.listDepartureCity.Contains(DepartureCity) &&
                        ListsOfEnums.listArrivalCity.Contains(ArrivalCity))
                {
                    var cheapestRoute = FindCheapestFlights.GetCheapestFlights(DepartureCity, ArrivalCity, departureTime, dbContext.Flights.ToList());
                    if (cheapestRoute != null)
                    {
                        
                        double checkPrice = cheapestRoute.Sum(x => x.Sum(y => y.Price));
                        var returnValue1 = new
                        {
                            TotalCost = checkPrice,                            
                            CheapestFlights = cheapestRoute
                        };
                        return Ok(returnValue1);
                    }
                    else
                    {
                        return BadRequest("There are no this flight!");
                    }
                }
                else
                {
                    return BadRequest("Invalid Input Parameters!");
                }
            }
            else if (DepartureCity != null && ArrivalCity != null && departureMonth != null)
            {
                DateTime inputDepTime;
                bool check_time = DateTime.TryParseExact(departureMonth, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                            DateTimeStyles.None, out inputDepTime);
                if (check_time && ListsOfEnums.listDepartureCity.Contains(DepartureCity) &&
                        ListsOfEnums.listArrivalCity.Contains(ArrivalCity))
                {
                    List<List<List<Flight>>> listFlight = new List<List<List<Flight>>>();
                    int month = inputDepTime.Month;
                    int year = inputDepTime.Year;
                    int daysInMonth = DateTime.DaysInMonth(year, month);
                    for (int day = 1; day <= daysInMonth; ++day)
                    {
                        var cheapestRoute = FindCheapestFlights.GetCheapestFlights(DepartureCity, ArrivalCity, inputDepTime.ToString("yyyy-MM-dd"), dbContext.Flights.ToList());
                        if (cheapestRoute != null)
                        {
                            listFlight.Add(cheapestRoute);
                        }
                        inputDepTime = inputDepTime.AddDays(1);
                    }
                    var count2 = listFlight.Count;
                    var returnValue2 = new
                    {
                        FlightsNumber = count2,
                        CheapestFlights = listFlight
                    };
                    return Ok(returnValue2);

                }
                return BadRequest("Invalid Input Parameters!");
            }
            else if (DepartureCity != null && ArrivalCountry != null && departureTime != null)
            {
                List<Flight> flightsInCountry = new List<Flight>();

                foreach (var flight in dbContext.Flights.ToList())
                {
                    DateTime depTime = DateTime.ParseExact(departureTime, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None);
                    if (DepartureCity == flight.DepartureCity && ArrivalCountry == flight.ArrivalCountry && depTime.Day == DateTime.Parse(flight.DepartureDateTime).Day && depTime.Month == DateTime.Parse(flight.DepartureDateTime).Month)
                    {
                        flightsInCountry.Add(flight);
                    }
                }

                if (flightsInCountry != null)
                {
                    return Ok(flightsInCountry);
                }
                else
                {
                    return BadRequest("Invalid Input Parameters!");
                }
            }
            var flights = dbContext.Flights.ToList();
            var totalCount = flights.Count;
            var result = new
            {
                TotalCount = totalCount,
                Flights = flights
            };
            return Ok(result);

        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetFlight([FromRoute] Guid id)
        {
            var flight = await dbContext.Flights.FindAsync(id);
            if (flight != null)
            {
                return Ok(flight);
            }
            return NotFound($"Flight with this Id: {id} does not exist.");
        }

        [HttpPost]
        public async Task<IActionResult> AddFlight(AddFlightRequest addFlightRequest)
        {

            if (addFlightRequest._isValid)
            {
                var flight = new Flight()
                {
                    Id = Guid.NewGuid(),
                    DepartureCity = addFlightRequest.DepartureCity,
                    ArrivalCity = addFlightRequest.ArrivalCity,
                    ArrivalCountry = addFlightRequest.ArrivalCountry,
                    DepartureDateTime = addFlightRequest.DepartureDateTime,
                    ArrivalDateTime = addFlightRequest.ArrivalDateTime,
                    Airline = addFlightRequest.Airline,
                    Price = addFlightRequest.Price
                };
                await dbContext.Flights.AddAsync(flight);
                await dbContext.SaveChangesAsync();
                return Ok(flight);
            }
            else
            {
                return BadRequest($"\"status\": 400, \"Errormessage\":\n {addFlightRequest.message} ");
            }

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteFlight([FromRoute] Guid id)
        {
            var flight = await dbContext.Flights.FindAsync(id);
            if (flight != null)
            {
                dbContext.Remove(flight);
                await dbContext.SaveChangesAsync();
                return Ok(flight);
            }
            return NotFound($"Flight with this Id: {id} does not exist. Cannot delete not exist item!");
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> EditFlight([FromRoute] Guid id, EditFlightRequest editFlightRequest)
        {
            var flight = await dbContext.Flights.FindAsync(id);
            if (flight == null)
            {
                return NotFound($"Flight with this Id: {id} does not exist. Cannot edit not exist item!");
            }
            if (editFlightRequest._isValid)
            {
                flight.DepartureCity = editFlightRequest.DepartureCity;
                flight.ArrivalCity = editFlightRequest.ArrivalCity;
                flight.ArrivalCountry = editFlightRequest.ArrivalCountry;
                flight.DepartureDateTime = editFlightRequest.DepartureDateTime;
                flight.ArrivalDateTime = editFlightRequest.ArrivalDateTime;
                flight.Airline = editFlightRequest.Airline;
                flight.Price = editFlightRequest.Price;
                await dbContext.SaveChangesAsync();
                return Ok(flight);
            }
            return BadRequest($"\"status\": 400, \n\"Errormessage\": \n{editFlightRequest.message} ");
        }

        /*[HttpPost]
        [Route("generateFlights")]
        public async Task<IActionResult> GenerateFlights()
        {
            List<Flight> flightstest = new List<Flight>();
            GenerateFlights generateFlightsForDb = new GenerateFlights();
            flightstest = generateFlightsForDb.GenerateFlightsCount(1000);
            foreach (var flight in flightstest)
            {
                await dbContext.Flights.AddAsync(flight);
                await dbContext.SaveChangesAsync();
            }
            return Ok();
        }*/
    }
}
