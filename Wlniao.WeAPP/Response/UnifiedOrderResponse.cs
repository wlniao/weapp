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
        public string paySign { get; set; }

        /// <summary>
        /// 小程序支付调起参数
        /// </summary>
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