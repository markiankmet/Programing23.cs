// ReSharper disable All
namespace Task_5_Prog
{
    public interface ISerivese
    {
        Car AddItem();
        Car DeleteItem();
        void SearchData(string data);
        string EditItem();
        string SortItems();
        void View();
        
        Car ViewById();
    }


    class ConcreteServise : ISerivese
    {
        public Collection carColl;

        public ConcreteServise(string filePath)
        {
            carColl = new Collection(filePath);
            carColl.Read_From_File();
        }
        public Car AddItem()
        {
            Car returnCar = carColl.Add_Car();
            return returnCar;
        }

        public Car DeleteItem()
        {
            Car returnCar = carColl.Delete_Car();
            return returnCar;
        }

        public void SearchData(string data)
        {
            carColl.Search_data(data);
        }

        public string EditItem()
        {
            string edited_string = carColl.EditCar();
            return edited_string;
        }

        public string SortItems()
        {
            string field = carColl.Sort_Cars();
            return field;
        }

        public void View()
        {
            Console.WriteLine(carColl);
        }

        public Car ViewById()
        {
            Car outCar = carColl.GetByID();
            return outCar;
        }
    }
}