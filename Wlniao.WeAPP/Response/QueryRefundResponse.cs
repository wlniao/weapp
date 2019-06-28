using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Response
{
    /// <summary>
    /// 微信支付查询退款 的输出内容
    /// </summary>
    public class QueryRefundResponse : Wlniao.Handler.IResponse
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
        /// 订单总退款次数
        /// </summary>
        /// <remarks>订单总共已发生的部分退款次数，当请求参数传入offset后有返回</remarks>
        public int total_refund_count { get; set; }
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
        /// 标价金额
        /// </summary>
        /// <remarks>订单总金额，单位为分</remarks>
        public int total_fee { get; set; }
        /// <summary>
        /// 应结订单金额
        /// </summary>
        /// <remarks>当订单使用了免充值型优惠券后返回该参数，应结订单金额=订单金额-免充值优惠券金额。 </remarks>
        public int settlement_total_fee { get; set; }
        /// <summary>
        /// 标价币种
        /// </summary>
        /// <remarks>符合ISO 4217标准的三位字母代码，默认人民币：CNY</remarks>
        public string fee_type { get; set; }
        /// <summary>
        /// 现金支付金额 
        /// </summary>
        /// <remarks>现金支付金额订单现金支付金额。 </remarks>
        public int cash_fee { get; set; }

        /// <summary>
        /// 退款笔数
        /// </summary>
        /// <remarks>当前返回退款笔数</remarks>
        public int refund_count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<RefundDetails> details { get; set; }
        /// <summary>
        /// 退款时间UNIXTIME
        /// </summary>
        public long RefundTime { get; set; }

        /// <summary>
        /// 退款明细
        /// </summary>
        public class RefundDetails
        {
            /// <summary>
            /// 商户退款单号
            /// </summary>
            /// <remarks>商户系统内部的退款单号，商户系统内部唯一，只能是数字、大小写字母_-|*@ ，同一退款单号多次请求只退一笔。</remarks>
            public string out_refund_no { get; set; }
            /// <summary>
            /// 微信退款单号
            /// </summary>
            public string refund_id { get; set; }
            /// <summary>
            /// 退款渠道 ORIGINAL—原路退款 BALANCE—退回到余额 OTHER_BALANCE—原账户异常退到其他余额账户 OTHER_BANKCARD—原银行卡异常退到其他银行卡
            /// </summary>
            public string refund_channel { get; set; }
            /// <summary>
            /// 申请退款金额
            /// </summary>
            public int refund_fee { get; set; }
            /// <summary>
            /// 退款金额
            /// </summary>
            /// <remarks>退款金额=申请退款金额-非充值代金券退款金额，退款金额小于等于申请退款金额</remarks>
            public int settlement_refund_fee { get; set; }
            /// <summary>
            /// 代金券类型 CASH--充值代金券  NO_CASH---非充值优惠券
            /// </summary>
            public string coupon_type { get; set; }
            /// <summary>
            /// 总代金券退款金额
            /// </summary>
            /// <remarks>代金券退款金额小于等于退款金额，退款金额-代金券或立减优惠退款金额为现金</remarks>
            public int coupon_refund_fee { get; set; }
            /// <summary>
            /// 退款状态： SUCCESS—退款成功 REFUNDCLOSE—退款关闭。 PROCESSING—退款处理中 CHANGE—退款异常，退款到银行发现用户的卡作废或者冻结了，导致原路退款银行卡失败，可前往商户平台（pay.weixin.qq.com）-交易中心，手动处理此笔退款。$n为下标，从0开始编号。
            /// </summary>
            public string refund_status { get; set; }
            /// <summary>
            /// 取当前退款单的退款入账方 1）退回银行卡：{银行名称}{卡类型}{卡尾号2）退回支付用户零钱:支付用户零钱3）退还商户:商户基本账户商户结算银行账户4）退回支付用户零钱通:支付用户零钱通
            /// </summary>
            public string refund_recv_accout { get; set; }
            /// <summary>
            /// 退款成功时间，当退款状态为退款成功时有返回
            /// </summary>
            public string refund_success_time { get; set; }
            /// <summary>
            /// 退款时间UNIXTIME
            /// </summary>
            public long RefundTime { get; set; }
        }
    }
}