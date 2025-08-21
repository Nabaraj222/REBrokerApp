using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REBrokerApp.Business.Services;
using REBrokerApp.Business.ViewModel;

namespace REBrokerApp.Web.Controllers
{
    [Authorize]
    public class PropertiesController : Controller
    {
        private readonly IPropertyService _propertyService;

        public PropertiesController(IPropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        // GET: Properties
        [AllowAnonymous]
        public async Task<IActionResult> Index(PropertySearchViewModel searchModel)
        {
            // Initialize view model if null
            searchModel ??= new PropertySearchViewModel();

            // If search parameters are provided, use search method
            if (!string.IsNullOrWhiteSpace(searchModel.Location) || 
                searchModel.MinPrice.HasValue || 
                searchModel.MaxPrice.HasValue)
            {
                searchModel.SearchResults = await _propertyService.SearchPropertiesAsync(
                    searchModel.Location,
                    searchModel.MinPrice,
                    searchModel.MaxPrice);
            }
            else
            {
                // Otherwise fetch all available properties
                searchModel.SearchResults = await _propertyService.GetAvailablePropertiesAsync();
            }

            return View(searchModel);
        }

        // POST: Properties/Buy/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> Buy(int id)
        {
            var success = await _propertyService.BuyPropertyAsync(id, User.Identity?.Name ?? "system");
            if (!success)
            {
                return NotFound();
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Properties/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var property = await _propertyService.GetPropertyByIdAsync(id.Value);
            if (property == null) return NotFound();

            var viewModel = _propertyService.MapToDetailsViewModel(property);
            return View(viewModel);
        }

        // GET: Properties/Create
        [Authorize(Roles = "Broker")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Properties/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> Create(PropertyCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _propertyService.CreatePropertyAsync(model, User.Identity?.Name ?? "system");
            return RedirectToAction(nameof(Index));
        }

        // GET: Properties/Edit/5
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var property = await _propertyService.GetPropertyByIdAsync(id.Value);
            if (property == null) return NotFound();

            var viewModel = new PropertyCreateViewModel
            {
                // Property Details
                Title = property.Title,
                Description = property.Description,
                BedRooms = property.BedRooms,
                BathRooms = property.BathRooms,
                Toilet = property.Toilet,
                CarPark = property.CarPark,
                Price = property.Price,
                BuilderName = property.BuilderName,
                BrokerName = property.BrokerName,
                BrokerPhone = property.BrokerPhone,

                // Location
                Address1 = property.PropertyLocation?.Address1,
                Address2 = property.PropertyLocation?.Address2,
                Suburb = property.PropertyLocation?.Suburb,
                State = property.PropertyLocation?.State,
                PostalCode = property.PropertyLocation?.PostalCode,
                Latitude = property.PropertyLocation?.Latitude,
                Longitude = property.PropertyLocation?.Longitude,

                // Features
                Internal = property.PropertyFeature?.Internal,
                External = property.PropertyFeature?.External,
                General = property.PropertyFeature?.General,
                Security = property.PropertyFeature?.Security,
                Location = property.PropertyFeature?.Location,
                LifeStyle = property.PropertyFeature?.LifeStyle,
            };

            // Store property ID in ViewData to use in form
            ViewData["PropertyId"] = property.Id;

            return View(viewModel);
        }

        // POST: Properties/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> Edit(int id, PropertyCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var success = await _propertyService.UpdatePropertyAsync(id, model, User.Identity?.Name ?? "system");
            if (!success)
            {
                return NotFound();
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Properties/Delete/5
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var property = await _propertyService.GetPropertyByIdAsync(id.Value, false);
            if (property == null)
            {
                return NotFound();
            }

            return View(property);
        }

        // POST: Properties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Broker")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _propertyService.DeletePropertyAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}