using Discount.API.Entities;
using System.Threading.Tasks;

namespace Discount.API.Repositories
{
    public interface IDiscountRepository
    {
        Task<Coupon> GetDiscount(string productName);

        Task CreateDiscount(Coupon coupon);
        Task UpdateDiscount(Coupon coupon);
        Task DeleteDiscount(string productName);
    }
}