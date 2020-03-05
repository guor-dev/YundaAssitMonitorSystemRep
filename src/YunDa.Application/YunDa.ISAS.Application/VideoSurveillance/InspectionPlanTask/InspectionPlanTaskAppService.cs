using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.Application.Core.Session;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionCardDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionItemDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.InspectionPlanTaskDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.PresetPointDto;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.Application.VideoSurveillance
{
    public class InspectionPlanTaskAppService : ISASAppServiceBase, IInspectionPlanTaskAppService
    {
        private readonly IRepository<InspectionPlanTask, Guid> _inspectionPlanTaskRepository;
        private readonly IRepository<InspectionCard, Guid> _inspectionCardRepository;
        private readonly IRepository<InspectionItem, Guid> _inspectionItemRepository;
        private readonly IRepository<VideoDev, Guid> _videoDevRepository;
        private readonly IRepository<PresetPoint, Guid> _presetPointRepository;

        public InspectionPlanTaskAppService(IRepository<InspectionPlanTask, Guid> inspectionPlanTaskRepository
            , IRepository<InspectionCard, Guid> inspectionCardRepository
            , IRepository<InspectionItem, Guid> inspectionItemRepository
            , IRepository<VideoDev, Guid> videoDevRepository
            , IRepository<PresetPoint, Guid> presetPointRepository
            , ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _inspectionPlanTaskRepository = inspectionPlanTaskRepository;
            _inspectionCardRepository = inspectionCardRepository;
            _inspectionItemRepository = inspectionItemRepository;
            _videoDevRepository = videoDevRepository;
            _presetPointRepository = presetPointRepository;
        }

        #region 添加或更新变电所视频巡检任务清单

        [HttpPost]
        public async Task<RequestResult<InspectionPlanTaskOutput>> CreateOrUpdateAsync(EditInspectionPlanTaskInput input)
        {
            if (input == null) return new RequestResult<InspectionPlanTaskOutput>();
            RequestResult<InspectionPlanTaskOutput> rst;
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

        private async Task<RequestResult<InspectionPlanTaskOutput>> UpdateAsync(EditInspectionPlanTaskInput input)
        {
            RequestResult<InspectionPlanTaskOutput> rst = new RequestResult<InspectionPlanTaskOutput>();
            try
            {
                var data = await _inspectionPlanTaskRepository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                ObjectMapper.Map(input, data);
                rst.Flag = true;
                rst.ResultData = ObjectMapper.Map<InspectionPlanTaskOutput>(data); ;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        private async Task<RequestResult<InspectionPlanTaskOutput>> CreateAsync(EditInspectionPlanTaskInput input)
        {
            RequestResult<InspectionPlanTaskOutput> rst = new RequestResult<InspectionPlanTaskOutput>();
            try
            {
                var data = ObjectMapper.Map<InspectionPlanTask>(input);
                data = await _inspectionPlanTaskRepository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<InspectionPlanTaskOutput>(data);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }

            return rst;
        }

        #endregion 添加或更新变电所视频巡检任务清单

        [HttpGet]
        public async Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                await _inspectionPlanTaskRepository.DeleteAsync(item => item.Id == id).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        [HttpPost]
        public async Task<RequestEasyResult> DeleteByIdsAsync(List<Guid> ids)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                await _inspectionPlanTaskRepository.DeleteAsync(item => ids.Contains(item.Id)).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        [HttpPost]
        public RequestPageResult<InspectionPlanTaskOutput> FindDatas(PageSearchCondition<InspectionPlanTaskSearchConditionInput> searchCondition)
        {
            RequestPageResult<InspectionPlanTaskOutput> rst = new RequestPageResult<InspectionPlanTaskOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var datas = searchCondition.SearchCondition.IsNeedParent ? _inspectionPlanTaskRepository.GetAllIncluding(item => item.InspectionCard) : _inspectionPlanTaskRepository.GetAll();
                if (searchCondition.SearchCondition.IsOnlyActive)
                    datas = datas.Where(ter => ter.IsActive);
                datas = datas
                    .WhereIf(!searchCondition.SearchCondition.PlanTaskName.IsNullOrEmpty(), item => item.PlanTaskName.Contains(searchCondition.SearchCondition.PlanTaskName, StringComparison.Ordinal))
                    .WhereIf(searchCondition.SearchCondition.InspectionCardId.HasValue, item => item.InspectionCardId == searchCondition.SearchCondition.InspectionCardId);
                //获取条件下所有数量
                rst.TotalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting).ThenBy(item => item.ExecutionWeek).ThenBy(item => item.ExecutionTime) : datas.OrderBy(item => item.SeqNo).ThenBy(item => item.ExecutionWeek).ThenBy(item => item.ExecutionTime);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                rst.ResultDatas = ObjectMapper.Map<List<InspectionPlanTaskOutput>>(datas);
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
        public RequestPageResult<InspectionPlanTaskInfoOutput> GetInspectionPlanTaskInfosBySubId(Guid? subId)
        {
            RequestPageResult<InspectionPlanTaskInfoOutput> rst = new RequestPageResult<InspectionPlanTaskInfoOutput>();
            try
            {
                var nvrs = _videoDevRepository.GetAll().Where(n => n.IsActive && n.DevType == VideoDevTypeEnum.硬盘录像机)
                    .WhereIf(subId.HasValue, c => c.TransformerSubstationId == subId);
                var terminals = _videoDevRepository.GetAll().Where(ter => ter.IsActive && ter.DevType != VideoDevTypeEnum.硬盘录像机)
                    .Join(nvrs, ter => ter.VideoDevId, nvr => nvr.Id, (ter, nvr) => new VideoTerminalProperty
                    {
                        Id = ter.Id,
                        SeqNo = ter.SeqNo,
                        DevName = ter.DevName,
                        DevType = ter.DevType,
                        ManufacturerInfoId = ter.ManufacturerInfoId,
                        Remark = ter.Remark,
                        ChannelNo = ter.ChannelNo,
                        DevNo = ter.DevNo,
                        IsPTZ = ter.IsPTZ,
                        IsActive = ter.IsActive,
                        VideoNVR = new VideoNVRProperty
                        {
                            Id = nvr.Id,
                            SeqNo = nvr.SeqNo,
                            DevName = nvr.DevName,
                            DevType = nvr.DevType,
                            ManufacturerInfoId = nvr.ManufacturerInfoId,
                            Remark = nvr.Remark,
                            IP = nvr.IP,
                            Port = nvr.Port,
                            DevUserName = nvr.DevUserName,
                            DevPassword = nvr.DevPassword,
                            IsActive = nvr.IsActive,
                            TransformerSubstationId = nvr.TransformerSubstationId
                        }
                    });
                var points = _presetPointRepository.GetAll().Where(c => c.IsActive)
                    .Select(p => new PresetPointProperty
                    {
                        Id = p.Id,
                        Number = p.Number,
                        Name = p.Name,
                        IsActive = p.IsActive,
                        VideoDevId = p.VideoDevId,
                    });
                var inspectionItems = _inspectionItemRepository.GetAll().Where(c => c.IsActive)
                    .Join(terminals, item => item.VideoDevId, ter => ter.Id, (item, ter) => new
                    {
                        item.Id,
                        item.SeqNo,
                        item.ItemName,
                        item.ProcessAction,
                        item.ProcessDuration,
                        item.IsActive,
                        item.Remark,
                        item.PresetPointId,
                        item.InspectionCardId,
                        VideoTerminal = ter
                    }).GroupJoin(points, item => item.PresetPointId, p => p.Id, (item, p) => new InspectionItemProperty
                    {
                        Id = item.Id,
                        SeqNo = item.SeqNo,
                        ItemName = item.ItemName,
                        ProcessAction = item.ProcessAction,
                        ProcessDuration = item.ProcessDuration,
                        IsActive = item.IsActive,
                        Remark = item.Remark,
                        InspectionCardId = item.InspectionCardId,
                        VideoTerminal = item.VideoTerminal,
                        PresetPoint = p.FirstOrDefault(),
                    });

                var cards = _inspectionCardRepository.GetAll().Where(c => c.IsActive)
                    .WhereIf(subId.HasValue, c => c.TransformerSubstationId == subId)
                    .GroupJoin(inspectionItems, c => c.Id, item => item.InspectionCardId, (c, items) => new
                    {
                        c.Id,
                        c.CardName,
                        c.Remark,
                        c.IsActive,
                        c.TransformerSubstationId,
                        InspectionItems = items
                    });

                var pTasks = _inspectionPlanTaskRepository.GetAll().Where(c => c.IsActive)
                    .Join(cards, pt => pt.InspectionCardId, c => c.Id, (pt, c) => new InspectionPlanTaskInfoOutput
                    {
                        Id = pt.Id,
                        SeqNo = pt.SeqNo,
                        ExecutionWeek = pt.ExecutionWeek,
                        ExecutionTime = pt.ExecutionTime,
                        IsActive = pt.IsActive,
                        Remark = pt.Remark,
                        InspectionCard = new InspectionCardProperty
                        {
                            Id = c.Id,
                            CardName = c.CardName,
                            Remark = c.Remark,
                            IsActive = c.IsActive,
                            TransformerSubstationId = c.TransformerSubstationId
                        },
                        InspectionItems = c.InspectionItems.OrderBy(item => item.SeqNo)
                    });
#if DEBUG
                //Console.WriteLine(pTasks.ToString());
#endif
                rst.ResultDatas = pTasks.ToList();
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        /// <summary>复制删除数据
        /// </summary>
        /// <param name="ids">ID列表</param>
        /// <returns>返回是否复制成功</returns>
        public async Task<RequestEasyResult> CopyTaskByIdsAsync(List<Guid> ids)
        {
            RequestEasyResult rst = new RequestEasyResult { Flag = false };
            if (ids == null || ids.Count == 0) return rst;
            try
            {
                var baseDatas = _inspectionPlanTaskRepository.GetAll();
                var conditionDatas = baseDatas.Where(item => ids.Contains(item.Id)).ToList().Distinct(new PlanTaskByExecutionTimeEqualityComparer());
                var datas = baseDatas.Where(t => conditionDatas.Contains(t, new PlanTaskByExecutionTimeEqualityComparer())).ToList();
                List<InspectionPlanTask> addTasks = new List<InspectionPlanTask>();
                InspectionPlanTask addTask = null;
                Array weeks = Enum.GetValues(typeof(WeekEnum));
                LoginUserOutput CurrentUser = base.GetCurrentUser();
                foreach (var task in conditionDatas)
                {
                    foreach (WeekEnum v in weeks)
                    {
                        if (task.ExecutionWeek == v) continue;
                        addTask = new InspectionPlanTask
                        {
                            SeqNo = 0,
                            PlanTaskName = task.PlanTaskName,
                            ExecutionWeek = v,
                            ExecutionTime = task.ExecutionTime,
                            IsActive = task.IsActive,
                            Remark = task.Remark,
                            InspectionCardId = task.InspectionCardId,
                            CreationTime = DateTime.Now,
                            CreatorUserId = CurrentUser.Id
                        };
                        var temp = datas.FirstOrDefault(t => t.InspectionCardId == addTask.InspectionCardId && t.ExecutionWeek == addTask.ExecutionWeek && t.ExecutionTime == addTask.ExecutionTime);
                        var temp_2 = addTasks.FirstOrDefault(t => t.InspectionCardId == addTask.InspectionCardId && t.ExecutionWeek == addTask.ExecutionWeek && t.ExecutionTime == addTask.ExecutionTime);
                        if (temp == null && temp_2 == null)
                            addTasks.Add(addTask);
                    }
                }
                if (addTasks.Count > 0)
                {
                    foreach (var task in addTasks)
                    {
                        await _inspectionPlanTaskRepository.InsertAsync(task);
                    }
                }
                rst.Flag = true;
            }
            catch { }
            return rst;
        }
    }

    public class PlanTaskByExecutionTimeEqualityComparer : IEqualityComparer<InspectionPlanTask>
    {
        public bool Equals([AllowNull] InspectionPlanTask x, [AllowNull] InspectionPlanTask y)
        {
            return x.InspectionCardId == y.InspectionCardId && x.ExecutionTime == y.ExecutionTime;
        }

        public int GetHashCode([DisallowNull] InspectionPlanTask obj)
        {
            return obj.GetHashCode();
        }
    }
}