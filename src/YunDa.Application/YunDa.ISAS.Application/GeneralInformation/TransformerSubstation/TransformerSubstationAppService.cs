using Abp.Domain.Repositories;
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
using YunDa.ISAS.DataTransferObject.GeneralInformation.TransformerSubstationDto;
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.Entities.GeneralInformation;

namespace YunDa.ISAS.Application.GeneralInformation
{
    public class TransformerSubstationAppService : ISASAppServiceBase, ITransformerSubstationAppService
    {
        private readonly IRepository<TransformerSubstation, Guid> _transformerSubstationRepository;
        private LoginUserOutput _currentUser = null;

        public TransformerSubstationAppService(IRepository<TransformerSubstation, Guid> transformerSubstationRepository, ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _transformerSubstationRepository = transformerSubstationRepository;
        }

        #region 增/改

        /// <summary>
        /// 增改线路数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<RequestResult<TransformerSubstationOutput>> CreateOrUpdateAsync(EditTransformerSubstationInput input)
        {
            if (input == null) return new RequestResult<TransformerSubstationOutput>();
            if (_currentUser == null)
                _currentUser = base.GetCurrentUser();
            RequestResult<TransformerSubstationOutput> rst;
            LoginUserOutput CurrentUser = base.GetCurrentUser();
            if (input.Id != null)
            {
                input.LastModificationTime = DateTime.Now;
                input.LastModifierUserId = CurrentUser.Id;
                rst = await this.UpdateAsync(input).ConfigureAwait(false);
            }
            else
            {
                rst = await this.CreateAsync(input).ConfigureAwait(false);
            }
            return rst;
        }

        private async Task<RequestResult<TransformerSubstationOutput>> UpdateAsync(EditTransformerSubstationInput input)
        {
            RequestResult<TransformerSubstationOutput> rst = new RequestResult<TransformerSubstationOutput>();
            try
            {
                var data = await _transformerSubstationRepository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = data.CreationTime;
                input.CreatorUserId = data.CreatorUserId;
                input.LastModificationTime = DateTime.Now;
                input.CreatorUserId = _currentUser.Id;
                ObjectMapper.Map(input, data);
                rst.Flag = true;
                rst.ResultData = ObjectMapper.Map<TransformerSubstationOutput>(data); ;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        private async Task<RequestResult<TransformerSubstationOutput>> CreateAsync(EditTransformerSubstationInput input)
        {
            RequestResult<TransformerSubstationOutput> rst = new RequestResult<TransformerSubstationOutput>();
            try
            {
                var data = ObjectMapper.Map<TransformerSubstation>(input);
                data = await _transformerSubstationRepository.InsertAsync(data).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<TransformerSubstationOutput>(data);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }

            return rst;
        }

        #endregion 增/改

        #region 删

        [HttpGet]
        public async Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                await _transformerSubstationRepository.DeleteAsync(item => item.Id == id).ConfigureAwait(false);
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
                await _transformerSubstationRepository.DeleteAsync(item => ids.Contains(item.Id)).ConfigureAwait(false); ;
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        #endregion 删

        #region 查

        [HttpPost]
        public RequestPageResult<TransformerSubstationOutput> FindDatas(PageSearchCondition<TransformerSubstationSearchConditionInput> searchCondition)
        {
            RequestPageResult<TransformerSubstationOutput> rst = new RequestPageResult<TransformerSubstationOutput>();
            if (searchCondition == null) return rst;
            try
            {
                var datas = searchCondition.SearchCondition.IsNeedChildren ? _transformerSubstationRepository.GetAllIncluding(Station => Station.VideoDevs) : _transformerSubstationRepository.GetAll();
                if (searchCondition.SearchCondition.IsOnlyActive)
                    datas = datas.Where(sub => sub.IsActive);
                datas = datas.WhereIf(searchCondition.SearchCondition.SubstationName != null, sub => sub.SubstationName.Contains(searchCondition.SearchCondition.SubstationName, StringComparison.OrdinalIgnoreCase));

                datas = datas.WhereIf(searchCondition.SearchCondition.PowerSupplyLineId != null, sub => (sub.PowerSupplyLineId ?? new Guid()).Equals(searchCondition.SearchCondition.PowerSupplyLineId));
                //获取条件下所有数量
                rst.TotalCount = datas.Count();
                //排序
                datas = !string.IsNullOrWhiteSpace(searchCondition.Sorting) ? datas.OrderBy(searchCondition.Sorting) : datas.OrderBy(sub => sub.SubstationName);
                //分页
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                rst.ResultDatas = ObjectMapper.Map<List<TransformerSubstationOutput>>(datas);
                rst.Flag = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                rst.Flag = false;
            }
            return rst;
        }

        #endregion 查
    }
}