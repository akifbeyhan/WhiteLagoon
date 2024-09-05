using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa { Id =1, Name = "Royal Villa", Occupancy = 4, Price = 500, Sqft = 250, Description = "250m2 Full deniz manzarali 3 odali", ImageUrl = "https://placehold.co/600x400" ,CreateDate = DateTime.Now},
                new Villa { Id = 2, Name = "Premium Pool Villa", Occupancy = 4, Price = 600, Sqft = 350, Description = "350m2 Full deniz manzarali 3 odali", ImageUrl = "https://placehold.co/600x400", CreateDate = DateTime.Now },
                new Villa { Id = 3, Name = "Luxury Villa", Occupancy = 4, Price = 700, Sqft = 400, Description = "400m2 Full deniz manzarali 3 odali", ImageUrl = "https://placehold.co/600x400", CreateDate = DateTime.Now }
                );
            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber { Villa_Number = 101, VillaId = 1 }, 
                new VillaNumber { Villa_Number = 102, VillaId = 1 }, 
                new VillaNumber { Villa_Number = 103, VillaId = 1 },
                new VillaNumber { Villa_Number = 201, VillaId = 2 },
                new VillaNumber { Villa_Number = 202, VillaId = 2 },
                new VillaNumber { Villa_Number = 203, VillaId = 2 },
                new VillaNumber { Villa_Number = 301, VillaId = 3 },
                new VillaNumber { Villa_Number = 302, VillaId = 3 },
                new VillaNumber { Villa_Number = 303, VillaId = 3 }
                );
        }
    }
}
