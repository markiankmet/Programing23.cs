// ReSharper disable All

using Newtonsoft.Json;

namespace Task_4_Prog
{

    class LogIn
    {
        public Collection CarsCollection;
        public CollectionOfUsers UserCollection;
        public ConcreteServise Seriveses;
        public ISerivese Proxy;
        public ISerivese Logger;
        public User currentUser;

        public LogIn(string carFile, string userFile, string outFile)
        {
            CarsCollection = new(carFile);
            CarsCollection.Read_From_File();
            UserCollection = new(userFile);
            UserCollection.ReadFromFile();
            Seriveses = new(carFile);
            Proxy = new PermissionProxy(currentUser, Seriveses);
        }

        public void StartMenu()
        {
            Dictionary<int, string> startMenu = new()
            {
                { 1, "Log In" },
                { 2, "Sign Up" },
                { 3, "Exit" }
            };
            while (true)
            {
                foreach (var item in startMenu)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }

                int option = Validation.input_numeric_number("option");
                if (option == 1)
                {
                    currentUser = UserCollection.LogIn();
                    Proxy = new PermissionProxy(currentUser, Seriveses);
                    Logger = new LoggerProxy(Proxy, "Logger.json", currentUser, "cars.json");
                    AfterLogIn();
                }
                else if (option == 2)
                {
                    UserCollection.SignUp();
                    continue;
                }
                else if (option == 3)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a number in range 1-3!");
                }
            }
        }

        public void AfterLogIn()
        {
            Dictionary<int, string> menuLoggedIn = new Dictionary<int, string>
            {
                { 1, "Search information in collection" },
                { 2, "Sort elements" },
                { 3, "View all items in collection" },
                { 4, "View item by ID" },
                { 5, "Delete car from collection by id" },
                { 6, "Add new element in collection and file" },
                { 7, "Edit element in collection by id" },
                { 8, "Log Out" }
            };
            while (true)
            {
                foreach (var item in menuLoggedIn)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }

                int option = Validation.input_numeric_number("option");
                if (option == 1)
                {
                    Console.Write("Enter value of what you want to search: ");
                    string search_data = Console.ReadLine();
                    Logger.SearchData(search_data);
                }
                else if (option == 2)
                {
                    Logger.SortItems();
                }
                else if (option == 3)
                {
                    Logger.View();
                }
                else if (option == 4)
                {
                    Console.WriteLine(Logger.ViewById());
                }
                else if (option == 5)
                {
                    Logger.DeleteItem();
                }
                else if (option == 6)
                {
                    Logger.AddItem();
                }
                else if (option == 7)
                {
                    Logger.EditItem();
                }
                else if (option == 8)
                {
                    Console.WriteLine("---Logging out---");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a number in range 1-3!");
                }
            }
        }
    }
}