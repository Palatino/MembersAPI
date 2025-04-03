using Application.Logging;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Utils
{
    [ExcludeFromCodeCoverage]
    public class DbUpdater
    {
        private readonly MembershipsDbContext _context;
        private readonly ILoggerAdapter<DbUpdater> _logger;
        public DbUpdater(MembershipsDbContext context, ILoggerAdapter<DbUpdater> logger)
        {
            _context = context;
            _logger = logger;
        }
        public void UpdateDB()
        {
            try
            {
                var missingMigrations = _context.Database.GetPendingMigrations();
                if (missingMigrations.Any())
                {
                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message, ex);
            }

        }

    }
}
