using System;
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
        internal static string _MsAppId = null;
        internal static string _MsAppSecret = null;
        /// <summary>
        /// 
        /// </summary>
        public static string CfgMsAppId
        {
            get
            {
                if (_MsAppId == null)
                {
                    _MsAppId = Config.GetSetting("MsAppId");
                }
                return _MsAppId;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public static string CfgMsAppSecret
        {
            get
            {
                if (_MsAppSecret == null)
                {
                    _MsAppSecret = Config.GetSetting("MsAppSecret");
                }
                return _MsAppSecret;
            }
        }
        #endregion

        /// <summary>
        /// 小程序ID
        /// </summary>
        public string MsAppId { get; set; }
        /// <summary>
        /// 小程序密钥
        /// </summary>
        public string MsAppSecret { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Handler handler = null;
        /// <summary>
        /// 
        /// </summary>
        public Client()
        {
            this.MsAppId = CfgMsAppId;
            this.MsAppSecret = CfgMsAppSecret;
            handler = new Handler();
        }
        /// <summary>
        /// 
        /// </summary>
        public Client(String MsAppId, String MsAppSecret)
        {
            this.MsAppId = MsAppId;
            this.MsAppSecret = MsAppSecret;
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
            ctx.MsAppId = MsAppId;
            ctx.MsAppSecret = MsAppSecret;
            ctx.Method = method == null ? System.Net.Http.HttpMethod.Get : method;
            ctx.Operation = operation;
            ctx.Request = request;
            ctx.Response = new TResponse();
            ctx.HttpClient = new System.Net.Http.HttpClient();
            ctx.HttpClient.BaseAddress = new Uri("https://api.weixin.qq.com");

            handler.HandleBefore(ctx);

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

        #region OrderQuery 小程序支付统一下单
        /// <summary>
        /// 微信支付订单查询
        /// </summary>
        public ApiResult<OrderQueryResponse> OrderQuery(OrderQueryRequest request)
        {
            if (request == null)
            {
                return new ApiResult<OrderQueryResponse>() { message = "require parameters" };
            }
            else if (string.IsNullOrEmpty(request.transaction_id) && string.IsNullOrEmpty(request.out_trade_no))
            {
                return new ApiResult<OrderQueryResponse>() { message = "missing transaction_id or out_trade_no" };
            }
            return GetResponseFromAsyncTask(OrderQueryAsync(request));
        }
        /// <summary>
        /// 微信支付订单查询
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>
        public ApiResult<OrderQueryResponse> OrderQuery(String out_trade_no)
        {
            var request = new OrderQueryRequest();
            request.out_trade_no = out_trade_no;
            return GetResponseFromAsyncTask(OrderQueryAsync(request));
        }
        /// <summary>
        /// 微信支付订单查询 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<OrderQueryResponse>> OrderQueryAsync(OrderQueryRequest request)
        {
            return CallAsync<OrderQueryRequest, OrderQueryResponse>("orderquery", request, System.Net.Http.HttpMethod.Get);
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