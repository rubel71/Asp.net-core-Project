using Microsoft.EntityFrameworkCore;

namespace BulkyBook.Data
{
    public class ApplicatinDbContext:DbContext
    {
        public ApplicatinDbContext(DbContextOptions<ApplicatinDbContext>options):base(options)
        {

        }
    }
}
