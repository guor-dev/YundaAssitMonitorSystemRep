using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace ToolLibrary
{
    public class HttpHelper
    {
        /// <summary>
        /// http请求，POST方式
        /// </summary>
        /// <param name="url">请求连接</param>
        /// <param name="dicParams">请求参数</param>
        /// <returns>根据泛型返回响应值</returns>
        public static T HttpPostRequest<T>(string url, object dicParams, string accessToken = null)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);//webrequest请求api地址
                request.Method = "POST";
                request.Accept = "application/json, text/javascript, */*";  //"text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                //request.Headers.Add("ContentType", "application/json");
                if (!string.IsNullOrEmpty(accessToken))
                    request.Headers.Add("Authorization", "Bearer" + " " + accessToken);
                if (dicParams != null)
                {
                    byte[] buffer = encoding.GetBytes(JsonConvert.SerializeObject(dicParams));
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                return GetResponseValue<T>(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// http请求，POST方式，不需要返回值
        /// </summary>
        /// <param name="url">请求连接</param>
        /// <param name="dicParams">请求参数</param>
        public static void HttpPostRequest(string url, object dicParams)
        {
            try
            {
                Encoding encoding = Encoding.UTF8;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);//webrequest请求api地址
                request.Method = "POST";
                request.Accept = "application/json, text/javascript, */*";  //"text/html, application/xhtml+xml, */*";
                request.ContentType = "application/json";
                if (dicParams != null)
                {
                    byte[] buffer = encoding.GetBytes(JsonConvert.SerializeObject(dicParams));
                    request.ContentLength = buffer.Length;
                    request.GetRequestStream().Write(buffer, 0, buffer.Length);
                }
                request.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// http请求，GET方式
        /// </summary>
        /// <param name="url">请求连接</param>
        /// <param name="dicParams">请求参数</param>
        /// <returns>根据泛型返回响应值</returns>
        public static T HttpGetRequest<T>(string url, Dictionary<string, string> dicParams = null, string accessToken = null)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append(url);
                if (dicParams != null && dicParams.Count > 0)
                {
                    sBuilder.Append("?");
                    int i = 0;
                    foreach (KeyValuePair<string, string> pair in dicParams)
                    {
                        if (i > 0)
                            sBuilder.Append("&");
                        sBuilder.AppendFormat("{0}={1}", pair.Key, pair.Value);
                        i++;
                    }
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sBuilder.ToString());
                request.Method = "GET";
                request.Accept = "application/json, text/javascript, */*";
                if (!string.IsNullOrEmpty(accessToken))
                    request.Headers.Add("Authorization", "Bearer" + " " + accessToken);

                return GetResponseValue<T>(request);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// http请求，GET方式，不需要返回值
        /// </summary>
        /// <param name="url">请求连接</param>
        /// <param name="dicParams">请求参数</param>
        /// <returns>根据泛型返回响应值</returns>
        public static void HttpGetRequest(string url, Dictionary<string, string> dicParams = null)
        {
            try
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append(url);
                if (dicParams != null && dicParams.Count > 0)
                {
                    sBuilder.Append("?");
                    int i = 0;
                    foreach (KeyValuePair<string, string> pair in dicParams)
                    {
                        if (i > 0)
                            sBuilder.Append("&");
                        sBuilder.AppendFormat("{0}={1}", pair.Key, pair.Value);
                        i++;
                    }
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(sBuilder.ToString());
                request.Method = "GET";
                request.GetResponse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static T GetResponseValue<T>(HttpWebRequest request)
        {
            try
            {
                T rstObj = default(T);
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    rstObj = JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                    reader.Close();
                    reader.Dispose();
                }
                response.Close();
                return rstObj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}