using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Task_5_StudyPractik.Data;
using Task_5_StudyPractik.Models;

namespace Task_5_StudyPractik.Controllers
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
        public async Task<IActionResult> GetFlights()
        {
            return Ok(await dbContext.Flights.ToListAsync());
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
                return NotFound($"Flight with this Id: {id} does not exist. Cannot delete not exist item!");
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

        [HttpGet]
        [Route("GetCheapestFlights/{DepartureCity}/{ArrivalCity}/{departureTime}")]
        public IActionResult GetCheapestFlights([FromRoute] string DepartureCity, [FromRoute] string ArrivalCity, [FromRoute] string departureTime)
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
                    return Ok(cheapestRoute);
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

        [HttpGet]
        [Route("GetCheapestFlightsInMonth/{DepartureCityA}/{ArrivalCityB}/{departureMonth}")]
        public IActionResult GetCheapestFlightsInMonth([FromRoute] string DepartureCityA, [FromRoute] string ArrivalCityB, [FromRoute] string departureMonth)
        {
            DateTime inputDepTime;
            bool check_time = DateTime.TryParseExact(departureMonth, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out inputDepTime);
            
            if (check_time && ListsOfEnums.listDepartureCity.Contains(DepartureCityA) &&
                        ListsOfEnums.listArrivalCity.Contains(ArrivalCityB))
            {
                List<List<List<Flight>>> listFlight = new List<List<List<Flight>>>();
                int month = inputDepTime.Month;
                int year = inputDepTime.Year;
                int daysInMonth = DateTime.DaysInMonth(year, month);
                for (int day = 1; day <= daysInMonth; ++day)
                {
                    var cheapestRoute = FindCheapestFlights.GetCheapestFlights(DepartureCityA, ArrivalCityB, inputDepTime.ToString("yyyy-MM-dd"), dbContext.Flights.ToList());
                    if (cheapestRoute != null)
                    {
                        listFlight.Add(cheapestRoute);
                    }
                    inputDepTime = inputDepTime.AddDays(1);
                }
                return Ok(listFlight);

            }
            return BadRequest("Invalid Input Parameters!");
        }

        [HttpGet]
        [Route("GetCheapestFlightsInCountry/{DepartureCity3}/{ArrivalCountry}/{departureTime}")]
        public IActionResult GetCheapestFlightsInCountry([FromRoute] string DepartureCity3, [FromRoute] string ArrivalCountry, [FromRoute] string departureTime)
        {
            List<Flight> flightsInCountry = new List<Flight>();

            foreach (var flight in dbContext.Flights.ToList()) 
            {
                DateTime depTime = DateTime.ParseExact(departureTime, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                    DateTimeStyles.None);
                if (DepartureCity3 == flight.DepartureCity && ArrivalCountry == flight.ArrivalCountry && depTime.Day == DateTime.Parse(flight.DepartureDateTime).Day)
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


    }
}
