using System.Net.Mail;

namespace Task7_Programing.Models
{
    static class Validation
    {
        const int LIMIT = 10000000;


        public static string Check_brand(string brand, ref bool isValid, ref string message)
        {
            if (string.IsNullOrWhiteSpace(brand))
            {
                message += "Brand cannot be empty!\n";
                isValid &= false;
            }
            else if (brand.All(c => c == '-'))
            {
                message += $"Invalid brand: {brand}!\n";
                isValid &= false;
            }
            else if (brand.All(c => (Char.IsLetter(c)) || (c == ' ') || (c == '-')))
            {
                return brand;
            }
            else
            {
                message += $"Invalid brand: {brand}. Must contain only letters!\n";
                isValid &= false;
            }
            return brand;
        }

        public static string Check_model(string model, ref bool isValid, ref string message)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                message += "Model cannot be empty!\n";
                isValid &= false;
            }
            if (model.All(c => (c == '-') || (Char.IsDigit(c))))
            {
                message += $"Invalid model: {model}!\n";
                isValid &= false;
            }
            if (model.All(c => (Char.IsLetter(c)) || (Char.IsDigit(c)) || (c == '-')))
            {
                return model;
            }
            else
            {
                message += $"Invalid model: {model}. Must contain only letters or digits!\n";
                isValid &= false;
            }
            return model;
        }

        public static string Check_registration_number(string registration_number, ref bool isValid, ref string message)
        {
            if (string.IsNullOrWhiteSpace(registration_number))
            {
                message += "Registration number cannot be empty!\n";
                isValid &= false;
            }
            if (registration_number.Length != 8)
            {
                message += "Registration number must be in format 'AA1111AA'!\n";
                isValid &= false;
            }
            else
            {
                Range r1 = 0..2;
                Range r2 = 2..6;
                Range r3 = 6..8;
                string reg_letters = registration_number[r1] + registration_number[r3];
                string reg_digits = registration_number[r2];
                if (reg_letters.All(c => Char.IsUpper(c)) && reg_digits.All(c => Char.IsDigit(c)))
                {
                    return registration_number;
                }
                else
                {
                    message += $"Invalid registration number {registration_number}!\n";
                    isValid &= false;
                }
            }
            return registration_number;
        }

        public static string Check_last_repaired(string last_repaired_at, ref bool isValid, ref string message)
        {
            DateTime return_value;
            bool check_input = DateTime.TryParse(last_repaired_at, out return_value);
            if (check_input)
            {
                return last_repaired_at;
            }
            else
            {
                message += $"Invalid date of last repaired at: {last_repaired_at}\n";
                isValid &= false;
            }
            return last_repaired_at;
        }

        public static string Check_bought_at(string bought_at, string last_repaired_at, ref bool isValid, ref string message)
        {
            DateTime return_value;
            DateTime input_last;
            bool check_last = DateTime.TryParse(last_repaired_at, out input_last);
            bool check_input = DateTime.TryParse(bought_at, out return_value);
            if (check_input && return_value < input_last)
            {
                return bought_at;
            }
            else if (check_input && return_value > input_last)
            {
                message += $"bought at date must be earlier than last repaired at date! Bought at: {bought_at}, Last repaired at: {last_repaired_at}\n";
                isValid &= false;
            }
            else
            {
                message += $"Invalid date of bought at: {bought_at}\n";
                isValid &= false;
            }
            return bought_at;
        }

        public static int Check_car_mileage(string car_mileage, ref bool isValid, ref string message)
        {
            int return_value;
            if (string.IsNullOrWhiteSpace(car_mileage))
            {
                message += $"Car mileage cannot be empty!\n";
                isValid &= false;
            }
            else
            {
                bool check_input = int.TryParse(car_mileage, out return_value);
                if (check_input && return_value > 0 && return_value < LIMIT)
                {
                    return return_value;
                }
                if (check_input && return_value > 0 && return_value > LIMIT)
                {
                    isValid &= false;
                    message += $"Car mileage exceeded a limit: {car_mileage}!\n";
                }
                else
                {
                    isValid &= false;
                    message += $"Invalid car mileage: {car_mileage}!\n";
                }
            }

            return 0;
        }

        public static string CheckName(string input_value, ref bool isValid, ref string message)
        {
            if (string.IsNullOrWhiteSpace(input_value))
            {
                message += "Name cannot be empty!\n";
                isValid &= false;
            }
            else if (input_value.All(c => Char.IsLetter(c)))
            {
                return input_value;
            }
            else
            {
                message += $"Invalid name: {input_value}. Must contain only letters!\n";
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckEmail(string input_value, ref bool isValid, ref string message)
        {
            try
            {
                var emailAddress = new MailAddress(input_value);
            }
            catch (Exception e)
            {
                message += $"Invalid email: {input_value}\n";
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckPassword(string input_value, ref bool isValid, ref string message)
        {
            if (input_value.Length < 8)
            {
                message += $"Invalid password: {input_value}!\n";
                isValid &= false;
            }
            else if (input_value.Any(c => Char.IsLetter(c)) && input_value.Any(c => Char.IsDigit(c))
                                                            && input_value.Any(c => Char.IsUpper(c)))
            {
                return input_value;
            }
            else
            {
                message += $"Invalid password: {input_value}\n";
                isValid &= false;
            }
            return input_value;
        }
    }
}
