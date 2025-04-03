using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SimplyBooksBE.Models
{
    public class Books
    {
        required public string uid { get; set; }
        public Authors AuthorId { get; set; }
        public string firebaseKey { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public decimal price { get; set; } = 0.00m; // default price is 0.00
        public bool sale { get; set; } = false; // default is not on sale
    }
}
