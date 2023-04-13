// ReSharper disable All

using System.Globalization;
using Newtonsoft.Json;

namespace Task_4_SP
{
    class Program
    {
        
        static void Main(string[] args)
        {
            Dictionary<int, string> menu = new Dictionary<int, string>
            {
                { 1, "View all flights" },
                { 2, "Add new flight" },
                { 3, "Delete flight" },
                { 4, "Edit flight" },
                { 5, "Find cheapest flight" },
                { 6, "Exit" }
            };
            Console.Write("Enter a file: ");
            string input_file = Console.ReadLine();
            string file_name = Validation.Check_File(input_file);
            GraphCollection graph = new GraphCollection("flights.json");

            while (true)
            {
                foreach (var item in menu)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
                int option = Validation.input_numeric_number("option");

                if (option == 1)
                {
                    Console.WriteLine(graph);
                }
                else if (option == 2)
                {
                    graph.AddItem();
                }
                else if (option == 3)
                {
                    graph.DeleteItem();
                }
                else if (option == 4)
                {
                    graph.EditItem();
                }
                else if (option == 5)
                {
                    Console.Write("Enter Departure City: ");
                    string depCity = Console.ReadLine();
                    Console.Write("Enter Arrival City: ");
                    string arrCity = Console.ReadLine();
                    Console.Write("Enter Departure Date: ");
                    string inputTime = Console.ReadLine();
                    DateTime depTime;
                    bool check_time = DateTime.TryParseExact(inputTime, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out depTime);
                    if (check_time && ListsOfEnums.listDepartureCity.Contains(depCity) &&
                        ListsOfEnums.listArrivalCity.Contains(arrCity))
                    {
                        var cheapestRoute = graph.GetCheapestFlights(depCity, arrCity, inputTime);
                        if (cheapestRoute.Count != 0)
                        {
                            Console.WriteLine("Cheapest flight:");
                            foreach (var item in cheapestRoute)
                            {
                                foreach (var item2 in item)
                                {

                                    Console.WriteLine(item2.ToString());

                                }

                                Console.WriteLine();
                            }
                        }
                        else
                        {
                            Console.WriteLine("No route found.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Invalid input date {depCity}, {arrCity}, {depTime}");
                    }
                    
                }
                else if (option == 6)
                {
                    Console.WriteLine("Good luck!");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid option. Please enter a number in range 1-6!");
                }
            }
            
        }
    }
}