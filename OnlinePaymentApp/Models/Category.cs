using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace OnlinePaymentApp.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")] // asp-for
        [Range(1, 100, ErrorMessage="The field Display Order must be between 1 - 100.")] // server validation
        public int DisplayOrder { get; set; }
    }
}
