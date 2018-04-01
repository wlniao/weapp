using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Request
{
    /// <summary>
    /// 发送现金红包的请求参数
    /// </summary>
    public class SendRedpackRequest : Wlniao.Handler.IRequest
    {
        /// <summary>
        /// 小程序/服务商ID
        /// </summary>
        /// <remarks>微信分配的小程序ID或服务商公众账号ID</remarks>
        public string wxappid { get; set; }
        /// <summary>
        /// 小程序或服务商的 app secret
        /// </summary>
        public string secret { get; set; }
        /// <summary>
        /// 触达用户的APPID（服务商模式下需要）
        /// </summary>
        /// <remarks>当前调起支付的小程序APPID</remarks>
        public string msgappid { get; set; }
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
        /// 商户订单号
        /// </summary>
        /// <remarks>商户系统内部订单号，要求32个字符内，只能是数字、大小写字母_-|*@ ，且在同一个商户号下唯一。</remarks>
        public string mch_billno { get; set; }
        /// <summary>
        /// 发送者名称
        /// </summary>
        /// <remarks>红包发送者名称</remarks>
        public string send_name { get; set; }
        /// <summary>
        /// 活动名称
        /// </summary>
        /// <remarks>活动名称</remarks>
        public string act_name { get; set; }
        /// <summary>
        /// 红包祝福语
        /// </summary>
        /// <remarks>红包祝福语</remarks>
        public string wishing { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        /// <remarks>备注信息</remarks>
        public string remark { get; set; }
        /// <summary>
        /// 红包个数
        /// </summary>
        public int total_num = 1;
        /// <summary>
        /// 红包金额
        /// </summary>
        /// <remarks>红包金额，单位为分</remarks>
        public int total_amount { get; set; }
        /// <summary>
        /// 用户OpenId
        /// </summary>
        /// <remarks>接受红包的用户 用户在wxappid下的openid，服务商模式下可填入msgappid下的openid。</remarks>
        public string re_openid { get; set; }
        /// <summary>
        /// 终端IP
        /// </summary>
        /// <remarks>调用接口的机器Ip地址</remarks>
        public string client_ip { get; set; }
        /// <summary>
        /// 场景ID
        /// </summary>
        /// <remarks>发放红包使用场景，红包金额大于200时必传</remarks>
        public string scene_id { get; set; }
        /// <summary>
        /// 扣钱方mchid
        /// </summary>
        /// <remarks>常规模式下无效，服务商模式下选填，服务商模式下不填默认扣子商户的钱</remarks>
        public string consume_mch_id { get; set; }
    }
}