using AutoMapper;


namespace Disunity.Store.Entities {

    public class ModDependencyProfile:Profile {

        public ModDependencyProfile() {
            CreateMap<ModDependency, ModDependencyDto>();
        }

    }

}