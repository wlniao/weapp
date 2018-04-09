using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Response
{
    /// <summary>
    /// 企业付款到零钱的输出内容
    /// </summary>
    public class TransfersResponse : Wlniao.Handler.IResponse
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
        /// 小程序/公众号ID
        /// </summary>
        /// <remarks>微信分配的小程序ID或服务商公众账号ID</remarks>
        public string mch_appid { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        /// <remarks>微信支付分配的商户号</remarks>
        public string mchid { get; set; }
        /// <summary>
        /// 设备号
        /// </summary>
        /// <remarks>微信支付分配的终端设备号</remarks>
        public string device_info { get; set; }
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
        /// 商户订单号
        /// </summary>
        /// <remarks>商户订单号，需保持历史全局唯一性(只能是字母或者数字，不能包含有符号)。</remarks>
        public string partner_trade_no { get; set; }
        /// <summary>
        /// 微信订单号
        /// </summary>
        /// <remarks>企业付款成功，返回的微信订单号</remarks>
        public string payment_no { get; set; }
        /// <summary>
        /// 微信支付成功时间
        /// </summary>
        /// <remarks>企业付款成功时间 ，格式为yyyy-MM-dd HH:mm:ss</remarks>
        public string payment_time { get; set; }
        /// <summary>
        /// 支付完成时间UNIXTIME
        /// </summary>
        public long PaymentTime { get; set; }
    }
}