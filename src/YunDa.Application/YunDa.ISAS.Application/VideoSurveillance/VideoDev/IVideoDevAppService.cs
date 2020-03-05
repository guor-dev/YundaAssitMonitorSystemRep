using System;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    public interface IVideoDevAppService : IAppServiceBase<VideoDevSearchConditionInput, VideoDevOutput, EditVideoDevInput, Guid>
    {
        /// <summary>
        /// 获取视频设备下拉框数据
        /// </summary>
        /// <returns>返回下拉框数据</returns>
        RequestPageResult<SelectModelOutput> FindVideoDevForSelect(PageSearchCondition<VideoDevSearchConditionInput> searchCondition);

        RequestPageResult<SelectModelOutput> FindDevTypeForSelect();

        Task<RequestPageResult<VideoDevOutput>> FindVideoDevById(Guid id);
    }
}