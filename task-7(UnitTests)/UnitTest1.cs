using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Security.Claims;
using Task7_Programing.Controllers;
using Task7_Programing.Data;
using Task7_Programing.Models;

namespace CarApiTests
{
    
    public class UnitTestAuthorization
    {
        UsersController usersController;

        public UnitTestAuthorization()
        {
            // create a mock DbContext
            var optionsBuilder = new DbContextOptionsBuilder<UsersAPIDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            var dbContext = new UsersAPIDbContext(optionsBuilder.Options);

            // seed the database with a test user
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Viktor",
                LastName = "Kovalski",
                Email = "vitalapka@example.com",
                Password = "Password228",
                Role = "Customer"
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            // create an instance of UsersController with the mock DbContext
            usersController = new UsersController(dbContext);
        }

        [Fact]
        public void SignInTestExist()
        {
            // call the SignIn method with valid credentials
            var result = usersController.SignIn(new SignInRequest
            {
                Email = "vitalapka@example.com",
                Password = "Password228"
            });

            // assert that the result is an OkObjectResult with a valid JWT token
            var okResult = Assert.IsType<OkObjectResult>(result);
            var token = Assert.IsType<string>(okResult.Value);
            Assert.True(!string.IsNullOrEmpty(token));
        }

        [Fact]
        public void SignInTestNotExist() 
        {
            var result = usersController.SignIn(new SignInRequest
            {
                Email = "user@example.com",
                Password = "Ppassword2"
            });
            Assert.IsType<UnauthorizedResult>(result);
            
        }

        [Fact]
        public async Task SignUpTestValid()
        {
            var result = await usersController.SignUp(new UserSignUpRequest
            {

                FirstName = "John",
                LastName = "Doe",
                Email = "johndoe@gmail.com",
                Password = "Password1234"
            });
            var okResult = Assert.IsType<OkObjectResult>(result);
            var user = Assert.IsType<User>(okResult.Value);
            Assert.True(user.Id != Guid.Empty);
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal("johndoe@gmail.com", user.Email);
            Assert.Equal("Customer", user.Role);
        }

        [Fact]
        public async Task SignUpTestNotValid()
        {
            var result = await usersController.SignUp(new UserSignUpRequest
            {
                FirstName = "Joh1",
                LastName = "Doe2",
                Email = "johndoegmail.com",
                Password = "assword1234"
            });
            Assert.IsType<BadRequestObjectResult>(result);
        }
    }

    public class UnitTestAuthorizationAndAddNewItem
    {

        CarsController carsController;
        public UnitTestAuthorizationAndAddNewItem()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarAPIDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            var _dbContext = new CarAPIDbContext(optionsBuilder.Options);

            // seed the database with a test user
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Viktor",
                LastName = "Kovalski",
                Email = "vitalapka@example.com",
                Password = "Password228",
                Role = "Customer"
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Mark",
                LastName = "Kmet",
                Email = "markkmet@gmail.com",
                Password = "Marek2018",
                Role = "Admin"
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Petro",
                LastName = "Lopata",
                Email = "petrolopata@gmail.com",
                Password = "Petia2003",
                Role = "Manager"
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            // create an instance of UsersController with the mock DbContext
            carsController = new CarsController(_dbContext);
        }

        [Fact]
        public async Task AddNewItemFromManager()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarAPIDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            var dbContext = new CarAPIDbContext(optionsBuilder.Options);

