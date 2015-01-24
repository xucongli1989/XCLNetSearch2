using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XCLNetSearch.Model
{
    /// <summary>
    /// 查询项实体（最终会根据此实体解析为SQL）
    /// </summary>
    public class QueryItemModel
    {
        /// <summary>
        /// 左括号Model
        /// </summary>
        public Model.SymbolModel LeftBracketModel
        {
            get;
            set;
        }

        /// <summary>
        /// 字段Model
        /// </summary>
        public Model.QueryFieldModel FieldModel
        {
            get;
            set;
        }

        /// <summary>
        /// 比较符号Model
        /// </summary>
        public Model.SymbolModel CompareModel
        {
            get;
            set;
        }

        /// <summary>
        /// 查询输入的值（不带符号的格式，也就是输入的值）
        /// </summary>
        public object FieldValue { get; set; }

        /// <summary>
        /// 查询输入的值（带符号的格式）
        /// 如：若符号的数据格式为"{0}%"，则此值就是"查询输入的值%"
        /// </summary>
        public object FieldValueWithFormat
        {
            get {
                return string.IsNullOrEmpty(this.CompareModel.ValueFormat) ? Convert.ToString(this.FieldValue) : string.Format(this.CompareModel.ValueFormat, this.FieldValue);
            }
        }

        /// <summary>
        /// 右括号Model
        /// </summary>
        public Model.SymbolModel RightBracketModel
        {
            get;
            set;
        }

        /// <summary>
        /// 逻辑符号Model
        /// </summary>
        public Model.SymbolModel LogicModel
        {
            get;
            set;
        }
    }
}
