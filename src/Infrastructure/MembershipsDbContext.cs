using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure.EntityConfigurations;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore.Design;

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
