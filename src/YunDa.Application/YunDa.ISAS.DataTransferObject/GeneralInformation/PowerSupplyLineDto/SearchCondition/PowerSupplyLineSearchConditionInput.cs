namespace YunDa.ISAS.DataTransferObject.GeneralInformation.PowerSupplyLineDto
{
    public class PowerSupplyLineSearchConditionInput
    {
        public virtual string LineName { get; set; }

        /// <summary>
        /// 是否需要查找子元素
        /// </summary>
        public virtual bool IsNeedChildren { get; set; }

        /// <summary>
        /// 是否只查找活动的线路
        /// </summary>
        public virtual bool IsOnlyActive { get; set; }
    }
}