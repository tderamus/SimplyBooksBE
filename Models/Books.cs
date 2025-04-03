using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SimplyBooksBE.Models
{
    public class Books
    {
        required public string uid { get; set; }
        public string AuthorId { get; set; } // Foreign key to Authors
        public Authors Author { get; set; }
        public string firebaseKey { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
        public decimal price { get; set; } = 0.00m;
        public bool sale { get; set; } = false;
    }
}
