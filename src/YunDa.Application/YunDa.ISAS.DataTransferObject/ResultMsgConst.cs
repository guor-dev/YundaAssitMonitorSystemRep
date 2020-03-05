namespace YunDa.ISAS.DataTransferObject
{
    public static class ResultMsgConst
    {
        #region 用户登录

        public const string PasswordIsNull = "用户名或密码不能为空！";
        public const string UserNameOrPasswordIsError = "用户名或密码不能为空！";
        public const string LoginSuccess = "用户名或密码不能为空！";

        #endregion 用户登录

        #region 修改密码

        public const string OldAndNewPasswordIsNull = "新/旧密码不能为空！";
        public const string CurrentUserPasswordIsError = "当前用户密码错误！";

        #endregion 修改密码

        public const string OperateSuccess = "操作成功！";
        public const string OperateFail = "操作失败！";

        public const string OperateFail_ExistName = "名称已存在，编辑失败！";
    }
}