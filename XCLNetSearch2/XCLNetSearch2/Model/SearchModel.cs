using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XCLNetSearch.Model
{
    /// <summary>
    /// 查询条件实体
    /// </summary>
    public class SearchModel
    {
        public SearchModel() { }

        public SearchModel(string oldWhere, System.Data.SqlClient.SqlParameter[] oldParameters) 
        {
            this.OldWhere = oldWhere;
            this.OldParameters = oldParameters;
        }

        public SearchModel(string oldWhere)
        {
            this.OldWhere = oldWhere;
        }


        /// <summary>
        /// 原始条件字符串，如"a=b or a=c"
        /// </summary>
        public string OldWhere { get; set; }

        /// <summary>
        /// 原始条件SQL参数
        /// </summary>
        public System.Data.SqlClient.SqlParameter[] OldParameters { get; set; }

        /// <summary>
        /// 根据本控件的查询条件与原始条件合并后的最终查询条件，包含SqlParameter参数，如"A=@A and B=@B"
        /// </summary>
        public string NewWhere
        {
            get;
            set;
        }

        /// <summary>
        /// 根据本控件的查询条件SQL参数与原始条件SQL参数合并后的最终查询条件SQL参数
        /// </summary>
        public System.Data.SqlClient.SqlParameter[] NewParameters
        {
            get;
            set;
        }

        /// <summary>
        /// 根据本控件的查询条件与原始条件合并后的最终查询条件，不包含SqlParameter参数，而是生成最原始的where字符串,如"A='a' and B=12"
        /// 注意：该where值中的输入的值已过滤SQL注入
        /// </summary>
        public string SQLWhere
        {
            get
            {
                string str = this.NewWhere;
                if (!string.IsNullOrEmpty(this.NewWhere)&&null!=this.NewParameters&&this.NewParameters.Length>0)
                {
                    foreach (var m in this.NewParameters)
                    {
                        switch (XCLNetSearch.Search.DataTypeInfoList.Find(k => k.SqlType == m.SqlDbType).SymbolDataType)
                        { 
                            case EnumTypes.SymbolDataType.Number:
                                str = str.Replace(m.ParameterName, Convert.ToString(m.Value));
                                break;
                            default:
                                str = str.Replace(m.ParameterName,string.Format("'{0}'",CommonHelper.No_SqlHack(Convert.ToString(m.Value))));
                                break;
                        }
                    }
                }
                return str;
            }
        }
    }
}
