using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

//namespace App.Repositories
//{
//    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//    {
//        public AppDbContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//            var connectionString = "Server=localhost;Database=stockdb;Uid=root;Pwd=90Polklm.;";

//            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));

//            return new AppDbContext(optionsBuilder.Options);
//        }
//    }
//}
