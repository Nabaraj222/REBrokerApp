using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using REBrokerApp.Business.Services;
using REBrokerApp.Domain.Entities;
using REBrokerApp.Infrastructure;

namespace REBrokerApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        private readonly IMemoryCache _memoryCache;
        private const string AvailablePropertiesCacheKey = "AvailableProperties";

        public PropertyController(IPropertyService propertyService, IMemoryCache memoryCache)
        {
            _propertyService = propertyService;
            _memoryCache = memoryCache;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // get available properties from cache
            if (!_memoryCache.TryGetValue(AvailablePropertiesCacheKey, out IEnumerable<Property> properties))
            {
                // Cache miss, get data from service
                properties = await _propertyService.GetAvailablePropertiesAsync();

                // Set cache options
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10));

                // Store data in cache
                _memoryCache.Set(AvailablePropertiesCacheKey, properties, cacheOptions);
            }

            return Ok(new ApiResponse(properties));
        }
    }
}
