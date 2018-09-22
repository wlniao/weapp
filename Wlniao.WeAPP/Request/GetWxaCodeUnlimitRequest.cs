using System;
using System.Collections.Generic;
namespace Wlniao.WeAPP.Request
{
    /// <summary>
    /// 获取无限制小程序二维码的请求参数
    /// </summary>
    public class GetWxaCodeUnlimitRequest : Wlniao.Handler.IRequest
    {
        private int _width = 430;
        /// <summary>
        /// 默认进入的页面 不能为空，最大长度 128 字节
        /// </summary>
        public string page { get; set; }
        /// <summary>
        /// 二维码场景值 最大32个可见字符，只支持数字，大小写英文以及部分特殊字符
        /// </summary>
        public string scene { get; set; }
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

        /// <summary>
        /// RGB颜色
        /// </summary>
        public class Color
        {
            /// <summary>
            /// 
            /// </summary>
            public int r { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int g { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public int b { get; set; }
        }
    }
}