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