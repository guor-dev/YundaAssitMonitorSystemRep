using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using YunDa.ISAS.Core;
using YunDa.ISAS.Core.Configuration;
using YunDa.ISAS.Core.Web;

namespace YunDa.ISAS.EntityFrameworkCore.EntityFrameworkCore
{
    /* This class is needed to run EF Core PMC commands. Not used anywhere else */

    public class ISASDbContextFactory : IDesignTimeDbContextFactory<ISASDbContext>
    {
        public ISASDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ISASDbContext>();
            var configuration = ISASConfiguration.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            DbContextOptionsConfigurer.Configure(
                builder,
                configuration.GetConnectionString(ISASConsts.ConnectionStringKey)
            );

            return new ISASDbContext(builder.Options);
        }
    }

    /* This class is needed to run EF Core PMC commands. Not used anywhere else */
}