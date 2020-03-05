using Microsoft.EntityFrameworkCore;

//using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
//using System;

namespace YunDa.ISAS.EntityFrameworkCore.EntityFrameworkCore
{
    public static class DbContextOptionsConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<ISASDbContext> dbContextOptions, string connectionString)
        {
            /* This is the single point to configure DbContextOptions for AbpProjectNameDbContext */
            //dbContextOptions.UseMySql(connectionString, mySqlOptions =>
            //{
            //    mySqlOptions.ServerVersion(new Version(8, 0, 15), ServerType.MySql); // replace with your Server Version and Type
            //});
            dbContextOptions.UseMySql(connectionString);
            //dbContextOptions.use
        }
    }
}