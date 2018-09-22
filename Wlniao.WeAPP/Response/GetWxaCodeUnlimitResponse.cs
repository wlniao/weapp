using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Response
{
    /// <summary>
    /// 获取无限制微信小程序码的输出内容
    /// </summary>
    public class GetWxaCodeUnlimitResponse : Wlniao.Handler.IResponse
    {
        /// <summary>
        /// 二维码数据
        /// </summary>
        public byte[] image { get; set; }
    }
}
