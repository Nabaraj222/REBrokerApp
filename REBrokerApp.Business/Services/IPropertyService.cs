using Microsoft.AspNetCore.Http;
using REBrokerApp.Business.ViewModel;
using REBrokerApp.Domain.Entities;

namespace REBrokerApp.Business.Services
{
    /// <summary>
    /// Interface for property management service
    /// </summary>
    public interface IPropertyService
    {
        /// <summary>
        /// Get all available properties
        /// </summary>
        Task<IEnumerable<Property>> GetAvailablePropertiesAsync();

        /// <summary>
        /// Get property by ID with related data
        /// </summary>
        Task<Property?> GetPropertyByIdAsync(int id, bool includeRelated = true);

        /// <summary>
        /// Map property entity to details view model
        /// </summary>
        PropertyDetailsViewModel MapToDetailsViewModel(Property property);

        /// <summary>
        /// Create a new property with related data
        /// </summary>
        Task<Property> CreatePropertyAsync(PropertyCreateViewModel model, string userName);

        /// <summary>
        /// Update an existing property with related data
        /// </summary>
        Task<bool> UpdatePropertyAsync(int id, PropertyCreateViewModel model, string userName);

        /// <summary>
        /// Process the purchase of a property
        /// </summary>
        Task<bool> BuyPropertyAsync(int id, string buyerUserName);

        /// <summary>
        /// Calculate broker commission for a property sale
        /// </summary>
        Task<BrokerCommission?> CalculateBrokerCommissionAsync(Property property);

        /// <summary>
        /// Delete a property
        /// </summary>
        Task<bool> DeletePropertyAsync(int id);

    /// <summary>
    /// Check if a property exists
    /// </summary>
    Task<bool> PropertyExistsAsync(int id);

    /// <summary>
    /// Search for properties by location and price range
    /// </summary>
    /// <param name="location">Location search term (suburb or state)</param>
    /// <param name="minPrice">Minimum price</param>
    /// <param name="maxPrice">Maximum price</param>
    /// <returns>Collection of properties matching the search criteria</returns>
    Task<IEnumerable<Property>> SearchPropertiesAsync(string? location, decimal? minPrice, decimal? maxPrice);
    }
}