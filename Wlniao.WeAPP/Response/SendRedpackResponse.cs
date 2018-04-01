using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Response
{
    /// <summary>
    /// 发送现金红包的输出内容
    /// </summary>
    public class SendRedpackResponse : Wlniao.Handler.IResponse
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
        public string wxappid { get; set; }
        /// <summary>
        /// 用户OpenId
        /// </summary>
        /// <remarks>接受收红包的用户用户在wxappid下的openid</remarks>
        public string re_openid { get; set; }
        /// <summary>
        /// 商户号（服务商模式下为服务商商户号）
        /// </summary>
        /// <remarks>微信支付分配的商户号</remarks>
        public string mch_id { get; set; }
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
        /// <remarks>商户订单号（每个订单号必须唯一） 组成：mch_id+yyyymmdd+10位一天内不能重复的数字。</remarks>
        public string mch_billno { get; set; }
        /// <summary>
        /// 微信单号
        /// </summary>
        /// <remarks>红包订单的微信单号</remarks>
        public string send_listid { get; set; }
        /// <summary>
        /// 红包金额
        /// </summary>
        /// <remarks>红包金额，单位为分</remarks>
        public int total_amount { get; set; }
    }
}