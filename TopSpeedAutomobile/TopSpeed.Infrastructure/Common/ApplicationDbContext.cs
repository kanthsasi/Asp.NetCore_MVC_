using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TopSpeed.Application.ApplicationConstants;
using TopSpeed.Domain.Models;

namespace TopSpeed.Infrastructure.Common
{
    //public class ApplicationDbContext : DbContext
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Brand> Brand { get; set; }

        public DbSet<VehicalType> VehicleType { get; set; }

        public DbSet<CarDetails> CarDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var masterAdminId = "b21ef45b-0e5e-48b3-b00a-098927a59d78";
            var adminId = "d4c7e1a0-98d1-4c0c-a3e7-03528d189ca1";
            var CustomerId = "dd92e4e1-f1a3-43e8-a594-a7d9b559ed26";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = masterAdminId,
                    ConcurrencyStamp = masterAdminId,
                    Name = CustomRole.MasterAdmin,
                    NormalizedName = CustomRole.MasterAdmin
                },
                new IdentityRole
                {
                    Id = adminId,
                    ConcurrencyStamp = adminId,
                    Name = CustomRole.Admin,
                    NormalizedName = CustomRole.Admin
                },
                new IdentityRole
                {
                    Id = CustomerId,
                    ConcurrencyStamp = CustomerId,
                    Name = CustomRole.Customer,
                    NormalizedName = CustomRole.Customer
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
