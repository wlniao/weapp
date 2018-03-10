using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Response
{
    /// <summary>
    /// 获取微信小程序码的输出内容
    /// </summary>
    public class GetWxaCodeResponse : Wlniao.Handler.IResponse
    {
        /// <summary>
        /// 二维码数据
        /// </summary>
        public byte[] image { get; set; }
    }
}
