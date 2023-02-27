using System;

namespace Task_1.Study.Pructic
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> menu = new Dictionary<int, string>
            {
                {1, "Generate a square matrix with dimension n"},
                {2, "Exit"}
            };
            while (true)
            {
                foreach (var item in menu)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
                int option = input_numeric_number("option");
                if (option == 1)
                {
                    int n = input_numeric_number("dimension");
                    int[,] matrix = generate_matrix(n);
                    for (int i = 0; i < n; ++i)
                    {
                        for (int j = 0; j < n; ++j)
                        {
                            Console.Write(matrix[i, j] + " ");
                        }
                        Console.WriteLine();
                    }
                }
                else if (option == 2) 
                {
                    Console.WriteLine("Good Bye");
                    break;
                }
                else { Console.WriteLine("Input a value 1 or 2!"); }
            }
           
            
        }


        static int input_numeric_number(string message="") 
        {
            int n;
            bool chech_input;
            do
            {
                Console.Write($"Enter a {message}: ");
                chech_input = int.TryParse(Console.ReadLine(), out n);

                if (chech_input && n > 0) break;
                else if (n < 0)
                {
                    Console.WriteLine($"{message} must be positive number!");
                }
                else
                {
                    Console.WriteLine("Invalid number!");
                }
            } while (!chech_input || n <= 0);

            return n;
        }

        static int[,] generate_matrix(int n)
        {
            int[,] return_matrix = new int[n, n];
            for (int i = 0; i < n; ++i)
            {
                for (int j = 0; j < n; ++j)
                {
                    return_matrix[i, j] = 0;
                }
            }

            int k = 0;
            int s = 0;
            for (int i = 0; i < n; ++i)
            {
                k += 1;
                s += 1;
                for (int j = 0; j < n - s + 1; ++j)
                {
                    return_matrix[i, j] = k;
                }
            }

            return return_matrix;
        }
    }

}
