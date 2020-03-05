using System;

namespace YunDa.ISAS.Web.Core.Models
{
    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public Guid UserId { get; set; }
    }
}