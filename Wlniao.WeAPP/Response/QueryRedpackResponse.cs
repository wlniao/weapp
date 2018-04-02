using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Response
{
    /// <summary>
    /// 现金红包查询的输出内容
    /// </summary>
    public class QueryRedpackResponse : Wlniao.Handler.IResponse
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
        /// 红包状态 
        /// </summary>
        /// <remarks>SENDING:发放中 SENT:已发放待领取 FAILED：发放失败 RECEIVED:已领取 RFUND_ING:退款中 REFUND:已退款</remarks>
        public string status { get; set; }
        /// <summary>
        /// 失败原因 
        /// </summary>
        /// <remarks>发送失败原因</remarks>
        public string reason { get; set; }
        /// <summary>
        /// 商户号
        /// </summary>
        /// <remarks>微信支付分配的商户号</remarks>
        public string mch_id { get; set; }
        /// <summary>
        /// 商户订单号
        /// </summary>
        /// <remarks>商户订单号（每个订单号必须唯一） 组成：mch_id+yyyymmdd+10位一天内不能重复的数字。</remarks>
        public string mch_billno { get; set; }
        /// <summary>
        /// 红包单号 
        /// </summary>
        /// <remarks>使用API发放现金红包时返回的红包单号 </remarks>
        public string detail_id { get; set; }
        /// <summary>
        /// 发放类型
        /// </summary>
        /// <remarks>API:通过API接口发放 UPLOAD:通过上传文件方式发放 ACTIVITY:通过活动方式发放</remarks>
        public string send_type { get; set; }
        /// <summary>
        /// 红包类型
        /// </summary>
        /// <remarks>GROUP:裂变红包 NORMAL:普通红包</remarks>
        public string hb_type { get; set; }
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
        /// 红包发送时间  
        /// </summary>
        /// <remarks>红包发送时间，格式为yyyy-MM-dd HH:mm:ss</remarks>
        public string send_time { get; set; }
        /// <summary>
        /// 红包的退款时间  
        /// </summary>
        /// <remarks>红包的退款时间（如果其未领取的退款） ，格式为yyyy-MM-dd HH:mm:ss</remarks>
        public string refund_time { get; set; }
        /// <summary>
        /// 红包金额
        /// </summary>
        /// <remarks>红包金额，单位为分</remarks>
        public int refund_amount { get; set; }
        /// <summary>
        /// 领取红包的时间  
        /// </summary>
        /// <remarks>领取红包的时间，格式为yyyy-MM-dd HH:mm:ss</remarks>
        public string rcv_time { get; set; }
        /// <summary>
        /// 领取金额 
        /// </summary>
        /// <remarks>领取金额 ，单位为分</remarks>
        public int amount { get; set; }
        /// <summary>
        /// 领取红包的openid
        /// </summary>
        /// <remarks>领取红包的openid。</remarks>
        public string openid { get; set; }

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
        /// 红包发送时间 UNIXTIME
        /// </summary>
        public long SendTime { get; set; }
        /// <summary>
        /// 红包的退款时间 UNIXTIME
        /// </summary>
        public long RefundTime { get; set; }
        /// <summary>
        /// 领取红包的时间 UNIXTIME
        /// </summary>
        public long ReceivedTime { get; set; }
    }
}