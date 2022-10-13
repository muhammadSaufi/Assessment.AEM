using System.ComponentModel.DataAnnotations;

namespace Assessment.AEM.Models
{
    public class Well
    {
        [Key]
        public int id { get; set; }
        //public int platformId { get; set; }
        public string uniqueName { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }

        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }
    }
}
