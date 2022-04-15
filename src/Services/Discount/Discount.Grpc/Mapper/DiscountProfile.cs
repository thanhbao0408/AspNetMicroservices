using Discount.Grpc.Entities;
using Discount.Grpc.Protos;
using AutoMapper;

namespace Discount.Grpc.Mapper
{
    public class DiscountProfile: Profile
    {
        public DiscountProfile()
        {
            CreateMap<Coupon, CouponModel>()
                .ForMember(des => des.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ReverseMap()
                .ForMember(des=> des.Id, opt => opt.MapFrom(src => Guid.Parse(src.Id)));
        }
    }
}
