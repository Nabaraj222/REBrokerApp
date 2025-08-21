using Microsoft.EntityFrameworkCore;
using REBrokerApp.Business.ViewModel;
using REBrokerApp.Domain.Entities;
using REBrokerApp.Domain.Enums;
using REBrokerApp.Infrastructure;
using REBrokerApp.Web.Data;

namespace REBrokerApp.Business.Services
{
    /// <summary>
    /// Implementation of dashboard service
    /// </summary>
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDbContext _context;

        public DashboardService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<DashboardViewModel> GetBrokerDashboardAsync(string userName)
        {
            var dashboard = new DashboardViewModel
            {
                PropertiesSoldCount = await GetBrokerSoldPropertiesCountAsync(userName),
                AvailablePropertiesCount = await GetAvailablePropertiesCountAsync(),
                TotalCommission = await GetBrokerTotalCommissionAsync(userName),
                RecentCommissions = await GetBrokerRecentCommissionsAsync(userName)
            };

            return dashboard;
        }

        /// <inheritdoc />
        public async Task<int> GetBrokerSoldPropertiesCountAsync(string userName)
        {
            return await _context.Properties
                .Where(p => p.CreatedBy == userName && p.PropertyStatus == PropertyStatus.Sold)
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetAvailablePropertiesCountAsync()
        {
            return await _context.Properties
                .Where(p => p.PropertyStatus == PropertyStatus.Available)
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task<decimal> GetBrokerTotalCommissionAsync(string userName)
        {
            // Find user by username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            
            if (user == null)
            {
                return 0;
            }

            // Sum all commissions for this broker
            return await _context.BrokerCommissions
                .Where(c => c.UserId == user.Id)
                .SumAsync(c => c.CommissionAmount);
        }

        /// <inheritdoc />
        public async Task<List<CommissionItemViewModel>> GetBrokerRecentCommissionsAsync(string userName, int count = 5)
        {
            // Find user by username
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            
            if (user == null)
            {
                return new List<CommissionItemViewModel>();
            }

            // Get recent commissions with property info
            return await _context.BrokerCommissions
                .Where(c => c.UserId == user.Id)
                .OrderByDescending(c => c.TransactionDate)
                .Take(count)
                .Select(c => new CommissionItemViewModel
                {
                    PropertyTitle = c.Property.Title,
                    Amount = c.CommissionAmount,
                    Percentage = c.CommissionPercentage,
                    TransactionDate = c.TransactionDate
                })
                .ToListAsync();
        }
    }
}