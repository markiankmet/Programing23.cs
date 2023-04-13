// ReSharper disable All
using Newtonsoft.Json;

namespace Task_5_Prog
{
    class Collection
    {
        public List<Car> collection;
        public string file_name;

        public Collection(string filename)
        {
            this.collection = new List<Car>();
            this.file_name = filename;
        }

        public override string ToString()
        {
            string text = "";
            foreach (Car item in collection)
            {
                text += item.ToString() + "\n";
            }
            return text;
        }

        public void ViewAll(bool checkRole)
        {
            if (checkRole)
            {
                Console.WriteLine(collection);
            }
            else
            {
                foreach (var VARIABLE in collection)
                {
                    if (VARIABLE.State == StateEnum.Published.ToString())
                    {
                        Console.WriteLine(VARIABLE);
                    }
                }
            }
        }

        public void Write_In_File()
        {
            string json_data = JsonConvert.SerializeObject(collection, Formatting.Indented);
            File.WriteAllText(file_name, json_data);
        }

        public Car Add_Car()
        {
            Car new_car = new Car();
            new_car.InputCar();
            if (new_car._isValid)
            {
                this.collection.Add(new_car);
                this.Write_In_File();
            }

            return new_car;
        }

        public void Read_From_File()
        {
            using (StreamReader file = new StreamReader(file_name))
            {
                string json_data = file.ReadToEnd();
                List<Car> cars = JsonConvert.DeserializeObject<List<Car>>(json_data);
                foreach (Car new_car in cars)
                {
                    if (new_car._isValid)
                    {
                        this.collection.Add(new_car);
                    }
                }
                file.Close();
            }
        }
        
        public void Delete_Car1(int id_delete)
        {
            Car DeleteCar = new Car();
            while (true)
            {
                
                bool existID = CheckID_exist(id_delete);
                if (existID)
                {
                    foreach (Car item in this.collection)
                    {
                        if (id_delete == item.ID)
                        {
                            DeleteCar = item;
                            this.collection.Remove(item);
                            break;
                        }
                    }
                    this.Write_In_File();
                    break;
                }
                else
                {
                    Console.WriteLine("There are no this ID! Try again!");
                    continue;
                }
            }
        }
        
        public Car Delete_Car()
        {
            Car DeleteCar = new Car();
            while (true)
            {
                Console.Write("Enter an ID of car you want to delete: ");
                int id_to_Delete = Validation.input_numeric_number("ID");
                bool existID = CheckID_exist(id_to_Delete);
                if (existID)
                {
                    foreach (Car item in this.collection)
                    {
                        if (id_to_Delete == item.ID)
                        {
                            DeleteCar = item;
                            this.collection.Remove(item);
                            break;
                        }
                    }
                    this.Write_In_File();
                    break;
                }
                else
                {
                    Console.WriteLine("There are no this ID! Try again!");
                    continue;
                }
            }
            return DeleteCar;
        }

        public void Search_data(string data)
        {
            bool contain = false;
            foreach(Car item in this.collection)
            {
                if (item.IsFound(data))
                {
                    Console.WriteLine(item);
                    contain = true;
                }
            }
            if (!contain)
            {
                Console.WriteLine($"Nothing was found for request: {data}");
            }
        }

        public bool CheckID_exist(int input_id)
        {
            foreach (Car item in this.collection)
            {
                if (input_id == item.ID)
                {
                    return true;
                }
            }
            return false;
        }

        public string EditCar()
        {
            string return_value = "";
            Console.Write("ID of element you want edit: ");
            string input_value = Console.ReadLine();
            bool id_exist;
            while (true)
            {
                bool is_valid = true;
                int edit_id = Validation.Check_ID(input_value, ref is_valid);
                id_exist = CheckID_exist(edit_id);
                if (id_exist)
                {
                    foreach (Car item in this.collection)
                    {
                        if (edit_id == item.ID)
                        {
                            
                            return_value += item.Edit_field();
                            if (item._isValid)
                            {
                                item.Edit();
                                this.Write_In_File();
                            }
                            else
                            {
                                return_value += "NOVALID";
                            }
                            this.collection.Clear();
                            this.Read_From_File();
                            break;
                        }                        
                    }
                    break;
                }
                else
                {
                    Console.WriteLine("This id does not exist! Try again!");
                    Console.Write("ID of element you want edit: ");
                    input_value = Console.ReadLine();
                }
            }
            return return_value;
        }

        public string Sort_Cars()
        {
            var setters = typeof(Car).GetProperties()
                .Where(p => p.GetSetMethod() != null)
                .ToList();
            string sort_by;
            while (true)
            {
                bool sorted = false;
                Console.Write("All fields: ");
                foreach (var item in setters)
                {
                    Console.Write(item.Name + " ");
                }
                Console.Write("Enter by what to sort: ");
                sort_by = Console.ReadLine();
                foreach (var setter in setters)
                {
                    if (sort_by == setter.Name)
                    {
                        List<Car> sortedList = collection.OrderBy(x => x.GetType().GetProperty(sort_by).GetValue(x, null)).ToList();
                        collection = sortedList;
                        this.Write_In_File();
                        sorted = true;
                        break;
                    }
                }
                if (sorted)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("There are not this field! Try again!");
                    continue;
                }
            }

            return sort_by;
        }

        public Car GetByID()
        {
            Car getCar = new Car();
            while (true)
            {
                Console.Write("Enter an ID of car you want to delete: ");
                int id_to_Get = Validation.input_numeric_number("ID");
                bool existID = CheckID_exist(id_to_Get);
                if (existID)
                {
                    foreach (Car item in this.collection)
                    {
                        if (id_to_Get == item.ID)
                        {
                            getCar = item;
                            break;
                        }
                    }

                    break;
                }
                else
                {
                    Console.WriteLine("There are no this ID! Try again!");
                    continue;
                }
            }

            return getCar;
        }
    }
}