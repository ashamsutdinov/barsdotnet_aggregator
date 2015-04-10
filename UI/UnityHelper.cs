using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace UI
{
    public static class UnityHelper
    {
        private static UnityContainer _container;

        private static readonly object SyncRoot = new object();

        public static UnityContainer Container
        {
            get
            {
                if (_container == null)
                {
                    lock (SyncRoot)
                    {
                        if (_container == null)
                        {
                            _container = new UnityContainer();
                            _container.LoadConfiguration();
                        }
                    }
                }
                return _container;
            }
        }
    }
}