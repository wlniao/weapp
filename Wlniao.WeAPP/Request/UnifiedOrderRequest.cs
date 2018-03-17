using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Request
{
    /// <summary>
    /// 小程序支付统一下单的请求参数
    /// </summary>
    public class UnifiedOrderRequest : Wlniao.Handler.IRequest
    {
        private string _sign_type = "MD5";
        private string _trade_type = "JSAPI";

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
        /// 设备号
        /// </summary>
        /// <remarks>自定义参数，可以为终端设备号(门店号或收银设备ID)，PC网页或公众号内支付可以传"WEB"</remarks>
        public string device_info { get; set; }
        /// <summary>
        /// 签名类型
        /// </summary>
        /// <remarks>签名类型，默认为MD5，支持HMAC-SHA256和MD5</remarks>
        public string sign_type { get { return _sign_type; } set { _sign_type = value; } }
        /// <summary>
        /// 商品描述
        /// </summary>
        /// <remarks>商品简单描述，该字段请按照规范传递</remarks>
        public string body { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        /// <remarks>商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。</remarks>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 标价币种
        /// </summary>
        /// <remarks>符合ISO 4217标准的三位字母代码，默认人民币：CNY</remarks>
        public string fee_type { get; set; }
        /// <summary>
        /// 标价金额
        /// </summary>
        /// <remarks>订单总金额，单位为分</remarks>
        public int total_fee { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        /// <remarks>APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP</remarks>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 交易起始时间
        /// </summary>
        /// <remarks>订单生成时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010</remarks>
        public string time_start { get; set; }
        /// <summary>
        /// 交易结束时间
        /// </summary>
        /// <remarks>订单失效时间，格式为yyyyMMddHHmmss，如2009年12月27日9点10分10秒表示为20091227091010。订单失效时间是针对订单号而言的，由于在请求支付的时候有一个必传参数prepay_id只有两小时的有效期，所以在重入时间超过2小时的时候需要重新请求下单接口获取新的prepay_id</remarks>
        public string time_expire { get; set; }
        /// <summary>
        /// 通知地址
        /// </summary>
        /// <remarks>异步接收微信支付结果通知的回调地址，通知url必须为外网可访问的url，不能携带参数。</remarks>
        public string notify_url { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        /// <remarks>小程序取值如下：JSAPI</remarks>
        public string trade_type { get { return _trade_type; } set { _trade_type = value; } }
        /// <summary>
        /// 商品ID
        /// </summary>
        /// <remarks>trade_type=NATIVE时（即扫码支付），此参数必传。此参数为二维码中包含的商品ID，商户自行定义</remarks>
        public string product_id { get; set; }
        /// <summary>
        /// 指定支付方式
        /// </summary>
        /// <remarks>上传此参数no_credit--可限制用户不能使用信用卡支付</remarks>
        public string limit_pay { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        /// <remarks>trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识。</remarks>
        public string openid { get; set; }
    }
}