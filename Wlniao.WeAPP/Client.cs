﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wlniao.WeAPP.Request;
using Wlniao.WeAPP.Response;
namespace Wlniao.WeAPP
{
    /// <summary>
    /// 微信小程序开放接口客户端
    /// </summary>
    public class Client : Wlniao.Handler.IClient
    {
        #region 微信小程序特定配置信息
        internal const string WLN_WX_SVR_PAYID = "1293401101";
        internal static string _WxAppId = null;
        internal static string _WxAppSecret = null;
        internal static string _WxPayId = null;
        internal static string _WxPaySecret = null;
        internal static string _WxSvrId = null;
        internal static string _WxSvrPayId = null;
        /// <summary>
        /// 小程序/公众号Id
        /// </summary>
        public static string CfgWxAppId
        {
            get
            {
                if (_WxAppId == null)
                {
                    _WxAppId = Config.GetSetting("WxAppId");
                }
                return _WxAppId;
            }
        }
        /// <summary>
        /// 小程序/公众号密钥
        /// </summary>
        public static string CfgWxAppSecret
        {
            get
            {
                if (_WxAppSecret == null)
                {
                    _WxAppSecret = Config.GetSetting("WxAppSecret");
                }
                return _WxAppSecret;
            }
        }
        /// <summary>
        /// 微信支付商户号
        /// </summary>
        public static string CfgWxPayId
        {
            get
            {
                if (_WxPayId == null)
                {
                    _WxPayId = Config.GetSetting("WxPayId");
                }
                return _WxPayId;
            }
        }
        /// <summary>
        /// 微信支付密钥（服务商模式为服务商密钥）
        /// </summary>
        public static string CfgWxPaySecret
        {
            get
            {
                if (_WxPaySecret == null)
                {
                    _WxPaySecret = Config.GetSetting("WxPaySecret");
                }
                return _WxPaySecret;
            }
        }
        /// <summary>
        /// 微信服务商公众号Id
        /// </summary>
        public static string CfgWxSvrId
        {
            get
            {
                if (_WxSvrId == null)
                {
                    _WxSvrId = Config.GetSetting("WxSvrId");
                }
                return _WxSvrId;
            }
        }
        /// <summary>
        /// 微信服务商商户号
        /// </summary>
        public static string CfgWxSvrPayId
        {
            get
            {
                if (_WxSvrPayId == null)
                {
                    _WxSvrPayId = Config.GetSetting("WxSvrPayId");
                }
                return _WxSvrPayId;
            }
        }
        #endregion

        /// <summary>
        /// 小程序ID
        /// </summary>
        public string WxAppId { get; set; }
        /// <summary>
        /// 小程序密钥
        /// </summary>
        public string WxAppSecret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Handler handler = null;
        /// <summary>
        /// 
        /// </summary>
        public Client()
        {
            this.WxAppId = CfgWxAppId;
            this.WxAppSecret = CfgWxAppSecret;
            handler = new Handler();
        }
        /// <summary>
        /// 
        /// </summary>
        public Client(String AppId, String AppSecret)
        {
            this.WxAppId = AppId;
            this.WxAppSecret = AppSecret;
            handler = new Handler();
        }


        private Task<ApiResult<TResponse>> CallAsync<TRequest, TResponse>(string operation, TRequest request, System.Net.Http.HttpMethod method)
            where TResponse : Wlniao.Handler.IResponse, new()
            where TRequest : Wlniao.Handler.IRequest
        {
            if (request == null)
            {
                throw new ArgumentNullException();
            }

            var ctx = new Context();
            ctx.AppId = WxAppId;
            ctx.AppSecret = WxAppSecret;
            ctx.Method = method == null ? System.Net.Http.HttpMethod.Get : method;
            ctx.Operation = operation;
            ctx.Request = request;
            ctx.RequestHost = "https://api.weixin.qq.com";

            handler.HandleBefore(ctx);
            if (ctx.Response == null)
            {
                return ctx.HttpTask.ContinueWith((t) =>
                {
                    handler.HandleAfter(ctx);
                    if (ctx.Response is Error)
                    {
                        var err = (Error)ctx.Response;
                        return new ApiResult<TResponse>() { success = false, message = err.errmsg, code = err.errcode };
                    }
                    return new ApiResult<TResponse>() { success = true, message = "success", data = (TResponse)ctx.Response };
                });
            }
            else
            {
                return new Task<ApiResult<TResponse>>(a =>
                {
                    if (ctx.Response is Error)
                    {
                        var err = (Error)ctx.Response;
                        return new ApiResult<TResponse>() { success = false, message = err.errmsg, code = err.errcode };
                    }
                    else
                    {
                        return new ApiResult<TResponse>() { success = true, message = "success", data = (TResponse)ctx.Response };
                    }
                }, null);
            }
        }
        private TResponse GetResponseFromAsyncTask<TResponse>(Task<TResponse> task)
        {
            try
            {
                task.Wait();
            }
            catch (System.AggregateException e)
            {
                log.Error(e.Message);
                throw e.GetBaseException();
            }

            return task.Result;
        }


