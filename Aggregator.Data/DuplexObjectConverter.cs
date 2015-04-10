using Aggregator.Contracts;
using AutoMapper;

namespace Aggregator.Data
{
    public class DuplexObjectConverter<TInteface, TObject, TDomainObject> :
        IDuplexObjectConverter<TInteface, TDomainObject>
        where TObject : class, TInteface
        where TInteface : class 
    {
        public DuplexObjectConverter()
        {
            Mapper.CreateMap<TObject, TDomainObject>();
            Mapper.CreateMap<TDomainObject, TObject>();
        }

        public TInteface Convert(TDomainObject obj)
        {
            return Mapper.Map<TDomainObject, TObject>(obj);
        }

        public TDomainObject Convert(TInteface obj)
        {
            return Mapper.Map<TObject, TDomainObject>(obj as TObject);
        }
    }
}