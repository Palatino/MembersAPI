using Application.Common.Interfaces;
using Domain;
using Infrastructure.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure
{
    [ExcludeFromCodeCoverage]

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
