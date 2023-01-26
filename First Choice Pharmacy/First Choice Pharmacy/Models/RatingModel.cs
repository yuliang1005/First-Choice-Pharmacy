using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Assignment2.Models
{
    public partial class RatingModel : DbContext
    {
        public RatingModel()
            : base("name=RatingModel")
        {
        }

        public virtual DbSet<Rating> Ratings { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>()
                .Property(e => e.Rate)
                .HasPrecision(18, 0);
        }
    }
}
