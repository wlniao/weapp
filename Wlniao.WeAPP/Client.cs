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
        #region 公众号/小程序配置信息
        internal const string WLN_WX_SVR_PAYID = "1293401101";
        internal static string _WxAppId = null;     //小程序/公众号Id
        internal static string _WxAppSecret = null; //小程序/公众号密钥
        internal static string _WxAppToken = null;  //公众号接口调用令牌
        internal static string _WxAppEncodingAESKey = null; //接口消息加密解密密钥
        internal static string _WxPayId = null;     //收款方商户号
        internal static string _WxPaySecret = null; //收款方支付密钥
        internal static string _WxSvrId = null;     //服务商公众号
        internal static string _WxSvrPayId = null;  //服务商商户号
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
        /// 公众号接口调用令牌
        /// </summary>
        public static string CfgWxAppToken
        {
            get
            {
                if (_WxAppToken == null)
                {
                    _WxAppToken = Config.GetSetting("WxAppToken");
                }
                return _WxAppToken;
            }
        }
        /// <summary>
        /// 接口消息加密解密密钥
        /// </summary>
        public static string CfgWxAppEncodingAESKey
        {
            get
            {
                if (_WxAppEncodingAESKey == null)
                {
                    _WxAppEncodingAESKey = Config.GetSetting("WxAppEncodingAESKey");
                }
                return _WxAppEncodingAESKey;
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
                if (ctx.Response is Error)
                {
                    var err = (Error)ctx.Response;
                    return Task<ApiResult<TResponse>>.Run(() =>
                    {
                        return new ApiResult<TResponse>() { success = false, message = err.errmsg, code = err.errcode };
                    });
                }
                else
                {
                    return Task<ApiResult<TResponse>>.Run(() =>
                    {
                        return new ApiResult<TResponse>() { success = true, message = "error", data = (TResponse)ctx.Response };
                    });
                }
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
        /// 生成小程序码
        /// </summary>
        public ApiResult<GetWxaCodeResponse> GetWxaCode(String path, Int32 size = 430)
        {
            if (string.IsNullOrEmpty(path))
            {
                return new ApiResult<GetWxaCodeResponse>() { message = "missing path" };
            }
            var request = new Wlniao.WeAPP.Request.GetWxaCodeRequest() { path = path, width = size };
            return GetResponseFromAsyncTask(GetWxaCodeAsync(request));
        }
        /// <summary>
        /// 生成小程序码
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
        #region GetWxaCode 生成无限制的小程序二维码
        /// <summary>
        /// 生成无限制小程序二维码
        /// </summary>
        public ApiResult<GetWxaCodeUnlimitResponse> GetWxaCodeUnlimit(String page, String scene, Int32 size = 430)
        {
            if (string.IsNullOrEmpty(scene))
            {
                return new ApiResult<GetWxaCodeUnlimitResponse>() { message = "missing scene" };
            }
            var request = new Wlniao.WeAPP.Request.GetWxaCodeUnlimitRequest() { page = page, scene = scene, width = size };
            return GetResponseFromAsyncTask(GetWxaCodeUnlimitAsync(request));
        }
        /// <summary>
        /// 生成无限制小程序二维码
        /// </summary>
        public ApiResult<GetWxaCodeUnlimitResponse> GetWxaCodeUnlimit(GetWxaCodeUnlimitRequest request)
        {
            if (request == null)
            {
                return new ApiResult<GetWxaCodeUnlimitResponse>() { message = "require parameters" };
            }
            else if (string.IsNullOrEmpty(request.scene))
            {
                return new ApiResult<GetWxaCodeUnlimitResponse>() { message = "missing scene" };
            }
            return GetResponseFromAsyncTask(GetWxaCodeUnlimitAsync(request));
        }
        /// <summary>
        /// 生成无限制小程序二维码 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<GetWxaCodeUnlimitResponse>> GetWxaCodeUnlimitAsync(GetWxaCodeUnlimitRequest request)
        {
            return CallAsync<GetWxaCodeUnlimitRequest, GetWxaCodeUnlimitResponse>("getwxacodeunlimit", request, System.Net.Http.HttpMethod.Get);
        }
        #endregion 

        #region UnifiedOrder 微信支付统一下单
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
            return UnifiedOrder(request);
        }
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
        /// 小程序支付统一下单 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<UnifiedOrderResponse>> UnifiedOrderAsync(UnifiedOrderRequest request)
        {
            return CallAsync<UnifiedOrderRequest, UnifiedOrderResponse>("unifiedorder", request, System.Net.Http.HttpMethod.Get);
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

        #region Micropay 刷卡支付提交
        /// <summary>
        /// 刷卡支付提交
        /// </summary>
        /// <param name="auth_code"></param>
        /// <param name="total_fee"></param>
        /// <param name="out_trade_no"></param>
        /// <param name="body"></param>
        /// <param name="spbill_create_ip"></param>
        /// <param name="attach"></param>
        /// <returns></returns>
        public ApiResult<MicropayResponse> Micropay(String auth_code, Int32 total_fee, String out_trade_no, String body, String spbill_create_ip, String attach)
        {
            var request = new MicropayRequest();
            request.auth_code = auth_code;
            request.total_fee = total_fee;
            request.out_trade_no = out_trade_no;
            request.body = body;
            request.spbill_create_ip = spbill_create_ip;
            request.attach = attach;
            return Micropay(request);
        }
        /// <summary>
        /// 刷卡支付提交
        /// </summary>
        public ApiResult<MicropayResponse> Micropay(MicropayRequest request)
        {
            if (request == null)
            {
                return new ApiResult<MicropayResponse>() { message = "require parameters" };
            }
            else if (string.IsNullOrEmpty(request.auth_code))
            {
                return new ApiResult<MicropayResponse>() { message = "missing auth_code" };
            }
            else if (string.IsNullOrEmpty(request.body))
            {
                return new ApiResult<MicropayResponse>() { message = "missing body" };
            }
            else if (string.IsNullOrEmpty(request.out_trade_no))
            {
                return new ApiResult<MicropayResponse>() { message = "missing out_trade_no" };
            }
            else if (request.total_fee <= 0)
            {
                return new ApiResult<MicropayResponse>() { message = "missing total_fee" };
            }
            else if (string.IsNullOrEmpty(request.spbill_create_ip))
            {
                return new ApiResult<MicropayResponse>() { message = "missing spbill_create_ip" };
            }
            return GetResponseFromAsyncTask(MicropayAsync(request));
        }
        /// <summary>
        /// 刷卡支付提交 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<MicropayResponse>> MicropayAsync(MicropayRequest request)
        {
            return CallAsync<MicropayRequest, MicropayResponse>("micropay", request, System.Net.Http.HttpMethod.Get);
        }
        #endregion 

        #region Refund 按订单申请退款
        /// <summary>
        /// 按订单申请退款
        /// </summary>
        public ApiResult<RefundResponse> Refund(RefundRequest request)
        {
            if (request == null)
            {
                return new ApiResult<RefundResponse>() { message = "require parameters" };
            }
            else if (string.IsNullOrEmpty(request.transaction_id) && string.IsNullOrEmpty(request.out_trade_no))
            {
                return new ApiResult<RefundResponse>() { message = "missing transaction_id or out_trade_no" };
            }
            return GetResponseFromAsyncTask(RefundAsync(request));
        }
        /// <summary>
        /// 按订单申请退款
        /// </summary>
        /// <param name="out_trade_no"></param>
        /// <returns></returns>
        public ApiResult<RefundResponse> Refund(String out_trade_no)
        {
            var request = new RefundRequest();
            request.out_trade_no = out_trade_no;
            return GetResponseFromAsyncTask(RefundAsync(request));
        }
        /// <summary>
        /// 按订单申请退款 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<RefundResponse>> RefundAsync(RefundRequest request)
        {
            return CallAsync<RefundRequest, RefundResponse>("refund", request, System.Net.Http.HttpMethod.Get);
        }
        #endregion

        #region Transfers 企业付款到零钱
        /// <summary>
        /// 企业付款到零钱
        /// </summary>
        /// <param name="trade_no"></param>
        /// <param name="amount"></param>
        /// <param name="openid"></param>
        /// <param name="desc"></param>
        /// <param name="check_name"></param>
        /// <returns></returns>
        public ApiResult<TransfersResponse> Transfers(String trade_no, Int32 amount, String openid
            , String desc, String check_name = null)
        {
            var request = new TransfersRequest();
            request.spbill_create_ip = Wlniao.OpenApi.Tool.GetIP();
            request.partner_trade_no = trade_no;
            request.amount = amount;
            request.openid = openid;
            request.desc = desc;
            if (!string.IsNullOrEmpty(check_name))
            {
                request.check_name = true;
                request.re_user_name = check_name;
            }
            return Transfers(request);
        }

        /// <summary>
        /// 企业付款到零钱
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ApiResult<TransfersResponse> Transfers(TransfersRequest request)
        {
            if (request == null)
            {
                return new ApiResult<TransfersResponse>() { message = "require parameters" };
            }
            else if (request.amount <= 0)
            {
                return new ApiResult<TransfersResponse>() { message = "missing amount" };
            }
            else if (string.IsNullOrEmpty(request.desc))
            {
                return new ApiResult<TransfersResponse>() { message = "missing desc" };
            }
            else if (string.IsNullOrEmpty(request.openid))
            {
                return new ApiResult<TransfersResponse>() { message = "missing openid" };
            }
            else if (string.IsNullOrEmpty(request.partner_trade_no))
            {
                return new ApiResult<TransfersResponse>() { message = "missing partner_trade_no" };
            }
            else if (string.IsNullOrEmpty(request.spbill_create_ip))
            {
                return new ApiResult<TransfersResponse>() { message = "missing spbill_create_ip" };
            }
            return GetResponseFromAsyncTask(TransfersAsync(request));
        }
        /// <summary>
        /// 企业付款到零钱 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<TransfersResponse>> TransfersAsync(TransfersRequest request)
        {
            return CallAsync<TransfersRequest, TransfersResponse>("transfers", request, System.Net.Http.HttpMethod.Get);
        }
        #endregion 

        #region SendRedpack 发送现金红包
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
            return SendRedpack(request);
        }
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
        /// 发送现金红包 的异步形式。
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public Task<ApiResult<SendRedpackResponse>> SendRedpackAsync(SendRedpackRequest request)
        {
            return CallAsync<SendRedpackRequest, SendRedpackResponse>("sendredpack", request, System.Net.Http.HttpMethod.Get);
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