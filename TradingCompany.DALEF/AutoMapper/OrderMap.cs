using AutoMapper;
using EF = TradingCompany.DALEF.Models;
using DTO = TradingCompany.DTO;

namespace TradingCompany.DALEF.AutoMapper
{
    public class OrderMap : Profile
    {
        public OrderMap()
        {
            CreateMap<EF.Order, DTO.Order>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive ?? false));

            CreateMap<DTO.Order, EF.Order>()
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => (bool?)src.IsActive));
        }
    }
}
