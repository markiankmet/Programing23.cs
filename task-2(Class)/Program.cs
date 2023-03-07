using System;

namespace Lab_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> menu = new Dictionary<int, string>
            {
                {1,  "Search information in collection"},
                {2, "Sort elements" },
                {3, "Delete car from collection by id" },
                {4, "Add new element in collection and file" },
                {5, "Edit element in collection by id" },
                {6, "Print collection" },
                {7, "Exit" }
            };

            Console.Write("Enter a file: ");
            string input_file = Console.ReadLine();
            string file_name = Validation.Check_File(input_file);
            Collection car_collection = new Collection(file_name);
            car_collection.Read_From_File();

            while (true)
            {
                foreach (var item in menu)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
                int option = Validation.input_numeric_number("option");
                if (option == 1)
                {
                    Console.Write("Enter what to search: ");
                    string search_element = Console.ReadLine();
                    car_collection.Search_data(search_element);
                }
                else if (option == 2) 
                {
                    car_collection.Sort_Cars();
                }
                else if (option == 3)
                {
                    Console.Write("Enter an ID of car you want to delete: ");
                    string del_id = Console.ReadLine();
                    int id_to_delete = Validation.Check_ID(del_id);
                    car_collection.Delete_Car(id_to_delete);
                }
                else if (option == 4)
                {
                    Console.WriteLine("Add new Car");
                    car_collection.Add_Car();
                }
                else if (option == 5)
                {
                    car_collection.EditCar();
                }
                else if (option == 6)
                {
                    Console.WriteLine(car_collection);
                }
                else if (option == 7)
                {
                    Console.WriteLine("Good luck, Bye!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a number in range 1-7!");
                }

            }
            
        }
    }
}