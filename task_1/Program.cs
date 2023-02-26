using System;

namespace Lab_1_Programing
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> menu = new Dictionary<int, string>
            {
                {1, "Enter vectors"},
                {2, "Generate vectors"},
                {3, "Exit"}
            };

            while (true)
            {
                foreach (var item in menu)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
                int option = input_numeric_number("option(1-3)");
                if (option == 1) 
                {
                    choose_1(option);
                }
                else if (option == 2)
                {
                    choose_1(option);
                }
                else if (option == 3)
                {
                    Console.WriteLine("Good Bye!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a number in range 1-3");
                }
            }

        }

        static int input_numeric_number(string message="")
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

            } while (!check_input || n < 0);
            

            return n;
        }

        static int input_interger(string message="")
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

        static int[] input_vector(int n, string message)
        {
            int[] result = new int[n];
            for (int i = 0; i < n; ++i)
            {
               result[i] = input_interger(message);
            }
            return result;
        }

        static void output_vector(int[] v, string message)
        {
            Console.WriteLine($"Vector {message}: ");
            foreach(int i in v)
            {
                Console.Write($"{i} ");
            }
            Console.WriteLine();
        }

        static void find_max_element(int[] v, out int index, out int max_value)
        {
            max_value = v.Max();
            index = -1;
            for (int i = 0; i < v.Length; ++i)
            {
                if(max_value == v[i])
                {
                    index = i;
                    break;
                }
            }
            
        }

        static int[] generate_vector(int n, int a, int b)
        {
            int[] v = new int[n];
            Random rnd = new Random();
            for (int i = 0; i < v.Length; ++i)
            {
                v[i] = rnd.Next(a, b);
            }
            return v;
        }

        static void choose_1(int option)
        {
            int n = input_numeric_number("dimension");
            if (option == 1)
            {
                int[] x = input_vector(n, "for vector x");
                int[] y = input_vector(n, "for vector y");
                int[] z = input_vector(n, "for vector z");

                int k = input_interger("k");

                int max_element_vector_x;
                int max_index_x;
                find_max_element(x, out max_index_x, out max_element_vector_x);

                int half_length = x.Length / 2;

                if (max_element_vector_x == k && y.All(i => i < 0) && max_index_x < half_length)
                {
                    for (int i = 0; i < max_index_x; i++)
                    {
                        x[i] = x[i] * x[i] * x[i];
                    }
                }

                output_vector(x, "x");
            }
            else if (option == 2)
            {
                while (true)
                {
                    int a = input_interger("a");
                    int b = input_interger("b");
                    if (a == b)
                    {
                        Console.WriteLine("a must not be equal to b!");
                    }
                    else if (a < b)
                    {
                        int[] x = generate_vector(n, a, b);
                        int[] y = generate_vector(n, a, b);
                        int[] z = generate_vector(n, a, b);
                        int k = input_interger("k");

                        int max_element_vector_x;
                        int max_index_x;
                        find_max_element(x, out max_index_x, out max_element_vector_x);

                        int half_length = x.Length / 2;

                        if (max_element_vector_x == k && y.All(i => i < 0) && max_index_x < half_length)
                        {
                            for (int i = 0; i < max_index_x; i++)
                            {
                                x[i] = x[i] * x[i] * x[i];
                            }
                        }

                        output_vector(x, "x");
                        output_vector(y, "y");
                        output_vector(z, "z");
                        break;
                    }
                    else if (a > b) { Console.WriteLine("b must be bigger that a!"); }
                }
            }                       
        }
    }
}