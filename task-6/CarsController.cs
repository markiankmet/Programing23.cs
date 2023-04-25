using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Task_6_PR_2._0.Data;
using Task_6_PR_2._0.Model;

namespace Task_6_PR_2._0.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : Controller
    {
        private readonly CarAPIDbContext dbContext;

        public CarsController(CarAPIDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DhftOS5uphK3vmCJQrexST1RsyjZBjXWRgJMFPU4"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken("https://localhost:7250/",
              "https://localhost:7250/",
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPost]
        [Route("SignIn")]
        public IActionResult SignIn(SignInRequest signInRequest)
        {
            var userExist = dbContext.Users.FirstOrDefault(user => user.Email == signInRequest.Email
            && user.Password == signInRequest.Password);

            if (userExist == null)
            {
                return Unauthorized();
            }

            if (userExist != null)
            {
                var token = GenerateJwtToken(userExist);
                return Ok(token);
            }
            return NotFound("There are no user with this Email/Password");
        }


        [HttpPost]
        [Route("SignUp")]
        public async Task<IActionResult> SignUp(UserSignUpRequest userSignUpRequest)
        {
            if (userSignUpRequest._isValid)
            {
                var user = new User()
                {
                    Id = Guid.NewGuid(),
                    FirstName = userSignUpRequest.FirstName,
                    LastName = userSignUpRequest.LastName,
                    Email = userSignUpRequest.Email,
                    Password = userSignUpRequest.Password,
                    Role = "Customer"
                };
                await dbContext.AddAsync(user);
                await dbContext.SaveChangesAsync();
                return Ok(user);
            }
            else
            {
                return BadRequest($"\"status\": 400, \"Errormessage\":\n {userSignUpRequest.message} ");
            }
        }


        [Authorize(Roles = "Admin,Manager,Customer")]
        [HttpGet]
        public IActionResult GetCars(string sortField)
        {
            if (sortField != "default")
            {
                var setter = typeof(Car).GetProperties().Where(p => p.GetSetMethod()
                != null).ToList();

                var field = setter.FirstOrDefault(setter => setter.Name == sortField);
                Console.WriteLine(field);
                if (field != null)
                {
                    if (User.IsInRole("Customer"))
                    {
                        var sortedList = dbContext.Cars.ToList().OrderBy(x => x.GetType().
                            GetProperty(sortField).GetValue(x, null));
                        List<Car> cars = sortedList.Where(x => x.State == "Published").ToList();

                        var logRecord = new Logger
                        {
                            Id = Guid.NewGuid(),
                            User = $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value} " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value} - " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value}",
                            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                            Action = "Get All",
                            Result = "Was viewed all items"
                        };

                        dbContext.Loggers.Add(logRecord);
                        dbContext.SaveChanges();
                        return Json(sortedList);
                    }
                    else
                    {
                        List<Car> sortedList = dbContext.Cars.ToList().OrderBy(x => x.GetType().
                            GetProperty(sortField).GetValue(x, null)).ToList();
                        var logRecord = new Logger
                        {
                            Id = Guid.NewGuid(),
                            User = $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value} " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value} - " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value}",
                            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                            Action = "Get All",
                            Result = "Was viewed all items"
                        };

                        dbContext.Loggers.Add(logRecord);
                        dbContext.SaveChanges();
                        return Json(sortedList);
                    }
                    
                }
                else
                {
                    return BadRequest("Invalid sort field");
                }
            }
            else
            {
                var logRecord = new Logger
                {
                    Id = Guid.NewGuid(),
                    User = $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value} " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value} - " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value}",
                            Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    Action = "Get All",
                    Result = "Was viewed all items"
                };

                dbContext.Loggers.Add(logRecord);
                dbContext.SaveChanges();
                if (User.IsInRole("Customer"))
                {
                    
                    return Ok(dbContext.Cars.Where(x => x.State == "Published").ToList());
                }
                else
                {
                    return Ok(dbContext.Cars.ToList());
                }
            }
        }


        [Authorize(Roles = "Admin,Manager,Customer")]
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCar([FromRoute] Guid id)
        {
            var car = await dbContext.Cars.FindAsync(id);
            if (car != null)
            {
                var logRecord = new Logger
                {
                    Id = Guid.NewGuid(),
                    User = $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value} " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value} - " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value}",
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    Action = "Get By Id",
                    Result = $"Was viewed car {JsonConvert.SerializeObject(car)}"
                };
                dbContext.Loggers.Add(logRecord);
                dbContext.SaveChanges();
                return Ok(car);
            }
            return NotFound($"Car with this Id: {id} does not exist.");
        }


        [Authorize(Roles = "Admin,Manager,Customer")]
        [HttpGet]
        [Route("search/{searchParameter}")]
        public async Task<IActionResult> SearchData([FromRoute] string searchParameter)
        {
            var carSearchList = dbContext.Cars.ToList().Where(x => x.IsFound(searchParameter));
            if (carSearchList != null)
            {
                var logRecord = new Logger
                {
                    Id = Guid.NewGuid(),
                    User = $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value} " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value} - " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value}",
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    Action = "Search",
                    Result = $"Was found {JsonConvert.SerializeObject(JsonConvert.SerializeObject(carSearchList))}"
                };
                dbContext.Loggers.Add(logRecord);
                dbContext.SaveChanges();
                return Ok(carSearchList);
            }
            return NotFound("Nothing was found!");
        }


        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public async Task<IActionResult> AddCar(AddCarRequest addCarRequest)
        {
            if (addCarRequest._isValid)
            {
                var car = new Car()
                {
                    Id = Guid.NewGuid(),
                    Brand = addCarRequest.Brand,
                    Model = addCarRequest.Model,
                    RegistrationNumber = addCarRequest.RegistrationNumber,
                    LastRepairedAt = addCarRequest.LastRepairedAt,
                    BoughtAt = addCarRequest.BoughtAt,
                    CarMileage = addCarRequest.CarMileage,
                    State = StateEnum.Draft.ToString(),
                };
                var logRecord = new Logger
                {
                    Id = Guid.NewGuid(),
                    User = $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value} " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value} - " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value}",
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    Action = "Add New Car",
                    Result = $"Added new car --> {JsonConvert.SerializeObject(car)}"
                };
                dbContext.Loggers.Add(logRecord);
                car._state = car.ConvertState(car.State);
                await dbContext.Cars.AddAsync(car);
                await dbContext.SaveChangesAsync();
                return Ok(car);
            }
            else
            {
                return BadRequest($"\"status\": 400, \"Errormessage\":\n {addCarRequest.message} ");
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteCar([FromRoute] Guid id)
        {
            var car = await dbContext.Cars.FindAsync(id);
            if (car != null)
            {
                dbContext.Remove(car);
                var logRecord = new Logger
                {
                    Id = Guid.NewGuid(),
                    User = $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value} " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value} - " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value}",
                    Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                    Action = "Delete By Id",
                    Result = $"Was Deleted car --> {JsonConvert.SerializeObject(car)}"
                };
                dbContext.Loggers.Add(logRecord);
                await dbContext.SaveChangesAsync();
                return Ok(car);
            }
            return NotFound($"Car with this Id: {id} does not exist. Cannot delete not exist item!");
        }


        [Authorize(Roles = "Admin,Manager")]
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> EditCar([FromRoute] Guid id, EditCarRequest editCarRequest)
        {
            var car = await dbContext.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound($"Car with this Id: {id} does not exist. Cannot edit not exist item!");
            }
            if (editCarRequest._isValid)
            {
                if (car.State != "Moderation")
                {
                    var previousCar = car;
                    car.Brand = editCarRequest.Brand;
                    car.Model = editCarRequest.Model;
                    car.RegistrationNumber = editCarRequest.RegistrationNumber;
                    car.LastRepairedAt = editCarRequest.LastRepairedAt;
                    car.BoughtAt = editCarRequest.BoughtAt;
                    car.CarMileage = editCarRequest.CarMileage;
                    car.SetState(new DraftState());
                    var logRecord = new Logger
                    {
                        Id = Guid.NewGuid(),
                        User = $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value} " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value} - " +
                            $"{User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value}",
                        Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                        Action = "Edit",
                        Result = $"Old Car : {JsonConvert.SerializeObject(previousCar)} ---> Edited car : {JsonConvert.SerializeObject(car)}"
                    };
                    dbContext.Loggers.Add(logRecord);
                    await dbContext.SaveChangesAsync();
                    return Ok(car);
                }
                else
                {
                    return BadRequest("This car in State 'Moderation'");
                }
            }
            return BadRequest($"\"status\": 400, \n\"Errormessage\": \n{editCarRequest.message} ");
        }


        [Authorize(Roles = "Admin,Manager")]
        [HttpPut]
        [Route("Moderate/{id:guid}")]
        public IActionResult ModerateCar([FromRoute] Guid id)
        {
            var car = dbContext.Cars.Find(id);
            if (car != null)
            {
                if (car.State == "Draft")
                {
                    car.SetState(new ModerationState());
                    dbContext.SaveChanges();
                    return Ok(car);
                }
                else
                {
                    return BadRequest("Car state is not 'Draft'!");
                }
            }
            return NotFound($"Car with this Id: {id} does not exist!");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Approve/{id:guid}")]
        public IActionResult ApproveCar([FromRoute] Guid id)
        {
            var car = dbContext.Cars.Find(id);
            if (car != null)
            {
                if (car.State == "Moderation")
                {
                    car.SetState(new PublishedState());
                    dbContext.SaveChanges();
                    return Ok(car);
                }
                else
                {
                    return BadRequest("Car state is not 'Moderation'!");
                }
            }
            return NotFound($"Car with this Id: {id} does not exist!");
        }
    }
}
