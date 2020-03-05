using System;
using YunDa.ISAS.Application.Core;
using YunDa.ISAS.DataTransferObject.GeneralInformation.TransformerSubstationDto;

namespace YunDa.ISAS.Application.GeneralInformation
{
    public interface ITransformerSubstationAppService : IAppServiceBase<TransformerSubstationSearchConditionInput, TransformerSubstationOutput, EditTransformerSubstationInput, Guid>
    {
    }
}