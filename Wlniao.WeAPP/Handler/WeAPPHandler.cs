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
                { "orderquery", OrderQueryEncode },
                { "jscode2session", JsCode2SessionEncode },
            };
            DecoderMap = new Dictionary<string, ResponseDecoder>() {
                { "getwxacode", GetWxaCodeDecode },
                { "unifiedorder", UnifiedOrderDecode },
                { "orderquery", OrderQueryDecode },
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
                var rlt = Wlniao.OpenApi.Wx.GetAccessToken(ctx.AppId, ctx.AppSecret);
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
                    if (string.IsNullOrEmpty(Client.CfgWxSvrId))
                    {
                        req.appid = Client.CfgWxAppId;
                    }
                    else
                    {
                        req.appid = Client.CfgWxSvrId;
                        req.sub_appid = Client.CfgWxAppId;
                    }
                    if (string.IsNullOrEmpty(req.appid))
                    {
                        ctx.Response = new Error() { errmsg = "missing appid" };
                        return;
                    }
                }
                if (string.IsNullOrEmpty(req.secret))
                {
                    req.secret = Client.CfgWxPaySecret;
                }
                if (string.IsNullOrEmpty(req.mch_id))
                {
                    if (string.IsNullOrEmpty(Client.CfgWxSvrPayId))
                    {
                        req.mch_id = Client.CfgWxPayId;
                    }
                    else
                    {
                        req.mch_id = Client.CfgWxSvrPayId;
                        req.sub_mch_id = Client.CfgWxPayId;
                    }
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
                if (!string.IsNullOrEmpty(req.sub_appid))
                {
                    kvList.Add(new KeyValuePair<String, String>("sub_appid", req.sub_appid));
                }
                if (!string.IsNullOrEmpty(req.sub_mch_id))
                {
                    kvList.Add(new KeyValuePair<String, String>("sub_mch_id", req.sub_mch_id));
                }
                kvList.Add(new KeyValuePair<String, String>("device_info", req.device_info));
                kvList.Add(new KeyValuePair<String, String>("nonce_str", nonceStr));
                kvList.Add(new KeyValuePair<String, String>("body", req.body));
                kvList.Add(new KeyValuePair<String, String>("sign_type", req.sign_type));
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
                kvList.Add(new KeyValuePair<String, String>("trade_type", req.trade_type));
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
                    if (string.IsNullOrEmpty(req.sub_appid))
                    {
                        kvList.Add(new KeyValuePair<String, String>("openid", req.openid));
                    }
                    else
                    {
                        kvList.Add(new KeyValuePair<String, String>("sub_openid", req.openid));
                    }
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
                if (req.mch_id == Client.WLN_WX_SVR_PAYID)
                {
                    //通过OpenApi在线签名
                    var rlt = Wlniao.OpenApi.Common.Post<String>("cashier", "wxsign", values.ToString(), new KeyValuePair<string, string>("sign_type", req.sign_type));
                    if (rlt.success)
                    {
                        kvList.Add(new KeyValuePair<String, String>("sign", rlt.data));
                    }
                    else
                    {
                        ctx.Response = new Error() { errmsg = rlt.message };
                        return;
                    }
                }
                else
                {
                    values.Append("&key=" + req.secret);
                    //生成sig
                    byte[] md5_result = System.Security.Cryptography.MD5.Create().ComputeHash(System.Text.Encoding.UTF8.GetBytes(values.ToString()));
                    System.Text.StringBuilder sig_builder = new System.Text.StringBuilder();
                    foreach (byte b in md5_result)
                    {
                        sig_builder.Append(b.ToString("x2"));
                    }
                    kvList.Add(new KeyValuePair<String, String>("sign", sig_builder.ToString().ToUpper()));
                }
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
                var req = (Request.UnifiedOrderRequest)ctx.Request;
                var res = new Response.UnifiedOrderResponse();
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml(ctx.HttpResponseString);
                res.return_code = doc.GetElementsByTagName("return_code")[0].InnerText.Trim();
                res.return_msg = doc.GetElementsByTagName("return_msg")[0].InnerText.Trim();
                if (res.return_code == "SUCCESS")
                {
                    res.result_code = doc.GetElementsByTagName("result_code")[0].InnerText.Trim();
                    res.appid = doc.GetElementsByTagName("appid")[0].InnerText.Trim();
                    res.mch_id = doc.GetElementsByTagName("mch_id")[0].InnerText.Trim();
                    res.nonce_str = doc.GetElementsByTagName("nonce_str")[0].InnerText.Trim();
                    res.sign = doc.GetElementsByTagName("sign")[0].InnerText.Trim();
                    if (res.result_code == "SUCCESS")
                    {
                        res.trade_type = doc.GetElementsByTagName("trade_type")[0].InnerText.Trim();
                        res.prepay_id = doc.GetElementsByTagName("prepay_id")[0].InnerText.Trim();
                        res.nonce_str = doc.GetElementsByTagName("nonce_str")[0].InnerText.Trim();
                        res.code_url = res.trade_type != "NATIVE" ? "" : doc.GetElementsByTagName("code_url")[0].InnerText.Trim();
                        res.timeStamp = DateTools.GetUnix().ToString();
                        res.signType = req.sign_type;
                        var str = "appId=" + res.appid + "&nonceStr=" + res.nonce_str + "&package=prepay_id=" + res.prepay_id + "&signType=" + res.signType + "&timeStamp=" + res.timeStamp;
                        if (req.mch_id == Client.WLN_WX_SVR_PAYID)
                        {
                            //通过OpenApi在线签名
                            var rlt = Wlniao.OpenApi.Common.Post<String>("cashier", "wxsign", str, new KeyValuePair<string, string>("sign_type", req.sign_type));
                            if (rlt.success)
                            {
                                res.paySign = rlt.data;
                            }
                            else
                            {
                                ctx.Response = new Error() { errmsg = rlt.message };
                            }
                        }
                        else
                        {
                            res.paySign = Encryptor.Md5Encryptor32(str + "&key=" + req.secret);
                        }
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
                    ctx.Response = new Error() { errmsg = res.return_msg };
                }
            }
            catch
            {
                ctx.Response = new Error() { errmsg = "InvalidXmlString" };
            }
        }
        #endregion

        #region OrderQuery
        private void OrderQueryEncode(Context ctx)
        {
            var req = (Request.OrderQueryRequest)ctx.Request;
            if (req != null)
            {
                if (string.IsNullOrEmpty(req.appid))
                {
                    if (string.IsNullOrEmpty(Client.CfgWxSvrId))
                    {
                        req.appid = Client.CfgWxAppId;
                    }
                    else
                    {
                        req.appid = Client.CfgWxSvrId;
                        req.sub_appid = Client.CfgWxAppId;
                    }
                    if (string.IsNullOrEmpty(req.mch_id))
                    {
                        ctx.Response = new Error() { errmsg = "missing appid" };
                    }
                }
                if (string.IsNullOrEmpty(req.secret))
                {
                    req.secret = Client.CfgWxPaySecret;
                    if (string.IsNullOrEmpty(req.secret))
                    {
                        ctx.Response = new Error() { errmsg = "missing secret" };
                    }
                }
                if (string.IsNullOrEmpty(req.mch_id))
                {
                    if (string.IsNullOrEmpty(Client.CfgWxSvrPayId))
                    {
                        req.mch_id = Client.CfgWxPayId;
                    }
                    else
                    {
                        req.mch_id = Client.CfgWxSvrPayId;
                        req.sub_mch_id = Client.CfgWxPayId;
                    }
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
                if (!string.IsNullOrEmpty(req.sub_appid))
                {
                    kvList.Add(new KeyValuePair<String, String>("sub_appid", req.sub_appid));
                }
                if (!string.IsNullOrEmpty(req.sub_mch_id))
                {
                    kvList.Add(new KeyValuePair<String, String>("sub_mch_id", req.sub_mch_id));
                }
                kvList.Add(new KeyValuePair<String, String>("nonce_str", nonceStr));
                kvList.Add(new KeyValuePair<String, String>("sign_type", req.sign_type));
                if (string.IsNullOrEmpty(req.transaction_id))
                {
                    kvList.Add(new KeyValuePair<String, String>("out_trade_no", req.out_trade_no));
                }
                else
                {
                    kvList.Add(new KeyValuePair<String, String>("transaction_id", req.transaction_id));
                }
                if (req.mch_id == Client.WLN_WX_SVR_PAYID)
                {
                    //通过OpenApi在线签名
                    var rlt = Wlniao.OpenApi.Common.Post<String>("cashier", "wxapisign", JsonConvert.SerializeObject(kvList), new KeyValuePair<string, string>("sign_type", req.sign_type));
                    if (rlt.success)
                    {
                        kvList.Add(new KeyValuePair<String, String>("sign", rlt.data));
                    }
                    else
                    {
                        ctx.Response = new Error() { errmsg = rlt.message };
                        return;
                    }
                }
                else
                {
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
                }
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
                ctx.RequestUrl = "pay/orderquery";
            }
        }
        private void OrderQueryDecode(Context ctx)
        {
            try
            {
                var req = (Request.OrderQueryRequest)ctx.Request;
                var res = new Response.OrderQueryResponse();
                var doc = new System.Xml.XmlDocument();
                doc.LoadXml(ctx.HttpResponseString);
                res.return_code = doc.GetElementsByTagName("return_code")[0].InnerText.Trim();
                res.return_msg = doc.GetElementsByTagName("return_msg")[0].InnerText.Trim();
                if (res.return_code == "SUCCESS")
                {
                    res.result_code = doc.GetElementsByTagName("result_code")[0].InnerText.Trim();
                    res.appid = doc.GetElementsByTagName("appid")[0].InnerText.Trim();
                    res.mch_id = doc.GetElementsByTagName("mch_id")[0].InnerText.Trim();
                    res.nonce_str = doc.GetElementsByTagName("nonce_str")[0].InnerText.Trim();
                    res.sign = doc.GetElementsByTagName("sign")[0].InnerText.Trim();
                    if (res.result_code == "SUCCESS")
                    {
                        res.trade_state = doc.GetElementsByTagName("trade_state")[0].InnerText.Trim();
                        res.out_trade_no = doc.GetElementsByTagName("out_trade_no")[0].InnerText.Trim();
                        if (res.trade_state == "SUCCESS")
                        {
                            res.openid = doc.GetElementsByTagName("openid")[0].InnerText.Trim();
                            res.trade_type = doc.GetElementsByTagName("trade_type")[0].InnerText.Trim();
                            res.bank_type = doc.GetElementsByTagName("bank_type")[0].InnerText.Trim();
                            res.total_fee = cvt.ToInt(doc.GetElementsByTagName("total_fee")[0].InnerText.Trim());
                            res.cash_fee = cvt.ToInt(doc.GetElementsByTagName("cash_fee")[0].InnerText.Trim());
                            res.transaction_id = doc.GetElementsByTagName("transaction_id")[0].InnerText.Trim();
                            res.out_trade_no = doc.GetElementsByTagName("out_trade_no")[0].InnerText.Trim();
                            res.time_end = doc.GetElementsByTagName("time_end")[0].InnerText.Trim();

                            try { res.device_info = doc.GetElementsByTagName("device_info")[0].InnerText.Trim(); } catch { }
                            try { res.is_subscribe = doc.GetElementsByTagName("is_subscribe")[0].InnerText.Trim(); } catch { }
                            try { res.fee_type = doc.GetElementsByTagName("fee_type")[0].InnerText.Trim(); } catch { }
                            try { res.trade_state_desc = doc.GetElementsByTagName("trade_state_desc")[0].InnerText.Trim(); } catch { }
                            try { res.settlement_total_fee = cvt.ToInt(doc.GetElementsByTagName("settlement_total_fee")[0].InnerText.Trim()); } catch { }
                            try
                            {
                                var t = new DateTime(int.Parse(res.time_end.Substring(0, 4)), int.Parse(res.time_end.Substring(4, 2)), int.Parse(res.time_end.Substring(6, 2)), int.Parse(res.time_end.Substring(8, 2)), int.Parse(res.time_end.Substring(10, 2)), int.Parse(res.time_end.Substring(12, 2)), DateTimeKind.Utc).AddHours(-8);
                                res.paytime = DateTools.GetUnix(t);
                            }
                            catch { res.paytime = DateTools.GetUnix(); }
                        }
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
                    ctx.Response = new Error() { errmsg = res.return_msg };
                }
            }
            catch
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
                    req.appid = ctx.AppId;
                }
                if (string.IsNullOrEmpty(req.secret))
                {
                    req.secret = ctx.AppSecret;
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