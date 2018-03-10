using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Request
{
    /// <summary>
    /// 获取微信小程序码的请求参数
    /// </summary>
    public class GetWxaCodeRequest : Wlniao.Handler.IRequest
    {
        private int _width = 430;
        /// <summary>
        /// 默认进入的页面 不能为空，最大长度 128 字节
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 二维码的宽度 默认430
        /// </summary>
        public int width { get { return _width; } set { _width = value; } }
        /// <summary>
        /// 自动配置线条颜色，如果颜色依然是黑色，则说明不建议配置主色调
        /// </summary>
        public bool auto_color { get; set; }
        /// <summary>
        /// 默认进入的页面 不能为空，最大长度 128 字节
        /// </summary>
        public Color line_color { get; set; }

        public class Color
        {
            public int r { get; set; }
            public int g { get; set; }
            public int b { get; set; }
        }
    }
}