using Microsoft.EntityFrameworkCore;
using System.Linq;
using YunDa.ISAS.Core.Helper;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.EntityFrameworkCore.EntityFrameworkCore.Seed
{
    public class InitialHostDbBuilder
    {
        private readonly ISASDbContext _context;

        public InitialHostDbBuilder(ISASDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            CreateSeedUser();
        }

        private void CreateSeedUser()
        {
            var adminUser = _context.SysUserDbSet.IgnoreQueryFilters().FirstOrDefault(u => u.UserName == SysUser.AdminUserName);
            if (adminUser == null)
            {
                adminUser = SysUser.CreateTenantAdminUser("admin@default.com");
                adminUser.Password = StringHelper.MD5Encrypt64(SysUser.DefaultPassword);
                _context.SysUserDbSet.Add(adminUser);
                _context.SaveChanges();
            }
            var testUser = _context.SysUserDbSet.IgnoreQueryFilters().FirstOrDefault(u => u.UserName == "test");
            if (testUser == null)
            {
                testUser = SysUser.CreateTenantAdminUser("test@default.com", "test");
                testUser.Password = StringHelper.MD5Encrypt64(SysUser.DefaultPassword);
                _context.SysUserDbSet.Add(testUser);
                _context.SaveChanges();
            }
        }

        private void CreateSeedFunction()
        {
            var adminUser = _context.SysUserDbSet.IgnoreQueryFilters().FirstOrDefault(u => u.UserName == SysUser.AdminUserName);
            if (adminUser == null)
            {
                adminUser = SysUser.CreateTenantAdminUser("admin@default.com");
                adminUser.Password = StringHelper.MD5Encrypt64(SysUser.DefaultPassword);
                _context.SysUserDbSet.Add(adminUser);
                _context.SaveChanges();
            }
            var testUser = _context.SysUserDbSet.IgnoreQueryFilters().FirstOrDefault(u => u.UserName == "test");
            if (testUser == null)
            {
                testUser = SysUser.CreateTenantAdminUser("test@default.com", "test");
                testUser.Password = StringHelper.MD5Encrypt64(SysUser.DefaultPassword);
                _context.SysUserDbSet.Add(testUser);
                _context.SaveChanges();
            }
        }

        private void CreateSeedRole()
        {
            var adminUser = _context.SysUserDbSet.IgnoreQueryFilters().FirstOrDefault(u => u.UserName == SysUser.AdminUserName);
            if (adminUser == null) return;
            var adminRole = _context.SysRoleDbSet.IgnoreQueryFilters().FirstOrDefault(r => r.RoleName == SysRole.AdminRole);
            if (adminRole == null)
            {
                adminRole.RoleName = SysRole.AdminRole;
                adminRole.IsActive = true;
                _context.SysRoleDbSet.Add(adminRole);
                _context.SaveChanges();
            }
        }
    }
}