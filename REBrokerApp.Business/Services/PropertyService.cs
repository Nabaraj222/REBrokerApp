using Microsoft.EntityFrameworkCore;
using REBrokerApp.Business.ViewModel;
using REBrokerApp.Domain.Entities;
using REBrokerApp.Web.Data;

namespace REBrokerApp.Business.Services
{
    /// <summary>
    /// Implementation of property management service
    /// </summary>
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _context;

        public PropertyService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Property>> GetAvailablePropertiesAsync()
        {
            return await _context.Properties
                .Where(p => p.PropertyStatus == Domain.Enums.PropertyStatus.Available)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Property?> GetPropertyByIdAsync(int id, bool includeRelated = true)
        {
            if (!includeRelated)
            {
                return await _context.Properties.FindAsync(id);
            }

            return await _context.Properties
                .Include(p => p.PropertyLocation)
                .Include(p => p.PropertyFeature)
                .Include(p => p.PropertyImages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        /// <inheritdoc />
        public PropertyDetailsViewModel MapToDetailsViewModel(Property property)
        {
            return new PropertyDetailsViewModel
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                PropertyStatus = property.PropertyStatus.ToString(),
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
                LocationFeatures = property.PropertyFeature?.Location,
                LifeStyle = property.PropertyFeature?.LifeStyle,

                // Images Convert byte[] to base64
                Images = property.PropertyImages.Select(img => new PropertyImageViewModel
                {
                    ImageSrc = $"{img.ImageMimeType};base64,{Convert.ToBase64String(img.ImageData)}",
                    AltText = img.FileName ?? "Property Image"
                }).ToList(),
            };
        }

        /// <inheritdoc />
        public async Task<Property> CreatePropertyAsync(PropertyCreateViewModel model, string userName)
        {
            // Create Property
            var property = new Property
            {
                Title = model.Title,
                Description = model.Description,
                PropertyStatus = Domain.Enums.PropertyStatus.Available,
                BedRooms = model.BedRooms,
                BathRooms = model.BathRooms,
                Toilet = model.Toilet,
                CarPark = model.CarPark,
                Price = model.Price,
                BuilderName = model.BuilderName,
                BrokerName = model.BrokerName,
                BrokerPhone = model.BrokerPhone,
                CreatedBy = userName,
                CreatedOn = DateTime.UtcNow,
                UpdatedBy = userName,
                UpdatedOn = DateTime.UtcNow
            };

            // Save to DB first to get ID
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            // Location (1:1)
            var location = new PropertyLocation
            {
                PropertyId = property.Id,
                Address1 = model.Address1,
                Address2 = model.Address2,
                Suburb = model.Suburb,
                State = model.State,
                PostalCode = model.PostalCode,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };
            _context.PropertyLocation.Add(location);

            // Features (1:1)
            var features = new PropertyFeature
            {
                PropertyId = property.Id,
                Internal = model.Internal,
                External = model.External,
                General = model.General,
                Security = model.Security,
                Location = model.Location,
                LifeStyle = model.LifeStyle
            };
            _context.PropertyFeature.Add(features);

            // Images (1:many)
            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);

                        var image = new PropertyImage
                        {
                            PropertyId = property.Id,
                            ImageData = memoryStream.ToArray(),
                            ImageMimeType = file.ContentType,
                            FileName = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName),
                            CreatedBy = userName,
                        };
                        _context.PropetyImages.Add(image);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return property;
        }

