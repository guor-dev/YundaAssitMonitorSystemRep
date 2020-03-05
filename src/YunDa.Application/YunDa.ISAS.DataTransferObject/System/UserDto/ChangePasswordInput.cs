using System;

namespace YunDa.ISAS.DataTransferObject.System.UserDto
{
    public class ChangePasswordInput
    {
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }
        public virtual string CurrentPassword { get; set; }
        public virtual string NewPassword { get; set; }
    }
}