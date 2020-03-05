namespace YunDa.ISAS.DataTransferObject
{
    public class RequestEasyResult
    {
        private bool _flag;
        private string _msg;

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
                return string.IsNullOrWhiteSpace(_msg) ? (this._flag ? ResultMsgConst.OperateSuccess : ResultMsgConst.OperateFail) : _msg;
            }

            set
            {
                _msg = value;
            }
        }
    }
}