            // seed the database with a test manager
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Petro",
                LastName = "Lopata",
                Email = "petrolopata@gmail.com",
                Password = "Petia2003",
                Role = "Manager"
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            // create a mock HTTP context with the manager user as the authenticated user
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role)
            }));

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var carsController = new CarsController(dbContext)
            {
                ControllerContext = controllerContext
            };

            var request = new AddCarRequest()
            {
                Brand = "Toyota",
                Model = "Camry",
                RegistrationNumber = "AB1233KI",
                LastRepairedAt = "2022-05-01",
                BoughtAt = "2021-10-21",
                CarMileage = 10000
            };

            var result = await carsController.AddCar(request);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsType<Car>(okResult.Value);
            var addedCar = okResult.Value as Car;

            var dbCar = await dbContext.Cars.FirstOrDefaultAsync(c => c.Id == addedCar.Id);
            Assert.NotNull(dbCar);
            Assert.Equal("Draft", addedCar?.State);

            var dbLogRecord = await dbContext.Loggers.FirstOrDefaultAsync(l => l.Result.Contains(JsonConvert.SerializeObject(addedCar)));
            Assert.NotNull(dbLogRecord);

            var moderationRes = carsController.ModerateCar(dbCar.Id);
            var moderationOk = Assert.IsType<OkObjectResult>(moderationRes);

            var moderatedCar = moderationOk.Value as Car;
            Assert.True(moderatedCar.State == "Moderation");

            // If car state is already Moderation --> cannot send to Moderation
            var moderationRes2 = carsController.ModerateCar(dbCar.Id);
            Assert.IsType<BadRequestObjectResult>(moderationRes2);
        }

        [Fact]
        public async Task CustomerCannotAddCar()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarAPIDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            var dbContext = new CarAPIDbContext(optionsBuilder.Options);
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Marko",
                LastName = "Zahura",
                Email = "markozahura@gmail.com",
                Password = "Marko1703",
                Role = "Customer"
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            // create a mock HTTP context with the customer user as the authenticated user
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role)
            }));

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var carsController = new CarsController(dbContext)
            {
                ControllerContext = controllerContext
            };

            var request = new AddCarRequest()
            {
                Brand = "BMW",
                Model = "M5",
                RegistrationNumber = "BC1000OK",
                LastRepairedAt = "2023-04-13",
                BoughtAt = "2020-09-23",
                CarMileage = 57688
            };

            // Act
            var result = await carsController.AddCar(request);

            // Assert
            Assert.IsType<ForbidResult>(result);
        }

        [Fact]
        public async Task ApproveCarFromAdminTest()
        {
            // Arrange
            var optionsBuilder = new DbContextOptionsBuilder<CarAPIDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            var dbContext = new CarAPIDbContext(optionsBuilder.Options);

            // seed the database with a test manager
            var user1 = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Petro",
                LastName = "Lopata",
                Email = "petrolopata@gmail.com",
                Password = "Petia2003",
                Role = "Manager"
            };
            dbContext.Users.Add(user1);
            dbContext.SaveChanges();

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Markiian",
                LastName = "Kmet",
                Email = "markkmet@gmail.com",
                Password = "Marek2018",
                Role = "Admin"
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            // create a mock HTTP context with the manager user as the authenticated user
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user1.FirstName),
                new Claim(ClaimTypes.Surname, user1.LastName),
                new Claim(ClaimTypes.Role, user1.Role)
            }));

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var carsController = new CarsController(dbContext)
            {
                ControllerContext = controllerContext
            };

            var request = new AddCarRequest()
            {
                Brand = "Toyota",
                Model = "Camry",
                RegistrationNumber = "AB1233KI",
                LastRepairedAt = "2022-05-01",
                BoughtAt = "2021-10-21",
                CarMileage = 10000
            };

            // Act
            var result = await carsController.AddCar(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsType<Car>(okResult.Value);
            var addedCar = okResult.Value as Car;

            // verify that the car was added to the database
            var dbCar = await dbContext.Cars.FirstOrDefaultAsync(c => c.Id == addedCar.Id);
            Assert.NotNull(dbCar);
            Assert.Equal("Draft", addedCar?.State);

            // verify that a log record was added
            var dbLogRecord = await dbContext.Loggers.FirstOrDefaultAsync(l => l.Result.Contains(JsonConvert.SerializeObject(addedCar)));
            Assert.NotNull(dbLogRecord);

            var moderationRes = carsController.ModerateCar(dbCar.Id);
            var moderationOk = Assert.IsType<OkObjectResult>(moderationRes);

            var moderatedCar = moderationOk.Value as Car;
            Assert.True(moderatedCar.State == "Moderation");

            // If car state is already Moderation --> cannot send to Moderation
            var moderationRes2 = carsController.ModerateCar(dbCar.Id);
            Assert.IsType<BadRequestObjectResult>(moderationRes2);

            // create a mock HTTP context with the manager user as the authenticated user
            var httpContext2 = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role)
            }));

            var controllerContext2 = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var carsController2 = new CarsController(dbContext)
            {
                ControllerContext = controllerContext
            };

            var approvenRes = carsController.ApproveCar(dbCar.Id);
            var approvenOk = Assert.IsType<OkObjectResult>(approvenRes);

            var approvedCar = approvenOk.Value as Car;
            Assert.True(approvedCar?.State == "Published");
        }

        [Fact]
        public async Task TryToEditCarInStateModeration()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarAPIDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            var dbContext = new CarAPIDbContext(optionsBuilder.Options);

            // seed the database with a test manager
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Petro",
                LastName = "Lopata",
                Email = "petrolopata@gmail.com",
                Password = "Petia2003",
                Role = "Manager"
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            // create a mock HTTP context with the manager user as the authenticated user
            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role)
            }));

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var carsController = new CarsController(dbContext)
            {
                ControllerContext = controllerContext
            };

            var request = new AddCarRequest()
            {
                Brand = "Toyota",
                Model = "Camry",
                RegistrationNumber = "AB1233KI",
                LastRepairedAt = "2022-05-01",
                BoughtAt = "2021-10-21",
                CarMileage = 10000
            };

            var result = await carsController.AddCar(request);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsType<Car>(okResult.Value);
            var addedCar = okResult.Value as Car;

            // verify that the car was added to the database
            var dbCar = await dbContext.Cars.FirstOrDefaultAsync(c => c.Id == addedCar.Id);
            Assert.NotNull(dbCar);
            Assert.Equal("Draft", addedCar?.State);

            // verify that a log record was added
            var dbLogRecord = await dbContext.Loggers.FirstOrDefaultAsync(l => l.Result.Contains(JsonConvert.SerializeObject(addedCar)));
            Assert.NotNull(dbLogRecord);

            var moderationRes = carsController.ModerateCar(dbCar.Id);
            var moderationOk = Assert.IsType<OkObjectResult>(moderationRes);

            var moderatedCar = moderationOk.Value as Car;
            Assert.True(moderatedCar.State == "Moderation");

            // If car state is already Moderation --> cannot send to Moderation
            var moderationRes2 = carsController.ModerateCar(dbCar.Id);
            Assert.IsType<BadRequestObjectResult>(moderationRes2);

            var editCar = new EditCarRequest()
            {
                Brand = "Audi",
                Model = "Rs7",
                RegistrationNumber = "AB1233KI",
                LastRepairedAt = "2022-05-01",
                BoughtAt = "2021-10-21",
                CarMileage = 10000
            };

            var tryToEditCar = await carsController.EditCar(addedCar.Id, editCar);
            Assert.IsType<BadRequestObjectResult>(tryToEditCar);

        }

        [Fact]
        public async Task DeleteCarTestMethod()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarAPIDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            var dbContext = new CarAPIDbContext(optionsBuilder.Options);
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Marko",
                LastName = "Zahura",
                Email = "markozahura@gmail.com",
                Password = "Marko1703",
                Role = "Admin"
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role)
            }));

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var carsController = new CarsController(dbContext)
            {
                ControllerContext = controllerContext
            };

            var request = new AddCarRequest()
            {
                Brand = "BMW",
                Model = "M5",
                RegistrationNumber = "BC1000OK",
                LastRepairedAt = "2023-04-13",
                BoughtAt = "2020-09-23",
                CarMileage = 57688
            };

            var result = await carsController.AddCar(request);

            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsType<Car>(okResult?.Value);
            var addedCar = okResult.Value as Car;
            Assert.Equal("M5", addedCar?.Model);
            Assert.True(addedCar?.State == "Draft");

            var deletResult = await carsController.DeleteCar(addedCar.Id);
            Assert.IsType<OkObjectResult>(deletResult);
            var successdelete = deletResult as OkObjectResult;
            Assert.IsType<Car>(successdelete?.Value);
            var deletedCar = successdelete.Value as Car;
            Assert.Equal("BC1000OK", deletedCar?.RegistrationNumber);

            var deleteAgain = await carsController.DeleteCar(addedCar.Id);
            Assert.IsType<NotFoundObjectResult>(deleteAgain);

        }

        [Fact]
        public async Task TryToEditWithNoValidDataTest()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CarAPIDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase");
            var dbContext = new CarAPIDbContext(optionsBuilder.Options);
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Marko",
                LastName = "Zahura",
                Email = "markozahura@gmail.com",
                Password = "Marko1703",
                Role = "Admin"
            };
            dbContext.Users.Add(user);
            dbContext.SaveChanges();

            var httpContext = new DefaultHttpContext();
            httpContext.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Surname, user.LastName),
                new Claim(ClaimTypes.Role, user.Role)
            }));

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext,
            };

            var carsController = new CarsController(dbContext)
            {
                ControllerContext = controllerContext
            };

            var request = new AddCarRequest()
            {
                Brand = "BMW",
                Model = "M5",
                RegistrationNumber = "BC1000OK",
                LastRepairedAt = "2023-04-13",
                BoughtAt = "2020-09-23",
                CarMileage = 57688
            };

            var result = await carsController.AddCar(request);
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.IsType<Car>(okResult.Value);
            var addedCar = okResult.Value as Car;

            var editCar = new EditCarRequest()
            {
                Brand = "1Audi",
                Model = "Rs7",
                RegistrationNumber = "A4B1233KI",
                LastRepairedAt = "202-05-32",
                BoughtAt = "20251-10-21",
                CarMileage = -10000
            };

            var tryToEditCar = await carsController.EditCar(addedCar.Id, editCar);
            Assert.IsType<BadRequestObjectResult>(tryToEditCar);

        }

    }
}