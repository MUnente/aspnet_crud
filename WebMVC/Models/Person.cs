using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class Person
    {
        public int? Id { get; set; }
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? Fullname { get; set; }
        [Required]
        public string? Fulldate { get; set; }
        public bool Active { get; set; }
        [Required]
        public string? Country { get; set; }
        public Role? Role { get; set; }
    }
}
