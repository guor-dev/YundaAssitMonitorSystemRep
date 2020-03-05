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
using YunDa.ISAS.DataTransferObject.GeneralInformation.PowerSupplyLineDto;
using YunDa.ISAS.DataTransferObject.GeneralInformation.TransformerSubstationDto;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.DataTransferObject.VideoSurveillance.VideoDevDto;
using YunDa.ISAS.Entities.GeneralInformation;
using YunDa.ISAS.Entities.VideoSurveillance;

namespace YunDa.ISAS.Application.GeneralInformation
{
    public class PowerSupplyLineAppService : ISASAppServiceBase, IPowerSupplyLineAppService
    {
        private readonly IRepository<PowerSupplyLine, Guid> _powerSupplyLineRepository;
        private readonly IRepository<TransformerSubstation, Guid> _transformerSubstationRepository;
        private readonly IRepository<VideoDev, Guid> _videoDevRepository;
        private LoginUserOutput _currentUser = null;

        public PowerSupplyLineAppService(
            IRepository<PowerSupplyLine, Guid> powerSupplyLineRepository,
            IRepository<TransformerSubstation, Guid> transformerSubstationRepository,
            IRepository<VideoDev, Guid> videoDevRepository,
            ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _powerSupplyLineRepository = powerSupplyLineRepository;
            _transformerSubstationRepository = transformerSubstationRepository;
            _videoDevRepository = videoDevRepository;
            //_currentUser = base.GetCurrentUser();
        }

        #region 增/改

        /// <summary>
        /// 增改线路数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RequestResult<PowerSupplyLineOutput>> CreateOrUpdateAsync(EditPowerSupplyLineInput input)
        {
            if (input == null) return null;
            if (_currentUser == null)
                _currentUser = base.GetCurrentUser();
            RequestResult<PowerSupplyLineOutput> rst;
            if (input.Id != null)
            {
                rst = await this.UpdateAsync(input).ConfigureAwait(false);
            }
            else
            {
                rst = await this.CreateAsync(input).ConfigureAwait(false);
            }
            return rst;
        }

        private async Task<RequestResult<PowerSupplyLineOutput>> UpdateAsync(EditPowerSupplyLineInput input)
        {
            RequestResult<PowerSupplyLineOutput> rst = new RequestResult<PowerSupplyLineOutput>();
            try
            {
                var data = await _powerSupplyLineRepository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                input.LastModificationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                ObjectMapper.Map(input, data);
                rst.Flag = true;
                rst.ResultData = ObjectMapper.Map<PowerSupplyLineOutput>(data); ;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        private async Task<RequestResult<PowerSupplyLineOutput>> CreateAsync(EditPowerSupplyLineInput input)
        {
            RequestResult<PowerSupplyLineOutput> rst = new RequestResult<PowerSupplyLineOutput>();
            try
            {
                input.LastModificationTime = DateTime.Now;
                input.LastModifierUserId = _currentUser.Id;
                var data = ObjectMapper.Map<PowerSupplyLine>(input);

                data = await _powerSupplyLineRepository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<PowerSupplyLineOutput>(data);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }

            return rst;
        }

        #endregion 增/改

        #region 查

        [AbpAllowAnonymous]
        [HttpPost]
        public RequestPageResult<PowerSupplyLineOutput> GetPowerSupplyLinesAndChildrenAsync(PageSearchCondition<PowerSupplyLineSearchConditionInput> searchCondition)
        {
            RequestPageResult<PowerSupplyLineOutput> rst = new RequestPageResult<PowerSupplyLineOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var datas = _powerSupplyLineRepository.GetAllIncluding(line => line.TransformerSubstations)
                    .WhereIf(!searchCondition.SearchCondition.LineName.IsNullOrEmpty(), line => line.LineName.Contains(searchCondition.SearchCondition.LineName, StringComparison.Ordinal))
                    .Where(line => line.IsActive);
                rst.TotalCount = datas.Count();
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas.PageBy(skipCount, searchCondition.PageSize);

                var devs = _videoDevRepository.GetAll().Where(dev => dev.IsActive);
                var tempDevs = _videoDevRepository.GetAll().Where(dev => dev.IsActive);
                IQueryable<VideoDevOutput> devOuputs = devs.GroupJoin(tempDevs, pDev => pDev.Id, cDev => cDev.VideoDevId, (pDev, cDev) => new VideoDevOutput
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
                    DevNo = pDev.DevNo,
                    IsPTZ = pDev.IsPTZ,
                    IsActive = pDev.IsActive,
                    TransformerSubstationId = pDev.TransformerSubstationId,
                    VideoDevId = pDev.VideoDevId,
                    Children = ObjectMapper.Map<List<VideoDevOutput>>(cDev.OrderBy(devOuput => devOuput.SeqNo))
                }).OrderBy(devOuput => devOuput.SeqNo).AsQueryable();

                var subs = _transformerSubstationRepository.GetAll().Where(dev => dev.IsActive);
                IQueryable<TransformerSubstationOutput> subOuputs = subs.GroupJoin(devOuputs, sub => sub.Id, dev => dev.TransformerSubstationId, (sub, dev) => new TransformerSubstationOutput
                {
                    Id = sub.Id,
                    SubstationName = sub.SubstationName,
                    CommMgrIP = sub.CommMgrIP,
                    Remark = sub.Remark,
                    IsActive = sub.IsActive,
                    PowerSupplyLineId = sub.PowerSupplyLineId,
                    VideoDevs = dev
                }).AsQueryable();

                List<PowerSupplyLineOutput> rstDatas = datas.GroupJoin(subOuputs, line => line.Id, sub => sub.PowerSupplyLineId, (line, sub) => new PowerSupplyLineOutput
                {
                    Id = line.Id,
                    LineName = line.LineName,
                    Remark = line.Remark,
                    IsActive = line.IsActive,
                    TransformerSubstations = sub
                }).ToList();
#if DEBUG
                //Console.WriteLine("【GetPowerSupplyLinesAndChildrenAsync】SQL："+ datas.ToString());
#endif
                rst.ResultDatas = rstDatas;
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        /// <summary>
        /// 获取线路数据
        /// </summary>
        /// <param name="searchCondition"></param>
        /// <returns></returns>
        [AbpAllowAnonymous]
        [HttpPost]
        public RequestPageResult<PowerSupplyLineOutput> FindDatas(PageSearchCondition<PowerSupplyLineSearchConditionInput> searchCondition)
        {
            RequestPageResult<PowerSupplyLineOutput> rst = new RequestPageResult<PowerSupplyLineOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var datas = searchCondition.SearchCondition.IsNeedChildren ? _powerSupplyLineRepository.GetAllIncluding(line => line.TransformerSubstations) : _powerSupplyLineRepository.GetAll();
                if (searchCondition.SearchCondition.IsOnlyActive)
                    datas = datas.Where(line => line.IsActive);
                datas = datas.WhereIf(!searchCondition.SearchCondition.LineName.IsNullOrEmpty(), line => line.LineName.Contains(searchCondition.SearchCondition.LineName));
                //获取条件下所有数量
                rst.TotalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting) : datas.OrderBy(line => line.LineName);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                rst.ResultDatas = ObjectMapper.Map<List<PowerSupplyLineOutput>>(datas);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        #endregion 查

        #region 删

        [HttpGet]
        public async Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                await _powerSupplyLineRepository.DeleteAsync(item => item.Id == id).ConfigureAwait(false);
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
                await _powerSupplyLineRepository.DeleteAsync(item => ids.Contains(item.Id)).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        #endregion 删
    }
}