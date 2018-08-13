using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Request
{
    /// <summary>
    /// 刷卡支付提交的请求参数
    /// </summary>
    public class MicropayRequest : Wlniao.Handler.IRequest
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
        /// 商品描述
        /// </summary>
        /// <remarks>商品简单描述，该字段请按照规范传递</remarks>
        public string body { get; set; }
        /// <summary>
        /// 商品详情
        /// </summary>
        /// <remarks>单品优惠功能字段，该字段请按照规范传递</remarks>
        public string detail { get; set; }
        /// <summary>
        /// 附加数据
        /// </summary>
        /// <remarks>附加数据，在查询API和支付通知中原样返回，该字段主要用于商户携带订单的自定义数据</remarks>
        public string attach { get; set; }
        /// <summary>
        /// 商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。
        /// </summary>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        /// <remarks>自定义参数，可以为终端设备号(门店号或收银设备ID)，PC网页或公众号内支付可以传"WEB"</remarks>
        public string device_info { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        /// <remarks>订单总金额，单位为分</remarks>
        public int total_fee { get; set; }
        /// <summary>
        /// 标价币种
        /// </summary>
        /// <remarks>符合ISO 4217标准的三位字母代码，默认人民币：CNY</remarks>
        public string fee_type { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        /// <remarks>APP和网页支付提交用户端ip，Native支付填调用微信支付API的机器IP</remarks>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 订单优惠标记 订单优惠标记，代金券或立减优惠功能的参数
        /// </summary>
        public string goods_tag { get; set; }
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
        /// 指定支付方式
        /// </summary>
        /// <remarks>上传此参数no_credit--可限制用户不能使用信用卡支付</remarks>
        public string limit_pay { get; set; }
        /// <summary>
        /// 授权码
        /// </summary>
        /// <remarks>扫码支付授权码，设备读取用户微信中的条码或者二维码信息</remarks>
        public string auth_code { get; set; }
    }
}