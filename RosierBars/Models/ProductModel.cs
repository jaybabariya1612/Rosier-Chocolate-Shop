using System;
using System.ComponentModel.DataAnnotations;

namespace RosierBars.Models
{
    public class ProductModel
    {
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9\s&.,'-]+$", ErrorMessage = "Brand contains invalid characters.")]
        public string Brand { get; set; }

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9\s&.,'-]+$", ErrorMessage = "Model name contains invalid characters.")]
        public string ModelName { get; set; }

        [Required]
        [StringLength(150)]
        [RegularExpression(@"^[a-zA-Z0-9\s&.,'-]+$", ErrorMessage = "Product name contains invalid characters.")]
        public string ProductName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(0.0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Original Price")]
        [DataType(DataType.Currency)]
        [Range(0.0, double.MaxValue, ErrorMessage = "Original price must be a positive number.")]
        public decimal OriginalPrice { get; set; }

        [Required]
        [Display(Name = "Discount (%)")]
        [Range(0, 100, ErrorMessage = "Discount must be between 0 and 100.")]
        public int DiscountPercent { get; set; }

        [Required]
        [Range(0.0, 5.0, ErrorMessage = "Rating must be between 0 and 5.")]
        public decimal Rating { get; set; }

        [Required]
        [Display(Name = "Total Ratings")]
        [Range(0, int.MaxValue, ErrorMessage = "Total ratings must be positive.")]
        public int TotalRatings { get; set; }

        [Required]
        [Display(Name = "Total Reviews")]
        [Range(0, int.MaxValue, ErrorMessage = "Total reviews must be positive.")]
        public int TotalReviews { get; set; }

        [Required]
        [RegularExpression(@"^(Yes|No)$", ErrorMessage = "Available must be Yes or No.")]
        public string Available { get; set; } = "Yes";

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive number.")]
        public int Stock { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Flavor contains invalid characters.")]
        public string Flavor { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Type contains invalid characters.")]
        public string Type { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z\s'-]+$", ErrorMessage = "Food Preference contains invalid characters.")]
        public string FoodPreference { get; set; }

        [Required]
        [Display(Name = "Pack of")]
        [Range(1, 1000, ErrorMessage = "Pack of must be between 1 and 1000.")]
        public int PackOf { get; set; }

        [Required]
        [Display(Name = "Net Quantity")]
        [StringLength(20)]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Net Quantity contains invalid characters.")]
        public string NetQuantity { get; set; }

        [Required]
        [Display(Name = "Shelf Life")]
        [StringLength(50)]
        [RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Shelf Life contains invalid characters.")]
        public string ShelfLife { get; set; }

        //[Range(1, 1000, ErrorMessage = "Order quantity must be between 1 and 1000.")]
        public int OrderQuantity { get; set; }

        [Required]
        [Display(Name = "Is Gourmet")]
        [RegularExpression(@"^(Yes|No)$", ErrorMessage = "Is Gourmet must be Yes or No.")]
        public string IsGourmet { get; set; } = "No";

        [Required]
        [Display(Name = "Is Homemade")]
        [RegularExpression(@"^(Yes|No)$", ErrorMessage = "Is Homemade must be Yes or No.")]
        public string IsHomemade { get; set; } = "No";

        [Required]
        [Display(Name = "Is Gift Pack")]
        [RegularExpression(@"^(Yes|No)$", ErrorMessage = "Is Gift Pack must be Yes or No.")]
        public string IsGiftPack { get; set; } = "No";

        [Required]
        [StringLength(100)]
        [RegularExpression(@"^[a-zA-Z0-9\s&.,'-]+$", ErrorMessage = "Seller contains invalid characters.")]
        public string Seller { get; set; }

        [Required]
        [Display(Name = "No Returns Allowed")]
        [RegularExpression(@"^(Yes|No)$", ErrorMessage = "No Returns Allowed must be Yes or No.")]
        public string NoReturnsAllowed { get; set; } = "No";

        [Required]
        [Display(Name = "GST Invoice Available")]
        [RegularExpression(@"^(Yes|No)$", ErrorMessage = "GST Invoice Available must be Yes or No.")]
        public string GstInvoiceAvailable { get; set; } = "No";

        [Required]
        [Display(Name = "Manufactured By")]
        [StringLength(150)]
        [RegularExpression(@"^[a-zA-Z0-9\s&.,'-]+$", ErrorMessage = "Manufactured By contains invalid characters.")]
        public string ManufacturedBy { get; set; }

        [Required]
        public string Ingredients { get; set; }

        [Required]
        [Display(Name = "Nutrition Info")]
        public string NutritionInfo { get; set; }

        [Required]
        [Display(Name = "Width (cm)")]
        [Range(0.0, 1000.0, ErrorMessage = "Width must be a valid number.")]
        public decimal WidthCm { get; set; }

        [Required]
        [Display(Name = "Height (cm)")]
        [Range(0.0, 1000.0, ErrorMessage = "Height must be a valid number.")]
        public decimal HeightCm { get; set; }

        [Required]
        [Display(Name = "Depth (cm)")]
        [Range(0.0, 1000.0, ErrorMessage = "Depth must be a valid number.")]
        public decimal DepthCm { get; set; }

        [Required]
        [Display(Name = "Weight (g)")]
        [Range(0.0, 100000.0, ErrorMessage = "Weight must be a valid number.")]
        public decimal WeightG { get; set; }

        [Display(Name = "Image URL")]
        [StringLength(255)]
        public string ImageUrl { get; set; }

        [Display(Name = "Created At")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Display(Name = "Updated At")]
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}