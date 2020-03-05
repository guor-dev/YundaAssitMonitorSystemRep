using System;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.CommonDto;
using YunDa.ISAS.DataTransferObject.System.FunctionDto;

namespace YunDa.ISAS.Application.System
{
    public interface IFunctionAppService : IAppServiceBase<FunctionSearchConditionInput, FunctionOutput, EditFunctionInput, Guid>
    {
        /// <summary>
        /// 获取可执行的巡视动作下拉框数据
        /// </summary>
        /// <returns>返回满足条件的巡视动作</returns>
        RequestPageResult<TreeModelOutput> FindFunctionTypeForTree();

        /// <summary>
        /// 获取供选择的所有功能类别
        /// </summary>
        /// <returns>返回所有功能类别</returns>
        RequestPageResult<SelectModelOutput> FindTypesForSelect();
    }
}