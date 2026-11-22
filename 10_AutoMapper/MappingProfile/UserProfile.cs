using _10_AutoMapper.Dtos;
using _10_AutoMapper.Models;
using AutoMapper;

namespace _10_AutoMapper.MappingProfile
{
    public class UserProfile : Profile // Profile miras alınabilmesi için Nuget paketlerinden AutoMapper kurulması gerekir.
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                // Özel alan eşleşmesi: FullName = FirstName + LastName
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                // Çift yönlü eşleme desteği
                .ReverseMap();
        }
    }
}
