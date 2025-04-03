using Application.Common.Interfaces;
using Domain;
using Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class MembershipsDbContext : DbContext, IMembershipsDbContext
    {
        public MembershipsDbContext()
        {

        }
        public MembershipsDbContext(DbContextOptions<MembershipsDbContext> options)
        : base(options)
        { }

        public DbSet<Membership> Memberships { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MembershipConfiguration());


        }
    }


}
