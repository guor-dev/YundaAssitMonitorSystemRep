using System;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.GeneralInformation.PowerSupplyLineDto;

namespace YunDa.ISAS.Application.GeneralInformation
{
    public interface IPowerSupplyLineAppService : IAppServiceBase<PowerSupplyLineSearchConditionInput, PowerSupplyLineOutput, EditPowerSupplyLineInput, Guid>
    {
        /// <summary>
        /// 查找线路下所有的子节点
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        RequestPageResult<PowerSupplyLineOutput> GetPowerSupplyLinesAndChildrenAsync(PageSearchCondition<PowerSupplyLineSearchConditionInput> searchCondition);
    }
}