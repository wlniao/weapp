using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Request
{
    /// <summary>
    /// 按订单申请退款的请求参数
    /// </summary>
    public class RefundRequest : Wlniao.Handler.IRequest
    {
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
        /// <remarks>商户系统内部的退款单号，商户系统内部唯一，只能是数字、大小写字母，同一退款单号多次请求只退一笔。</remarks>
        public string out_refund_no { get; set; }
        /// <summary>
        /// 订单金额 
        /// </summary>
        /// <remarks>订单总金额，单位为分</remarks>
        public int total_fee { get; set; }
        /// <summary>
        /// 申请退款金额 
        /// </summary>
        /// <remarks>申请退款金额总金额，单位为分</remarks>
        public int refund_fee { get; set; }
        /// <summary>
        /// 退款原因
        /// </summary>
        /// <remarks>若商户传入，会在下发给用户的退款消息中体现退款原因</remarks>
        public string refund_desc { get; set; }
        /// <summary>
        /// 通知地址 异步接收微信支付结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。
        /// </summary>
        public string notify_url { get; set; }
    }
}