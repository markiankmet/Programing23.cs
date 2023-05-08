// ReSharper disable All

using Newtonsoft.Json;

namespace ModulePR
{

    class Collection
    {
        public List<Booking> _collection;
        private string _fileName;

        public Collection(string fileName)
        {
            this._collection = new List<Booking>();
            this._fileName = fileName;
        }

        private void WriteInFile()
        {
            string json_data = JsonConvert.SerializeObject(_collection, Formatting.Indented);
            File.WriteAllText(_fileName, json_data);
        }

        public void ReadFromFile()
        {
            using (StreamReader file = new StreamReader(_fileName))
            {
                string readData = file.ReadToEnd();
                List<Booking> bookings = JsonConvert.DeserializeObject<List<Booking>>(readData);

                foreach (Booking item in bookings)
                {
                    if (item._isValid)
                    {
                        this._collection.Add(item);
                    }
                }
            }
        }

        public override string ToString()
        {
            string str = "";
            foreach (Booking item in _collection)
            {
                str += item + "\n";
            }
            return str;
        }

        public void CountPriceForBook(string name)
        {
            foreach (var VARIABLE in _collection)
            {
                if (VARIABLE.Name == name)
                {
                    Console.WriteLine(VARIABLE.CountPriceForBooking());
                }
            }
        }
        
        public void AddBooking(Booking newBooking)
        {
            bool isValidBooking = true;
            foreach (Booking booking in _collection)
            {
                if (booking.Name != newBooking.Name)
                    continue;
        
                DateTime startDate, endDate;
                if (DateTime.TryParse(booking.StartDate, out startDate) && 
                    DateTime.TryParse(booking.EndDate, out endDate) && 
                    DateTime.TryParse(newBooking.StartDate, out var newStartDate) &&
                    DateTime.TryParse(newBooking.EndDate, out var newEndDate))
                {
                    if (!((newStartDate < startDate && newEndDate < startDate) || 
                          (newStartDate > endDate && newEndDate > endDate)))
                    {
                        Console.WriteLine("Booking can't be added. There is already a booking for this period.");
                        isValidBooking = false;
                        break;
                    }
                }
            }

            if (isValidBooking)
            {
                foreach (Booking booking in _collection)
                {
                    if (booking.Name == newBooking.Name)
                    {
                        ++booking.bookCount;
                        if (booking.bookCount % 2 == 0)
                        {
                            booking.PricePerDay += 10.0;
                            break;
                        }
                    }
                }
                Console.WriteLine("Booking has been added.");
                _collection.Add(newBooking);
                WriteInFile();
            }
        }
        
    }
}