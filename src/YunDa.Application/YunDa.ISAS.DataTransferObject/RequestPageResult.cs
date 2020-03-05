using System.Collections.Generic;

namespace YunDa.ISAS.DataTransferObject
{
    public class RequestPageResult<T>
    {
        private int _totalCount;
        private int _pageIndex;
        private int _pageSize;
        private List<T> _resultDatas;
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
        /// 当前页码
        /// </summary>
        public int PageIndex
        {
            get
            {
                return _pageIndex;
            }

            set
            {
                _pageIndex = value;
            }
        }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize
        {
            get
            {
                return _pageSize;
            }

            set
            {
                _pageSize = value;
            }
        }

        /// <summary>
        /// 结果集
        /// </summary>
        public List<T> ResultDatas
        {
            get
            {
                return _resultDatas;
            }

            set
            {
                _resultDatas = value;
            }
        }

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