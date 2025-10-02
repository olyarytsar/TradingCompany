using AutoMapper;
using EF = TradingCompany.DALEF.Models;
using DTO = TradingCompany.DTO;

namespace TradingCompany.DALEF.AutoMapper
{
    public class EmployeeMap : Profile
    {
        public EmployeeMap()
        {
            CreateMap<EF.Employee, DTO.Employee>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId ?? 0));

            CreateMap<DTO.Employee, EF.Employee>()
                .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (int?)src.RoleId));
        }
    }
}
