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
using YunDa.ISAS.DataTransferObject.Session;
using YunDa.ISAS.DataTransferObject.System.UserDto;
using YunDa.ISAS.Entities.System;

namespace YunDa.ISAS.Application.System
{
    public class UserAppService : ISASAppServiceBase, IUserAppService
    {
        private readonly IRepository<SysUser, Guid> _sysUserRepository;

        public UserAppService(IRepository<SysUser, Guid> sysUserRepository, ISessionAppService sessionAppService) :
              base(sessionAppService)
        {
            _sysUserRepository = sysUserRepository;
        }

        [HttpPost]
        public virtual RequestPageResult<UserOutput> FindDatas([FromBody]PageSearchCondition<UserSearchConditionInput> searchCondition)
        {
            RequestPageResult<UserOutput> rst = new RequestPageResult<UserOutput>();
            if (searchCondition == null) return rst;
            try
            {
                int skipCount = searchCondition.PageIndex <= 0 ? -1 : ((searchCondition.PageIndex - 1) * searchCondition.PageSize);
                var datas = _sysUserRepository.GetAllIncluding()
                       .WhereIf(!searchCondition.SearchCondition.UserName.IsNullOrEmpty(), u => u.UserName.Contains(searchCondition.SearchCondition.UserName, StringComparison.Ordinal));
                if (searchCondition.SearchCondition.IsOnlyActive)
                    datas = datas.Where(u => u.IsActive);
                rst.TotalCount = datas.Count();
                if (skipCount != -1)
                    datas = datas.PageBy(skipCount, searchCondition.PageSize);
                if (!string.IsNullOrWhiteSpace(searchCondition.Sorting))
                    datas = datas.OrderBy(searchCondition.Sorting);
                else
                    datas = datas.OrderBy(u => u.UserName);
                rst.ResultDatas = ObjectMapper.Map<List<UserOutput>>(datas);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        [HttpPost]
        public async Task<RequestResult<UserOutput>> CreateOrUpdateAsync(EditUserInput input)
        {
            if (input == null) return new RequestResult<UserOutput>();
            RequestResult<UserOutput> rst;
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

        [HttpPost]
        public async Task<RequestEasyResult> DeleteByIdsAsync(List<Guid> ids)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                await _sysUserRepository.DeleteAsync(u => ids.Contains(u.Id)).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        [HttpGet]
        public async Task<RequestEasyResult> DeleteByIdAsync(Guid id)
        {
            RequestEasyResult rst = new RequestEasyResult();
            try
            {
                await _sysUserRepository.DeleteAsync(u => u.Id == id).ConfigureAwait(false);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        private async Task<RequestResult<UserOutput>> UpdateAsync(EditUserInput input)
        {
            RequestResult<UserOutput> rst = new RequestResult<UserOutput>();
            try
            {
                var user = await _sysUserRepository.FirstOrDefaultAsync(u => u.Id == input.Id).ConfigureAwait(false);
                input.CreationTime = user.CreationTime;
                input.CreatorUserId = user.CreatorUserId;
                ObjectMapper.Map(input, user);
                rst.Flag = true;
                rst.ResultData = ObjectMapper.Map<UserOutput>(user); ;
            }
            catch
            {
                rst.Flag = false;
            }
            return rst;
        }

        private async Task<RequestResult<UserOutput>> CreateAsync(EditUserInput input)
        {
            RequestResult<UserOutput> rst = new RequestResult<UserOutput>();
            try
            {
                var sysUser = ObjectMapper.Map<SysUser>(input);
                sysUser = await _sysUserRepository.InsertAsync(sysUser).ConfigureAwait(false);
                rst.ResultData = ObjectMapper.Map<UserOutput>(sysUser);
                rst.Flag = true;
            }
            catch
            {
                rst.Flag = false;
            }

            return rst;
        }
    }
}