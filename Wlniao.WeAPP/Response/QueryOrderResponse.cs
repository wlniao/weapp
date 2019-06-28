using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Response
{
    /// <summary>
    /// 微信支付订单查询的输出内容
    /// </summary>
    public class QueryOrderResponse : Wlniao.Handler.IResponse
    {
        /// <summary>
        /// 返回状态码
        /// </summary>
        /// <remarks>SUCCESS/FAIL 此字段是通信标识，非交易标识，交易是否成功需要查看trade_state来判断</remarks>
        public string return_code { get; set; }
        /// <summary>
        /// 返回信息 
        /// </summary>
        /// <remarks>返回信息，如非空，为错误原因</remarks>
        public string return_msg { get; set; }



        /// <summary>
        /// 小程序ID
        /// </summary>
        /// <remarks>微信分配的小程序ID</remarks>
        public string appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        /// <remarks>微信支付分配的商户号</remarks>
        public string mch_id { get; set; }
        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <remarks>微信返回的随机字符串</remarks>
        public string nonce_str { get; set; }
        /// <summary>
        /// 签名 
        /// </summary>
        public string sign { get; set; }
        /// <summary>
        /// 业务结果
        /// </summary>
        /// <remarks>SUCCESS/FAIL</remarks>
        public string result_code { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        /// <remarks>错误码</remarks>
        public string err_code { get; set; }
        /// <summary>
        /// 错误代码描述 
        /// </summary>
        /// <remarks>结果信息描述</remarks>
        public string err_code_des { get; set; }
        /// <summary>
        /// 退款时间UNIXTIME
        /// </summary>
        public long RefundTime { get; set; }


        /// <summary>
        /// 设备号
        /// </summary>
        /// <remarks>自定义参数，可以为终端设备号(门店号或收银设备ID)，PC网页或公众号内支付可以传"WEB"</remarks>
        public string device_info { get; set; }
        /// <summary>
        /// 用户标识
        /// </summary>
        /// <remarks>trade_type=JSAPI，此参数必传，用户在商户appid下的唯一标识。</remarks>
        public string openid { get; set; }
        /// <summary>
        /// 是否关注公众账号 
        /// </summary>
        /// <remarks>用户是否关注公众账号，Y-关注，N-未关注，仅在公众账号类型支付有效</remarks>
        public string is_subscribe { get; set; }
        /// <summary>
        /// 交易类型
        /// </summary>
        /// <remarks>调用接口提交的交易类型，取值如下：JSAPI，NATIVE，APP，MICROPAY</remarks>
        public string trade_type { get; set; }
        /// <summary>
        /// 交易状态
        /// SUCCESS—支付成功 REFUND—转入退款 NOTPAY—未支付 CLOSED—已关闭 REVOKED—已撤销（刷卡支付） USERPAYING--用户支付中 PAYERROR--支付失败(其他原因，如银行返回失败)
        /// </summary>
        /// <remarks>微信生成的预支付会话标识，用于后续接口调用中使用，该值有效期为2小时</remarks>
        public string trade_state { get; set; }
        /// <summary>
        /// 付款银行 
        /// </summary>
        /// <remarks>银行类型，采用字符串类型的银行标识 </remarks>
        public string bank_type { get; set; }
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
        /// 应结订单金额
        /// </summary>
        /// <remarks>当订单使用了免充值型优惠券后返回该参数，应结订单金额=订单金额-免充值优惠券金额。 </remarks>
        public int settlement_total_fee { get; set; }
        /// <summary>
        /// 现金支付金额 
        /// </summary>
        /// <remarks>现金支付金额订单现金支付金额。 </remarks>
        public int cash_fee { get; set; }
        /// <summary>
        /// 微信订单号
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        /// <remarks>商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。</remarks>
        public string out_trade_no { get; set; }
        /// <summary>
        /// 支付完成时间 
        /// </summary>
        /// <remarks>订单支付时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010</remarks>
        public string time_end { get; set; }
        /// <summary>
        /// 交易状态描述
        /// </summary>
        /// <remarks>对当前查询订单状态的描述和下一步操作的指引</remarks>
        public string trade_state_desc { get; set; }
        /// <summary>
        /// 支付时间UNIXTIME
        /// </summary>
        public long PayTime { get; set; }
    }
}