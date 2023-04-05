// ReSharper disable All

using Newtonsoft.Json;

namespace Task_5_Prog
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            LogIn main = new("cars.json", "users.json", "Logger.json");
            main.StartMenu();
        }
    }
}