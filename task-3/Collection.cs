using System.Reflection;


namespace Task_3
{
    class Collection<T>
    {
        public List<T> collection;
        public string file_name;

        public Collection(string filename)
        {
            this.collection = new List<T>();
            this.file_name = filename;
        }

        public override string ToString()
        {
            string text = "";
            foreach (T item in collection)
            {
                text += item.ToString() + "\n";
            }
            return text;
        }

        public void Write_In_File()
        {
            using (StreamWriter file = new StreamWriter(file_name))
            {
                foreach (T item in collection)
                {
                    file.WriteLine(item.ToString());
                }
                file.Close();
            }
            
        }

        public void Add_Item(T new_item)
        {
            this.collection.Add(new_item);
            this.Write_In_File();
        }

        public void Read_From_File(Func<string[], T> createItem)
        {
            using (StreamReader file = new StreamReader(file_name))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] data = line.Split(" ");
                    T new_item = createItem(data);
                    this.collection.Add(new_item);
                }
                file.Close();
            }
        }

        public void Delete_Car(Predicate<T> predicate)
        {
            T itemToDelete = this.collection.Find(predicate);
            if (itemToDelete != null)
            {
                this.collection.Remove(itemToDelete);
                Console.WriteLine("Successfully deleted!");
                this.Write_In_File();
                return;
            }
            Console.WriteLine("There are not item with this ID!");
        }

        public List<T> Search_data(Func<T, bool> predicate)
        {
            List<T> result = this.collection.Where(predicate).ToList();
            return result;
        }

        public bool CheckID_exist(int input_id)
        {
            foreach (T item in this.collection)
            {
                PropertyInfo idProperty = item.GetType().GetProperty("ID");
                if (idProperty != null && (int)idProperty.GetValue(item) == input_id)
                {
                    return true;
                }
            }
            return false;
        }

        public void EditItem(Predicate<T> predicate)
        {
            T itemToEdit = this.collection.Find(predicate);
            if (itemToEdit != null)
            {
                PropertyInfo[] properties = itemToEdit.GetType().GetProperties(BindingFlags.Public |
                                                                               BindingFlags.Instance |
                                                                               BindingFlags.GetProperty |
                                                                               BindingFlags.SetProperty);
                Console.WriteLine("Enter new values for the following fields:");
                foreach (PropertyInfo property in properties)
                {
                    if (property.GetSetMethod() != null)
                    {
                        while (true)
                        {
                            Console.WriteLine(property.Name + ": ");
                            string newValue = Console.ReadLine();
                            try
                            {
                                property.SetValue(itemToEdit, Convert.ChangeType(newValue, property.PropertyType));
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Invalid input: {newValue}");
                            }
                        }
                    }
                }
                this.Write_In_File();
            }
        }

        public void Sort_Items(Func<T, object> keySelector)
        {
            List<T> sortedList = collection.OrderBy(keySelector).ToList();
            this.collection = sortedList;
            this.Write_In_File();
        }
    }
}
