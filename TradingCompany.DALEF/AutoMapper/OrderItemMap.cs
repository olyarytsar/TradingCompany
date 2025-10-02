using AutoMapper;
using EF = TradingCompany.DALEF.Models;
using DTO = TradingCompany.DTO;

namespace TradingCompany.DALEF.AutoMapper
{
    public class OrderItemMap : Profile
    {
        public OrderItemMap()
        {
            CreateMap<EF.OrderItem, DTO.OrderItem>();
            CreateMap<DTO.OrderItem, EF.OrderItem>();
        }
    }
}
