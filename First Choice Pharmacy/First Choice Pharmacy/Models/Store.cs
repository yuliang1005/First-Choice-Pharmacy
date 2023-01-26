namespace Assignment2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Store")]
    public partial class Store
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayFormat(DataFormatString = "{0:###.########}")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "numeric")]
        [DisplayFormat(DataFormatString = "{0:###.########}")]
        public decimal Longitude { get; set; }
    }
}
