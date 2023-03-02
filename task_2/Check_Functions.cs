using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Lab_2
{
    static class CheckFunctions
    {
        public static int input_integer(string message = "")
        {
            int return_value;
            bool chech_input;
            do
            {
                Console.Write($"Enter a number {message}: ");
                chech_input = int.TryParse(Console.ReadLine(), out return_value);

                if (chech_input) break;
                else
                {
                    Console.WriteLine("Invalid number!");
                }
            } while (!chech_input);

            return return_value;
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

        public static int check_index(int size)
        {
            int index_to_insert;
            while (true)
            {
                index_to_insert = CheckFunctions.input_numeric_number("position");
                if (index_to_insert >= size + 2)
                {
                    Console.WriteLine("Position out of range!");
                }
                else { return index_to_insert; }
            }
        }

    }
}
