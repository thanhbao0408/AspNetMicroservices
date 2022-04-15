using Discount.Grpc.Entities;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);

        Task CreateDiscount(Coupon coupon);
        Task UpdateDiscount(Coupon coupon);
        Task DeleteDiscount(string productName);
    }
}