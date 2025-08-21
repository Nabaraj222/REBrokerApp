using System.Collections.Generic;

namespace REBrokerApp.Business.ViewModel
{
    public class DashboardViewModel
    {
        /// <summary>
        /// Number of properties sold by the broker
        /// </summary>
        public int PropertiesSoldCount { get; set; }

        /// <summary>
        /// Total number of available properties
        /// </summary>
        public int AvailablePropertiesCount { get; set; }

        /// <summary>
        /// Total commission earned by the broker
        /// </summary>
        public decimal TotalCommission { get; set; }

        /// <summary>
        /// Latest commissions for the broker
        /// </summary>
        public List<CommissionItemViewModel> RecentCommissions { get; set; } = new List<CommissionItemViewModel>();
    }

    public class CommissionItemViewModel
    {
        /// <summary>
        /// Property title
        /// </summary>
        public string PropertyTitle { get; set; }

        /// <summary>
        /// Commission amount
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Commission percentage
        /// </summary>
        public decimal Percentage { get; set; }

        /// <summary>
        /// Transaction date
        /// </summary>
        public DateTime TransactionDate { get; set; }
    }
}