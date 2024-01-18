using Microsoft.EntityFrameworkCore;

namespace AomTung.DataAccessLayer.Data
{
    public class AomTungExtendDbContext : AomTungDbContext
    {
        public AomTungExtendDbContext(DbContextOptions<AomTungDbContext> options) : base(options)
        {
        }

        public string GetAesSaltKey()
        {
            var key = Database
                .SqlQuery<string>($"SELECT GetAesSaltKey() AS salt;")
                .AsEnumerable();
            return key.Single();
        }
    }
}
