using AutoMapper;
using TradingCompany.DTO;
using TradingCompany.MVC.Models;

namespace TradingCompany.MVC.App.MappingProfiles
{
    public class EditProductModelProfile : Profile
    {
        public EditProductModelProfile()
        {
            CreateMap<Product, EditProductModel>();

            CreateMap<EditProductModel, Product>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.Supplier, opt => opt.Ignore());
        }
    }
}