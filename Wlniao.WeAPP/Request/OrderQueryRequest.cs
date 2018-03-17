using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Request
{
    /// <summary>
    /// 微信支付订单查询的请求参数
    /// </summary>
    public class OrderQueryRequest : Wlniao.Handler.IRequest
    {
        private string _sign_type = "";

        /// <summary>
        /// 小程序ID
        /// </summary>
        /// <remarks>微信分配的小程序ID</remarks>
        public string appid { get; set; }
        /// <summary>
        /// 小程序的 app secret
        /// </summary>
        public string secret { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        /// <remarks>微信支付分配的商户号</remarks>
        public string mch_id { get; set; }
        /// <summary>
        /// 微信订单号
        /// </summary>
        /// <remarks>微信的订单号，优先使用。</remarks>
        public string transaction_id { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        /// <remarks>商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。</remarks>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 签名类型
        /// </summary>
        /// <remarks>签名类型，默认为MD5，支持HMAC-SHA256和MD5</remarks>
        public string sign_type
        {
            get
            {
                if (string.IsNullOrEmpty(_sign_type))
                {
                    return "MD5";
                }
                return _sign_type;
            }
            set { _sign_type = value; }
        }

    }
}