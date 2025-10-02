using AutoMapper;
using EF = TradingCompany.DALEF.Models;
using DTO = TradingCompany.DTO;

namespace TradingCompany.DALEF.AutoMapper
{
    public class ProductMap : Profile
    {
        public ProductMap()
        {
            CreateMap<EF.Product, DTO.Product>();
            CreateMap<DTO.Product, EF.Product>();
        }
    }
}
