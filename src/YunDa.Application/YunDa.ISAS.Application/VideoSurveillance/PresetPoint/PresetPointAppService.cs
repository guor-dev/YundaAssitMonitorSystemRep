using Abp.Authorization;
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
using YunDa.ISAS.DataTransferObject.VideoSurveillance.PresetPointDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    public class PresetPointAppService : ISASAppServiceBase, IPresetPointAppService
    {
        private readonly IRepository<PresetPoint, Guid> _presetPointRepository;

        public PresetPointAppService(IRepository<PresetPoint, Guid> presetPointRepository, ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _presetPointRepository = presetPointRepository;
        }

        #region 添加或更新变电所视频巡检任务清单

        [AbpAllowAnonymous]
        [HttpPost]
        public async Task<RequestResult<PresetPointOutput>> CreateOrUpdateAsync(EditPresetPointInput input)
        {
            if (input == null) return null;
            RequestResult<PresetPointOutput> rst;
            LoginUserOutput CurrentUser = base.GetCurrentUser();
            if (input.Id != null)
            {
                input.LastModificationTime = DateTime.Now;
                input.LastModifierUserId = CurrentUser.Id;
                rst = await this.UpdateAsync(input).ConfigureAwait(false);
            }
            else
            {
                input.CreationTime = DateTime.Now;
                input.CreatorUserId = CurrentUser.Id;
                rst = await this.CreateAsync(input).ConfigureAwait(false);
            }
            return rst;
        }

        private async Task<RequestResult<PresetPointOutput>> UpdateAsync(EditPresetPointInput input)
        {
            RequestResult<PresetPointOutput> rst = new RequestResult<PresetPointOutput>();
            try
            {
                var data = await _presetPointRepository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                ObjectMapper.Map(input, data);
                rst.Flag = true;
                rst.ResultData = ObjectMapper.Map<PresetPointOutput>(data); ;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        private async Task<RequestResult<PresetPointOutput>> CreateAsync(EditPresetPointInput input)
        {
            RequestResult<PresetPointOutput> rst = new RequestResult<PresetPointOutput>();
            try
            {
                var data = ObjectMapper.Map<PresetPoint>(input);
                data = await _presetPointRepository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<PresetPointOutput>(data);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }

            return rst;
        }

        #endregion 添加或更新变电所视频巡检任务清单

        [AbpAllowAnonymous]
        [HttpGet]
        public async Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                await _presetPointRepository.DeleteAsync(item => item.Id == id).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        [AbpAllowAnonymous]
        [HttpPost]
        public async Task<RequestEasyResult> DeleteByIdsAsync(List<Guid> ids)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                await _presetPointRepository.DeleteAsync(item => ids.Contains(item.Id)).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        [AbpAllowAnonymous]
        [HttpPost]
        public RequestPageResult<PresetPointOutput> FindDatas(PageSearchCondition<PresetPointSearchCondition> searchCondition)
        {
            RequestPageResult<PresetPointOutput> rst = new RequestPageResult<PresetPointOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var datas = _presetPointRepository.GetAll();
                if (searchCondition.SearchCondition.IsOnlyActive)
                    datas = datas.Where(ter => ter.IsActive);
                datas = datas
                    .WhereIf(!searchCondition.SearchCondition.Name.IsNullOrEmpty(), p => p.Name.Contains(searchCondition.SearchCondition.Name, StringComparison.Ordinal))
                    .WhereIf(searchCondition.SearchCondition.VideoDevId.HasValue, p => p.VideoDevId == searchCondition.SearchCondition.VideoDevId)
                    .WhereIf(searchCondition.SearchCondition.Number != 0, p => p.Number == searchCondition.SearchCondition.Number);
                //获取条件下所有数量
                rst.TotalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting) : datas.OrderBy(ter => ter.Name);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != 0)
                    datas.PageBy(skipCount, searchCondition.PageSize);
                rst.ResultDatas = ObjectMapper.Map<List<PresetPointOutput>>(datas);
                rst.Flag = true;
                rst.Message = "操作成功";
            }
            catch (Exception ex)
            {
                rst.Flag = false;
                rst.Message = ex.ToString();
            }
            return rst;
        }

        public RequestPageResult<SelectModelOutput> FindPresetPointsForSelect(PageSearchCondition<PresetPointSearchCondition> searchCondition)
        {
            RequestPageResult<SelectModelOutput> rst = new RequestPageResult<SelectModelOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var datas = _presetPointRepository.GetAll();
                if (searchCondition.SearchCondition.IsOnlyActive)
                    datas = datas.Where(ter => ter.IsActive);
                datas = datas
                    .WhereIf(!searchCondition.SearchCondition.Name.IsNullOrEmpty(), p => p.Name.Contains(searchCondition.SearchCondition.Name, StringComparison.Ordinal))
                    .WhereIf(searchCondition.SearchCondition.VideoDevId.HasValue, p => p.VideoDevId == searchCondition.SearchCondition.VideoDevId)
                    .WhereIf(searchCondition.SearchCondition.Number != 0, p => p.Number == searchCondition.SearchCondition.Number);
                //获取条件下所有数量
                //rst.TotalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting) : datas.OrderBy(ter => ter.Name);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != 0)
                    datas.PageBy(skipCount, searchCondition.PageSize);
                rst.ResultDatas = datas.Select(p => new SelectModelOutput
                {
                    Key = p.Id,
                    Text = p.Name,
                    Value = p.Id.ToString().ToLower()
                }).ToList();
                rst.Flag = true;
                rst.Message = "操作成功";
            }
            catch (Exception ex)
            {
                rst.Flag = false;
                rst.Message = ex.ToString();
            }
            return rst;
        }
    }
}