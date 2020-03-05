using Abp.Application.Services;
using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using YunDa.ISAS.DataTransferObject;

namespace YunDa.ISAS.Application.Core
{
    /// <summary>
    /// 基础接口服务
    /// </summary>
    /// <typeparam name="TSearchConditionInput">查找条件</typeparam>
    /// <typeparam name="TResultOuput">结果返回值</typeparam>
    /// <typeparam name="TEditInput">添加或修改输入值</typeparam>
    /// <typeparam name="TDelPrimaryKey">数据主键</typeparam>
    public interface IAppServiceBase<TSearchConditionInput, TResultOuput, TEditInput, TDelPrimaryKey> : IApplicationService
        where TSearchConditionInput : new()
        where TResultOuput : EntityDto<Guid>
        where TEditInput : EntityDto<Guid?>
    {
        /// <summary>
        /// 按条件查找
        /// </summary>
        /// <param name="searchCondition">查询条件</param>
        /// <returns>返回满足条件的结果</returns>
        RequestPageResult<TResultOuput> FindDatas(PageSearchCondition<TSearchConditionInput> searchCondition);

        /// <summary>
        /// 添加或更新数据
        /// </summary>
        /// <param name="input">输入任务单</param>
        /// <returns>返回是否成功，并返回更新或添加后的数据</returns>
        Task<RequestResult<TResultOuput>> CreateOrUpdateAsync(TEditInput input);

        /// <summary>
        /// 根据ID列表删除数据
        /// </summary>
        /// <param name="ids">ID列表</param>
        /// <returns>返回是否删除成功</returns>
        Task<RequestEasyResult> DeleteByIdsAsync(List<TDelPrimaryKey> ids);

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>返回是否删除成功</returns>
        Task<RequestEasyResult> DeleteByIdAsync(TDelPrimaryKey id);
    }
}