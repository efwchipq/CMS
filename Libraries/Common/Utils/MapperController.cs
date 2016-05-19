using AutoMapper;

namespace Common.Utils {

    public class MapperUtils {
        public static TDestination Map<TSource, TDestination>(TSource entity)
            where TSource : class
            where TDestination : class {
            var controller = new MapperController<TSource, TDestination>();
            var model = controller.Map(entity);
            return model;
        }
    }

    public class MapperController<TSource, TDestination>
        where TSource : class
        where TDestination : class {

        public MapperController() {
            Mapper.CreateMap<TSource, TDestination>();
        }

        public TDestination Map(TSource entity) {
            var model = Mapper.Map<TSource, TDestination>(entity);
            return model;
        }

    }
}