using REBrokerApp.Business.ViewModel;

namespace REBrokerApp.Business.Services
{
    /// <summary>
    /// Interface for dashboard service to provide statistics for brokers
    /// </summary>
    public interface IDashboardService
    {
        /// <summary>
        /// Get dashboard data for a broker user
        /// </summary>
        /// <param name="userName">The username of the broker</param>
        /// <returns>Dashboard data with property and commission statistics</returns>
        Task<DashboardViewModel> GetBrokerDashboardAsync(string userName);

        /// <summary>
        /// Get the count of properties sold by a specific broker
        /// </summary>
        /// <param name="userName">The username of the broker</param>
        /// <returns>Number of properties sold</returns>
        Task<int> GetBrokerSoldPropertiesCountAsync(string userName);

        /// <summary>
        /// Get the count of all available properties
        /// </summary>
        /// <returns>Number of available properties</returns>
        Task<int> GetAvailablePropertiesCountAsync();

        /// <summary>
        /// Get the total commission earned by a broker
        /// </summary>
        /// <param name="userName">The username of the broker</param>
        /// <returns>Total commission amount</returns>
        Task<decimal> GetBrokerTotalCommissionAsync(string userName);

        /// <summary>
        /// Get recent commission transactions for a broker
        /// </summary>
        /// <param name="userName">The username of the broker</param>
        /// <param name="count">Number of recent transactions to retrieve</param>
        /// <returns>List of recent commission transactions</returns>
        Task<List<CommissionItemViewModel>> GetBrokerRecentCommissionsAsync(string userName, int count = 5);
    }
}