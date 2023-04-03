using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection.Metadata;
using System.Data.Common;


namespace Lab_2
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

        public void Write_In_File()
        {
            using (StreamWriter file = new StreamWriter(file_name))
            {
                foreach (Car car in collection)
                {
                    file.WriteLine(car.ToString());
                }
                file.Close();
            }
            
        }

        public void Add_Car()
        {
            Car new_car = new Car();
            new_car.InputCar();
            this.collection.Add(new_car);
            this.Write_In_File();
        }

        public void Read_From_File()
        {
            using (StreamReader file = new StreamReader(file_name))
            {
                string line;
                while ((line = file.ReadLine()) != null)
                {
                    string[] data = line.Split(" ");
                    Car new_car = new Car(data[0], data[1], data[2], data[3], data[4], data[5], data[6]);
                    
                    this.collection.Add(new_car);
                }
                file.Close();
            }
        }

        public void Delete_Car(int id)
        {
            foreach (Car item in this.collection)
            {
                if (id == item.ID_)
                {
                    this.collection.Remove(item);
                    break;
                }    
            }
            this.Write_In_File();
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
                if (input_id == item.ID_)
                {
                    return true;
                }
            }
            return false;
        }

        public void EditCar()
        {
            Console.Write("ID of element you want edit: ");
            string input_value = Console.ReadLine();
            bool id_exist;
            while (true)
            {

                int edit_id = Validation.Check_ID(input_value);
                id_exist = CheckID_exist(edit_id);
                if (id_exist)
                {
                    foreach (Car item in this.collection)
                    {
                        if (edit_id == item.ID_)
                        {
                            item.Edit_field();
                            this.Write_In_File();
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
            
        }

        public void Sort_Cars()
        {
            var setters = typeof(Car).GetProperties()
                .Where(p => p.GetSetMethod() != null)
                .ToList();

            while (true)
            {
                bool sorted = false;
                Console.Write("All fields: ");
                foreach (var item in setters)
                {
                    Console.Write(item.Name + " ");
                }
                Console.Write("Enter by what to sort: ");
                string sort_by = Console.ReadLine();
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
        }
    }
}
