using AutoMapper;
using EF = TradingCompany.DALEF.Models;
using DTO = TradingCompany.DTO;

namespace TradingCompany.DALEF.AutoMapper
{
    public class RoleMap : Profile
    {
        public RoleMap()
        {
            CreateMap<EF.Role, DTO.Role>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role1));

            CreateMap<DTO.Role, EF.Role>()
                .ForMember(dest => dest.Role1, opt => opt.MapFrom(src => src.RoleName));
        }
    }
}
