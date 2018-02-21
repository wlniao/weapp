using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Request
{
    /// <summary>
    /// code 换取 session_key
    /// </summary>
    public class JsCode2SessionRequest : Wlniao.Handler.IRequest
    {
        /// <summary>
        /// 小程序唯一标识
        /// </summary>
        public string appid { get; set; }
        /// <summary>
        /// 小程序的 app secret
        /// </summary>
        public string secret { get; set; }
        /// <summary>
        /// 登录时获取的 code
        /// </summary>
        public string js_code { get; set; }
        /// <summary>
        /// 填写为 authorization_code
        /// </summary>
        public string grant_type = "authorization_code";
    }
}
