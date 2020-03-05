using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.ManufacturerInfoDto;

namespace YunDa.ISAS.Application.GeneralInformation
{
    public interface IManufacturerInfoAppService : IAppServiceBase<ManufacturerInfoSearchConditionInput, ManufacturerInfoOutput, EditManufacturerInfoInput, Guid>
    {
        /// <summary>
        /// 根据ID数组对数据进行软删除
        /// </summary>
        /// <param name="ids">ID数组</param>
        /// <returns>软删除操作结果</returns>
        Task<RequestEasyResult> SoftDeleteByIdsAsync(List<Guid> ids);

        /// <summary>
        /// 根据ID数组对数据恢复软删除的数据
        /// </summary>
        /// <param name="ids">ID数组</param>
        /// <returns>软恢复操作结果</returns>
        Task<RequestEasyResult> RecoverByIdsAsync(List<Guid> ids);

        RequestPageResult<SelectModelOutput> FindManufacturerInfoForSelect(PageSearchCondition<ManufacturerInfoSearchConditionInput> searchCondition);
    }
}