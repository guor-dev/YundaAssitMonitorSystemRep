using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestConsoleApp1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Console.WriteLine(Program.MD5Encrypt64("123qwe"));
            List<TaskDemo> tasks = new List<TaskDemo>();
            string exWeek = DateTime.Now.DayOfWeek.ToString();
            string exTime = DateTime.Now.ToString("HH:mm");
            var v = tasks.Where(t => t.ExTime == exTime && t.ExWeek == exWeek).FirstOrDefault();
            if (v != null)
            {
                //TODO 线程处理 巡检任务
                HandleTask(v);
            }
            Console.ReadLine();
        }

        public static string MD5Encrypt64(string password)
        {
            string cl = password;
            //string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像
            SHA256 SHA256 = SHA256.Create();
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = SHA256.ComputeHash(Encoding.UTF8.GetBytes(cl));
            return Convert.ToBase64String(s);
        }

        public static bool HandleTask(TaskDemo t)
        {
            Task.Run(() =>
            {
                //TODO 巡检任务
            });
            return true;
        }
    }

    public class TaskDemo
    {
        public string ExWeek { get; set; }
        public string ExTime { get; set; }
    }
}