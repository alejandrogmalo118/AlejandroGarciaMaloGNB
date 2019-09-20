using AlejandroGarciaMalo.Models.DBContext;
using AlejandroGarciaMalo.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AlejandroGarciaMalo.Models
{
    /// <summary>
    /// Extensions for database
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Get Rates from Database SQLite
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="RateValue"></param>
        /// <returns></returns>
        public static IQueryable<Rate> GetRates(this MyDbContext dbContext, string From = "", string To = "", float? RateValue = null)
        {
            // Get query from DbSet
            var query = dbContext.Rates.AsQueryable();

            // Filter by: 'From'
            if (!From.Equals(null) && !From.Equals(""))
                query = query.Where(r => r.From.Equals(From));

            // Filter by: 'To'
            if (!To.Equals(null) && !To.Equals(""))
                query = query.Where(r => r.To.Equals(To));

            // Filter by: 'RateValue'
            if (RateValue.HasValue)
                query = query.Where(r => r.RateValue == RateValue);

            return query;
        }

        /// <summary>
        /// Get Transactions from Database SQLite
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="pageSize"></param>
        /// <param name="Sku"></param>
        /// <param name="Amount"></param>
        /// <param name="Currency"></param>
        /// <returns></returns>
        public static IQueryable<Transaction> GetTransactions(this MyDbContext dbContext, string Sku = "", float? Amount = null, string Currency = "")
        {
            // Get query from DbSet
            var query = dbContext.Transactions.AsQueryable();

            // Filter by: 'Sku'
            if (!Sku.Equals(null) && !Sku.Equals(""))
                query = query.Where(r => r.Sku.Equals(Sku));

            // Filter by: 'Amount'
            if (Amount.HasValue)
                query = query.Where(r => r.Amount == Amount);

            // Filter by: 'Currency'
            if (!Currency.Equals(null) && !Currency.Equals(""))
                query = query.Where(r => r.Currency.Equals(Currency));

            return query;
        }
        /// <summary>
        /// Get rates by index: fields From and To
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<Rate> GetRateByIndexAsync(this MyDbContext dbContext, Rate entity)
            => await dbContext.Rates.FirstOrDefaultAsync(item => item.From.Equals(entity.From) && item.To.Equals(entity.To));

    }
    
}
