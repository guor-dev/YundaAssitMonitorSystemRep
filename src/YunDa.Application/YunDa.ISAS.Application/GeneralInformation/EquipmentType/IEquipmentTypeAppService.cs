using System;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentTypeDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentTypeDto.SearchCondition;

namespace YunDa.ISAS.Application.GeneralInformation
{
    public interface IEquipmentTypeAppService : IAppServiceBase<EquipmentTypeSearchConditionInput, EquipmentTypeOutput, EditEquipmentTypeInput, Guid>
    {
        /// <summary>
        /// 获取可以选择的设备类型列表
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        RequestPageResult<SelectModelOutput> FindEquipmentTypeForSelect(PageSearchCondition<EquipmentTypeSearchConditionInput> searchCondition);
    }
}