using System.ComponentModel.DataAnnotations;

namespace MinimalAPIDemo.Models
{
    public class Employee
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Position is required")]
        [StringLength(50, ErrorMessage = "Position can't be longer than 50 characters")]
        public string Position { get; set; }
        [Range(30000, 200000, ErrorMessage = "Salary must be between 30000 and 200000")]
        public decimal Salary { get; set; }
    }
}
