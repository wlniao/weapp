using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Request
{
    /// <summary>
    /// 微信支付查询退款 的请求参数
    /// </summary>
    public class QueryRefundRequest : Wlniao.Handler.IRequest
    {
        private string _sign_type = "";

        /// <summary>
        /// 小程序/服务商ID
        /// </summary>
        /// <remarks>微信分配的小程序ID或服务商公众账号ID</remarks>
        public string appid { get; set; }
        /// <summary>
        /// 小程序或服务商的 app secret
        /// </summary>
        public string secret { get; set; }
        /// <summary>
        /// 小程序的APPID（服务商模式下需要）
        /// </summary>
        /// <remarks>当前调起支付的小程序APPID</remarks>
        public string sub_appid { get; set; }
        /// <summary>
        /// 商户号（服务商模式下为服务商商户号）
        /// </summary>
        /// <remarks>微信支付分配的商户号</remarks>
        public string mch_id { get; set; }
        /// <summary>
        /// 子商户号（服务商模式下需要）
        /// </summary>
        /// <remarks>微信支付分配的子商户号</remarks>
        public string sub_mch_id { get; set; }
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
        /// 商户退款单号
        /// </summary>
        /// <remarks>商户系统内部的退款单号，商户系统内部唯一，只能是数字、大小写字母_-|*@ ，同一退款单号多次请求只退一笔。</remarks>
        public string out_refund_no { get; set; }
        /// <summary>
        /// 微信退款单号
        /// </summary>
        /// <remarks>微信生成的退款单号，在申请退款接口有返回。</remarks>
        public string refund_id { get; set; }
        /// <summary>
        /// 偏移量
        /// </summary>
        /// <remarks>当部分退款次数超过10次时可使用，表示返回的查询结果从这个偏移量开始取记录</remarks>
        public int offset { get; set; }
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