// ReSharper disable All
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Task_5_Prog
{
    public enum UserRole
    {
        Admin,
        Customer,
        Manager
    }
    class User
    {
        [JsonIgnore]
        public string firstName;
        [JsonIgnore]
        public string lastName;
        [JsonIgnore]
        public string email;
        [JsonIgnore]
        public UserRole role;
        [JsonIgnore]
        public string password;
        [JsonIgnore]
        public bool isValid = true;

        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = Validation.CheckName(value, ref isValid);
            }
        }

        public string LastName
        {
            get => lastName;
            set
            {
                lastName = Validation.CheckName(value, ref isValid);
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = Validation.CheckEmail(value, ref isValid);
            }
        }

        public string Password
        {
            get => password;
            set
            {
                password = Validation.CheckPassword(value, ref isValid);
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public UserRole Role
        {
            get => role;
            set => role = value;
        }

        public User()
        {
            FirstName = "firName";
            LastName = "lasName";
            Email = "mark@gmail.com";
            Password = "Password123";
            Role = UserRole.Customer;
        }

        public User(string firName, string lasName, string _email, string psw, UserRole _role = UserRole.Customer)
        {
            FirstName = firName;
            LastName = lasName;
            Email = _email;
            Password = psw;
            Role = _role;
        }

        public void InputUser()
        {
            var setters = typeof(User).GetProperties()
                .Where(p => p.GetSetMethod() != null)
                .ToList();

            foreach (var setter in setters)
            {
                while (true)
                {
                    if (setter.Name != "Role")
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
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}