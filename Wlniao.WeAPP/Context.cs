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
        /// 要调用的URL
        /// </summary>
        public string RequestUrl { get; set; }
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
        /// Http客户端
        /// </summary>
        public System.Net.Http.HttpClient HttpClient;
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
    }
}