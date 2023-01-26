using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace Assignment2.Models
{
    public partial class ProductsModel : DbContext
    {
        public ProductsModel()
            : base("name=ProductsModel")
        {
        }

        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<Rating> Rating { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .Property(e => e.Path)
                .IsUnicode(false);
        }
    }
}
