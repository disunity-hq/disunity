using AutoMapper;

using Disunity.Client.v1.Models;


namespace Disunity.Store.Entities {

    public class ModDependencyProfile:Profile {

        public ModDependencyProfile() {
            CreateMap<ModDependency, ModDependencyDto>();
        }

    }

}