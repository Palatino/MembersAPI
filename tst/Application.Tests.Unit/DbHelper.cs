using Application.Common.Interfaces;
using Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

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
