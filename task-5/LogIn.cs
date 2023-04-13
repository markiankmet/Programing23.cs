// ReSharper disable All

using Newtonsoft.Json;

namespace Task_5_Prog
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
                { 8, "Moderate a car" },
                { 9, "Approve some changes" },
                { 10, "Log Out" }
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
                    if (currentUser.role == UserRole.Customer)
                    {
                        CarsCollection.ViewAll(false);
                    }
                    else
                    {
                        CarsCollection.ViewAll(true);
                    }
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
                    if (currentUser.role == UserRole.Customer)
                    {
                        Console.WriteLine("You have no permission!");
                        continue;
                    }

                    Console.Write("Moderate object (1: yes, 2: no)");
                    int option1 = Validation.input_numeric_number("option");
                    while (true)
                    {
                        if (option1 == 1)
                        {
                            foreach (var VARIABLE in CarsCollection.collection)
                            {
                                if (VARIABLE.state == StateEnum.Draft.ToString())
                                {
                                    Console.WriteLine(VARIABLE);
                                }
                            }
                            Car new_car = CarsCollection.GetByID();
                            CarsCollection.Delete_Car1(new_car.ID);
                            new_car.Submit();
                            CarsCollection.collection.Add(new_car);
                            CarsCollection.Write_In_File();
                            break;
                        }

                        else if (option1 == 2) break;
                        else
                        {
                            Console.WriteLine("Invalid option. Please enter a number in range 1-2!");
                        }
                    }
                }
                else if (option == 9)
                {
                    if (currentUser.role != UserRole.Admin)
                    {
                        Console.WriteLine("You have no permission!");
                        continue;
                    }

                    Console.Write("Approve changes (1: yes, 2: no)");
                    int option1 = Validation.input_numeric_number("option");
                    while (true)
                    {
                        if (option1 == 1)
                        {
                            foreach (var VARIABLE in CarsCollection.collection)
                            {
                                if (VARIABLE.state == StateEnum.Moderation.ToString())
                                {
                                    Console.WriteLine(VARIABLE);
                                }
                            }

                            Car new_car = CarsCollection.GetByID();
                            new_car.Approve();
                            CarsCollection.Write_In_File();
                            break;
                        }

                        else if (option1 == 2) break;
                        else
                        {
                            Console.WriteLine("Invalid option. Please enter a number in range 1-2!");
                        }
                    }
                }
                
                else if (option == 10)
                {
                    Console.WriteLine("---Logging out---");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a number in range 1-10!");
                }
            }
        }
    }
}