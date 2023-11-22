using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Domain.Entity;
using Domain.ViewModel;

namespace Service.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserVm>();
            CreateMap<VoteVm, Vote>()
                .ForMember(x => x.GroupId, y => y.MapFrom(z => Guid.Parse(z.GroupId)));
        }
    }
}