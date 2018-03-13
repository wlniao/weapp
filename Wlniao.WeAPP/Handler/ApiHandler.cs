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
            if (_ctx.Method == System.Net.Http.HttpMethod.Post)
            {
                System.Net.Http.HttpContent content;
                if (_ctx.HttpResponseBody != null)
                {
                    content = new System.Net.Http.ByteArrayContent(_ctx.HttpRequestBody);
                }
                else if (!string.IsNullOrEmpty(_ctx.HttpRequestString))
                {
                    content = new System.Net.Http.StringContent(_ctx.HttpRequestString);
                }
                else
                {
                    content = null;
                }
                if (content != null && _ctx.HttpRequestHeaders != null)
                {
                    foreach (var item in _ctx.HttpRequestHeaders)
                    {
                        content.Headers.Add(item.Key, item.Value);
                    }
                }
                _ctx.HttpTask = _ctx.HttpClient.PostAsync(_ctx.RequestUrl, content);
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

    }
}