        #region GetWxaCode 生成小程序码
        /// <summary>
        /// 
        /// </summary>
        public ApiResult<GetWxaCodeResponse> GetWxaCode(GetWxaCodeRequest request)
        {
            if (request == null)
            {
                return new ApiResult<GetWxaCodeResponse>() { message = "require parameters" };
            }
            else if (string.IsNullOrEmpty(request.path))
            {
                return new ApiResult<GetWxaCodeResponse>() { message = "missing path" };
            }
            return GetResponseFromAsyncTask(GetWxaCodeAsync(request));
        }
        /// <summary>
        /// 生成小程序码 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<GetWxaCodeResponse>> GetWxaCodeAsync(GetWxaCodeRequest request)
        {
            return CallAsync<GetWxaCodeRequest, GetWxaCodeResponse>("getwxacode", request, System.Net.Http.HttpMethod.Get);
        }
        #endregion 

        #region UnifiedOrder 小程序支付统一下单
        /// <summary>
        /// 小程序支付统一下单
        /// </summary>
        public ApiResult<UnifiedOrderResponse> UnifiedOrder(UnifiedOrderRequest request)
        {
            if (request == null)
            {
                return new ApiResult<UnifiedOrderResponse>() { message = "require parameters" };
            }
            else if (string.IsNullOrEmpty(request.body))
            {
                return new ApiResult<UnifiedOrderResponse>() { message = "missing body" };
            }
            else if (string.IsNullOrEmpty(request.out_trade_no))
            {
                return new ApiResult<UnifiedOrderResponse>() { message = "missing out_trade_no" };
            }
            else if (request.total_fee <= 0)
            {
                return new ApiResult<UnifiedOrderResponse>() { message = "missing total_fee" };
            }
            else if (string.IsNullOrEmpty(request.spbill_create_ip))
            {
                return new ApiResult<UnifiedOrderResponse>() { message = "missing spbill_create_ip" };
            }
            else if (string.IsNullOrEmpty(request.notify_url))
            {
                return new ApiResult<UnifiedOrderResponse>() { message = "missing notify_url" };
            }
            else if (string.IsNullOrEmpty(request.openid) && request.trade_type == "JSAPI")
            {
                return new ApiResult<UnifiedOrderResponse>() { message = "missing openid" };
            }
            return GetResponseFromAsyncTask(UnifiedOrderAsync(request));
        }
        /// <summary>
        /// 小程序支付统一下单
        /// </summary>
        /// <param name="trade_no"></param>
        /// <param name="total_fee"></param>
        /// <param name="body"></param>
        /// <param name="spbill_create_ip"></param>
        /// <param name="notify_url"></param>
        /// <param name="trade_type"></param>
        /// <param name="openid"></param>
        /// <returns></returns>
        public ApiResult<UnifiedOrderResponse> UnifiedOrder(String trade_no, Int32 total_fee, String body, String spbill_create_ip, String notify_url, String trade_type, String openid)
        {
            var request = new UnifiedOrderRequest();
            request.out_trade_no = trade_no;
            request.body = body;
            request.total_fee = total_fee;
            request.spbill_create_ip = spbill_create_ip;
            request.notify_url = notify_url;
            request.trade_type = trade_type;
            request.openid = openid;
            return GetResponseFromAsyncTask(UnifiedOrderAsync(request));
        }

        /// <summary>
        /// 小程序支付统一下单 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<UnifiedOrderResponse>> UnifiedOrderAsync(UnifiedOrderRequest request)
        {
            return CallAsync<UnifiedOrderRequest, UnifiedOrderResponse>("unifiedorder", request, System.Net.Http.HttpMethod.Get);
        }
        #endregion 

        #region SendRedpack 发送现金红包
        /// <summary>
        /// 发送现金红包
        /// </summary>
        public ApiResult<SendRedpackResponse> SendRedpack(SendRedpackRequest request)
        {
            if (request == null)
            {
                return new ApiResult<SendRedpackResponse>() { message = "require parameters" };
            }
            else if (string.IsNullOrEmpty(request.mch_billno))
            {
                return new ApiResult<SendRedpackResponse>() { message = "missing mch_billno" };
            }
            else if (string.IsNullOrEmpty(request.send_name))
            {
                return new ApiResult<SendRedpackResponse>() { message = "missing send_name" };
            }
            else if (string.IsNullOrEmpty(request.act_name))
            {
                return new ApiResult<SendRedpackResponse>() { message = "missing act_name" };
            }
            else if (string.IsNullOrEmpty(request.wishing))
            {
                return new ApiResult<SendRedpackResponse>() { message = "missing wishing" };
            }
            else if (string.IsNullOrEmpty(request.remark))
            {
                return new ApiResult<SendRedpackResponse>() { message = "missing remark" };
            }
            else if (request.total_amount <= 0)
            {
                return new ApiResult<SendRedpackResponse>() { message = "missing total_amount" };
            }
            else if (string.IsNullOrEmpty(request.client_ip))
            {
                return new ApiResult<SendRedpackResponse>() { message = "missing client_ip" };
            }
            else if (string.IsNullOrEmpty(request.re_openid))
            {
                return new ApiResult<SendRedpackResponse>() { message = "missing re_openid" };
            }
            return GetResponseFromAsyncTask(SendRedpackAsync(request));
        }
        /// <summary>
        /// 发送现金红包
        /// </summary>
        /// <param name="billno"></param>
        /// <param name="amount"></param>
        /// <param name="openid"></param>
        /// <param name="act_name"></param>
        /// <param name="send_name"></param>
        /// <param name="wishing"></param>
        /// <param name="remark"></param>
        /// <returns></returns>
        public ApiResult<SendRedpackResponse> SendRedpack(String billno, Int32 amount, String openid
            , String act_name, String send_name, String wishing, String remark)
        {
            var request = new SendRedpackRequest();
            request.mch_billno = billno;
            request.total_amount = amount;
            request.re_openid = openid;
            request.act_name = act_name;
            request.send_name = send_name;
            request.wishing = wishing;
            request.remark = remark;
            return GetResponseFromAsyncTask(SendRedpackAsync(request));
        }

