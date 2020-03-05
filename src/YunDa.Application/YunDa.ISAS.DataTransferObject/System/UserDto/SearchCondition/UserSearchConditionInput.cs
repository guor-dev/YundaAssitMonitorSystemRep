namespace YunDa.ISAS.DataTransferObject.System.UserDto
{
    public class UserSearchConditionInput
    {
        public virtual string UserName { get; set; }

        /// <summary>
        /// 是否只查找活动的线路
        /// </summary>
        public virtual bool IsOnlyActive { get; set; }
    }
}