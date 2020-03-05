using Abp.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using YunDa.ISAS.Entities.GeneralInformation;
using YunDa.ISAS.Entities.System;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.EntityFrameworkCore.EntityFrameworkCore
{
    public class ISASDbContext : AbpDbContext
    {
        ////输出到debug输出
        //public static readonly LoggerFactory LoggerFactory =
        //       new LoggerFactory(new[] { new DebugLoggerProvider((_, __) => true) });
        // 输出到Console
        //private static readonly LoggerFactory loggerFactory =
        //       new LoggerFactory(new[] { new ConsoleLoggerProvider((_, __) => true, true) });
        //Add DbSet properties for your entities...

        public ISASDbContext(DbContextOptions<ISASDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes().SelectMany(d => d.GetForeignKeys()))
            {
                //Cascade:删除实体
                //ClientSetNull:外键属性设置为 null
                //SetNull:外键属性设置为 null
                //Restrict:无
                foreignKey.DeleteBehavior = DeleteBehavior.Cascade;
            }
            base.OnModelCreating(modelBuilder);
        }

        #region 系统基本信息

        /// <summary>
        /// 系统用户
        /// </summary>
        public virtual DbSet<SysUser> SysUserDbSet { get; set; }

        /// <summary>
        /// 系统角色
        /// </summary>
        public virtual DbSet<SysRole> SysRoleDbSet { get; set; }

        /// <summary>
        /// 系统功能
        /// </summary>
        public virtual DbSet<SysFunction> SysFunctionDbSet { get; set; }

        /// <summary>
        /// 系统角色功能对照表
        /// </summary>
        public virtual DbSet<SysRoleFunction> SysRoleFunctionDbSet { get; set; }

        /// <summary>
        /// 系统用户角色对照表
        /// </summary>
        public virtual DbSet<SysUserRole> SysUserRoleDbSet { get; set; }

        #endregion 系统基本信息

        #region 业务基本信息

        /// <summary>
        /// 线路
        /// </summary>
        public virtual DbSet<PowerSupplyLine> PowerSupplyLineDbSet { get; set; }

        /// <summary>
        /// 变电所
        /// </summary>
        public virtual DbSet<TransformerSubstation> TransformerSubstationDbSet { get; set; }

        /// <summary>
        /// 设备类型
        /// </summary>
        public virtual DbSet<EquipmentType> EquipmentTypeDbSet { get; set; }

        /// <summary>
        /// 设备
        /// </summary>
        public virtual DbSet<EquipmentInfo> EquipmentInfoDbSet { get; set; }

        /// <summary>
        /// 生产商
        /// </summary>
        public virtual DbSet<ManufacturerInfo> ManufacturerInfoDbSet { get; set; }

        #endregion 业务基本信息

        #region 视频相关

        /// <summary>
        /// 视频设备（如NVR、DVR或摄像头）
        /// </summary>
        public virtual DbSet<VideoDev> VideoDevDbSet { get; set; }

        /// <summary>
        /// 视频设备To监控设备
        /// </summary>
        public virtual DbSet<VideoDevEquipmentInfo> VideoDevEquipmentInfoDbSet { get; set; }

        /// <summary>
        /// 视频监控终端（摄像头）预置点
        /// </summary>
        public virtual DbSet<PresetPoint> PresetPointDbSet { get; set; }

        /// <summary>
        /// 视频巡检任务单
        /// </summary>
        public virtual DbSet<InspectionCard> InspectionCardDbSet { get; set; }

        /// <summary>
        /// 视频巡检任务项
        /// </summary>
        public virtual DbSet<InspectionItem> InspectionItemDbSet { get; set; }

        /// <summary>
        /// 视频巡检计划任务
        /// </summary>
        public virtual DbSet<InspectionPlanTask> InspectionPlanTaskDbSet { get; set; }

        #endregion 视频相关
    }
}