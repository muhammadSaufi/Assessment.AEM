using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Assessment.AEM.Models
{
    //[Keyless]
    public class Platform
    {
        [Key]
        public int id { get; set; }
        //public int id { get; set; }
        public string uniqueName { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
        [Column("platformId")]
        public List<Well> Well { get; set; }
    }
}
