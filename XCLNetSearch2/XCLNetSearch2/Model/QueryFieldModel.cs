using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XCLNetSearch.Model;

namespace XCLNetSearch.Model
{
    /// <summary>
    /// 要查询的字段实体（用于初始化查询控件,由用户初始化时填写）
    /// </summary>
    public class QueryFieldModel
    {
        private EnumTypes.SymbolDataType _fieldSymbolDataType = EnumTypes.SymbolDataType.String;
        private EnumTypes.InputControlTypeEnum _inputControlType = EnumTypes.InputControlTypeEnum.TextBox;

        public QueryFieldModel() { }

        public QueryFieldModel(string fieldName, string fieldNameFormat, string fieldValueFormat, string fieldDisplayName, EnumTypes.SymbolDataType fieldSymbolDataType, string fieldDateFormat, EnumTypes.InputControlTypeEnum inputControlType, string customInputControlAttributes, string inputControlInitValue)
        {
            this.FieldName = fieldName;
            this.FieldNameFormat = fieldNameFormat;
            this.FieldValueFormat = fieldValueFormat;
            this.FieldDisplayName = fieldDisplayName;
            this.FieldSymbolDataType = fieldSymbolDataType;
            this.FieldDateFormat = fieldDateFormat;
            this.InputControlType = inputControlType;
            this.CustomInputControlAttributes = customInputControlAttributes;
            this.InputControlInitValue = inputControlInitValue;
        }

        public QueryFieldModel(string fieldName, string fieldDisplayName, EnumTypes.SymbolDataType fieldSymbolDataType, EnumTypes.InputControlTypeEnum inputControlType)
        {
            this.FieldName = fieldName;
            this.FieldDisplayName = fieldDisplayName;
            this.FieldSymbolDataType = fieldSymbolDataType;
            this.InputControlType = inputControlType;
        }

        public QueryFieldModel(string fieldName, string fieldDisplayName, EnumTypes.SymbolDataType fieldSymbolDataType, EnumTypes.InputControlTypeEnum inputControlType, string inputControlInitValue)
        {
            this.FieldName = fieldName;
            this.FieldDisplayName = fieldDisplayName;
            this.FieldSymbolDataType = fieldSymbolDataType;
            this.InputControlType = inputControlType;
            this.InputControlInitValue = inputControlInitValue;
        }


        /// <summary>
        /// 数据库中对应的字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 字段名的格式字符串，用于生成的SQL中对该字段进行处理
        /// 如："cast({0} as int)"，其中{0}表示该字段，也就是FieldName
        /// </summary>
        public string FieldNameFormat
        {
            get;
            set;
        }

        /// <summary>
        /// 查询输入值的格式，与QueryFieldModel中的FieldNameFormat配合使用
        /// 如:输入的值为"2013-11",想转为日期的形式，那该属性值就是："{0}-01"，也可以对其使用SQL函数等
        /// </summary>
        public string FieldValueFormat { get; set; }

        /// <summary>
        /// 在界面上显示的字段名
        /// </summary>
        public string FieldDisplayName { get; set; }

        /// <summary>
        /// 查询的字段项的数据类型，主要是用于生成后面的比较符号，不同的类型有不同的符号
        /// 注意：这里的类型应该是经过格式化处理后的类型，比如:year(FieldName),那这个类型应该是Number型
        /// </summary>
        public XCLNetSearch.EnumTypes.SymbolDataType FieldSymbolDataType
        {
            get {
                return this._fieldSymbolDataType;
            }
            set {
                this._fieldSymbolDataType = value;
            }
        }

        /// <summary>
        /// 输入值（也就是SQL中的查询参数）的类型，主要是用于生成SQL中的SqlParameter
        /// </summary>
        public System.Data.SqlDbType InputValueDataType { get; set; }

        /// <summary>
        /// 输入值（也就是SQL中的查询参数）的Size，主要是用于生成SQL中的SqlParameter
        /// </summary>
        public int? InputValueDataSize { get; set; }

        /// <summary>
        /// 时间日期格式，如：yyyy-MM-dd
        /// 注：只要此值不为空，则自动调用JS日期选择控件
        /// </summary>
        public string FieldDateFormat
        {
            get;
            set;
        }

        /// <summary>
        /// 输入区域的控件类型
        /// </summary>
        public EnumTypes.InputControlTypeEnum InputControlType
        {
            get { return this._inputControlType; }
            set { this._inputControlType = value; }
        }

        /// <summary>
        /// 输入区域控件的自定义属性，如："rel='???' abc='???'"
        /// 切勿重复定义本控件所使用的属性
        /// </summary>
        public string CustomInputControlAttributes
        {
            get;
            set;
        }

        /// <summary>
        /// 输入区域的初始值，如当输入区域的控件为select时，该初始值可以是里面的options
        /// </summary>
        public string InputControlInitValue { get; set; }
    }
}
