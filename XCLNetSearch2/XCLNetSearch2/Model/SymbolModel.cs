using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XCLNetSearch.Model
{
    /// <summary>
    /// 条件符号model
    /// </summary>
    public class SymbolModel
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 符号类型
        /// </summary>
        public EnumTypes.SymbolType SymbolType { get; set; }

        /// <summary>
        /// 前台显示名
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 标志名，唯一
        /// </summary>
        public string FlagName { get; set; }

        /// <summary>
        /// 实际名称，如like、(、=、or
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否需要有输入值(如 is null的时候就不需要输入值)
        /// </summary>
        public bool IsNeedValue { get; set; }

        /// <summary>
        /// 值的格式，如："{0}%",其中{0}表示在输入值的后面都加一个%
        /// </summary>
        public string ValueFormat { get; set; }

        /// <summary>
        /// 是否为null或not null
        /// </summary>
        public bool IsNullOrNotNull
        {
            get { return this.FlagName == "IsNull" || this.FlagName == "IsNotNull"; }
        }
    }
}
