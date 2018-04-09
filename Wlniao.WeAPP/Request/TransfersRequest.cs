using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Request
{
    /// <summary>
    /// 企业付款到零钱的请求参数
    /// </summary>
    public class TransfersRequest : Wlniao.Handler.IRequest
    {
        /// <summary>
        /// 小程序/公众号ID
        /// </summary>
        /// <remarks>微信分配的小程序ID或服务商公众账号ID</remarks>
        public string mch_appid { get; set; }
        /// <summary>
        /// 小程序或服务商的 app secret
        /// </summary>
        public string secret { get; set; }
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
        /// 商户订单号
        /// </summary>
        /// <remarks>商户订单号，需保持唯一性(只能是字母或者数字，不能包含有符号)</remarks>
        public string partner_trade_no { get; set; }
        /// <summary>
        /// 付款金额
        /// </summary>
        /// <remarks>企业付款金额，单位为分</remarks>
        public int amount { get; set; }
        /// <summary>
        /// 企业付款描述信息
        /// </summary>
        /// <remarks>企业付款操作说明信息。必填。</remarks>
        public string desc { get; set; }
        /// <summary>
        /// 用户OpenId
        /// </summary>
        /// <remarks>商户appid下，某用户的openid。</remarks>
        public string openid { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        /// <remarks>该IP同在商户平台设置的IP白名单中的IP没有关联，该IP可传用户端或者服务端的IP</remarks>
        public string spbill_create_ip { get; set; }
        /// <summary>
        /// 校验用户姓名选项
        /// </summary>
        /// <remarks>NO_CHECK：不校验真实姓名 FORCE_CHECK：强校验真实姓名</remarks>
        public bool check_name { get; set; }
        /// <summary>
        /// 收款用户姓名
        /// </summary>
        /// <remarks>收款用户真实姓名。如果check_name设置为FORCE_CHECK，则必填用户真实姓名</remarks>
        public string re_user_name { get; set; }
    }
}