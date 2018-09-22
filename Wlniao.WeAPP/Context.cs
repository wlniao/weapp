using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Wlniao.WeAPP
{
    /// <summary>
    /// 请求线程
    /// </summary>
    public class Context : Wlniao.Handler.IContext
    {
        /// <summary>
        /// 小程序ID
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 小程序密钥
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 接口调用凭据
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// HTTP请求方式
        /// </summary>
        public System.Net.Http.HttpMethod Method { get; set; }
        /// <summary>
        /// 要调用的API操作
        /// </summary>
        public string Operation { get; set; }
        /// <summary>
        /// 请求的地址
        /// </summary>
        public string RequestHost { get; set; }
        /// <summary>
        /// 要调用的路径
        /// </summary>
        public string RequestPath { get; set; }
        /// <summary>
        /// 要发送的请求内容
        /// </summary>
        public Wlniao.Handler.IRequest Request { get; set; }
        /// <summary>
        /// API的输出内容
        /// </summary>
        public Wlniao.Handler.IResponse Response { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Task<System.Net.Http.HttpResponseMessage> HttpTask;
        /// <summary>
        /// 请求使用的证书
        /// </summary>
        public System.Security.Cryptography.X509Certificates.X509Certificate Certificate;
        /// <summary>
        /// 输出的状态
        /// </summary>
        public System.Net.HttpStatusCode StatusCode = System.Net.HttpStatusCode.Created;
        /// <summary>
        /// 请求的Headers参数
        /// </summary>
        public Dictionary<String, String> HttpRequestHeaders;
        /// <summary>
        /// 输出的Headers参数
        /// </summary>
        public Dictionary<String, String> HttpResponseHeaders;
        /// <summary>
        /// 请求的内容
        /// </summary>
        public byte[] HttpRequestBody { get; set; }
        /// <summary>
        /// 请求的内容
        /// </summary>
        public string HttpRequestString { get; set; }
        /// <summary>
        /// 输出的内容
        /// </summary>
        public byte[] HttpResponseBody { get; set; }
        /// <summary>
        /// 输出的内容
        /// </summary>
        public string HttpResponseString { get; set; }
        /// <summary>
        /// 请求重试次数
        /// </summary>
        public int Retry { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Context()
        {
            Retry = 0;
        }

        private static Dictionary<String, TokenCache> tokens = new Dictionary<string, TokenCache>();
        /// <summary>
        /// 检查或同步
        /// </summary>
        /// <returns></returns>
        public bool CheckAccessToken()
        {
            if (!tokens.ContainsKey(AppId))
            {
                tokens.Add(AppId, new TokenCache());
            }
            if (tokens[AppId].past < DateTime.Now)
            {
                var rlt = Wlniao.OpenApi.Wx.GetAccessToken(AppId, AppSecret);
                if (rlt.success)
                {
                    AccessToken = rlt.data;
                    var expires = 900;
                    if (rlt.message.Contains("expires in"))
                    {
                        expires = cvt.ToInt(rlt.message.SplitBy(" ")[2]);
                    }
                    tokens[AppId].past = DateTime.Now.AddSeconds(expires);
                    tokens[AppId].token = rlt.data;
                }
            }
            else
            {
                AccessToken = tokens[AppId].token;
            }
            if (string.IsNullOrEmpty(AccessToken))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private class TokenCache
        {
            public String token = "";
            public DateTime past = DateTime.MinValue;
        }
    }
}