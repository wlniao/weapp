using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Request
{
    /// <summary>
    /// 现金红包查询的请求参数
    /// </summary>
    public class QueryRedpackRequest : Wlniao.Handler.IRequest
    {
        private string _bill_type = "";

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
        /// 商户号（服务商模式下为服务商商户号）
        /// </summary>
        /// <remarks>微信支付分配的商户号</remarks>
        public string mch_id { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        /// <remarks>商户订单号（每个订单号必须唯一） 组成：mch_id+yyyymmdd+10位一天内不能重复的数字。</remarks>
        public string mch_billno { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        /// <remarks>MCHT:通过商户订单号获取红包信息。</remarks>
        public string bill_type
        {
            get
            {
                if (string.IsNullOrEmpty(_bill_type))
                {
                    return "MCHT";
                }
                return _bill_type;
            }
            set { _bill_type = value; }
        }

    }
}