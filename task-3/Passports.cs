namespace Task_3
{
    
    class Passports
    {
        public int id;
        public string country_code;
        public string passport_no;
        public string name;
        public string date_of_issue;
        public string date_of_expire;
        public string date_of_birth;

        public int ID
        {
            get => id;
            set => id = value;
        }

        public string Country_code
        {
            get => country_code;
            set => country_code = value;
        }

        public string Passport_no
        {
            get => passport_no;
            set => passport_no = value;
        }

        public string Name
        {
            get => name;
            set => name = value;
        }

        public string Date_of_issue
        {
            get => date_of_issue;
            set => date_of_issue = value;
        }

        public string Date_of_expire
        {
            get => date_of_expire;
            set => date_of_expire = value;
        }

        public string Date_of_birth
        {
            get => date_of_birth;
            set => date_of_birth = value;
        }
        
        public Passports(int id, string country_code, string passport_no, string name, string date_of_issue,
            string date_of_expire, string date_of_birth)
        {
            this.id = id;
            this.country_code = country_code;
            this.passport_no = passport_no;
            this.name = name;
            this.date_of_issue = date_of_issue;
            this.date_of_expire = date_of_expire;
            this.date_of_birth = date_of_birth;
        }
        
        public Passports() {}

        public override string ToString()
        {
            return $"{ID} {Country_code} {Passport_no} {Name} {Date_of_issue} {Date_of_expire} {Date_of_birth}";
        }
        
        public void Add()
        {
            var setters = typeof(Passports).GetProperties()
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

        public bool FoundPassport(string search_object)
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