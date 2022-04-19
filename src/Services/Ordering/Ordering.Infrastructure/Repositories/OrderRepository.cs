using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
        {
            try
            {
                var test =  _dbContext.Database.GetConnectionString();
                var abc = await _dbContext.Orders.ToListAsync();
                if(!(await _dbContext.Orders
                    .Where(o => o.UserName == userName)
                    .AnyAsync()))
                {
                    return null; 
                }
                var orderList = await _dbContext.Orders
                                    .Where(o => o.UserName == userName)
                                    .ToListAsync();
                return orderList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
