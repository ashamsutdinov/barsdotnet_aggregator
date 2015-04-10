using System.Reflection;
using Aggregator.Contracts;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Mapping.ByCode;

namespace Aggregator.Dal
{
    public class DatabaseManager : 
        IDatabaseManager
    {
        public void Initialize()
        {
            var configuration = new Configuration();
            var nhConfig = configuration.Configure();
            var mapper = new ModelMapper();
            mapper.AddMappings(Assembly.GetExecutingAssembly().GetExportedTypes());
            var domainMapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
            nhConfig.AddMapping(domainMapping);
            SessionFactory = nhConfig.BuildSessionFactory();
        }

        internal static ISessionFactory SessionFactory { get; private set; }
    }
}
