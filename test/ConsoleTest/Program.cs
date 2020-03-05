using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using ToolLibrary;
using YunDa.ISAS.DataTransferObject;
using YunDa.ISAS.DataTransferObject.System.UserDto;

namespace ConsoleTest
{
    internal class Program
    {
        private static string url = "http://localhost:9090/";

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            JObject jo = new JObject();
            jo.Add("UserName", "admin");
            jo.Add("Password", "123qwe");
            JObject rstJObject = HttpHelper.HttpPostRequest<JObject>(url + "api/TokenAuth/Authenticate", jo);
            AuthenticateResultModel rstModel = JsonConvert.DeserializeObject<AuthenticateResultModel>(rstJObject["result"].ToString());
            Console.WriteLine("AccessToken:" + rstModel.AccessToken);
            PageSearchCondition<UserSearchConditionInput> searchCondition = new PageSearchCondition<UserSearchConditionInput>();
            searchCondition.PageIndex = 1;
            searchCondition.PageSize = 10;
            searchCondition.SearchCondition = new UserSearchConditionInput();
            searchCondition.SearchCondition.UserName = "";
            JObject rstJObject1 = HttpHelper.HttpPostRequest<JObject>(url + "api/services/isas/User/FindDatas", searchCondition, rstModel.AccessToken);
            Console.WriteLine("Result:" + rstJObject1);
            Console.ReadLine();
        }
    }

    public class AuthenticateResultModel
    {
        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public Guid UserId { get; set; }
    }
}