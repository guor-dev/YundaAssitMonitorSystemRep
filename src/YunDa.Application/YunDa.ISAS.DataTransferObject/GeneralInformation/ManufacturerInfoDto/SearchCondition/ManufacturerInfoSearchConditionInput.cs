namespace YunDa.ISAS.DataTransferObject.GeneralInformation.ManufacturerInfoDto
{
    public class ManufacturerInfoSearchConditionInput
    {
        public virtual string ManufacturerName { get; set; }

        /// <summary>
        /// 是否只查找活动的线路
        /// </summary>
        public virtual bool IsOnlyActive { get; set; }

        /// <summary>
        /// 是否查找已经删除的数据
        /// </summary>
        public virtual bool IsOnlyDeleted { get; set; }
    }
}