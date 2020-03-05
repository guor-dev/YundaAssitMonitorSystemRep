namespace YunDa.ISAS.DataTransferObject
{
    public class PageSearchCondition<TSearchCondition> where TSearchCondition : new()
    {
        private int _pageIndex;
        private int _pageSize;
        private string _sorting;
        private TSearchCondition _SearchCondition;

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
        /// "Name"
        /// "Name DESC"
        /// "Name ASC, Age DESC"
        /// </summary>
        public string Sorting
        {
            get
            {
                return _sorting;
            }

            set
            {
                _sorting = value;
            }
        }

        public TSearchCondition SearchCondition
        {
            get
            {
                return _SearchCondition == null ? new TSearchCondition() : _SearchCondition;
            }

            set
            {
                _SearchCondition = value;
            }
        }
    }
}