using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Response
{
    /// <summary>
    /// 按订单申请退款的输出内容
    /// </summary>
    public class RefundResponse : Wlniao.Handler.IResponse
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
        /// 退款状态
        /// </summary>
        /// <remarks>SUCCESS-退款成功 CHANGE-退款异常 REFUNDCLOSE—退款关闭</remarks>
        public string refund_status { get; set; }
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
        /// 微信退款单号
        /// </summary>
        public string refund_id { get; set; }
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
    }
}