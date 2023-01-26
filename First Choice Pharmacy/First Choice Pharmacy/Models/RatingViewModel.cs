using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Assignment2.Models
{
    public partial class RatingViewModel : DbContext
    {
        public RatingViewModel()
            : base("name=RatingViewModel")
        {
        }

        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<AspNetUsers> Users { get;set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
