using Microsoft.EntityFrameworkCore;

namespace AomTung.DataAccessLayer.Data
{
    public class AomTungExtendDbContext : AomTungDbContext
    {
        public AomTungExtendDbContext(DbContextOptions<AomTungDbContext> options) : base(options)
        {
        }
    }
}
