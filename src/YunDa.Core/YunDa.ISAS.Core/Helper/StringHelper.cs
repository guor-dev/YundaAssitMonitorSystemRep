using System;
using System.Security.Cryptography;
using System.Text;

namespace YunDa.ISAS.Core.Helper
{
    public static class StringHelper
    {
        public static string MD5Encrypt64(string password)
        {
            string cl = password;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }

        /// <summary>
        /// 根据GUID获取16位的唯一字符串
        /// </summary>
        /// <param name=\"guid\"></param>
        /// <returns></returns>
        public static string GuidTo16String()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 根据GUID获取19位的唯一数字序列
        /// </summary>
        /// <returns></returns>
        public static long GuidToLongID(Guid g)
        {
            byte[] buffer = g.ToByteArray();
            return BitConverter.ToInt64(buffer, 0);
        }
    }
}