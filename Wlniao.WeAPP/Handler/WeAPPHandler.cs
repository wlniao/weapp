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
                { "unifiedorder", UnifiedOrderEncode },
                { "jscode2session", JsCode2SessionEncode },
            };
            DecoderMap = new Dictionary<string, ResponseDecoder>() {
                { "getwxacode", GetWxaCodeDecode },
                { "unifiedorder", UnifiedOrderDecode },
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
                ctx.HttpRequestString = JsonConvert.SerializeObject(ctx.Request);
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


        #region UnifiedOrder
        private void UnifiedOrderEncode(Context ctx)
        {
            var req = (Request.UnifiedOrderRequest)ctx.Request;
            if (req != null)
            {
                if (string.IsNullOrEmpty(req.appid))
                {
                    req.appid = ctx.MsAppId;
                }
                if (string.IsNullOrEmpty(req.secret))
                {
                    req.secret = Wlniao.Config.GetSetting("MspaySecret");
                    if (string.IsNullOrEmpty(req.secret))
                    {
                        ctx.Response = new Error() { errmsg = "missing secret" };
                    }
                }
                if (string.IsNullOrEmpty(req.mch_id))
                {
                    req.mch_id = Wlniao.Config.GetSetting("MspayMchId");
                    if (string.IsNullOrEmpty(req.mch_id))
                    {
                        ctx.Response = new Error() { errmsg = "missing mch_id" };
                    }
                }
                #region 生成签名
                var nonceStr = strUtil.CreateRndStrE(20).ToUpper();
                var kvList = new List<KeyValuePair<String, String>>();
                kvList.Add(new KeyValuePair<String, String>("appid", req.appid));
                kvList.Add(new KeyValuePair<String, String>("mch_id", req.mch_id));
                kvList.Add(new KeyValuePair<String, String>("device_info", req.device_info));
                kvList.Add(new KeyValuePair<String, String>("nonce_str", nonceStr));
                kvList.Add(new KeyValuePair<String, String>("body", req.body));
                if (!string.IsNullOrEmpty(req.sign_type))
                {
                    kvList.Add(new KeyValuePair<String, String>("sign_type", req.sign_type));
                }
                kvList.Add(new KeyValuePair<String, String>("out_trade_no", req.out_trade_no));
                if (!string.IsNullOrEmpty(req.fee_type))
                {
                    kvList.Add(new KeyValuePair<String, String>("fee_type", req.fee_type));
                }
                kvList.Add(new KeyValuePair<String, String>("total_fee", req.total_fee.ToString()));
                kvList.Add(new KeyValuePair<String, String>("spbill_create_ip", req.spbill_create_ip));
                if (!string.IsNullOrEmpty(req.time_start))
                {
                    kvList.Add(new KeyValuePair<String, String>("time_start", req.time_start));
                }
                if (!string.IsNullOrEmpty(req.time_expire))
                {
                    kvList.Add(new KeyValuePair<String, String>("time_expire", req.time_expire));
                }
                kvList.Add(new KeyValuePair<String, String>("notify_url", req.notify_url));
                kvList.Add(new KeyValuePair<String, String>("trade_type", string.IsNullOrEmpty(req.trade_type) ? "JSAPI" : req.trade_type));
                if (!string.IsNullOrEmpty(req.product_id))
                {
                    kvList.Add(new KeyValuePair<String, String>("product_id", req.product_id));
                }
                if (!string.IsNullOrEmpty(req.limit_pay))
                {
                    kvList.Add(new KeyValuePair<String, String>("limit_pay", req.limit_pay));
                }
                if (!string.IsNullOrEmpty(req.openid))
                {
                    kvList.Add(new KeyValuePair<String, String>("openid", req.openid));
                }
                kvList.Sort(delegate (KeyValuePair<String, String> small, KeyValuePair<String, String> big) { return small.Key.CompareTo(big.Key); });
                var values = new System.Text.StringBuilder();
                foreach (var kv in kvList)
                {
                    if (!string.IsNullOrEmpty(kv.Value))
                    {
                        if (values.Length > 0)
                        {
                            values.Append("&");
                        }
                        values.Append(kv.Key + "=" + kv.Value);
                    }
                }
                values.Append("&key=" + req.secret);
                //生成sig
                byte[] md5_result = System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(values.ToString()));
                System.Text.StringBuilder sig_builder = new System.Text.StringBuilder();
                foreach (byte b in md5_result)
                {
                    sig_builder.Append(b.ToString("x2"));
                }
                kvList.Add(new KeyValuePair<String, String>("sign", sig_builder.ToString().ToUpper()));
                #endregion

                #region 生成POST数据
                var sb = new System.Text.StringBuilder();
                sb.Append("<xml>");
                foreach (var kv in kvList)
                {
                    if (string.IsNullOrEmpty(kv.Value))
                    {
                        continue;
                    }
                    else
                    {
                        sb.Append("<" + kv.Key + ">" + kv.Value + "</" + kv.Key + ">");
                    }
                }
                sb.Append("</xml>");
                #endregion

                ctx.Method = System.Net.Http.HttpMethod.Post;
                ctx.HttpRequestString = sb.ToString();
                ctx.HttpClient.BaseAddress = new Uri("https://api.mch.weixin.qq.com");
                ctx.RequestUrl = "pay/unifiedorder";
            }
        }
        private void UnifiedOrderDecode(Context ctx)
        {
            try
            {
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml(ctx.HttpResponseString);
                var return_code = doc.GetElementsByTagName("return_code")[0].InnerText.Trim();
                if (return_code == "SUCCESS")
                {
                    var result_code = doc.GetElementsByTagName("result_code")[0].InnerText.Trim();
                    if (result_code == "SUCCESS")
                    {
                        var req = (Request.UnifiedOrderRequest)ctx.Request;
                        var res = new Response.UnifiedOrderResponse();
                        res.appid = doc.GetElementsByTagName("appid")[0].InnerText.Trim();
                        res.mch_id = doc.GetElementsByTagName("mch_id")[0].InnerText.Trim();
                        res.trade_type = doc.GetElementsByTagName("trade_type")[0].InnerText.Trim();
                        res.prepay_id = doc.GetElementsByTagName("prepay_id")[0].InnerText.Trim();
                        res.nonce_str = doc.GetElementsByTagName("nonce_str")[0].InnerText.Trim();
                        res.code_url = res.trade_type != "NATIVE" ? "" : doc.GetElementsByTagName("code_url")[0].InnerText.Trim();
                        res.timeStamp = DateTools.GetUnix().ToString();
                        res.signType = req.sign_type;
                        res.paySign = Encryptor.Md5Encryptor32("appId=" + res.appid + "&nonceStr=" + res.nonce_str + "&package=" + res.prepay_id + "&signType=" + res.signType + "&timeStamp=" + res.timeStamp + "&key=" + req.secret);
                        ctx.Response = res;
                    }
                    else
                    {
                        try
                        {
                            ctx.Response = new Error() { errmsg = doc.GetElementsByTagName("err_code_des")[0].InnerText.Trim() };
                        }
                        catch
                        {
                            ctx.Response = new Error() { errmsg = doc.GetElementsByTagName("err_code")[0].InnerText.Trim() };
                        }
                    }
                }
                else
                {
                    ctx.Response = new Error() { errmsg = doc.GetElementsByTagName("return_msg")[0].InnerText.Trim() };
                }
            }
            catch (Exception ex)
            {
                ctx.Response = new Error() { errmsg = "InvalidXmlString" };
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