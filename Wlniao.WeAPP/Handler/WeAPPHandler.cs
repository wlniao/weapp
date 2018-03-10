using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Wlniao.Handler;
namespace Wlniao.WeAPP
{
    /// <summary>
    /// 
    /// </summary>
    public class WeAPPHandler : PipelineHandler
    {
        private Dictionary<string, ResponseEncoder> EncoderMap;
        private Dictionary<string, ResponseDecoder> DecoderMap;
        private delegate void ResponseEncoder(Context ctx);
        private delegate void ResponseDecoder(Context ctx);

        /// <summary>
        /// 
        /// </summary>
        public WeAPPHandler(PipelineHandler handler) : base(handler)
        {
            EncoderMap = new Dictionary<string, ResponseEncoder>() {
                { "getwxacode", GetWxaCodeEncode },
                { "jscode2session", JsCode2SessionEncode },
            };
            DecoderMap = new Dictionary<string, ResponseDecoder>() {
                { "getwxacode", GetWxaCodeDecode },
                { "jscode2session", JsCode2SessionDecode },
            };
        }

        #region Handle
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        public override void HandleBefore(IContext ctx)
        {
            var _ctx = (Context)ctx;
            EncoderMap[_ctx.Operation](_ctx);
            if (string.IsNullOrEmpty(_ctx.RequestUrl))
            {
                _ctx.RequestUrl = _ctx.Operation;
            }
            if (_ctx.Method == System.Net.Http.HttpMethod.Post)
            {
                var message = JsonConvert.SerializeObject(_ctx.Request);
                _ctx.HttpRequestBody = System.Text.Encoding.UTF8.GetBytes(message);
            }
            inner.HandleBefore(ctx);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ctx"></param>
        public override void HandleAfter(IContext ctx)
        {
            inner.HandleAfter(ctx);
            var _ctx = (Context)ctx;
            if (_ctx.HttpResponseString.Contains("errmsg") || _ctx.HttpResponseString.Contains("errcode"))
            {
                _ctx.Response = JsonConvert.DeserializeObject<Error>(_ctx.HttpResponseString);
            }
            else
            {
                DecoderMap[_ctx.Operation](_ctx);
            }
        }
        #endregion

        #region GetWxaCode
        private void GetWxaCodeEncode(Context ctx)
        {
            if (string.IsNullOrEmpty(ctx.AccessToken))
            {
                var rlt = Wlniao.OpenApi.Wx.GetAccessToken(ctx.MsAppId, ctx.MsAppSecret);
                if (rlt.success)
                {
                    ctx.AccessToken = rlt.data;
                }
                else
                {
                    ctx.HttpResponseString = JsonConvert.SerializeObject(new { success = false, message = rlt.message });
                }
            }
            if (!string.IsNullOrEmpty(ctx.AccessToken))
            {
                ctx.Method = System.Net.Http.HttpMethod.Post;
                ctx.RequestUrl = "/wxa/getwxacode"
                    + "?access_token=" + ctx.AccessToken;
            }
        }
        private void GetWxaCodeDecode(Context ctx)
        {
            if (ctx.HttpResponseBody.Length > 0)
            {
                ctx.Response = new Response.GetWxaCodeResponse() { image = ctx.HttpResponseBody };
            }
            else
            {
                ctx.Response = new Error() { errmsg = "InvalidJsonString" };
            }
        }
        #endregion

        #region JsCode2Session
        private void JsCode2SessionEncode(Context ctx)
        {
            var req = (Request.JsCode2SessionRequest)ctx.Request;
            if (req != null)
            {
                if (string.IsNullOrEmpty(req.appid))
                {
                    req.appid = ctx.MsAppId;
                }
                if (string.IsNullOrEmpty(req.secret))
                {
                    req.secret = ctx.MsAppSecret;
                }
                ctx.RequestUrl = "/sns/jscode2session"
                    + "?appid=" + req.appid
                    + "&secret=" + req.secret
                    + "&js_code=" + req.js_code
                    + "&grant_type=" + req.grant_type;
            }
        }
        private void JsCode2SessionDecode(Context ctx)
        {
            var res = JsonConvert.DeserializeObject<Response.JsCode2SessionResponse>(ctx.HttpResponseString);
            if (res == null || string.IsNullOrEmpty(res.openid))
            {
                ctx.Response = new Error() { errmsg = "InvalidJsonString" };
            }
            else
            {
                if (res != null && res.unionid == null)
                {
                    res.unionid = "";
                }
                ctx.Response = res;
            }
        }
        #endregion
    }
}