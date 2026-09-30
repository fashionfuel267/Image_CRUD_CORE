using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhotoPath { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}