namespace Task7_Programing.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class SignInRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserSignUpRequest
    {
        public bool _isValid = true;
        public string message = "";

        public string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = Validation.CheckName(value, ref _isValid, ref message);
            }
        }
        public string _lastName;
        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = Validation.CheckName(value, ref _isValid, ref message);
            }
        }
        public string _email;
        public string Email
        {
            get => _email;
            set
            {
                _email = Validation.CheckEmail(value, ref _isValid, ref message);
            }
        }
        public string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = Validation.CheckPassword(value, ref _isValid, ref message);
            }
        }
    }
}
