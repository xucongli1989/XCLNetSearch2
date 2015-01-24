using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace XCLNetSearch
{
    /// <summary>
    /// 公共方法类
    /// </summary>
    internal class CommonHelper
    {
        /// <summary>
        /// Escape
        /// </summary>
        public static string Escape(string s)
        {
            return string.IsNullOrEmpty(s) ? "" : Microsoft.JScript.GlobalObject.escape(s);
        }

        /// <summary>
        /// Unescape
        /// </summary>
        public static string Unescape(string s)
        {
            return string.IsNullOrEmpty(s) ? "" : Microsoft.JScript.GlobalObject.unescape(s);
        }

        /// <summary>
        /// 根据guid生成字符串(16位)
        /// 例如：aded0a2611f8aa4a
        /// </summary>
        public static string GenerateStringId()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }

        /// <summary>
        /// 将sql数据类型的string值转为c#的类型
        /// </summary>
        public static object ConvertValueBySqlDbType(SqlDbType sqlType ,string value)
        {
            object obj = value;
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    obj = Int64.Parse(value);
                    break;
                case SqlDbType.Bit:
                    obj = Boolean.Parse(value);
                    break;
                case SqlDbType.SmallDateTime:
                case SqlDbType.DateTime:
                    obj= DateTime.Parse(value);
                    break;
                case SqlDbType.SmallMoney:
                case SqlDbType.Money:
                case SqlDbType.Decimal:
                    obj=decimal.Parse(value);
                    break;
                case SqlDbType.Float:
                    obj=double.Parse(value);
                    break;
                case SqlDbType.Int:
                    obj = Int32.Parse(value);
                    break;
                case SqlDbType.SmallInt:
                    obj = Int16.Parse(value);
                    break;
                case SqlDbType.TinyInt:
                    obj= byte.Parse(value);
                    break;
            }
            return obj;
        }

        /// <summary>
        /// 将符号信息转为option
        /// </summary>
        public static string GetOptionsBySymbolList(List<Model.SymbolModel> lst)
        {
            StringBuilder str = new StringBuilder();
            if (null != lst && lst.Count > 0)
            {
                lst = lst.OrderBy(k => k.Sort).ToList();
                foreach (Model.SymbolModel m in lst)
                {
                    str.AppendFormat(@"<option value=""{0}"" IsNeedValue=""{2}"">{1}</option>", m.ID, m.DisplayName,Convert.ToString(m.IsNeedValue).ToLower());
                }
            }
            return str.ToString();
        }

        /// <summary>
        /// 根据数据库和数据库数据类型，返回用于SQLParams的类型
        /// </summary>
        /// <param name="db">数据库</param>
        /// <param name="typeName">数据类型名，如“BigInt、Varchar”</param>
        public static System.Data.SqlDbType GetSqlDataType(EnumTypes.DataBaseType db, string typeName)
        {
            return (System.Data.SqlDbType)Enum.Parse(typeof(System.Data.SqlDbType), typeName);
        }

        /// <summary>
        /// 根据查询的URL值，返回查询ModelList
        /// </summary>
        /// <param name="queryUrl">查询的URL值</param>
        public static List<Model.QueryItemModel> GetQueryItemModelListByUrl(string queryUrl,List<XCLNetSearch.Model.QueryFieldModel> queryModelList)
        {
            List<Model.QueryItemModel> lst = null;
            if (!string.IsNullOrEmpty(queryUrl))
            {
                string[] queryItemArray = queryUrl.Split(',');
                if (null != queryItemArray && queryItemArray.Length > 0)
                {
                    lst = new List<Model.QueryItemModel>();
                    Model.QueryItemModel model = null;
                    for (int i = 0; i < queryItemArray.Length; i++)
                    {
                        model = new Model.QueryItemModel();
                        //0:左括号,1:字段名,2:比较符,3:输入值,4:右括号,5:逻辑符
                        string[] q = queryItemArray[i].Split('|');
                        try
                        {
                            if (!string.IsNullOrEmpty(q[0]))
                            {
                                model.LeftBracketModel = Search.SymbolList.Find(k => k.ID == Int32.Parse(q[0]));
                            }
                            if (!string.IsNullOrEmpty(q[1]))
                            { 
                               model.FieldModel = queryModelList.Find(k => k.FieldName == CommonHelper.Unescape(q[1]).Trim()); 
                            }
                            if (!string.IsNullOrEmpty(q[2]))
                            {
                                model.CompareModel = Search.SymbolList.Find(k => k.ID == Int32.Parse(q[2]));
                            }

                            #region 字段格式处理
                            model.FieldValue = CommonHelper.Unescape(q[3]).Trim();
                            if (model.CompareModel.IsNeedValue)
                            {
                                model.FieldValue = ConvertValueBySqlDbType(model.FieldModel.InputValueDataType, Convert.ToString(model.FieldValue));
                            }
                            #endregion

                            if (!string.IsNullOrEmpty(q[4]))
                            { 
                                model.RightBracketModel = Search.SymbolList.Find(k => k.ID == Int32.Parse(q[4]));
                            }
                            if (!string.IsNullOrEmpty(q[5]))
                            { 
                                model.LogicModel = Search.SymbolList.Find(k => k.ID == Int32.Parse(q[5]));
                            }
                            lst.Add(model);
                        }
                        catch (Exception e) {}
                    }
                }
            }
            return lst;
        }

        /// <summary>
        /// 返回查询URL参数值的ModelList形式的json形式
        /// </summary>
        public static string GetUrlParamJsonValueByQueryItemModelList(List<Model.QueryItemModel> lst)
        {
            string str = "[]";
            if (null != lst && lst.Count > 0)
            {
                StringBuilder strBld = new StringBuilder();
                foreach (var m in lst)
                {
                    strBld.AppendFormat(@"{{""LeftBracket"":""{0}"",""Field"":""{1}"",""Compare"":""{2}"",""FieldValue"":""{3}"",""RightBracket"":""{4}"",""Logic"":""{5}""}},",
                        null == m.LeftBracketModel ? "" :m.LeftBracketModel.ID.ToString(),
                        null == m.FieldModel?"":CommonHelper.Escape(m.FieldModel.FieldName),
                        null == m.CompareModel?"":m.CompareModel.ID.ToString(),
                        null == m.FieldValue?"":CommonHelper.Escape(Convert.ToString(m.FieldValue)),
                        null == m.RightBracketModel?"":m.RightBracketModel.ID.ToString(),
                        null == m.LogicModel?"":m.LogicModel.ID.ToString()
                    );
                }
                str =string.Format("[{0}]", strBld.ToString().TrimEnd(','));
            }
            return str;
        }

        /// <summary>
        /// 生成SQL语句,结果存至searchModel中
        /// </summary>
        public static void GetSQLWhereByQueryItemModelList(List<Model.QueryItemModel> lst,Model.SearchModel searchModel)
        {
            List<System.Data.SqlClient.SqlParameter> paramLst = new List<System.Data.SqlClient.SqlParameter>();
            if (null != searchModel.OldParameters)
            {
                paramLst.AddRange(searchModel.OldParameters.ToList());
            }

            StringBuilder strSqlWhere = new StringBuilder();
            if (null != lst && lst.Count > 0)
            {
                #region
                for (int i = 0; i < lst.Count; i++)
                {
                    Model.QueryItemModel m = lst[i];
                    //左括号
                    if (null != m.LeftBracketModel)
                    {
                        strSqlWhere.Append(m.LeftBracketModel.Name);
                    }
                    //字段名
                    string fieldName = m.FieldModel.FieldName;
                    if (!string.IsNullOrEmpty(m.FieldModel.FieldNameFormat))
                    {
                        fieldName = string.Format(m.FieldModel.FieldNameFormat, fieldName);
                    }
                    #region 有null时，算长度
                    if (m.CompareModel.IsNullOrNotNull)
                    {
                        fieldName = string.Format("isnull(len(ltrim(rtrim({0}))),0)", fieldName);
                    } 
                    #endregion
                    strSqlWhere.Append(fieldName);
                    //比较符
                    strSqlWhere.Append(string.Format(" {0} ", m.CompareModel.Name));
                    //输入值
                    string fieldParamName = string.Format("@{0}{1}", m.FieldModel.FieldName, i);
                    if (m.CompareModel.IsNeedValue)
                    {
                        strSqlWhere.Append(string.IsNullOrEmpty(m.FieldModel.FieldValueFormat) ? fieldParamName : string.Format(m.FieldModel.FieldValueFormat, fieldParamName));
                    }

                    //右括号
                    if (null != m.RightBracketModel)
                    {
                        strSqlWhere.Append(m.RightBracketModel.Name);
                    }
                    //逻辑符
                    if (i != lst.Count - 1)
                    {
                        strSqlWhere.AppendFormat(" {0} ", m.LogicModel.Name);
                    }

                    if (m.CompareModel.IsNeedValue)
                    {
                        System.Data.SqlClient.SqlParameter sqlParamObj = m.FieldModel.InputValueDataSize == null ? new System.Data.SqlClient.SqlParameter(fieldParamName, m.FieldModel.InputValueDataType) : new System.Data.SqlClient.SqlParameter(fieldParamName, m.FieldModel.InputValueDataType,Convert.ToInt32(m.FieldModel.InputValueDataSize));
                        sqlParamObj.Value = m.FieldValueWithFormat;
                        paramLst.Add(sqlParamObj);
                    }
                } 
                #endregion
            }
            searchModel.NewParameters = paramLst.ToArray();
            searchModel.NewWhere = strSqlWhere.ToString();

            if (!string.IsNullOrEmpty(searchModel.OldWhere))
            {
                searchModel.NewWhere = string.Format("({0}) {1} ",searchModel.OldWhere, string.IsNullOrEmpty(searchModel.NewWhere)?"":string.Format(" and ({0}) ",searchModel.NewWhere));
            }
        }

        /// <summary>
        /// 正则查找开始字符串与结束字符串的中间字符串
        /// </summary>
        public static string GetCenterString(string str, string s, string e)
        {
            Regex rg = new Regex("(?<=(" + s + "))[.\\s\\S]*?(?=(" + e + "))", RegexOptions.Multiline | RegexOptions.Singleline);
            return rg.Match(str).Value;
        }

        /// <summary>
        /// 防止SQL注入
        /// </summary>
        public static string No_SqlHack(string inputStr)
        {
            string NoSqlHack_AllStr = "|;|and|chr(|exec|insert|select|delete|from|update|mid(|master.|";
            string SqlHackGet = inputStr;
            string[] AllStr = NoSqlHack_AllStr.Split('|');
            string[] GetStr = SqlHackGet.Split(' ');
            if (SqlHackGet != "")
            {
                for (int j = 0; j < GetStr.Length; j++)
                {
                    for (int i = 0; i < AllStr.Length; i++)
                    {
                        if (GetStr[j].ToLower() == AllStr[i].ToLower())
                        {
                            GetStr[j] = "";
                            break;
                        }
                    }
                }
                SqlHackGet = "";
                for (int i = 0; i < GetStr.Length; i++)
                {
                    SqlHackGet += GetStr[i].ToString() + " ";
                }
                return SqlHackGet.TrimEnd(' ').Replace("'", "").Replace(",", "");
            }
            else
            {
                return "";
            }
        }
    }
}
