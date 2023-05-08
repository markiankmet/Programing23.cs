// ReSharper disable All

namespace ModulePR
{
    class Program
    {
        static void Main(string[] args)
        {

            string fileName = @"C:\Users\Markiyanchyk\RiderProjects\ModulePR\ModulePR\bookings.json";
            Collection bookingCollection = new Collection(fileName);
            bookingCollection.ReadFromFile();
            Console.WriteLine(bookingCollection);
            
            //Console.WriteLine("Testing task-4");
            //Console.Write("Input name of booking you want to count: ");
            //string input_value_name = Console.ReadLine();
            //bookingCollection.CountPriceForBook(input_value_name);
            
            Console.WriteLine("Testing task-2");
            Booking newBooking = new Booking("Animal Farm", 30.0, "2022-11-03", "2022-11-05");
            bookingCollection.AddBooking(newBooking);
            Console.WriteLine(bookingCollection);
            Booking newBooking2 = new Booking("Animal Farm", 30.0, "2022-12-03", "2022-12-05");
            bookingCollection.AddBooking(newBooking2);
            Console.WriteLine(bookingCollection);

            Console.WriteLine("Testing task-5");
            Booking newBooking3 = new Booking("Animal Farm", 30.0, "2022-05-03", "2022-05-05");
            bookingCollection.AddBooking(newBooking3);
            Console.WriteLine(bookingCollection);
            
            Booking newBooking4 = new Booking("Animal Farm", 30.0, "2022-01-03", "2022-01-05");
            bookingCollection.AddBooking(newBooking4);
            Console.WriteLine(bookingCollection);
        }
    }
}