namespace Aggregator.Contracts
{
    public interface IServiceFactory
    {
        TInterface Get<TInterface>();
    }
}