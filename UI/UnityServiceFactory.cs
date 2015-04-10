using Aggregator.Contracts;
using Microsoft.Practices.Unity;

namespace UI
{
    public class UnityServiceFactory : 
        IServiceFactory
    {
        public TInterface Get<TInterface>()
        {
            return UnityHelper.Container.Resolve<TInterface>();
        }
    }
}