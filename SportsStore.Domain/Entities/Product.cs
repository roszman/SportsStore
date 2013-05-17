
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SportsStore.Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        [Required(ErrorMessage="Please enter a product name")]
        public int ProductID { get; set; }
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage="Please enter a description")]
        public string Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage="Please enter a positive price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage="Please specify a category")]
        public string Category { get; set; }
        public int TestProperty { get; set; }
    }
}
