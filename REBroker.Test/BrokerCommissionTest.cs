using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using REBrokerApp.Business.Services;
using REBrokerApp.Domain.Entities;
using REBrokerApp.Web.Data;

namespace REBroker.Test
{
    public class BrokerComissionTest
    {
        [TestFixture]
        public class PropertiesServiceTests
        {
            private ApplicationDbContext _context;
            private PropertyService _service;

            [SetUp]
            public void Setup()
            {
                var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique DB for each test
                    .Options;

                _context = new ApplicationDbContext(options);
                _service = new PropertyService(_context);
            }

            [TearDown]
            public void Teardown()
            {
                _context.Dispose();
            }

            [Test]
            public async Task CalculateBrokerCommissionAsync_ReturnsCommission_WhenBrokerAndSetupExist()
            {
                // Arrange
                var brokerUser = new IdentityUser { Id = "broker1", UserName = "brokerUser" };
                _context.Users.Add(brokerUser);

                _context.BrokerCommisionSetups.Add(new BrokerCommisionSetup
                {
                    Id = 1,
                    MinPrice = 100000,
                    MaxPrice = 500000,
                    Percentage = 2.5m
                });

                var property = new Property
                {
                    Id = 1,
                    Title = "Test House",
                    Description = "Nice property",
                    Price = 200000,
                    CreatedBy = "brokerUser",  // Important: maps to IdentityUser.UserName
                    BrokerName = "Broker John",
                    BrokerPhone = "123456789"
                };

                await _context.SaveChangesAsync();

                // Act
                var result = await _service.CalculateBrokerCommissionAsync(property);

                // Assert
                Assert.IsNotNull(result);
                Assert.That(result.CommissionAmount, Is.EqualTo(200000 * 0.025m));
                Assert.That(result.UserId, Is.EqualTo("broker1"));
            
            }

            [Test]
            public async Task CalculateBrokerCommissionAsync_ReturnsNull_WhenBrokerNotFound()
            {
                // Arrange
                var property = new Property
                {
                    Id = 1,
                    Title = "Test House",
                    Description = "Nice property",
                    Price = 200000,
                    CreatedBy = "nonExistingUser",
                    BrokerName = "Broker John",
                    BrokerPhone = "123456789"
                };

                // Act
                var result = await _service.CalculateBrokerCommissionAsync(property);

                // Assert
                Assert.IsNull(result);
            }

            [Test]
            public async Task CalculateBrokerCommissionAsync_ReturnsNull_WhenNoMatchingCommissionSetup()
            {
                // Arrange
                var brokerUser = new IdentityUser { Id = "broker1", UserName = "brokerUser" };
                _context.Users.Add(brokerUser);

                // Notice: commission range does not match property price
                _context.BrokerCommisionSetups.Add(new BrokerCommisionSetup
                {
                    Id = 1,
                    MinPrice = 500000,
                    MaxPrice = 1000000,
                    Percentage = 3.0m
                });

                var property = new Property
                {
                    Id = 1,
                    Title = "Test House",
                    Description = "Nice property",
                    Price = 200000,
                    CreatedBy = "brokerUser",
                    BrokerName = "Broker John",
                    BrokerPhone = "123456789"
                };

                await _context.SaveChangesAsync();

                // Act
                var result = await _service.CalculateBrokerCommissionAsync(property);

                // Assert
                Assert.IsNull(result);
            }
        }
    }
}
