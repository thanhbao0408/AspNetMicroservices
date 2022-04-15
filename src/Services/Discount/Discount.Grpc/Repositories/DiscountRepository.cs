using Discount.Grpc.Data;
using Discount.Grpc.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
//using Npgsql;
using System;
using System.Threading.Tasks;

namespace Discount.Grpc.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly DiscountDbContext _dbContext;
        public DiscountRepository(DiscountDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Coupon> GetDiscount(string productName)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == productName);
            return coupon;
        }

        public async Task CreateDiscount(Coupon coupon)
        {
            coupon.Id = Guid.NewGuid();
            _dbContext.Coupons.Add(coupon);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateDiscount(Coupon coupon)
        {
             _dbContext.Coupons.Update(coupon);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteDiscount(string productName)
        {
            var coupon = await _dbContext.Coupons.FirstOrDefaultAsync(c => c.ProductName == productName);
            _dbContext.Coupons.Remove(coupon);
            await _dbContext.SaveChangesAsync();
        }
    }
}