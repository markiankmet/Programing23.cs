using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace Task_3
{
    class Car
    {
        public int id;
        public string brand;
        public string model;
        public string registration_number;
        public DateTime last_repaired_at;
        public DateTime bought_at;
        public int car_mileage;

        public int ID
        { 
            get { return id; }
            set { id = Validation.Check_ID(value.ToString()); } 
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

        public DateTime Last_repaired_at
        {
            get { return last_repaired_at; }
            set
            {
                last_repaired_at = Validation.Check_last_repaired(value.ToString());
            }
        }

        public DateTime Bought_at
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
            this.last_repaired_at = new DateTime(2018, 10, 10);
            this.bought_at = new DateTime(2017, 11, 10);
            this.car_mileage = 0;
        }

        public Car(params string[] par)
        {
            ID = Validation.Check_ID(par[0]);
            Brand = par[1];
            Model = par[2];
            Registration_number = par[3];
            Last_repaired_at = Validation.Check_last_repaired(par[4]);
            Bought_at = Validation.Check_bought_at(par[5], Last_repaired_at);
            Car_mileage = Validation.Check_car_mileage(par[6]);
        }
        
        

        public override string ToString()
        {
            return
                $"{ID} {Brand} {Model} {Registration_number} {Last_repaired_at.ToShortDateString()} {bought_at.ToShortDateString()} {car_mileage}";
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
                        var convertedInput = Convert.ChangeType(input, setter.PropertyType);
                        setter.SetValue(this, convertedInput);
                        break;
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
    }
}
