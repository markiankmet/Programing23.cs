using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task_3
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<int, string> menu = new Dictionary<int, string>
            {
                { 1, "Search information in collection" },
                { 2, "Sort elements" },
                { 3, "Delete car from collection by id" },
                { 4, "Add new element in collection and file" },
                { 5, "Edit element in collection by id" },
                { 6, "Print collection" },
                { 7, "Exit" }
            };

            Console.Write("Enter a file: ");
            string input_file = Console.ReadLine();
            string file_name = Validation.Check_File(input_file);

            Collection<Car> car_collection = new Collection<Car>(file_name);
            car_collection.Read_From_File(data => new Car(data));

            /*Collection<Passports> passport_collection = new Collection<Passports>(file_name);
            passport_collection.Read_From_File(data =>
                new Passports(int.Parse(data[0]), data[1], data[2], data[3], data[4], data[5], data[6]));*/

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
                    var searchResults = car_collection.Search_data(car => car.IsFound(search_element));
                    foreach (var VARIABLE in searchResults)
                    {
                        Console.WriteLine(VARIABLE);
                    }
                }
                else if (option == 2)
                {
                    
                    PropertyInfo sort_field;
                    PrintCarAttributes<Car>(out sort_field);
                    
                    car_collection.Sort_Items(car => sort_field.GetValue(car));
                }
                else if (option == 3)
                {
                    Console.Write("Enter an ID of car you want to delete: ");
                    string del_id = Console.ReadLine();
                    int id_to_delete = Validation.Check_ID(del_id);
                    car_collection.Delete_Car(car => car.ID == id_to_delete);
                }
                else if (option == 4)
                {
                    Console.WriteLine("Add new Car");
                    Car new_car = new Car();
                    new_car.InputCar();
                    car_collection.Add_Item(new_car);
                }
                else if (option == 5)
                {
                    Console.Write("Enter ID of item which want to edit: ");
                    string input_id = Console.ReadLine();
                    int id_edit = Validation.Check_ID(input_id);
                    bool exist_id = car_collection.CheckID_exist(id_edit);
                    if (exist_id)
                    {
                        Predicate<Car> predicate = item => item.ID == id_edit;
                        car_collection.EditItem(predicate);
                    }
                    else
                    {
                        Console.WriteLine("There are not item with this ID!");
                    }
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

            /*while (true)
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
                    var searchResults =
                        passport_collection.Search_data(passport => passport.FoundPassport(search_element));
                    foreach (var VARIABLE in searchResults)
                    {
                        Console.WriteLine(VARIABLE);
                    }
                }
                else if (option == 2)
                {

                    PropertyInfo sort_field;
                    PrintCarAttributes<Passports>(out sort_field);

                    passport_collection.Sort_Items(passport => sort_field.GetValue(passport));
                }
                else if (option == 3)
                {
                    Console.Write("Enter an ID of car you want to delete: ");
                    string del_id = Console.ReadLine();
                    int id_to_delete = Validation.Check_ID(del_id);
                    passport_collection.Delete_Car(passport => passport.ID == id_to_delete);
                }
                else if (option == 4)
                {
                    Console.WriteLine("Add new Passport");
                    Passports new_item = new Passports();
                    new_item.Add();
                    passport_collection.Add_Item(new_item);
                }
                else if (option == 5)
                {
                    Console.Write("Enter ID of item which want to edit: ");
                    string input_id = Console.ReadLine();
                    int id_edit = Validation.Check_ID(input_id);
                    bool exist_id = passport_collection.CheckID_exist(id_edit);
                    if (exist_id)
                    {
                        Predicate<Passports> predicate = item => item.ID == id_edit;
                        passport_collection.EditItem(predicate);
                    }
                    else
                    {
                        Console.WriteLine("There are not item with this ID!");
                    }
                }
                else if (option == 6)
                {
                    Console.WriteLine(passport_collection);
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
            }*/
        }

        
        static void PrintCarAttributes<T>(out PropertyInfo sort_field)
        {
            var setters = typeof(T).GetProperties()
                .Where(p => p.GetSetMethod() != null)
                .ToList();
            Console.Write("All fields => ");
            foreach (var setter in setters)
            {
                Console.Write($"{setter.Name} ");
            }
            Console.Write("Enter field by which will be sorting: ");
            string input_field = Console.ReadLine();
                
            while (true)
            {
                foreach (var setter in setters)
                {
                    if (input_field == setter.Name)
                    {
                        sort_field = setter;
                        return;
                    }
                }
                Console.WriteLine("There are not this field!\nTry again:");
                input_field = Console.ReadLine();
            }
        }
    }
}