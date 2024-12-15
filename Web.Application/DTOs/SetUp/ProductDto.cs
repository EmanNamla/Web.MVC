using System.ComponentModel.DataAnnotations;


namespace Web.Application.DTOs.SetUp
{
    public class ProductDto
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }

        public DateTime CreationDate  = DateTime.Now;

        [Display(Name = "StartDate")]
        [Required(ErrorMessage = "Start Date is required")]
        public DateTime StartDate { get; set; }

        [Display(Name = "EndDate")]
        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000000, ErrorMessage = "Price must be between 0.01 and 1,000,000")]
        public decimal Price { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is required")]
        public int? CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