        /// <inheritdoc />
        public async Task<bool> UpdatePropertyAsync(int id, PropertyCreateViewModel model, string userName)
        {
            // Get existing property with related entities
            var property = await _context.Properties
                .Include(p => p.PropertyLocation)
                .Include(p => p.PropertyFeature)
                .Include(p => p.PropertyImages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (property == null)
            {
                return false;
            }

            // Update Property
            property.Title = model.Title;
            property.Description = model.Description;
            property.BedRooms = model.BedRooms;
            property.BathRooms = model.BathRooms;
            property.Toilet = model.Toilet;
            property.CarPark = model.CarPark;
            property.Price = model.Price;
            property.BuilderName = model.BuilderName;
            property.BrokerName = model.BrokerName;
            property.BrokerPhone = model.BrokerPhone;
            property.UpdatedBy = userName;
            property.UpdatedOn = DateTime.UtcNow;

            // Update Location (1:1)
            if (property.PropertyLocation != null)
            {
                property.PropertyLocation.Address1 = model.Address1;
                property.PropertyLocation.Address2 = model.Address2;
                property.PropertyLocation.Suburb = model.Suburb;
                property.PropertyLocation.State = model.State;
                property.PropertyLocation.PostalCode = model.PostalCode;
                property.PropertyLocation.Latitude = model.Latitude;
                property.PropertyLocation.Longitude = model.Longitude;
            }
            else
            {
                // Create new location if it doesn't exist
                var location = new PropertyLocation
                {
                    PropertyId = property.Id,
                    Address1 = model.Address1,
                    Address2 = model.Address2,
                    Suburb = model.Suburb,
                    State = model.State,
                    PostalCode = model.PostalCode,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude
                };
                _context.PropertyLocation.Add(location);
            }

            // Update Features (1:1)
            if (property.PropertyFeature != null)
            {
                property.PropertyFeature.Internal = model.Internal;
                property.PropertyFeature.External = model.External;
                property.PropertyFeature.General = model.General;
                property.PropertyFeature.Security = model.Security;
                property.PropertyFeature.Location = model.Location;
                property.PropertyFeature.LifeStyle = model.LifeStyle;
            }
            else
            {
                // Create new features if they don't exist
                var features = new PropertyFeature
                {
                    PropertyId = property.Id,
                    Internal = model.Internal,
                    External = model.External,
                    General = model.General,
                    Security = model.Security,
                    Location = model.Location,
                    LifeStyle = model.LifeStyle
                };
                _context.PropertyFeature.Add(features);
            }

            // Handle Images (1:many)
            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        using var memoryStream = new MemoryStream();
                        await file.CopyToAsync(memoryStream);

                        var image = new PropertyImage
                        {
                            PropertyId = property.Id,
                            ImageData = memoryStream.ToArray(),
                            ImageMimeType = file.ContentType,
                            FileName = Path.GetFileNameWithoutExtension(file.FileName) + Path.GetExtension(file.FileName),
                            CreatedBy = userName,
                        };
                        _context.PropetyImages.Add(image);
                    }
                }
            }

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await PropertyExistsAsync(property.Id))
                {
                    return false;
                }
                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> BuyPropertyAsync(int id, string buyerUserName)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return false;
            }

            var buyerUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == buyerUserName);
            if (buyerUser == null)
            {
                return false;
            }

            // Change status to Sold
            property.PropertyStatus = Domain.Enums.PropertyStatus.Sold;
            property.UpdatedBy = buyerUserName;
            property.UpdatedOn = DateTime.UtcNow;
            property.BuyerId = buyerUser.Id;

            // Calculate and add broker commission
            var commission = await CalculateBrokerCommissionAsync(property);
            if (commission != null)
            {
                _context.BrokerCommissions.Add(commission);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<BrokerCommission?> CalculateBrokerCommissionAsync(Property property)
        {
            // 1. Find the broker user ID from the property's CreatedBy field
            string brokerUsername = property.CreatedBy;
            var brokerUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == brokerUsername);

            // If broker user not found, return null
            if (brokerUser == null)
            {
                return null;
            }

            // 2. Find the appropriate commission rate based on property price
            var commissionSetup = await _context.BrokerCommisionSetups
                .Where(c => property.Price >= c.MinPrice && property.Price <= c.MaxPrice)
                .FirstOrDefaultAsync();

            // If no matching commission setup found, return null
            if (commissionSetup == null)
            {
                return null;
            }

            // 3. Calculate commission amount
            decimal commissionPercentage = commissionSetup.Percentage;
            decimal commissionAmount = property.Price * (commissionPercentage / 100);

            // 4. Create and return commission record
            return new BrokerCommission
            {
                PropertyId = property.Id,
                UserId = brokerUser.Id,
                CommissionAmount = commissionAmount,
                CommissionPercentage = commissionPercentage,
                TransactionDate = DateTime.UtcNow
            };
        }

        /// <inheritdoc />
        public async Task<bool> DeletePropertyAsync(int id)
        {
            var property = await _context.Properties.FindAsync(id);
            if (property == null)
            {
                return false;
            }

            _context.Properties.Remove(property);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc />
        public async Task<bool> PropertyExistsAsync(int id)
        {
            return await _context.Properties.AnyAsync(e => e.Id == id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Property>> SearchPropertiesAsync(string? location, decimal? minPrice, decimal? maxPrice)
        {
            // Start with a query for available properties
            var query = _context.Properties
                .Where(p => p.PropertyStatus == Domain.Enums.PropertyStatus.Available)
                .Include(p => p.PropertyLocation);

            // Cast back to IQueryable<Property> so we can use Where
            var propertyQuery = query as IQueryable<Property>;

            // Filter by location if provided
            if (!string.IsNullOrWhiteSpace(location))
            {
                // Search in Suburb or State (case-insensitive)
                propertyQuery = propertyQuery.Where(p =>
                    p.PropertyLocation.Suburb.ToLower().Contains(location.ToLower()) ||
                    p.PropertyLocation.State.ToLower().Contains(location.ToLower()));
            }

            // Filter by minimum price if provided
            if (minPrice.HasValue)
            {
                propertyQuery = propertyQuery.Where(p => p.Price >= minPrice.Value);
            }

            // Filter by maximum price if provided
            if (maxPrice.HasValue)
            {
                propertyQuery = propertyQuery.Where(p => p.Price <= maxPrice.Value);
            }

            return await propertyQuery.ToListAsync();
        }
    }
}