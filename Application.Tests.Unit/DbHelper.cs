using Application.Common.Interfaces;
using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Tests.Unit
{

    internal static class DbHelper
    {
        internal static IMembershipsDbContext CreateSqlLiteIDbContext(SqliteConnection connection)
        {
            connection.Open();

            var contextOptions = new DbContextOptionsBuilder<MembershipsDbContext>()
                .UseSqlite(connection)
                .Options;
            var context = new MembershipsDbContext(contextOptions);
            context.Database.EnsureCreated();

            return context;

        }
    }
}