        /// <summary>
        /// 小程序支付统一下单 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<SendRedpackResponse>> SendRedpackAsync(SendRedpackRequest request)
        {
            return CallAsync<SendRedpackRequest, SendRedpackResponse>("sendredpack", request, System.Net.Http.HttpMethod.Get);
        }
        #endregion 

        #region QueryOrder 微信支付订单统一查询
        /// <summary>
        /// 微信支付订单查询
        /// </summary>
        public ApiResult<QueryOrderResponse> QueryOrder(QueryOrderRequest request)
        {
            if (request == null)
            {
                return new ApiResult<QueryOrderResponse>() { message = "require parameters" };
            }
            else if (string.IsNullOrEmpty(request.transaction_id) && string.IsNullOrEmpty(request.out_trade_no))
            {
                return new ApiResult<QueryOrderResponse>() { message = "missing transaction_id or out_trade_no" };
            }
            return GetResponseFromAsyncTask(QueryOrderAsync(request));
        }
        /// <summary>
        /// 微信支付订单查询
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>
        public ApiResult<QueryOrderResponse> QueryOrder(String out_trade_no)
        {
            var request = new QueryOrderRequest();
            request.out_trade_no = out_trade_no;
            return GetResponseFromAsyncTask(QueryOrderAsync(request));
        }
        /// <summary>
        /// 微信支付订单查询 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<QueryOrderResponse>> QueryOrderAsync(QueryOrderRequest request)
        {
            return CallAsync<QueryOrderRequest, QueryOrderResponse>("queryorder", request, System.Net.Http.HttpMethod.Get);
        }
        #endregion

        #region QueryRedpack 微信现金红包查询
        /// <summary>
        /// 微信现金红包查询
        /// </summary>
        public ApiResult<QueryRedpackResponse> QueryRedpack(QueryRedpackRequest request)
        {
            if (request == null)
            {
                return new ApiResult<QueryRedpackResponse>() { message = "require parameters" };
            }
            else if (string.IsNullOrEmpty(request.mch_billno) && string.IsNullOrEmpty(request.mch_billno))
            {
                return new ApiResult<QueryRedpackResponse>() { message = "missing mch_billno" };
            }
            return GetResponseFromAsyncTask(QueryRedpackAsync(request));
        }
        /// <summary>
        /// 微信现金红包查询
        /// </summary>
        /// <param name="mch_billno"></param>
        /// <returns></returns>
        public ApiResult<QueryRedpackResponse> QueryRedpack(String mch_billno)
        {
            var request = new QueryRedpackRequest();
            request.mch_billno = mch_billno;
            return GetResponseFromAsyncTask(QueryRedpackAsync(request));
        }
        /// <summary>
        /// 微信现金红包查询 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<QueryRedpackResponse>> QueryRedpackAsync(QueryRedpackRequest request)
        {
            return CallAsync<QueryRedpackRequest, QueryRedpackResponse>("queryredpack", request, System.Net.Http.HttpMethod.Get);
        }
        #endregion



        #region JsCode2Session
        /// <summary>
        /// code 换取 session_key。
        /// </summary>
        public ApiResult<JsCode2SessionResponse> JsCode2Session(JsCode2SessionRequest request)
        {
            if (request == null)
            {
                return new ApiResult<JsCode2SessionResponse>() { message = "require parameters" };
            }
            else if (string.IsNullOrEmpty(request.js_code))
            {
                return new ApiResult<JsCode2SessionResponse>() { message = "missing js_code" };
            }
            return GetResponseFromAsyncTask(JsCode2SessionAsync(request));
        }
        /// <summary>
        /// code 换取 session_key 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<JsCode2SessionResponse>> JsCode2SessionAsync(JsCode2SessionRequest request)
        {
            return CallAsync<JsCode2SessionRequest, JsCode2SessionResponse>("jscode2session", request, System.Net.Http.HttpMethod.Get);
        }
        #endregion 
    }
}