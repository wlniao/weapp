using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wlniao.Handler;
namespace Wlniao.WeAPP
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiHandler : PipelineHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        public override void HandleBefore(IContext ctx)
        {
            var _ctx = (Context)ctx;
            ////计算并添加参数签名
            //_ctx.HttpRequestHeaders.Add("signature", MakeRequestSignature(_ctx));
            if (_ctx.Method == System.Net.Http.HttpMethod.Post)
            {
                if (_ctx.HttpRequestBody == null)
                {
                    _ctx.HttpTask = _ctx.HttpClient.PostAsync(_ctx.RequestUrl, null);
                }
                else
                {
                    var content = new System.Net.Http.ByteArrayContent(_ctx.HttpRequestBody);
                    content.Headers.Clear();
                    foreach (var item in _ctx.HttpRequestHeaders)
                    {
                        content.Headers.Add(item.Key, item.Value);
                    }
                    _ctx.HttpTask = _ctx.HttpClient.PostAsync(_ctx.RequestUrl, content);
                }
            }
            else
            {
                _ctx.HttpTask = _ctx.HttpClient.GetAsync(_ctx.RequestUrl);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        public override void HandleAfter(IContext ctx)
        {
            var _ctx = (Context)ctx;
            var responseMessage = _ctx.HttpTask.Result;
            var task = responseMessage.Content.ReadAsByteArrayAsync();
            task.Wait();

            _ctx.StatusCode = responseMessage.StatusCode;
            _ctx.HttpResponseBody = task.Result;
            _ctx.HttpResponseString = System.Text.UTF8Encoding.UTF8.GetString(_ctx.HttpResponseBody);
            _ctx.HttpResponseHeaders = new Dictionary<string, string>();
            var status = (int)_ctx.StatusCode;
            if (status >= 200 && status < 300)
            {
                foreach (var item in responseMessage.Headers)
                {
                    var em = item.Value.GetEnumerator();
                    if (em.MoveNext())
                    {
                        _ctx.HttpResponseHeaders.Add(item.Key.ToLower(), em.Current);
                    }
                }
            }
            else if (status == 404 || status == 502)
            {
                throw new Exception(status + ":" + responseMessage.ReasonPhrase);
            }
            else
            {
                _ctx.HttpResponseString = "{errcode:" + status + ",errmsg:\"" + responseMessage.ReasonPhrase + "\"}";
            }
        }

        /// <summary>
        /// 连接Headers字段
        /// </summary>
        /// <param name="headers"></param>
        /// <returns></returns>
        private string MakeHeaderString(Dictionary<string, string> headers)
        {
            var items = new List<string>();
            foreach (var item in headers)
            {
                if (item.Key.StartsWith("x-ots-"))
                {
                    items.Add(String.Format("{0}:{1}", item.Key, item.Value));
                }
            }
            items.Sort();
            return String.Join("\n", items);
        }
        /// <summary>
        /// 计算签名
        /// </summary>
        /// <param name="signatureString"></param>
        /// <param name="accessKeySecret"></param>
        /// <returns></returns>
        private string ComputeSignature(string signatureString, string accessKeySecret)
        {
            var hmac = new System.Security.Cryptography.HMACSHA1(System.Text.Encoding.ASCII.GetBytes(accessKeySecret));
            var hashValue = hmac.ComputeHash(System.Text.Encoding.ASCII.GetBytes(signatureString));
            var signature = System.Convert.ToBase64String(hashValue);
            return signature;
        }

        private string MakeRequestSignature(Context ctx)
        {
            var headerString = MakeHeaderString(ctx.HttpRequestHeaders);
            var signatureString = ctx.Operation + "\nPOST\n\n" + headerString + '\n';
            return ComputeSignature(signatureString, ctx.MsAppSecret);
        }

        private string MakeResponseSignature(Context ctx)
        {
            string headerString = MakeHeaderString(ctx.HttpResponseHeaders);
            string signatureString = headerString + "\n" + ctx.Operation;
            return ComputeSignature(signatureString, ctx.MsAppSecret);
        }
    }
}