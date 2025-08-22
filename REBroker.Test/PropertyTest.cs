using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using REBrokerApp.Business.Services;
using REBrokerApp.Business.ViewModel;
using REBrokerApp.Domain.Entities;
using REBrokerApp.Domain.Enums;
using REBrokerApp.Web.Data;

namespace REBroker.Test
{
    [TestFixture]
    public class PropertyServiceTests
    {
        private ApplicationDbContext _context;
        private PropertyService _service;

        [SetUp]
        public void Setup()
        {
            // Use unique database name per test to avoid interference
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            _context = new ApplicationDbContext(options);
            _service = new PropertyService(_context);

            // Clear data before each test
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            _context?.Dispose();
        }

        [Test]
        public async Task GetAvailablePropertiesAsync_ReturnsOnlyAvailableProperties()
        {
            // Arrange
            _context.Properties.AddRange(new List<Property>
            {
                new Property { Id = 1, Description = "Nice", Title = "Available Prop", PropertyStatus = PropertyStatus.Available },
                new Property { Id = 2,Description = "Nice", Title = "Sold Prop", PropertyStatus = PropertyStatus.Sold }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetAvailablePropertiesAsync();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.First().PropertyStatus, Is.EqualTo(PropertyStatus.Available));
        }

        [Test]
        public async Task GetPropertyByIdAsync_WithInclude_ReturnsPropertyWithRelatedData()
        {
            // Arrange
            var property = new Property
            {
                Id = 1,
                Title = "Test Property",
                Description = "Nice Property",
                PropertyLocation = new PropertyLocation { Address1 = "123 Main St" },
                PropertyFeature = new PropertyFeature { Internal = "AC, Heating" },
                PropertyImages = new List<PropertyImage>
                {
                    new PropertyImage
                    {
                        ImageData = new byte[] { 0x01, 0x02 },
                        ImageMimeType = "image/jpeg",
                        FileName = "img1.jpg"
                    }
                }
            };

            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetPropertyByIdAsync(1, true);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.IsNotNull(result.PropertyLocation);
            Assert.That(result.PropertyLocation.Address1, Is.EqualTo("123 Main St"));
            Assert.IsNotNull(result.PropertyFeature);
            Assert.That(result.PropertyFeature.Internal, Is.EqualTo("AC, Heating"));
            Assert.That(result.PropertyImages.Count, Is.GreaterThan(0));
            Assert.That(result.PropertyImages.First().FileName, Is.EqualTo("img1.jpg"));
        }

        [Test]
        public async Task GetPropertyByIdAsync_WithoutInclude_ReturnsPropertyOnly()
        {
            // Arrange
            var property = new Property { Id = 1, Title = "Simple Prop", Description = "Basic property" };
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetPropertyByIdAsync(1, false);

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.IsNull(result.PropertyLocation); // Should not be loaded
            Assert.IsNull(result.PropertyFeature);
            Assert.IsEmpty(result.PropertyImages);
        }

        [Test]
        public void MapToDetailsViewModel_MapsAllFieldsCorrectly()
        {
            // Arrange
            var property = new Property
            {
                Id = 1,
                Title = "Luxury Villa",
                Description = "Beautiful villa with pool",
                PropertyStatus = PropertyStatus.Available,
                BedRooms = 4,
                BathRooms = 3,
                Toilet = 2,
                CarPark = 2,
                Price = 850000,
                BuilderName = "ABC Constructions",
                BrokerName = "John Doe",
                BrokerPhone = "123-456-7890",

                PropertyLocation = new PropertyLocation
                {
                    Address1 = "456 Oak Ave",
                    Address2 = "Unit 2",
                    Suburb = "Springfield",
                    State = "NSW",
                    PostalCode = "2000",
                    Latitude = "-33.865",
                    Longitude = "151.209"
                },

                PropertyFeature = new PropertyFeature
                {
                    Internal = "Ducted AC",
                    External = "Pool, Garden",
                    General = "Balcony",
                    Security = "CCTV",
                    Location = "Near park",
                    LifeStyle = "Family friendly"
                },

                PropertyImages = new List<PropertyImage>
                {
                    new PropertyImage
                    {
                        ImageData = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 },
                        ImageMimeType = "image/jpeg",
                        FileName = "villa.jpg"
                    }
                }
            };

            // Act
            var viewModel = _service.MapToDetailsViewModel(property);

            // Assert
            Assert.IsNotNull(viewModel);
            Assert.That(viewModel.Id, Is.EqualTo(1));
            Assert.That(viewModel.Title, Is.EqualTo("Luxury Villa"));
            Assert.That(viewModel.PropertyStatus, Is.EqualTo("Available"));
            Assert.That(viewModel.BedRooms, Is.EqualTo(4));
            Assert.That(viewModel.BathRooms, Is.EqualTo(3));
            Assert.That(viewModel.Price, Is.EqualTo(850000));
            Assert.That(viewModel.Suburb, Is.EqualTo("Springfield"));
            Assert.That(viewModel.State, Is.EqualTo("NSW"));
            Assert.That(viewModel.Images.Count, Is.EqualTo(1));
            Assert.That(viewModel.Images[0].ImageSrc, Does.StartWith("image/jpeg;base64,/9j/"));
            Assert.That(viewModel.Images[0].AltText, Is.EqualTo("villa.jpg"));
        }

