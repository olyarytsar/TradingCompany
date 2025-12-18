using AutoMapper;
using TradingCompany.DTO;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TradingCompany.MVC.App.MappingProfiles
{
    public class SupplierListItemProfile : Profile
    {
        public SupplierListItemProfile()
        {
            CreateMap<Supplier, SelectListItem>()
                .ForMember(dest => dest.Value, src => src.MapFrom(s => s.SupplierId))
                .ForMember(dest => dest.Text, src => src.MapFrom(s => s.Brand));
        }
    }
}