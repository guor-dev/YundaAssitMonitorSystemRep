using System;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.PresetPointDto;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    public interface IPresetPointAppService : IAppServiceBase<PresetPointSearchCondition, PresetPointOutput, EditPresetPointInput, Guid>
    {
        /// <summary>
        /// 获取摄像头预置点下拉框数据
        /// </summary>
        /// <returns>返回满足条件的数据</returns>
        RequestPageResult<SelectModelOutput> FindPresetPointsForSelect(PageSearchCondition<PresetPointSearchCondition> searchCondition);
    }
}