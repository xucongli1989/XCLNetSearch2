using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XCLNetSearch.Model
{
    /// <summary>
    /// 数据类型
    /// </summary>
    public class DataTypeInfoModel
    {
        /// <summary>
        /// 所属数据库
        /// </summary>
        public EnumTypes.DataBaseType DataBase
        {
            get;
            set;
        }

        /// <summary>
        /// 类型名
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库中的类型
        /// </summary>
        public System.Data.SqlDbType SqlType
        {
            get;
            set;
        }

        /// <summary>
        /// 分类，如“字符串，数字”
        /// </summary>
        public EnumTypes.SymbolDataType SymbolDataType
        {
            get;
            set;
        }

        /// <summary>
        /// 允许的比较符号
        /// </summary>
        public List<Model.SymbolModel> AllowCompareSymbol
        {
            get;
            set;
        }
    }
}
