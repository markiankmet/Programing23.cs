// ReSharper disable All
using Newtonsoft.Json;

namespace Task_5_Prog
{

    class CollectionOfUsers
    {
        public List<User> userCollection;
        public string fileName;

        public CollectionOfUsers(string file_name)
        {
            this.userCollection = new List<User>();
            this.fileName = file_name;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(userCollection, Formatting.Indented);

        }

        public void WriteInFile()
        {
            string json_data = JsonConvert.SerializeObject(userCollection, Formatting.Indented);
            File.WriteAllText(fileName, json_data);
        }

        public void ReadFromFile()
        {
            using (StreamReader file = new StreamReader(fileName))
            {
                string json_data = file.ReadToEnd();
                List<User> users = JsonConvert.DeserializeObject<List<User>>(json_data);
                foreach (User user in users)
                {
                    if (user.isValid)
                    {
                        this.userCollection.Add(user);
                    }
                }
                file.Close();
            }
        }

        public void SignUp()
        {
            User new_user = new User();
            Console.WriteLine("---Sign Up---");
            new_user.InputUser();
            if (new_user.isValid)
            {
                this.userCollection.Add(new_user);
                this.WriteInFile();
            }
        }
        
        public User LogIn()
        {
            bool check_input = true;
            do
            {
                Console.WriteLine("----Logging in----");
                Console.Write("Enter email: ");
                string email = Console.ReadLine();
                Console.Write("Enter password: ");
                string password = Console.ReadLine();
                foreach (User user in userCollection)
                {
                    if (email == user.Email && password == user.Password)
                    {
                        check_input = false;
                        Console.WriteLine("____Logged in____");
                        return user;
                    }
                }
                Console.WriteLine("Invalid user Log In! Try again!");
            } while (check_input);

            return null;
        }
        
    }
}