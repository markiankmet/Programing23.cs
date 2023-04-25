
using Newtonsoft.Json;

namespace Task_6_PR_2._0.Model
{
    public enum StateEnum
    {
        Published,
        Draft,
        Moderation
    }

    public class Car
    {

        
        public Guid Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string RegistrationNumber { get; set; }
        public string LastRepairedAt { get; set; }
        public string BoughtAt { get; set; }
        public int CarMileage { get; set; }
        
        [JsonIgnore]
        public string state;
        [JsonIgnore]
        public IState _state;

        public string State
        {
            get => state;
            set => state = value;
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

        public void SetState(IState curState)
        {
            _state = curState;
            State = curState.ToString();
        }

        public override string ToString()
        {
            return
                $"{Id} {Brand} {Model} {RegistrationNumber} {LastRepairedAt} {BoughtAt} {CarMileage}";
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

