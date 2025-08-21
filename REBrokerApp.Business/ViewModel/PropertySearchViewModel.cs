using System.ComponentModel.DataAnnotations;

namespace REBrokerApp.Business.ViewModel
{
    /// <summary>
    /// View model for property search functionality
    /// </summary>
    public class PropertySearchViewModel
    {
        /// <summary>
        /// Location search term (suburb or state)
        /// </summary>
        [Display(Name = "Location")]
        public string? Location { get; set; }

        /// <summary>
        /// Minimum price for price range search
        /// </summary>
        [Display(Name = "Min Price")]
        public decimal? MinPrice { get; set; }

        /// <summary>
        /// Maximum price for price range search
        /// </summary>
        [Display(Name = "Max Price")]
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// The search results
        /// </summary>
        public IEnumerable<Domain.Entities.Property>? SearchResults { get; set; }
    }
}