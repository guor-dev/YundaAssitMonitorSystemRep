using Abp.Auditing;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Runtime.Validation;
using Abp.UI;
using System;
using System.Linq;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core.Session;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.Application.Core.Auditing
{
    public class ISASAuditingStore : IAuditingStore
    {
        private readonly ISessionAppService _sessionAppService;
        private readonly IRepository<SysAuditLog, Guid> _auditLogRepository;

        public ISASAuditingStore(ISessionAppService sessionAppService)
        {
            _sessionAppService = sessionAppService;
        }

        public void Save(AuditInfo auditInfo)
        {
            throw new NotImplementedException();
        }

        public Task SaveAsync(AuditInfo auditInfo)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 从给定的 <see cref="auditInfo"/> 审计信息创建一个新的 MongoDb 审计日志实体
        /// (<see cref="MongoDbAuditEntity"/>)。
        /// </summary>
        /// <param name="auditInfo">原始审计日志信息。</param>
        /// <returns>创建完成的 <see cref="MongoDbAuditEntity"/> 实体对象。</returns>
        private SysAuditLog CreateFromAuditInfo(AuditInfo auditInfo)
        {
            var expMsg = GetAbpClearException(auditInfo.Exception);

            return new SysAuditLog
            {
                ServiceName = auditInfo.ServiceName.TruncateWithPostfix(SysAuditLog.MaxServiceNameLength),
                MethodName = auditInfo.MethodName.TruncateWithPostfix(SysAuditLog.MaxMethodNameLength),
                Parameters = auditInfo.Parameters.TruncateWithPostfix(SysAuditLog.MaxParametersLength),
                ExecutionTime = auditInfo.ExecutionTime,
                ExecutionDuration = auditInfo.ExecutionDuration,
                ClientIpAddress = auditInfo.ClientIpAddress.TruncateWithPostfix(SysAuditLog.MaxClientIpAddressLength),
                ClientName = auditInfo.ClientName.TruncateWithPostfix(SysAuditLog.MaxClientNameLength),
                BrowserInfo = auditInfo.BrowserInfo.TruncateWithPostfix(SysAuditLog.MaxBrowserInfoLength),
                Exception = expMsg.TruncateWithPostfix(SysAuditLog.MaxExceptionLength),
                CustomData = auditInfo.CustomData.TruncateWithPostfix(SysAuditLog.MaxCustomDataLength)
            };
        }

        /// <summary>
        /// 创建更加清楚明确的异常信息。
        /// </summary>
        /// <param name="exception">要处理的异常数据。</param>
        private static string GetAbpClearException(Exception exception)
        {
            var clearMessage = "";
            switch (exception)
            {
                case null:
                    return null;

                case AbpValidationException abpValidationException:
                    clearMessage = "异常为参数验证错误，一共有 " + abpValidationException.ValidationErrors.Count + "个错误:";
                    foreach (var validationResult in abpValidationException.ValidationErrors)
                    {
                        var memberNames = "";
                        if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                        {
                            memberNames = " (" + string.Join(", ", validationResult.MemberNames) + ")";
                        }

                        clearMessage += "\r\n" + validationResult.ErrorMessage + memberNames;
                    }
                    break;

                case UserFriendlyException userFriendlyException:
                    clearMessage =
                        $"业务相关错误，错误代码: {userFriendlyException.Code} \r\n 异常详细信息: {userFriendlyException.Details}";
                    break;
            }

            return exception + (string.IsNullOrEmpty(clearMessage) ? "" : "\r\n\r\n" + clearMessage);
        }
    }
}