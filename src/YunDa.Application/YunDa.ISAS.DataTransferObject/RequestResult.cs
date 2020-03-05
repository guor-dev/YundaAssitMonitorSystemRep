namespace YunDa.ISAS.DataTransferObject
{
    public class RequestResult<T>
    {
        private int _totalCount;
        private T _resultData;
        private bool _flag;
        private string _msg;

        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount
        {
            get
            {
                return _totalCount;
            }

            set
            {
                _totalCount = value;
            }
        }

        /// <summary>
        /// 结果集
        /// </summary>
        public T ResultData
        {
            get
            {
                return _resultData;
            }

            set
            {
                _resultData = value;
            }
        }

        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool Flag
        {
            get
            {
                return _flag;
            }

            set
            {
                _flag = value;
            }
        }

        /// <summary>
        /// 操作结果信息
        /// </summary>
        public string Message
        {
            get
            {
                return string.IsNullOrWhiteSpace(this._msg) ? (this._flag ? ResultMsgConst.OperateSuccess : ResultMsgConst.OperateFail) : this._msg;
            }

            set
            {
                _msg = value;
            }
        }
    }
}