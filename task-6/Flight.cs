using Newtonsoft.Json;

namespace Task_6_StudyPractik.Models
{
    public class Flight
    {
        public Guid Id { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }
        public string ArrivalCountry { get; set; }
        public string DepartureDateTime { get; set; }
        public string ArrivalDateTime { get; set; }
        public string Airline { get; set; }
        public double Price { get; set; }

        [JsonIgnore]
        public bool _isValid = true;
    }
}