        [Test]
        public async Task CreatePropertyAsync_AddsNewPropertyWithAllRelatedEntities()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            var content = "dummy image content";
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write(content);
            writer.Flush();
            stream.Position = 0;

            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.FileName).Returns("test.jpg");
            fileMock.Setup(f => f.Length).Returns(stream.Length);
            fileMock.Setup(f => f.ContentType).Returns("image/jpeg");

            var model = new PropertyCreateViewModel
            {
                Title = "New House",
                Description = "Nice house",
                BedRooms = 3,
                BathRooms = 2,
                Toilet = 1,
                CarPark = 2,
                Price = 600000,
                BuilderName = "BuildCo",
                BrokerName = "Agent Smith",
                BrokerPhone = "0412345678",
                Address1 = "789 Pine Rd",
                Suburb = "Hills",
                State = "VIC",
                PostalCode = "3000",
                Latitude = "-37.8136",
                Longitude = "144.9631",
                Internal = "Modern kitchen",
                External = "Deck",
                General = "Garage",
                Security = "Alarm",
                Location = "Quiet street",
                LifeStyle = "Close to schools",
                ImageFiles = new List<IFormFile> { fileMock.Object }
            };

            // Act
            var result = await _service.CreatePropertyAsync(model, "admin");

            // Assert
            Assert.IsNotNull(result);
            Assert.That(result.Id, Is.GreaterThan(0));
            Assert.That(result.Title, Is.EqualTo("New House"));
            Assert.That(result.Price, Is.EqualTo(600000));

            // Check related data
            var dbProperty = await _context.Properties
                .Include(p => p.PropertyLocation)
                .Include(p => p.PropertyFeature)
                .Include(p => p.PropertyImages)
                .FirstOrDefaultAsync(p => p.Id == result.Id);

            Assert.IsNotNull(dbProperty);
            Assert.IsNotNull(dbProperty.PropertyLocation);
            Assert.That(dbProperty.PropertyLocation.Address1, Is.EqualTo("789 Pine Rd"));
            Assert.IsNotNull(dbProperty.PropertyFeature);
            Assert.That(dbProperty.PropertyFeature.Internal, Is.EqualTo("Modern kitchen"));
            Assert.That(dbProperty.PropertyImages.Count, Is.EqualTo(1));
            Assert.That(dbProperty.PropertyImages.First().FileName, Is.EqualTo("test.jpg"));
        }

        [Test]
        public async Task BuyPropertyAsync_ChangesStatusAndCreatesCommission()
        {
            // Arrange
            var brokerUser = new IdentityUser { Id = "broker1", UserName = "broker1" };
            var property = new Property
            {
                Id = 1,
                Title = "Test Property",
                Description = "Nice Property",
                Price = 500000,
                PropertyStatus = PropertyStatus.Available,
                CreatedBy = brokerUser.UserName
            };

            var commissionSetup = new BrokerCommisionSetup
            {
                MinPrice = 400000,
                MaxPrice = 600000,
                Percentage = 2.5m
            };

            _context.Users.Add(brokerUser);
            _context.Properties.Add(property);
            _context.BrokerCommisionSetups.Add(commissionSetup);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.BuyPropertyAsync(1, "buyer1");

            // Assert
            Assert.IsTrue(result);

            var updatedProperty = await _context.Properties.FindAsync(1);
            Assert.That(updatedProperty.PropertyStatus, Is.EqualTo(PropertyStatus.Sold));
            Assert.That(updatedProperty.BuyerId, Is.EqualTo("buyer1"));

            var commission = await _context.BrokerCommissions.FirstOrDefaultAsync();
            Assert.IsNotNull(commission);
            Assert.That(commission.PropertyId, Is.EqualTo(1));
            Assert.That(commission.UserId, Is.EqualTo("broker1"));
            Assert.That(commission.CommissionAmount, Is.EqualTo(12500)); // 2.5% of 500k
            Assert.That(commission.CommissionPercentage, Is.EqualTo(2.5m));
        }

        [Test]
        public async Task DeletePropertyAsync_RemovesPropertyIfExists()
        {
            // Arrange
            var property = new Property { Id = 1, Title = "To Delete", Description = "Will be removed" };
            _context.Properties.Add(property);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeletePropertyAsync(1);

            // Assert
            Assert.IsTrue(result);
            var inDb = await _context.Properties.FindAsync(1);
            Assert.IsNull(inDb);
        }

        [Test]
        public async Task SearchPropertiesAsync_FiltersByLocationAndPrice()
        {
            // Arrange
            _context.Properties.AddRange(new List<Property>
            {
                new Property
                {
                    Id = 1,
                    Title = "Sydney House",Description = "Nice",
                    Price = 450000,
                    PropertyStatus = PropertyStatus.Available,
                    PropertyLocation = new PropertyLocation { Suburb = "Sydney", State = "NSW" }
                },
                new Property
                {
                    Id = 2,
                    Title = "Melbourne Unit",Description = "Nice",
                    Price = 600000,
                    PropertyStatus = PropertyStatus.Available,
                    PropertyLocation = new PropertyLocation { Suburb = "Melbourne", State = "VIC" }
                }
            });
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.SearchPropertiesAsync("Sydney", 400000, 500000);

            // Assert
            var list = result.ToList();
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0].Title, Is.EqualTo("Sydney House"));
        }
    }
}