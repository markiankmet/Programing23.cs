namespace Task_6_PR_2._0.Model
{
    public interface IState
    {
        void Add(Car new_car);
        void Edit(Car edit_car);
        void Submit(Car car);
        void Approve(Car car);
    }
    public class DraftState : IState
    {
        public void Add(Car new_car)
        {

        }

        public void Edit(Car edit_car)
        {
            edit_car.SetState(new DraftState());
        }

        public void Submit(Car car)
        {
            car.SetState(new ModerationState());
        }

        public void Approve(Car car)
        {
            car.SetState(new PublishedState());
        }
        public override string ToString() => StateEnum.Draft.ToString();
    }


    public class ModerationState : IState
    {
        public void Add(Car new_car)
        {

        }

        public void Edit(Car edit_car)
        {

        }

        public void Submit(Car car)
        {

        }

        public void Approve(Car car)
        {
            car.SetState(new PublishedState());
        }

        public override string ToString() => StateEnum.Moderation.ToString();

    }


    public class PublishedState : IState
    {
        public void Add(Car new_car)
        {
            new_car.SetState(new DraftState());
        }

        public void Edit(Car edit_car)
        {
            edit_car.SetState(new DraftState());
        }

        public void Submit(Car car)
        {

        }

        public void Approve(Car car)
        {

        }
        public override string ToString() => StateEnum.Published.ToString();
    }
}
