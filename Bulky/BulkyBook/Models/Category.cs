using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Category name is required")]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
