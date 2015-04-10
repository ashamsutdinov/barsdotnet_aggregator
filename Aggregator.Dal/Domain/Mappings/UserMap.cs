using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Aggregator.Dal.Domain.Mappings
{
    public class UserMap :
        ClassMapping<User>
    {
        public UserMap()
        {
            Table("Users");
            Id(u => u.Id, m => m.Generator(Generators.Identity));
            Property(u => u.Login);
            Property(u => u.PasswordHash);
            Property(u => u.Salt);
            Property(u => u.DateRegistered);
        }
    }
}
