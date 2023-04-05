// ReSharper disable All
namespace Task_5_Prog
{
    class PermissionProxy : ISerivese
    {
        public User user;
        public ConcreteServise Servise;

        public PermissionProxy(User user, ConcreteServise servise)
        {
            this.user = user;
            this.Servise = servise;
        }
        
        public Car AddItem()
        {
            Car returnCar = new Car();
            if (user.role == UserRole.Admin || user.role == UserRole.Manager)
            {
                returnCar = Servise.AddItem();
            }
            else
            {
                Console.WriteLine("You are not Admin to do this!");
            }
            return returnCar;
        }

        public Car DeleteItem()
        {
            Car returnCar = new Car();
            if (user.role == UserRole.Admin)
            {
                returnCar = Servise.DeleteItem();
            }
            else
            {
                Console.WriteLine("You are not Admin to do this!");
            }
            return returnCar;
        }

        public void SearchData(string data)
        {
            Servise.SearchData(data);
        }

        public string EditItem()
        {
            string returnString = "";
            if (user.role == UserRole.Admin || user.role == UserRole.Manager)
            {
                returnString = Servise.EditItem();
            }
            else
            {
                Console.WriteLine("You are not Admin to do this!");
            }

            return returnString;
        }

        public string SortItems()
        {
            string field = Servise.SortItems();
            return field;
        }

        public void View()
        {
            Servise.View();
        }

        public Car ViewById()
        {
            Car out_car = Servise.ViewById();
            return out_car;
        }
    }
}