// ReSharper disable All
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Task_5_Prog
{
    public enum StateEnum
    {
        Published,
        Draft,
        Moderation
    }
    
    public class Car
    {
        [JsonIgnore]
        public int id;
        [JsonIgnore]
        public string brand;
        [JsonIgnore]
        public string model;
        [JsonIgnore]
        public string registration_number;
        [JsonIgnore]
        public string last_repaired_at;
        [JsonIgnore]
        public string bought_at;
        [JsonIgnore]
        public int car_mileage;
        [JsonIgnore]
        public bool _isValid = true;
        [JsonIgnore]
        public string state;
        [JsonIgnore] 
        public IState _state;
        
        public int ID
        {
            get => id;
            set
            {
                id = Validation.Check_ID(value.ToString(), ref _isValid);
            } 
        }

        public string Brand
        {
            get { return brand; }
            set
            {
                brand = Validation.Check_brand(value, ref _isValid);
            }
        }

        public string Model
        {
            get { return model; }
            set
            {
                model = Validation.Check_model(value, ref _isValid);
            }
        }

        public string Registration_number
        {
            get { return registration_number; }
            set
            {
                registration_number = Validation.Check_registration_number(value, ref _isValid);
            }
        }

        public string Last_repaired_at
        {
            get { return last_repaired_at; }
            set
            {
                last_repaired_at = Validation.Check_last_repaired(value.ToString(), ref _isValid);
            }
        }

        public string Bought_at
        {
            get { return bought_at; }
            set
            {
                bought_at = Validation.Check_bought_at(value.ToString(), last_repaired_at, ref _isValid);
            }
        }
            
        public int Car_mileage
        {
            get { return car_mileage; }
            set
            {
                car_mileage = Validation.Check_car_mileage(value.ToString(), ref _isValid);
            }
        }
        
        public string State
        {
            get => state;
            set => state = value;
        }

        public Car()
        {
            this.id = 1;
            this.brand = "Audi";
            this.model = "R7";
            this.registration_number = "BC3213KI";
            this.last_repaired_at = "2018-10-10";
            this.bought_at = "2017-11-10";
            this.car_mileage = 0;
            this.state = StateEnum.Draft.ToString();
            this._state = new DraftState();
        }
        
        public Car(int Id, string _brand, string _model, string reg_number, string last_rep, string boug_at, int car_m, string stEn)
        {
            ID = Id;
            Brand = _brand;
            Model = _model;
            Registration_number = reg_number;
            Last_repaired_at = last_rep;
            Bought_at = boug_at;
            Car_mileage = car_m;
            State = stEn;
            _state = ConvertState(State);
        }

        public IState ConvertState(string curr)
        {
            IState currState = null;
            if (State == StateEnum.Published.ToString())
            {
                return new PublishedState();
            }

            if (State == StateEnum.Draft.ToString())
            {
                return new DraftState();
            }

            if (State == StateEnum.Moderation.ToString())
            {
                return new ModerationState();
            }

            return currState;
        }
        
        public override string ToString()
        {
            return
                $"{ID} {Brand} {Model} {Registration_number} {Last_repaired_at} {bought_at} {car_mileage}";
        }

        public void SetState(IState curState)
        {
            _state = curState;
            State = curState.ToString();
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
                    if (setter.Name != "State")
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
        }
         public string Edit_field()
         {
             string return_field = "";
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
                        return_field += $"Field: {key}, '";
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
                    var convertedInput = Convert.ChangeType(value, setters[index_key].PropertyType);
                    if (key == "Last_repaired_at")
                    {
                        int comparer = value.CompareTo(this.Bought_at);
                        if (comparer > 0)
                        {
                            return_field += $"{setters[index_key].GetValue(this)}' --> '";
                            GetType().GetProperty(key).SetValue(this, convertedInput);
                            return_field += $"{setters[index_key].GetValue(this)}'";
                            this._isValid = true;
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Bought at date must be earlier");
                            this._isValid = false;
                        }
                    }
                    else
                    {
                        return_field += $"{setters[index_key].GetValue(this)}' --> '";
                        GetType().GetProperty(key).SetValue(this, convertedInput);
                        return_field += $"{setters[index_key].GetValue(this)}'";
                        break;
                    }
                }
                catch (Exception ex) { Console.WriteLine($"Invalid input: {ex.Message}"); }
            }

            return return_field;
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

        public void Add()
        {
            _state = ConvertState(state);
            _state.Add(this);
        }

        public void Edit()
        {
            _state = ConvertState(state);
            _state.Edit(this);
        }

        public void Submit()
        {
            _state = ConvertState(state);
            _state.Submit(this);
        }

        public void Approve()
        {
            _state = ConvertState(state);
            _state.Approve(this);
        }
    }
}