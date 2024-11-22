using System.ComponentModel.DataAnnotations;

namespace Project_Coffe.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public byte[]? Img { get; set; }
        public byte[]? Music { get; set; }
    }
}
