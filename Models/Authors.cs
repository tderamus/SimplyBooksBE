using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace SimplyBooksBE.Models
{
    public class Authors
    {
        [Key]
        required public string uid { get; set; }
        public string? firebaseKey { get; set; }
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? email { get; set; }
        public string? image { get; set; }
        public bool favorite { get; set; } = false;
    }
}
