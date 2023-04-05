// ReSharper disable All

using Newtonsoft.Json;

namespace Task_4_Prog
{
    class LoggerProxy : ISerivese
    {
        public ISerivese Servises;
        public string fileOut;
        public User user;
        public string nameOfFile;
        public Collection carCollection;

        public LoggerProxy(ISerivese ser, string file_out, User user1, string name_of_file)
        {
            Servises = ser;
            fileOut = file_out;
            user = user1;
            nameOfFile = name_of_file;
            carCollection = new Collection(name_of_file);
            carCollection.Read_From_File();
        }
        
        private void LogInFile(string action, string result)
        {
            var logRecord = new
            {
                User = $"{user.FirstName} {user.LastName} - {user.Role}",
                Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                Action = action,
                Result = result
            };
        
            var logRecordString = JsonConvert.SerializeObject(logRecord, Formatting.Indented);
        
            using (var writer = new StreamWriter(fileOut, append: true))
            {
                writer.WriteLine(logRecordString);
            }
        }
        
        public Car AddItem()
        {
            Car returnCar = new Car();
            if (user.Role == UserRole.Admin)
            {
                returnCar = Servises.AddItem();
                if (returnCar._isValid)
                {
                    LogInFile("Add", $"Add new item: {returnCar}");
                }
                else
                {
                    LogInFile("Add", $"Was trying to add invalid item");
                }
            }
            else
            {
                Console.WriteLine("Only Admin has permission to do this action!");
            }
            return returnCar;
        }

        public Car DeleteItem()
        {
            Car returnCar = new Car();
            if (user.Role == UserRole.Admin)
            {
                returnCar = Servises.DeleteItem();
                LogInFile("Delete", $"Deleted item: {returnCar}");
            }
            else
            {
                Console.WriteLine("Only Admin has permission to do this action!");
            }
            return returnCar;
        }

        public void SearchData(string data="")
        {
            Servises.SearchData(data);
            LogInFile("Search", $"Searched for request: {data}");
        }

        public string EditItem()
        {
            string returnString = "";
            if (user.Role == UserRole.Admin)
            {
                returnString = Servises.EditItem();
                if (!returnString.Contains("NOVALID"))
                {
                    LogInFile("Edit", $"Edited item: {returnString}");
                }
                else
                {
                    LogInFile("Edit", "Tried to edit field: with invalid data");
                }
            }
            else
            {
                Console.WriteLine("Only Admin has permission to do this action!");
            }
            return returnString;
        }

        public string SortItems()
        {
            string sortedField = Servises.SortItems();
            LogInFile("Sort", $"All items was sorted by {sortedField}");
            return sortedField;
        }

        public void View()
        {
            Servises.View();
            string allItems = $"Was viewed {carCollection.collection.Count} items";
            LogInFile("View", allItems);
        }

        public Car ViewById()
        {
            Car out_Car = Servises.ViewById();
            LogInFile("View By ID", $"Item was viewed: {out_Car}");
            return out_Car;
        }
    }
}
