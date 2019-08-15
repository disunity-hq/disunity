using AutoMapper;

using Disunity.Client.v1.Models;


namespace Disunity.Store.Entities {

    public class TargetProfile : Profile {

        public TargetProfile() {
            CreateMap<Target, TargetDto>();
        }

    }

}