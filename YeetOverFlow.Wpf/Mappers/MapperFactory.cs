using AutoMapper;
using System.Collections.Generic;

namespace YeetOverFlow.Wpf.Mappers
{
    //https://long2know.com/2017/10/net-core-factory-pattern-for-multiple-mappers-automapper/
    public interface IMapperFactory
    {
        IMapper GetMapper(string mapperName);
        void AddMapper(string mapperName, IMapper mapper);
    }

    public class MapperFactory : IMapperFactory
    {
        Dictionary<string, IMapper> _mappers = new Dictionary<string, IMapper>();

        public void AddMapper(string mapperName, IMapper mapper)
        {
            _mappers.Add(mapperName, mapper);
        }

        public IMapper GetMapper(string mapperName)
        {
            return _mappers[mapperName];
        }
    }
}
