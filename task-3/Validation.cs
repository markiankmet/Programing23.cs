using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


namespace Task_3
{

    static class Validation
    {
        const int LIMIT = 10000000;
        public static int Check_ID(string input_value)
        {
            int ID;
            while (true)
            {                
                if (string.IsNullOrWhiteSpace(input_value))
                {
                    Console.WriteLine($"ID cannot be empty!");
                    Console.Write("Enter new ID: ");
                    input_value = Console.ReadLine();
                    continue;
                }
                bool check_input = int.TryParse(input_value, out ID);
                if (check_input && ID > 0)
                {
                    return ID;
                }
                else
                {
                    Console.WriteLine($"Invalid ID: {input_value}!");
                    Console.Write("Enter new ID: ");
                    input_value = Console.ReadLine();
                }
            }
        }

        public static string Check_brand(string brand)
        {
            while (true)
            {
                if (string.IsNullOrWhiteSpace(brand))
                {
                    Console.WriteLine("Brand cannot be empty!");
                    Console.Write("Enter new brand: ");
                    brand = Console.ReadLine();
                    continue;
                }
                if (brand.All(c => c == '-'))
                {
                    Console.WriteLine($"Invalid brand: {brand}!");
                    Console.Write("Enter new brand: ");
                    brand = Console.ReadLine();
                    continue;
                }
                if (brand.All(c => (Char.IsLetter(c)) || (c == ' ') || (c == '-')))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Invalid brand: {brand}. Must contain only letters!");
                    Console.Write("Enter new brand: ");
                    brand = Console.ReadLine();
                }
            }
            return brand;
        }

        public static string Check_model(string model)
        {
            while (true)
            {
                if (string.IsNullOrWhiteSpace(model))
                {
                    Console.WriteLine("Model cannot be empty!");
                    Console.Write("Enter new model: ");
                    model = Console.ReadLine();
                    continue;
                }
                if (model.All(c => (c == '-') || (Char.IsDigit(c))))
                {
                    Console.WriteLine($"Invalid model: {model}!");
                    Console.Write("Enter new model: ");
                    model = Console.ReadLine();
                    continue;
                }
                if (model.All(c => (Char.IsLetter(c)) || (Char.IsDigit(c)) || (c == '-')))
                {
                    break;
                }
                else
                {
                    Console.WriteLine($"Invalid model: {model}. Must contain only letters and digits!");
                    Console.Write("Enter new model: ");
                    model = Console.ReadLine();
                }
            }
            return model;
        }

        public static string Check_registration_number(string registration_number)
        {
            while (true) 
            {
                if (string.IsNullOrWhiteSpace(registration_number))
                {
                    Console.WriteLine("Registration number cannot be empty!");
                    Console.Write("Enter new registration number: ");
                    registration_number = Console.ReadLine();
                    continue;
                }
                if (registration_number.Length != 8)
                {
                    Console.WriteLine("Registration number must be in format 'AA1111AA'!");
                    Console.Write("Enter new registration number: ");
                    registration_number = Console.ReadLine();
                    continue;
                }
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
                    Console.Write("Enter new registration number: ");
                    registration_number = Console.ReadLine();
                }
            }
        }

        public static DateTime Check_last_repaired(string last_repaired_at)
        {
            DateTime return_value;
            while (true)
            {
                bool check_input = DateTime.TryParse(last_repaired_at, out return_value);
                if (check_input)
                {
                    return return_value;
                }
                else
                {
                    Console.WriteLine($"Invalid date of last repaired at: {last_repaired_at}");
                    Console.Write("Enter a new date: ");
                    last_repaired_at = Console.ReadLine();
                }
            }
        }

        public static DateTime Check_bought_at(string bought_at, DateTime last_repaired_at)
        {
            DateTime return_value;
            while (true)
            {
                bool check_input = DateTime.TryParse(bought_at, out return_value);
                if (check_input && return_value < last_repaired_at)
                {
                    return return_value;
                }
                else if (check_input && return_value > last_repaired_at)
                {
                    Console.WriteLine($"bought at date must be earlier than last repaired at date! Bought at: {bought_at}, Last repaired at: {last_repaired_at}");
                    Console.Write("Enter a new date: ");
                    bought_at = Console.ReadLine();
                }
                else
                {
                    Console.WriteLine($"Invalid date of bought at: {bought_at}");
                    Console.Write("Enter a new date: ");
                    bought_at = Console.ReadLine();
                }
            }
        }

        public static int Check_car_mileage(string car_mileage)
        {
            int return_value;
            while (true)
            {
                if (string.IsNullOrWhiteSpace(car_mileage))
                {
                    Console.WriteLine($"Car mileage cannot be empty!");
                    Console.Write("Enter new car mileage: ");
                    car_mileage = Console.ReadLine();
                    continue;
                }
                bool check_input = int.TryParse(car_mileage, out return_value);
                if (check_input && return_value > 0 && return_value < LIMIT)
                {
                    return return_value;
                }
                if (check_input && return_value > 0 && return_value > LIMIT)
                {
                    Console.WriteLine($"Car mileage exceeded a limit: {car_mileage}!");
                    Console.Write("Enter new car mileage: ");
                    car_mileage = Console.ReadLine();
                    continue;
                }
                else
                {
                    Console.WriteLine($"Invalid car mileage: {car_mileage}!");
                    Console.Write("Enter new car mileage: ");
                    car_mileage = Console.ReadLine();
                }
            }
        }

        public static string Check_File(string file)
        {
            while (true)
            {
                if (File.Exists(file) && Path.GetExtension(file).Equals(".txt", StringComparison.InvariantCultureIgnoreCase))
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
    }
}