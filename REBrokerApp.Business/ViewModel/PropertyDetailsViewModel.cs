namespace REBrokerApp.Business.ViewModel
{
    public class PropertyDetailsViewModel
    {
        // --- Property Info ---
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PropertyStatus { get; set; }
        public int BedRooms { get; set; }
        public int BathRooms { get; set; }
        public int Toilet { get; set; }
        public int CarPark { get; set; }
        public decimal Price { get; set; }
        public string? BuilderName { get; set; }

        public string? BrokerName { get; set; }
        public string? BrokerPhone { get; set; }

        // --- Location ---
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? Suburb { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        public string? Latitude { get; set; }
        public string? Longitude { get; set; }

        // --- Features ---
        public string? Internal { get; set; }
        public string? External { get; set; }
        public string? General { get; set; }
        public string? Security { get; set; }
        public string? LocationFeatures { get; set; }
        public string? LifeStyle { get; set; }

        // --- Images ---
        public List<PropertyImageViewModel> Images { get; set; } = new();

    }

    // Helper for images
    public class PropertyImageViewModel
    {
        public string ImageSrc { get; set; } = string.Empty; // base64 string
        public string AltText { get; set; } = string.Empty;
    }
}
