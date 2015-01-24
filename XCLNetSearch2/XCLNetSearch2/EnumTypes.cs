using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XCLNetSearch
{
    /// <summary>
    /// 枚举类
    /// </summary>
    public class EnumTypes
    {
        /// <summary>
        /// 符号类型
        /// </summary>
        public enum SymbolType
        { 
            左括号,
            右括号,
            比较符,
            逻辑符
        }

        /// <summary>
        /// 符号对应的数据类型（仅限比较符）
        /// </summary>
        public enum SymbolDataType
        { 
            /// <summary>
            /// 数字型
            /// </summary>
            Number,
            /// <summary>
            /// 字符串型
            /// </summary>
            String,
            /// <summary>
            /// Text形式的字符串型，ntext/text，查询时只能使用[not] like、is [not] null
            /// </summary>
            TextString,
            /// <summary>
            /// 日期时间型
            /// </summary>
            DateTime
        }

        /// <summary>
        /// 输入区域控件类型
        /// </summary>
        public enum InputControlTypeEnum
        { 
            /// <summary>
            /// 文本框
            /// </summary>
            TextBox,
            /// <summary>
            /// 下拉选择框
            /// </summary>
            Select
        }

        /// <summary>
        /// 提交表单的方式
        /// </summary>
        public enum FormMethodEnum
        { 
            /// <summary>
            /// Get
            /// </summary>
            get,
            /// <summary>
            /// Post
            /// </summary>
            post
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public enum DataBaseType
        {
            /// <summary>
            /// sql server
            /// </summary>
            MSSQL
        }
    }
}
