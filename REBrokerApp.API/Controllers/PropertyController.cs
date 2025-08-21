using Microsoft.AspNetCore.Mvc;
using REBrokerApp.Business.Services;
using REBrokerApp.Infrastructure;

namespace REBrokerApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PropertyController : ControllerBase
    {
        private readonly IPropertyService _propertyService;
        public PropertyController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var response = await _propertyService.GetAvailablePropertiesAsync();
            return Ok(new ApiResponse(response));
        }
    }
}
