using System;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevEquipmentInfoDto;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    public interface IVideoDevEquipmentInfoAppService : IAppServiceBase<VideoDevEquipmentInfoSearchConditionInput, VideoDevEquipmentInfoOutput, EditVideoDevEquipmentInfoInput, Guid>
    {
        /// <summary>
        /// 获取视频摄像头下拉框数据
        /// </summary>
        /// <returns>返回下拉框数据</returns>
        RequestPageResult<SelectModelOutput> FindVideoTerminalForSelect(PageSearchCondition<VideoDevEquipmentInfoSearchConditionInput> searchCondition);
    }
}