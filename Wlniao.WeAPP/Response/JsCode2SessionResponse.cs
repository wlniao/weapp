using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Response
{
    /// <summary>
    /// code 换取 session_key
    /// </summary>
    public class JsCode2SessionResponse : Wlniao.Handler.IResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string session_key { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string unionid { get; set; }
    }
}
