using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace YunDa.ISAS.Entities.System
{
    [Table("sys_audit_log")]
    public class SysAuditLog : Entity<Guid>
    {
        /// <summary>
        /// <see cref="ServiceName"/> 属性的最大长度。
        /// </summary>
        public static int MaxServiceNameLength = 256;

        /// <summary>
        /// <see cref="MethodName"/> 属性的最大长度。
        /// </summary>
        public static int MaxMethodNameLength = 256;

        /// <summary>
        /// <see cref="Parameters"/> 属性的最大长度。
        /// </summary>
        public static int MaxParametersLength = 1024;

        /// <summary>
        /// <see cref="ClientIpAddress"/> 属性的最大长度。
        /// </summary>
        public static int MaxClientIpAddressLength = 64;

        /// <summary>
        /// <see cref="ClientName"/> 属性的最大长度。
        /// </summary>
        public static int MaxClientNameLength = 128;

        /// <summary>
        /// <see cref="BrowserInfo"/> 属性的最大长度。
        /// </summary>
        public static int MaxBrowserInfoLength = 512;

        /// <summary>
        /// <see cref="Exception"/> 属性的最大长度。
        /// </summary>
        public static int MaxExceptionLength = 2000;

        /// <summary>
        /// <see cref="CustomData"/> 属性的最大长度。
        /// </summary>
        public static int MaxCustomDataLength = 2000;

        /// <summary>
        /// 调用接口时用户的Id，如果是匿名访问，则可能为 null。
        /// </summary>
        public Guid? UserId { get; set; }

        /// <summary>
        /// 调用接口时用户名，如果是匿名访问，则可能为 null。
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 调用接口时，请求的应用服务/控制器名称。
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 调用接口时，请求的的具体方法/接口名称。
        /// </summary>
        public string MethodName { get; set; }

        /// <summary>
        /// 调用接口时，传递的具体参数。
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// 调用接口的时间，以服务器的时间进行记录。
        /// </summary>
        public DateTime ExecutionTime { get; set; }

        /// <summary>
        /// 调用接口执行方法时所消耗的时间，以毫秒为单位。
        /// </summary>
        public int ExecutionDuration { get; set; }

        /// <summary>
        /// 调用接口时客户端的 IP 地址。
        /// </summary>
        public string ClientIpAddress { get; set; }

        /// <summary>
        /// 调用接口时客户端的名称(通常为计算机名)。
        /// </summary>
        public string ClientName { get; set; }

        /// <summary>
        /// 调用接口的浏览器信息。
        /// </summary>
        public string BrowserInfo { get; set; }

        /// <summary>
        /// 调用接口时如果产生了异常，则记录在本字段，如果没有异常则可能 null。
        /// </summary>
        public string Exception { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }

        public override string ToString()
        {
            return string.Format(
                "审计日志: {0}.{1} 由用户 {2}【{3}】 执行，花费了 {4} 毫秒，请求的源 IP 地址为: {5} 。",
                ServiceName, MethodName, UserName, UserId, ExecutionDuration, ClientIpAddress
            );
        }
    }
}