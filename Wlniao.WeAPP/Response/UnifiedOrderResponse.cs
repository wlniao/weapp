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
        /// <summary>
        /// 随机字符串
        /// </summary>
        /// <remarks>微信返回的随机字符串</remarks>
        public string nonce_str { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <remarks>时间戳从1970年1月1日00:00:00至今的秒数,即当前的时间</remarks>
        public string timeStamp { get; set; }
        /// <summary>
        /// 签名方式
        /// </summary>
        /// <remarks>签名类型，默认为MD5，支持HMAC-SHA256和MD5。注意此处需与统一下单的签名类型一致</remarks>
        public string signType { get; set; }



        /// <summary>
        /// 小程序调起支付数据签名
        /// </summary>
        /// <returns></returns>
        public string paySign { get; set; }
        /// <summary>
        /// 小程序微信支付调起参数
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public object PayRequet
        {
            get
            {
                return new
                {
                    timeStamp = timeStamp
                    ,
                    nonceStr = nonce_str
                    ,
                    package = "prepay_id=" + prepay_id
                    ,
                    signType = signType
                    ,
                    paySign = paySign
                };
            }
        }
    }
}