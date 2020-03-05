using Abp.Application.Services;
using Abp.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using YunDa.ISAS.Application.Core.Session;
using YunDa.ISAS.Core;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.Session;

namespace YunDa.ISAS.Application.Core
{
    /// <summary>
    /// Derive your application services from this class.
    /// </summary>
    [AbpAuthorize]
    public abstract class ISASAppServiceBase : ApplicationService
    {
        private readonly ISessionAppService _sessionAppService;

        protected ISASAppServiceBase(ISessionAppService sessionAppService)
        {
            LocalizationSourceName = ISASConsts.LocalizationSourceName;
            _sessionAppService = sessionAppService;
        }

        protected virtual LoginUserOutput GetCurrentUser()
        {
            var user = _sessionAppService.GetCurrentLoginInformations().User;
            return user == null ? new LoginUserOutput() : user;
        }

        protected virtual List<SelectModelOutput> GetEnumTypes<TEnum>() where TEnum : Enum
        {
            List<SelectModelOutput> types = new List<SelectModelOutput>();
            foreach (var value in Enum.GetValues(typeof(TEnum)))
            {
                types.Add(new SelectModelOutput
                {
                    Key = value,
                    Value = Convert.ToInt32(value).ToString(),
                    Text = ((DescriptionAttribute)Attribute.GetCustomAttribute(typeof(TEnum).GetField(value.ToString()), typeof(DescriptionAttribute))).Description
                });
            }
            return types;
        }
    }
}