namespace Tutorial
{
    public interface IUpdated
    {
        bool UpdateRequest();
        void Updated();
    }

    public interface IFixedUpdated
    {
        bool UpdateRequest();
        void FixedUpdated();
    }

    public interface ILateUpdated
    {
        bool UpdateRequest();
        void LateUpdated();
    }

    public interface IUnitMove_offline
    {
        void isStop();
    }
}