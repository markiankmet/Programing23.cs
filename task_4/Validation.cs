// ReSharper disable All
using System.Net.Mail;

namespace Task_4_Prog
{

    static class Validation
    {
        const int LIMIT = 10000000;
        
        public static int Check_ID(string input_value, ref bool isValid)
        {
            int ID;
            if (string.IsNullOrWhiteSpace(input_value))
            {
                isValid &= false;
                Console.WriteLine($"ID cannot be empty!");
            }
            bool checkInput = int.TryParse(input_value, out ID);
            if (checkInput && ID > 0)
            {
                isValid &= true;
                return ID;
            }
            else
            {
                 isValid &= false;
                 Console.WriteLine($"Invalid ID: {input_value}!");
            }
            return 0;
        }
        
        
        public static string Check_brand(string brand, ref bool isValid)
        {
            if (string.IsNullOrWhiteSpace(brand))
            {
                Console.WriteLine("Brand cannot be empty!");
                isValid &= false;
            }
            else if (brand.All(c => c == '-'))
            {
                Console.WriteLine($"Invalid brand: {brand}!");
                isValid &= false;
            }
            else if (brand.All(c => (Char.IsLetter(c)) || (c == ' ') || (c == '-')))
            {
                return brand;
            }
            else
            {
                Console.WriteLine($"Invalid brand: {brand}. Must contain only letters!");
                isValid &= false;
            }
            return brand;
        }

        public static string Check_model(string model, ref bool isValid)
        {
            if (string.IsNullOrWhiteSpace(model))
            {
                Console.WriteLine("Model cannot be empty!");
                isValid &= false;
            }
            if (model.All(c => (c == '-') || (Char.IsDigit(c))))
            {
                Console.WriteLine($"Invalid model: {model}!");
                isValid &= false;
            }
            if (model.All(c => (Char.IsLetter(c)) || (Char.IsDigit(c)) || (c == '-')))
            {
                return model;
            }
            else
            {
                Console.WriteLine($"Invalid model: {model}. Must contain only letters or digits!");
                isValid &= false;
            }
            return model;
        }

        public static string Check_registration_number(string registration_number, ref bool isValid)
        {
            if (string.IsNullOrWhiteSpace(registration_number))
            {
                Console.WriteLine("Registration number cannot be empty!");
                isValid &= false;
            }
            if (registration_number.Length != 8)
            {
                Console.WriteLine("Registration number must be in format 'AA1111AA'!");
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
                    Console.WriteLine($"Invalid registration number {registration_number}!");
                    isValid &= false;
                }
            }
            return registration_number;
        }

        public static string Check_last_repaired(string last_repaired_at, ref bool isValid)
        {
            DateTime return_value;
            bool check_input = DateTime.TryParse(last_repaired_at, out return_value);
            if (check_input)
            { 
                return last_repaired_at;
            }
            else
            {
                Console.WriteLine($"Invalid date of last repaired at: {last_repaired_at}");
                isValid &= false;
            }
            return last_repaired_at;
        }

        public static string Check_bought_at(string bought_at, string last_repaired_at, ref bool isValid)
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
                Console.WriteLine($"bought at date must be earlier than last repaired at date! Bought at: {bought_at}, Last repaired at: {last_repaired_at}");
                isValid &= false;
            }
            else
            {
                Console.WriteLine($"Invalid date of bought at: {bought_at}");
                isValid &= false;
            }
            return bought_at;
        }

        public static int Check_car_mileage(string car_mileage, ref bool isValid)
        {
            int return_value;
            if (string.IsNullOrWhiteSpace(car_mileage))
            {
                Console.WriteLine($"Car mileage cannot be empty!");
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
                    Console.WriteLine($"Car mileage exceeded a limit: {car_mileage}!");
                }
                else
                {
                    isValid &= false;
                    Console.WriteLine($"Invalid car mileage: {car_mileage}!");
                }
            }

            return 0;
        }

        public static string Check_File(string file)
        {
            while (true)
            {
                if (File.Exists(file) && Path.GetExtension(file).Equals(".json", StringComparison.InvariantCultureIgnoreCase))
                {
                    return file;
                }
                else
                {
                    Console.Write("File name is incorrect. Try again: ");
                    file = Console.ReadLine();
                }
            }
        }

        public static int input_numeric_number(string message = "")
        {
            int n;
            bool check_input;
            do
            {
                Console.Write($"Enter a {message}: ");
                check_input = int.TryParse(Console.ReadLine(), out n);

                if (check_input && n > 0) break;
                else if (n < 0)
                {
                    Console.WriteLine($"{message} must be positive number!");
                }
                else
                {
                    Console.WriteLine("Invalid number!");
                }

            } while (!check_input || n <= 0);


            return n;
        }

        public static string CheckName(string input_value, ref bool isValid)
        {
            if (string.IsNullOrWhiteSpace(input_value))
            {
                Console.WriteLine("Brand cannot be empty!");
                isValid &= false;
            }
            else if (input_value.All(c => Char.IsLetter(c)))
            {
                return input_value;
            }
            else
            {
                Console.WriteLine($"Invalid name: {input_value}. Must contain only letters!");
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckEmail(string input_value, ref bool isValid)
        {
            try
            {
                var emailAddress = new MailAddress(input_value);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Invalid email: {input_value}");
                isValid &= false;
            }
            return input_value;
        }

        public static string CheckPassword(string input_value, ref bool isValid)
        {
            if (input_value.Length < 8)
            {
                Console.WriteLine($"Invalid password: {input_value} 2");
                isValid &= false;
            }
            else if (input_value.Any(c => Char.IsLetter(c)) && input_value.Any(c => Char.IsDigit(c)) 
                                                            && input_value.Any(c => Char.IsUpper(c)))
            {
                return input_value;
            }
            else
            {
                Console.WriteLine($"Invalid password: {input_value}");
                isValid &= false;
            }
            return input_value;
        }
    }
}