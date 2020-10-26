using Microsoft.EntityFrameworkCore;

namespace BioMad_backend.Data
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions options) : base(options)
        {
        }
    }
}