using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Response
{
    /// <summary>
    /// 小程序支付统一下单的输出内容
    /// </summary>
    public class UnifiedOrderResponse : Wlniao.Handler.IResponse
    {
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
        /// 交易类型
        /// </summary>
        public string trade_type { get; set; }
        /// <summary>
        /// 预支付交易会话标识
        /// </summary>
        /// <remarks>微信生成的预支付会话标识，用于后续接口调用中使用，该值有效期为2小时</remarks>
        public string prepay_id { get; set; }
        /// <summary>
        /// 二维码链接
        /// </summary>
        /// <remarks>trade_type为NATIVE时有返回，用于生成二维码，展示给用户进行扫码支付</remarks>
        public string code_url { get; set; }
    }
}
