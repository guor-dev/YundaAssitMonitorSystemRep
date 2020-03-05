using System;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentInfoDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.EquipmentInfoDto.SearchCondition;

namespace YunDa.ISAS.Application.GeneralInformation
{
    public interface IEquipmentInfoAppService : IAppServiceBase<EquipmentInfoSearchConditionInput, EquipmentInfoOutput, EditEquipmentInfoInput, Guid>
    {
        RequestPageResult<SelectModelOutput> FindEquipmentInfoForSelect(PageSearchCondition<EquipmentInfoSearchConditionInput> searchCondition);

        /// <summary>
        /// 通过Id查找单个数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<RequestPageResult<EquipmentInfoOutput>> FindEquipmentById(Guid id);
    }
}