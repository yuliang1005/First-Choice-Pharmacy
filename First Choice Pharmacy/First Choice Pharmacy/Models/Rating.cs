namespace Assignment2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Rating")]
    public partial class Rating
    {
        public int Id { get; set; }

        [Required]
        public string Rate { get; set; }

        [Required]
        public string Comment { get; set; }

        public DateTime Time { get; set; }

        public int ProductId { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }
    }
}
