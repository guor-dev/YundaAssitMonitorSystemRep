using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using AutoMapper;
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
using YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevEquipmentInfoDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    public class VideoDevEquipmentInfoAppService : ISASAppServiceBase, IVideoDevEquipmentInfoAppService
    {
        private readonly IRepository<VideoDevEquipmentInfo, Guid> _repository;
        private LoginUserOutput _currentUser;

        public VideoDevEquipmentInfoAppService(IRepository<VideoDevEquipmentInfo, Guid> videoDevEquipmentInfoRepository, ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _repository = videoDevEquipmentInfoRepository;
        }

        #region 增/改

        [HttpPost]
        public async Task<RequestResult<VideoDevEquipmentInfoOutput>> CreateOrUpdateAsync(EditVideoDevEquipmentInfoInput input)
        {
            if (input == null) return null;
            if (_currentUser == null)
                _currentUser = base.GetCurrentUser();
            return input.Id != null ? await this.UpdateAsync(input).ConfigureAwait(false) : await this.CreateAsync(input).ConfigureAwait(false);
        }

        private async Task<RequestResult<VideoDevEquipmentInfoOutput>> CreateAsync(EditVideoDevEquipmentInfoInput input)
        {
            var rst = new RequestResult<VideoDevEquipmentInfoOutput>() { Flag = false };
            try
            {
                input.CreationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                var data = ObjectMapper.Map<VideoDevEquipmentInfo>(input);
                data = await _repository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<VideoDevEquipmentInfoOutput>(data);
                rst.Flag = true;
            }
            catch (AutoMapperMappingException ex)
            {
                Console.WriteLine(ex.ToString());
            }
            catch
            {
            }
            return rst;
        }

        private async Task<RequestResult<VideoDevEquipmentInfoOutput>> UpdateAsync(EditVideoDevEquipmentInfoInput input)
        {
            var rst = new RequestResult<VideoDevEquipmentInfoOutput>() { Flag = false };
            try
            {
                var data = await _repository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                input.LastModificationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                ObjectMapper.Map(input, data);
                rst.ResultData = ObjectMapper.Map<VideoDevEquipmentInfoOutput>(data);
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        #endregion 增/改

        #region 删

        [HttpGet]
        public Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            throw new NotImplementedException();
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
        public RequestPageResult<VideoDevEquipmentInfoOutput> FindDatas(PageSearchCondition<VideoDevEquipmentInfoSearchConditionInput> searchCondition)
        {
            RequestPageResult<VideoDevEquipmentInfoOutput> rst = new RequestPageResult<VideoDevEquipmentInfoOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var datas = _repository.GetAllIncluding(dev => dev.EquipmentInfo, video => video.VideoDev);
                datas = GetRepositoryData(datas, searchCondition);

                //获取条件下所有数量
                rst.TotalCount = datas.Count();
                //排序
                datas = datas.OrderBy<VideoDevEquipmentInfo>(searchCondition.Sorting).ThenBy(dev => dev.Id);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                var rstDatas = ObjectMapper.Map<List<VideoDevEquipmentInfoOutput>>(datas);
                rst.ResultDatas = rstDatas;
                rst.Flag = true;
            }
            catch { }
            return rst;
        }

        [HttpPost]
        public RequestPageResult<SelectModelOutput> FindVideoTerminalForSelect(PageSearchCondition<VideoDevEquipmentInfoSearchConditionInput> searchCondition)
        {
            RequestPageResult<SelectModelOutput> rst = new RequestPageResult<SelectModelOutput>();
            if (searchCondition == null) return rst;
            try
            {
                searchCondition.SearchCondition.IsOnlyActive = true;
                var datas = _repository.GetAllIncluding(video => video.VideoDev);
                var videoTerminalDatas = GetRepositoryData(datas, searchCondition).Select(item => item.VideoDev);
                videoTerminalDatas = videoTerminalDatas.OrderBy(v => v.SeqNo).ThenBy(v => v.DevName);
                rst.ResultDatas = videoTerminalDatas.Select(dev => new SelectModelOutput
                {
                    Key = dev.Id,
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

        private IQueryable<VideoDevEquipmentInfo> GetRepositoryData(IQueryable<VideoDevEquipmentInfo> datas, PageSearchCondition<VideoDevEquipmentInfoSearchConditionInput> searchCondition)
        {
            if (datas == null) return null;
            return datas
                    .WhereIf(searchCondition.SearchCondition.IsOnlyActive, dev => dev.IsActive)
                    .WhereIf(searchCondition.SearchCondition.TransformerSubstationId.HasValue, dev => dev.TransformerSubstationId == searchCondition.SearchCondition.TransformerSubstationId)
                    .WhereIf(searchCondition.SearchCondition.EquipmentInfoId.HasValue, dev => dev.EquipmentInfoId == searchCondition.SearchCondition.EquipmentInfoId)
                    .WhereIf(searchCondition.SearchCondition.VideoDevId.HasValue, dev => dev.VideoDevId == searchCondition.SearchCondition.VideoDevId)
                    .WhereIf(!searchCondition.SearchCondition.VideoDevName.IsNullOrWhiteSpace(), dev => dev.VideoDev.DevName.Contains(searchCondition.SearchCondition.VideoDevName))
                    .WhereIf(!searchCondition.SearchCondition.EquipmentInfoName.IsNullOrWhiteSpace(), dev => dev.EquipmentInfo.Name.Contains(searchCondition.SearchCondition.EquipmentInfoName));
        }

        #endregion 查
    }
}