using AutoMapper;


namespace Disunity.Store.Entities {

    public class TargetProfile : Profile {

        public TargetProfile() {
            CreateMap<Target, TargetDto>();
        }

    }

}