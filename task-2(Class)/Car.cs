using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Lab_2
{
    class Car
    {
        public int ID;
        public string brand;
        public string model;
        public string registration_number;
        public DateOnly last_repaired_at;
        public DateOnly bought_at;
        public int car_mileage;

        public int ID_
        { get { return ID; }
          set 
            {
                ID = Validation.Check_ID(value.ToString());
            } 
        }

        public string Brand
        {
            get { return brand; }
            set
            {
                brand = Validation.Check_brand(value);
            }
        }

        public string Model
        {
            get { return model; }
            set
            {
                model = Validation.Check_model(value);
            }
        }

        public string Registration_number
        {
            get { return registration_number; }

            set
            {
                registration_number = Validation.Check_registration_number(value);
            }
        }

        public DateOnly Last_repaired_at
        {
            get { return last_repaired_at; }
            set
            {
                last_repaired_at = Validation.Check_last_repaired(value.ToString());
            }
        }

        public DateOnly Bought_at
        {
            get { return bought_at; }
            set
            {
                bought_at = Validation.Check_bought_at(value.ToString(), last_repaired_at);
            }
        }
            
        public int Car_mileage
        {
            get { return car_mileage; }
            set
            {
                car_mileage = Validation.Check_car_mileage(value.ToString());
            }
        }

        public Car()
        {
            this.ID = 0;
            this.brand = "Audi";
            this.model = "R7";
            this.registration_number = "BC3213KI";
            this.last_repaired_at = new DateOnly(2018, 10, 10);
            this.bought_at = new DateOnly(2017, 11, 10);
            this.car_mileage = 0;
        }

        public Car(string iD, string brand, string model, string registration_number, string last_repaired_at, string bought_at, string car_mileage)
        {
            ID = Validation.Check_ID(iD);
            this.brand = Validation.Check_brand(brand);
            this.model = Validation.Check_model(model);
            this.registration_number = Validation.Check_registration_number(registration_number);
            this.last_repaired_at = Validation.Check_last_repaired(last_repaired_at);
            this.bought_at = Validation.Check_bought_at(bought_at, this.last_repaired_at);
            this.car_mileage = Validation.Check_car_mileage(car_mileage);
        }



        public override string ToString()
        {
            return $"{this.ID} {this.brand} {this.model} {this.registration_number} {this.last_repaired_at} {this.bought_at} {this.car_mileage}";
        }

        public void InputCar()
        {
            var setters = typeof(Car).GetProperties()
                .Where(p => p.GetSetMethod() != null)
                .ToList();

            foreach (var setter in setters)
            {
                while (true)
                {
                    Console.Write($"Input {setter.Name}: ");
                    string input = Console.ReadLine();

                    try
                    {
                        object parameterType = setter.PropertyType;
                        if (parameterType.ToString() == "System.DateOnly")
                        {                           
                            DateTime dt = DateTime.Parse(input);
                            var convertedInput = new DateOnly(dt.Year, dt.Month, dt.Day);
                            setter.SetValue(this, convertedInput);
                            break;
                        }
                        else 
                        {
                            var convertedInput = Convert.ChangeType(input, setter.PropertyType);
                            setter.SetValue(this, convertedInput);
                            break;
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Invalid input: {input}");
                    }
                }
            }
        }

        public bool IsFound(string search_object)
        {
            string[] allParameters = this.ToString().Split(' ');

            foreach (string parameter in allParameters)
            {
                if (parameter.Contains(search_object))
                {
                    return true;
                }
            }
            return false;
        }

        
        public void Edit_field()
        {
            var setters = typeof(Car).GetProperties()
                .Where(p => p.GetSetMethod() != null)
                .ToList();

            Console.Write("All fields: ");
            foreach (var item in setters)
            {
                Console.Write(item.Name + " ");
            }
            Console.Write("\nInput what field you want to edit: ");
            string key = Console.ReadLine();
            int index_key = 0;
            while (true)
            {
                index_key = 0;
                bool check_field = false;
                foreach (var setter in setters)
                {
                    if (key == setter.Name)
                    {
                        check_field = true;
                        break;

                    }
                    ++index_key;
                }
                if (check_field)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("There aren't this field!");
                    Console.Write("Enter field again: ");
                    key = Console.ReadLine();
                }
            }
            object parameterType = setters[index_key].PropertyType;
            while (true)
            {
                Console.Write($"\nEnter value for {key}: ");
                string value = Console.ReadLine();
                try
                {                    
                    if (parameterType.ToString() == "System.DateOnly")
                    {
                        DateTime dt = DateTime.Parse(value);
                        var convertedInput = new DateOnly(dt.Year, dt.Month, dt.Day);
                        setters[index_key].SetValue(this, convertedInput);
                        break;
                    }
                    else
                    {
                        GetType().GetProperty(key).SetValue(this, value);
                        break;
                    }

                }
                catch (Exception ex) { Console.WriteLine($"Invalid input: {ex.Message}"); }
            }
        }
    }
}
