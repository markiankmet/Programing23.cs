using System;

namespace Lab_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> menu = new Dictionary<int, string>
            {
                {1,  "Enter list by keyboard"},
                {2, "Random generate list in a certain range" },
                {3, "Do option 11" },
                {4, "Exit" }
            };

            while (true)
            {
                foreach (var item in menu)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
                int option = CheckFunctions.input_numeric_number("option");
                if (option == 1)
                {
                    int n = CheckFunctions.input_numeric_number("dimension");
                    LinkedList l = new LinkedList();
                    Console.WriteLine("Enter a list");
                    l.InputList(n);
                    l.PrintList();
                    question_delete_insert(l);
                }
                else if (option == 2)
                {
                    LinkedList l2 = new LinkedList();
                    l2.GenerateList();
                    l2.PrintList();
                    question_delete_insert(l2);
                }
                else if (option == 3)
                {
                    choose3();
                }
                else if (option == 4)
                {
                    Console.WriteLine("Good luck, Bye!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a number in range 1-4!");
                }
            }
            
        }

        static void question_delete_insert(LinkedList list_)
        {
            Dictionary<int, string> question_options = new Dictionary<int, string>
            {
                {1, "Delete on k position" },
                {2, "Insert in k position" },
                {3, "Exit" }
            };
            while (true)
            {
                foreach (var item in question_options)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
                int option = CheckFunctions.input_numeric_number("option");
                if (option == 1) 
                {
                    if (list_.GetLength() != 0)
                    {
                        list_.erase();
                        list_.PrintList();
                    }
                    else
                    {
                        Console.WriteLine("List is empty!");
                    }
                }
                else if (option == 2)
                {
                    list_.insert();
                    list_.PrintList();
                }
                else if (option == 3)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a number in range 1-3!");
                }
            }
        }

        static void choose3()
        {
            LinkedList x = new LinkedList();
            LinkedList y = new LinkedList();
            LinkedList z = new LinkedList();
            int n = CheckFunctions.input_numeric_number("dimension");
            Console.WriteLine("Input vectors x, y, z");
            x.InputList(n);
            x.PrintList();
            y.InputList(n);
            y.PrintList();
            z.InputList(n);
            z.PrintList();

            int max_value; int max_index;
            x.find_max(out max_value, out max_index);
            int idx_half = n / 2;
            int k = CheckFunctions.input_integer("k");
            if (max_value == k && y.is_negative() && max_index < idx_half)
            {
                x.made_kub(max_index);
            }
            x.PrintList();
        }
    }
}
