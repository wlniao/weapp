using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Response
{
    /// <summary>
    /// 刷卡支付提交的输出内容
    /// </summary>
    public class MicropayResponse : Wlniao.Handler.IResponse
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
        /// 小程序/服务商ID
        /// </summary>
        /// <remarks>微信分配的小程序ID或服务商公众账号ID</remarks>
        public string appid { get; set; }
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
        /// 设备号 调用接口提交的终端设备号
        /// </summary>
        public string device_info { get; set; }
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
        /// 用户标识 用户在商户appid 下的唯一标识用户在商户appid 下的唯一标识
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户是否关注公众账号，仅在公众账号类型支付有效，取值范围：Y或N;Y-关注;N-未关注
        /// </summary>
        public string is_subscribe { get; set; }
        /// <summary>
        /// 用户子标识
        /// </summary>
        /// <remarks>子商户appid下用户唯一标识，如需返回则请求时需要传sub_appid</remarks>
        public string sub_openid { get; set; }
        /// <summary>
        /// 用户是否关注子公众账号，仅在公众账号类型支付有效，取值范围：Y或N;Y-关注;N-未关注
        /// </summary>
        public string sub_is_subscribe { get; set; }
        /// <summary>
        /// 交易类型 为MICROPAY(即扫码支付)
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 付款银行 采用字符串类型的银行标识
        /// </summary>
        public string bank_type { get; set; }
        /// <summary>
        /// 标价币种 符合ISO 4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        public string fee_type { get; set; }
        /// <summary>
        /// 标价金额
        /// </summary>
        /// <remarks>订单总金额，单位为分</remarks>
        public int total_fee { get; set; }
        /// <summary>
        /// 现金支付币种 符合ISO 4217标准的三位字母代码，默认人民币：CNY
        /// </summary>
        public string cash_fee_type { get; set; }
        /// <summary>
        /// 现金支付金额
        /// </summary>
        /// <remarks>订单现金支付金额，单位为分</remarks>
        public int cash_fee { get; set; }
        /// <summary>
        /// 应结订单金额
        /// </summary>
        /// <remarks>当订单使用了免充值型优惠券后返回该参数，应结订单金额=订单金额-免充值优惠券金额</remarks>
        public int settlement_total_fee { get; set; }
        /// <summary>
        /// 代金券金额
        /// </summary>
        /// <remarks>代金券金额小于订单金额，订单金额-代金券金额=现金支付金额</remarks>
        public int coupon_fee { get; set; }
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
        /// 附加数据 商户携带的自定义数据
        /// </summary>
        public string attach { get; set; }
        /// <summary>
        /// 支付完成时间 
        /// </summary>
        /// <remarks>订单支付时间，格式为yyyyMMddHHmmss，如2009年12月25日9点10分10秒表示为20091225091010</remarks>
        public string time_end { get; set; }
        /// <summary>
        /// 支付时间UNIXTIME
        /// </summary>
        public long PayTime { get; set; }
        

        /// <summary>
        /// 用户是否关注公众账号
        /// </summary>
        public bool IsSubscribe
        {
            get
            {
                return is_subscribe == "Y";
            }
        }
        /// <summary>
        /// 用户是否关注子公众账号
        /// </summary>
        public bool IsSubscribeSub
        {
            get
            {
                return sub_is_subscribe == "Y";
            }
        }
    }
}