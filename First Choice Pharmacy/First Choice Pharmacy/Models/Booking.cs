namespace Assignment2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Booking")]
    public partial class Booking
    {
        public int Id { get; set; }

        public DateTime Start { get; set; }

        [Required]
        [StringLength(128)]
        public string PatientId { get; set; }

        [StringLength(128)]
        public string PharmacistId { get; set; }

        public DateTime End { get; set; }

        [Required]
        public string Title { get; set; }
    }
}
