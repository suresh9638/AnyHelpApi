

using Microsoft.EntityFrameworkCore;

#nullable disable

namespace anyhelp.Data.DataContext
{
    public partial class anyhelpContext : DbContext
    {
        public anyhelpContext()
        {
        }

        public anyhelpContext(DbContextOptions<anyhelpContext> options)
            : base(options)
        {
        }
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                
                AppConfiguration appConfig = new AppConfiguration();
                
                optionsBuilder.UseSqlServer(appConfig.SqlConnectonString);
            }
        }



        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
