using AutoMapper;
using EF = TradingCompany.DALEF.Models;
using DTO = TradingCompany.DTO;

namespace TradingCompany.DALEF.AutoMapper
{
    public class SupplierMap : Profile
    {
        public SupplierMap()
        {
            CreateMap<EF.Supplier, DTO.Supplier>();
            CreateMap<DTO.Supplier, EF.Supplier>();
        }
    }
}
