using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.Application.Core.Session;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    public class VideoDevAppService : ISASAppServiceBase, IVideoDevAppService
    {
        private readonly IRepository<VideoDev, Guid> _repository;
        private LoginUserOutput _currentUser;

        public VideoDevAppService(IRepository<VideoDev, Guid> videoDevRepository, ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _repository = videoDevRepository;
            // _currentUser = base.GetCurrentUser();
        }

        #region 增/改

        [HttpPost]
        public async Task<RequestResult<VideoDevOutput>> CreateOrUpdateAsync(EditVideoDevInput input)
        {
            if (input == null) return null;
            if (_currentUser == null)
                _currentUser = base.GetCurrentUser();
            return input.Id != null ? await this.UpdateAsync(input).ConfigureAwait(false) : await this.CreateAsync(input).ConfigureAwait(false);
        }

        #region 添加/更新 实现

        private async Task<RequestResult<VideoDevOutput>> CreateAsync(EditVideoDevInput input)
        {
            var rst = new RequestResult<VideoDevOutput>() { Flag = false };
            try
            {
                input.CreationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                var data = ObjectMapper.Map<VideoDev>(input);
                data = await _repository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<VideoDevOutput>(data);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        private async Task<RequestResult<VideoDevOutput>> UpdateAsync(EditVideoDevInput input)
        {
            var rst = new RequestResult<VideoDevOutput>() { Flag = false };
            try
            {
                var data = await _repository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                input.LastModificationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                ObjectMapper.Map(input, data);
                rst.ResultData = ObjectMapper.Map<VideoDevOutput>(data);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        #endregion 添加/更新 实现

        #endregion 增/改

        #region 删

        [HttpGet]
        public async Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            RequestEasyResult rst = new RequestEasyResult() { };
            try
            {
                await _repository.DeleteAsync(item => item.Id == id).ConfigureAwait(false);
                await _repository.DeleteAsync(item => item.VideoDevId == id).ConfigureAwait(false);//删除子集
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        [HttpPost]
        public async Task<RequestEasyResult> DeleteByIdsAsync(List<Guid> ids)
        {
            RequestEasyResult rst = new RequestEasyResult() { Flag = false };
            try
            {
                await _repository.DeleteAsync(item => ids.Contains(item.Id)).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        #endregion 删

        #region 查

        [HttpPost]
        public RequestPageResult<VideoDevOutput> FindDatas(PageSearchCondition<VideoDevSearchConditionInput> searchCondition)
        {
            RequestPageResult<VideoDevOutput> rst = new RequestPageResult<VideoDevOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var preDatas = searchCondition.SearchCondition.IsNeedPresetPoint ? _repository.GetAllIncluding(dev => dev.PresetPoints, dev => dev.ManufacturerInfo) : _repository.GetAllIncluding(dev => dev.ManufacturerInfo);

                var datas = GetReponsityData(searchCondition, preDatas);
                //获取条件下所有数量
                rst.TotalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting).ThenBy(dev => dev.SeqNo) : datas.OrderBy(dev => dev.SeqNo);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);

                List<VideoDevOutput> rstDatas = null;
                if (!searchCondition.SearchCondition.IsVideoTerminal && searchCondition.SearchCondition.IsNeedChildren)
                {
                    var temp = _repository.GetAllIncluding(dev => dev.ManufacturerInfo).Where(dev => dev.DevType != VideoDevTypeEnum.硬盘录像机);
                    temp = temp.WhereIf(searchCondition.SearchCondition.IsOnlyActive, dev => dev.IsActive);

                    rstDatas = datas.GroupJoin(temp, pDev => pDev.Id, cDev => cDev.VideoDevId, (pDev, cDev) => new VideoDevOutput
                    {
                        Id = pDev.Id,
                        SeqNo = pDev.SeqNo,
                        DevName = pDev.DevName,
                        DevType = pDev.DevType,
                        ManufacturerInfoId = pDev.ManufacturerInfoId,
                        IP = pDev.IP,
                        Port = pDev.Port,
                        DevUserName = pDev.DevUserName,
                        DevPassword = pDev.DevPassword,
                        ChannelNo = pDev.ChannelNo,
                        IsPTZ = pDev.IsPTZ,
                        IsActive = pDev.IsActive,
                        TransformerSubstationId = pDev.TransformerSubstationId,
                        VideoDevId = pDev.VideoDevId,
                        Children = ObjectMapper.Map<List<VideoDevOutput>>(cDev.OrderBy(dev => dev.SeqNo))
                    }).ToList();
                }
                else
                    rstDatas = ObjectMapper.Map<List<VideoDevOutput>>(datas);

                rst.ResultDatas = rstDatas;
                rst.Flag = true;
            }
            catch //(Exception ex)
            {
                //throw new Exception(ex.ToString());
                rst.Flag = false;
            }
            return rst;
        }

        [HttpPost]
        public RequestPageResult<SelectModelOutput> FindDevTypeForSelect()
        {
            RequestPageResult<SelectModelOutput> rst = new RequestPageResult<SelectModelOutput>() { Flag = false };
            try
            {
                rst.ResultDatas = base.GetEnumTypes<VideoDevTypeEnum>();
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        [HttpPost]
        public RequestPageResult<SelectModelOutput> FindVideoDevForSelect(PageSearchCondition<VideoDevSearchConditionInput> searchCondition)
        {
            RequestPageResult<SelectModelOutput> rst = new RequestPageResult<SelectModelOutput>();
            if (searchCondition == null) return rst;
            try
            {
                searchCondition.SearchCondition.IsOnlyActive = true;
                var preDatas = searchCondition.SearchCondition.IsNeedPresetPoint ? _repository.GetAllIncluding(dev => dev.PresetPoints) : _repository.GetAllIncluding();
                var datas = GetReponsityData(searchCondition, preDatas);
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting).ThenBy(dev => dev.SeqNo) : datas.OrderBy(dev => dev.SeqNo).ThenBy(dev => dev.DevName);
                rst.ResultDatas = datas.Select(dev => new SelectModelOutput
                {
                    Key = dev.Id,
                    ParentId = dev.VideoDevId,
                    Text = dev.DevName,
                    Value = dev.Id.ToString().ToLower()
                }).ToList();
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        [HttpGet]
        public async Task<RequestPageResult<VideoDevOutput>> FindVideoDevById(Guid id)
        {
            RequestPageResult<VideoDevOutput> rst = new RequestPageResult<VideoDevOutput>();
            if (id == null || id == default) return rst;
            try
            {
                var data = await _repository.FirstOrDefaultAsync(u => u.Id == id).ConfigureAwait(false);
                rst.ResultDatas = new List<VideoDevOutput>(1) { ObjectMapper.Map<VideoDevOutput>(data) };
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        private IQueryable<VideoDev> GetReponsityData(PageSearchCondition<VideoDevSearchConditionInput> searchCondition, IQueryable<VideoDev> datas)
        {
            return datas
                   .WhereIf(searchCondition.SearchCondition.IsOnlyActive, dev => dev.IsActive)
                   .WhereIf(searchCondition.SearchCondition.DevType.HasValue, dev => dev.DevType == searchCondition.SearchCondition.DevType)
                   .WhereIf(searchCondition.SearchCondition.ManufacturerInfoId.HasValue, dev => dev.ManufacturerInfoId == searchCondition.SearchCondition.ManufacturerInfoId)
                   .WhereIf(searchCondition.SearchCondition.TransformerSubstationId.HasValue, dev => dev.TransformerSubstationId == searchCondition.SearchCondition.TransformerSubstationId)
                   .WhereIf(searchCondition.SearchCondition.VideoDevId.HasValue, dev => dev.VideoDevId == searchCondition.SearchCondition.VideoDevId && dev.DevType != VideoDevTypeEnum.硬盘录像机)
                   .WhereIf(searchCondition.SearchCondition.IsVideoTerminal, dev => dev.DevType != VideoDevTypeEnum.硬盘录像机)
                   .WhereIf(!searchCondition.SearchCondition.DevName.IsNullOrEmpty(), dev => dev.DevName.Contains(searchCondition.SearchCondition.DevName, StringComparison.Ordinal));
        }

        #endregion 查
    }
}