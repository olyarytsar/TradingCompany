using AutoMapper;
using EF = TradingCompany.DALEF.Models;
using DTO = TradingCompany.DTO;

namespace TradingCompany.DALEF.AutoMapper
{
    public class CategoryMap : Profile
    {
        public CategoryMap()
        {
            CreateMap<EF.Category, DTO.Category>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category1));

            CreateMap<DTO.Category, EF.Category>()
                .ForMember(dest => dest.Category1, opt => opt.MapFrom(src => src.CategoryName));
        }
    }
}
