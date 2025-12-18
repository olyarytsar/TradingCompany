using AutoMapper;
using TradingCompany.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TradingCompany.MVC.App.MappingProfiles
{
    public class CategoryListItemProfile : Profile
    {
        public CategoryListItemProfile()
        {
            CreateMap<Category, SelectListItem>()
                .ForMember(dest => dest.Value, src => src.MapFrom(c => c.CategoryId))
                .ForMember(dest => dest.Text, src => src.MapFrom(c => c.CategoryName));
        }
    }
}