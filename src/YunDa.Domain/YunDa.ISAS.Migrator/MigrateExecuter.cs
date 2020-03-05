using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Reflection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Transactions;
using YunDa.ISAS.Core;
using YunDa.ISAS.Core.Configuration;
using YunDa.ISAS.EntityFrameworkCore.EntityFrameworkCore;
using YunDa.ISAS.EntityFrameworkCore.EntityFrameworkCore.Seed;

namespace YunDa.ISAS.Migrator
{
    public class MigrateExecuter : ITransientDependency
    {
        private readonly Log _log;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDbContextResolver _dbContextResolver;

        public MigrateExecuter(
            IUnitOfWorkManager unitOfWorkManager,
           IDbContextResolver dbContextResolver,
            Log log)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _dbContextResolver = dbContextResolver;
            _log = log;
            _appConfiguration = ISASConfiguration.Get(typeof(ISASMigratorModule).GetAssembly().GetDirectoryPathOrNull());
        }

        public bool Run(bool skipConnVerification)
        {
            var hostConnStr = _appConfiguration.GetConnectionString(ISASConsts.ConnectionStringKey);
            if (hostConnStr.IsNullOrWhiteSpace())
            {
                _log.Write("Configuration file should contain a connection string named 'Default'");
                return false;
            }

            _log.Write("Host database: " + hostConnStr);
            if (!skipConnVerification)
            {
                _log.Write("Continue to migration for this host database and all tenants..? (Y/N): ");
                var command = Console.ReadLine();
                if (!command.IsIn("Y", "y"))
                {
                    _log.Write("Migration canceled.");
                    return false;
                }
            }

            _log.Write("HOST database migration started...");

            try
            {
                CreateOrMigrateForHost(SeedHelper.SeedHostDb, hostConnStr);
            }
            catch (Exception ex)
            {
                _log.Write("An error occured during migration of host database:");
                _log.Write(ex.ToString());
                _log.Write("Canceled migrations.");
                return false;
            }

            _log.Write("HOST database migration completed.");
            _log.Write("--------------------------------------------------------");
            _log.Write("All databases have been migrated.");

            return true;
        }

        //private static string CensorConnectionString(string connectionString)
        //{
        //    var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
        //    var keysToMask = new[] { "password", "pwd", "user id", "uid" };

        //    foreach (var key in keysToMask)
        //    {
        //        if (builder.ContainsKey(key))
        //        {
        //            builder[key] = "*****";
        //        }
        //    }

        //    return builder.ToString();
        //}
        #region

        public virtual void CreateOrMigrateForHost(Action<ISASDbContext> seedAction, string nameOrConnectionString)
        {
            CreateOrMigrate(seedAction, nameOrConnectionString);
        }

        protected virtual void CreateOrMigrate(Action<ISASDbContext> seedAction, string nameOrConnectionString)
        {
            //var args = new DbPerTenantConnectionStringResolveArgs(null, MultiTenancySides.Host)
            //{
            //    ["DbContextType"] = typeof(ISASDbContext),
            //    ["DbContextConcreteType"] = typeof(ISASDbContext)
            //};

            using var uow = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress);
            using var dbContext = _dbContextResolver.Resolve<ISASDbContext>(nameOrConnectionString, null);
            dbContext.Database.Migrate();
            seedAction?.Invoke(dbContext);
            //SeedHelper.SeedHostDb(dbContext);
            _unitOfWorkManager.Current.SaveChanges();
            uow.Complete();
            #endregion
        }
    }

    public class DbPerTenantConnectionStringResolveArgs : ConnectionStringResolveArgs
    {
        public int? TenantId { get; set; }

        public DbPerTenantConnectionStringResolveArgs(int? tenantId, MultiTenancySides? multiTenancySide = null)
            : base(multiTenancySide)
        {
            TenantId = tenantId;
        }

        public DbPerTenantConnectionStringResolveArgs(int? tenantId, ConnectionStringResolveArgs baseArgs)
        {
            TenantId = tenantId;
            MultiTenancySide = baseArgs.MultiTenancySide;

            foreach (var kvPair in baseArgs)
            {
                Add(kvPair.Key, kvPair.Value);
            }
        }
    }
}