namespace Aggregator.Contracts
{
    public interface IUserManager
    {
        IUser Register(string login, string password);
        IUser CheckAndGet(string login, string password);
    }
}