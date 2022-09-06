namespace WebMVC.Models
{
    public class Person
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
        public string? Fullname { get; set; }
        public string? Fulldate { get; set; }
        public bool Active { get; set; }
        public string? Country { get; set; }
        public Role? Role { get; set; }
    }
}
