using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace XCLNetSearch
{
    [ToolboxData("<{0}:Search runat=server></{0}:Search>")]
    public class Search : WebControl
    {
        #region 变量声明
        private static string assName = "XCLNetSearch";
        private static readonly string guidIDString = string.Format("_{0}", CommonHelper.GenerateStringId());
        private string _queryUrlParamName = "where";
        private EnumTypes.FormMethodEnum _formMethod = EnumTypes.FormMethodEnum.get;
        private int _maxLine = 20;
        private bool _isAjaxSubmit = false;
        private bool _isCompressHTML = true;
        private XCLNetSearch.Model.SearchModel _searchModel = null;
        #endregion

        #region 初始化
        /// <summary>
        /// 是否压缩输入的HTML代码
        /// </summary>
        public bool IsCompressHTML
        {
            get { return this._isCompressHTML; }
            set { this._isCompressHTML = value; }
        }

        /// <summary>
        /// 查询条件初始化
        /// </summary>
        public List<XCLNetSearch.Model.QueryFieldModel> QueryFields
        {
            get;
            set;
        }

        /// <summary>
        /// 配置文件
        /// </summary>
        internal static XmlNode XmlConfigNode
        {
            get {
                StreamReader reader = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("{0}.Resources.Config.SymbolConfig.xml", assName)));
                string str = reader.ReadToEnd();
                reader.Close();
                System.Xml.XmlDocument doc = new XmlDocument();
                doc.LoadXml(str);
                XmlNodeList nodeList = doc.SelectNodes(string.Format("//Root//SQLType[@Name='{0}']", Convert.ToString(CurrentDataBaseType)));
                if (null == nodeList || nodeList.Count != 1)
                {
                    throw new Exception("sql符号配置必须存在！");
                }
                return nodeList[0];
            }
        }

        /// <summary>
        /// 符号信息
        /// </summary>
        internal static List<Model.SymbolModel> SymbolList
        {
            get {
                XmlNodeList symList = XmlConfigNode.SelectNodes("//SymbolInfo//Symbol");
                if (null == symList || symList.Count == 0)
                {
                    throw new Exception("具体sql符号必须存在！");
                }
                List<Model.SymbolModel> lst = new List<Model.SymbolModel>();
                foreach (XmlNode node in symList)
                {
                    Model.SymbolModel m = new Model.SymbolModel();
                    XmlAttributeCollection attr = node.Attributes;
                    m.SymbolType = (EnumTypes.SymbolType)Enum.Parse(typeof(EnumTypes.SymbolType), attr["SymbolType"].Value.Trim(), true);
                    m.ID = Int32.Parse(attr["ID"].Value.Trim());
                    m.Name = attr["Name"].Value.Trim().Replace("&gt;",">").Replace("&lt;","<");
                    m.FlagName = attr["FlagName"].Value.Trim();
                    m.DisplayName = attr["DisplayName"].Value.Trim();
                    if (null != attr["IsNeedValue"])
                    {
                        m.IsNeedValue = bool.Parse(attr["IsNeedValue"].Value.Trim());
                    }
                    if (null != attr["ValueFormat"])
                    {
                        m.ValueFormat = attr["ValueFormat"].Value.Trim();
                    }

                    m.Sort = Int32.Parse(attr["Sort"].Value.Trim());
                    lst.Add(m);
                }
                return lst.OrderBy(k=>k.Sort).ToList();
            }
        }

        /// <summary>
        /// 数据类型
        /// </summary>
        internal static List<Model.DataTypeInfoModel> DataTypeInfoList
        {
            get {
                XmlNodeList symList = XmlConfigNode.SelectNodes("//DataTypeInfo//Type");
                if (null == symList || symList.Count == 0)
                {
                    throw new Exception("具体sql数据类型必须存在！");
                }
                List<Model.DataTypeInfoModel> lst = new List<Model.DataTypeInfoModel>();
                foreach (XmlNode node in symList)
                {
                    Model.DataTypeInfoModel m = new Model.DataTypeInfoModel();
                    XmlAttributeCollection attr = node.Attributes;
                    m.DataBase = CurrentDataBaseType;
                    m.Name = attr["Name"].Value.Trim();
                    m.SqlType = CommonHelper.GetSqlDataType(CurrentDataBaseType, m.Name);
                    m.SymbolDataType = (EnumTypes.SymbolDataType)Enum.Parse(typeof(EnumTypes.SymbolDataType), attr["SymbolDataType"].Value.Trim());
                    string[] allows = attr["AllowCompareSymbol"].Value.Split(',');
                    if (null != allows && allows.Length > 0)
                    {
                        m.AllowCompareSymbol = new List<Model.SymbolModel>();
                        for (int i = 0; i < allows.Length; i++)
                        {
                            m.AllowCompareSymbol.Add(SymbolList.Find(k => k.DisplayName == allows[i].Trim()));
                        }
                    }
                    lst.Add(m);
                }
                return lst;
            }
        }

        /// <summary>
        /// 根据符号类型返回符号信息列表list
        /// </summary>
        internal static List<Model.SymbolModel> GetSymbolList(EnumTypes.SymbolType type)
        {
            List<Model.SymbolModel> lst = null;
            if (null != SymbolList & SymbolList.Count > 0)
            {
                lst = SymbolList.Where(k => k.SymbolType == type).ToList();
            }
            return lst;
        }

        /// <summary>
        /// 当前数据库类型,默认为MSSQL
        /// </summary>
        internal static EnumTypes.DataBaseType CurrentDataBaseType
        {
            get {
                return EnumTypes.DataBaseType.MSSQL;
            }
        }

        #endregion

        #region 查询参数相关
        /// <summary>
        /// 查询条件存放的URL中的参数名，默认为where
        /// </summary>
        public string QueryUrlParamName
        {
            get { return this._queryUrlParamName; }
            set { this._queryUrlParamName = value; }
        }

        /// <summary>
        /// 查询条件存放的URL中的参数的值
        /// </summary>
        internal string QueryUrlParamValue
        {
            get {
                string str = string.Empty;
                switch (this.FormMethod)
                { 
                    case EnumTypes.FormMethodEnum.get:
                        str=this.Context.Request.QueryString[this.QueryUrlParamName] ?? "";
                        break;
                    case EnumTypes.FormMethodEnum.post:
                        str = this.Context.Request.Form[this.QueryUrlParamName] ?? "";
                        break;
                }
                return str;
            }
        }

        /// <summary>
        /// 根据url中查询参数的值返回查询项的ModelList
        /// </summary>
        public List<Model.QueryItemModel> GetQueryItemModelList
        {
            get {
                return CommonHelper.GetQueryItemModelListByUrl(this.QueryUrlParamValue, this.QueryFields);
            }
        }
        #endregion

        #region 控件的ID或Name相关
        /// <summary>
        /// 查询控件所在容器的ID
        /// </summary>
        public string ContainerID
        {
            get {
                return string.Format("{0}_{1}", guidIDString, this.UniqueID.Replace('$', '_'));
            }
        }
        #endregion

        #region 表单相关
        /// <summary>
        /// 表单提交的方式，默认为get
        /// </summary>
        public EnumTypes.FormMethodEnum FormMethod
        {
            get { return this._formMethod; }
            set { this._formMethod = value; }
        }

        /// <summary>
        /// 是否以AJAX方式提交表单，默认为false
        /// </summary>
        public bool IsAjaxSubmit
        {
            get { return this._isAjaxSubmit; }
            set { this._isAjaxSubmit = value; }
        }
        #endregion

        #region 界面显示相关
        /// <summary>
        /// 信息提示，用于显示在界面上
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 允许最多的条件数，也就是界面上的条件行数
        /// </summary>
        public int MaxLine
        {
            get{return this._maxLine;}
            set{this._maxLine=value;}
        }
        #endregion

        #region 查询条件处理相关
        /// <summary>
        /// 查询条件及参数(关键，这是所有条件经本控件解析后的最终存放位置)
        /// </summary>
        public XCLNetSearch.Model.SearchModel SearchModel
        {
            get 
            {
                return this._searchModel;
            }
            set 
            {
                this._searchModel = value;
                CommonHelper.GetSQLWhereByQueryItemModelList(this.GetQueryItemModelList, this._searchModel);
                Regex reg = new Regex(@"^[^\(\)]*(((?<o>\()[^\(\)]*)+[^\(\)]*((?<-o>\))[^\(\)]*)+)*(?(o)(?!))$");
                if (!string.IsNullOrEmpty(this._searchModel.NewWhere) && !reg.IsMatch(this._searchModel.NewWhere))
                {
                    this.Message = "括号不匹配！";
                    this._searchModel.NewParameters = this._searchModel.OldParameters;
                    this._searchModel.NewWhere = this._searchModel.OldWhere;
                }
            }
        }
        #endregion

        #region 渲染HTML相关
        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            writer.Write(GetRemark);
            base.RenderBeginTag(writer);
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            writer.Write(GetRemark);
            base.RenderEndTag(writer);
        }

        protected override void RenderContents(HtmlTextWriter output)
        {
            #region 查询字段的select option
            StringBuilder strFields = new StringBuilder();
            foreach (var m in this.QueryFields)
            {
                string allows = "", symbolDataTypeStr = "";
                Model.DataTypeInfoModel dtModel = DataTypeInfoList.Find(k => k.SymbolDataType == m.FieldSymbolDataType);
                if (null != dtModel)
                {
                    symbolDataTypeStr = Convert.ToString((int)dtModel.SymbolDataType);
                    if (null != dtModel.AllowCompareSymbol && dtModel.AllowCompareSymbol.Count > 0)
                    {
                        allows = string.Join(",", dtModel.AllowCompareSymbol.Select(k => k.ID.ToString()).ToArray());
                    }
                }

                strFields.AppendFormat(@"<option value=""{0}"" InputControlType=""{2}"" InputControlInitValue=""{3}"" FieldDateFormat=""{4}"" AllowSymbols=""{5}"" SymbolDataType=""{6}"" CustomInputControlAttributes=""{7}"">{1}</option>", 
                                                    m.FieldName, 
                                                    m.FieldDisplayName, 
                                                    (int)m.InputControlType, 
                                                    CommonHelper.Escape(m.InputControlInitValue), 
                                                    m.FieldDateFormat, 
                                                    allows, 
                                                    symbolDataTypeStr,
                                                    CommonHelper.Escape(m.CustomInputControlAttributes)
                                                    );
            } 
            #endregion

            #region 模板占位赋值
            XCLNetSearch.Templates.TemplateModel tempModel = new Templates.TemplateModel();
            tempModel.查询提交按钮的ID = string.Format("{0}_btnSearch", guidIDString);
            tempModel.查询提交按钮的图片URL = this.GetResourceURL(string.Format("{0}.Resources.Images.search2.gif", assName));
            tempModel.查询提交按钮鼠标划过的图片URL = this.GetResourceURL(string.Format("{0}.Resources.Images.search.gif", assName));
            tempModel.查询项所在容器ID = string.Format("{0}_divSearch", guidIDString);
            tempModel.存放最终解析结果的控件Name = this.QueryUrlParamName;
            tempModel.动态增删行的行Class = string.Format("{0}_items", guidIDString);
            tempModel.动态增删行的模板行Class = string.Format("{0}_temps", guidIDString);
            tempModel.符号选择框的Name = string.Format("{0}_SymbolSelect", guidIDString);
            tempModel.符号选择框的Options = CommonHelper.GetOptionsBySymbolList(GetSymbolList(EnumTypes.SymbolType.比较符));
            tempModel.逻辑选择框的Name = string.Format("{0}_LogicSelect", guidIDString);
            tempModel.逻辑选择框的Options = CommonHelper.GetOptionsBySymbolList(GetSymbolList(EnumTypes.SymbolType.逻辑符));
            tempModel.容器ID = this.ContainerID;
            tempModel.删除搜索条件按钮的Class = string.Format("{0}_delBtn", guidIDString);
            tempModel.删除搜索条件按钮的图片URL = this.GetResourceURL(string.Format("{0}.Resources.Images.del_search.gif", assName));
            tempModel.输入区域控件的Name = string.Format("{0}_InputArea", guidIDString);
            tempModel.右括号选择框的Name = string.Format("{0}_RightBracketSelect", guidIDString);
            tempModel.右括号选择框的Options = CommonHelper.GetOptionsBySymbolList(GetSymbolList(EnumTypes.SymbolType.右括号));
            tempModel.增加搜索条件按钮的Class = string.Format("{0}_addBtn", guidIDString);
            tempModel.增加搜索条件按钮的图片URL = this.GetResourceURL(string.Format("{0}.Resources.Images.add_search.gif", assName));
            tempModel.隐藏搜索框的图片URL = this.GetResourceURL(string.Format("{0}.Resources.Images.up.gif", assName));
            tempModel.展开搜索框的图片URL = this.GetResourceURL(string.Format("{0}.Resources.Images.down.gif", assName));
            tempModel.展开搜索框的图片按钮ID = string.Format("{0}_openImg", guidIDString);
            tempModel.字段选择框的Name = string.Format("{0}_FieldSelect", guidIDString);
            tempModel.字段选择框的Options = strFields.ToString();
            tempModel.左括号选择框的Name = string.Format("{0}_LeftBracketSelect", guidIDString);
            tempModel.左括号选择框的Options = CommonHelper.GetOptionsBySymbolList(GetSymbolList(EnumTypes.SymbolType.左括号));
            tempModel.动态增删行的行的最大行数 = this.MaxLine;
            tempModel.动态内容区JS插件名称 = string.Format("{0}_DynamicCon", guidIDString);
            tempModel.查询处理后的URL值的Json = CommonHelper.GetUrlParamJsonValueByQueryItemModelList(this.GetQueryItemModelList);
            tempModel.提示信息 = string.IsNullOrEmpty(this.Message) ? "" : string.Format(@"<br/><span style=""color:#f00;font-size:12px;"">{0}</span>", this.Message);

            StringBuilder symbolsJson = new StringBuilder();
            foreach (var m in SymbolList)
            {
                symbolsJson.AppendFormat(@"""{0}"":""{1}"",", m.FlagName, m.ID);
            }
            #endregion

            #region 读取模板
            StreamReader reader = new StreamReader(System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format("{0}.Templates.T1.html", assName)));
            string str = reader.ReadToEnd();
            reader.Close();
            if (string.IsNullOrEmpty(str))
            {
                throw new Exception("模板HTML文件不能为空！");
            }
            System.Reflection.PropertyInfo[] pros = tempModel.GetType().GetProperties();
            for (int i = 0; i < pros.Length; i++)
            {
                str = new Regex(string.Format(@"\{{\s*XCL\s*:\s*{0}\s*\}}", pros[i].Name)).Replace(str, Convert.ToString(pros[i].GetValue(tempModel, null)));
            } 
            #endregion

            #region 输出HTML
            string strOutHtml = str.ToString();
            if (this.IsCompressHTML)
            {
                string sc = CommonHelper.GetCenterString(strOutHtml, @"<script type=""text/javascript"">", "</script>");
                //压缩JS
                strOutHtml = strOutHtml.Replace(sc, new Yahoo.Yui.Compressor.JavaScriptCompressor().Compress(sc));
                //替换多个空格为一个空格，主要是针对html
                strOutHtml = Regex.Replace(strOutHtml, @"(\s+)|(/\*.*\*/)", " ");
            }
            output.Write(strOutHtml);
            base.RenderContents(output); 
            #endregion
        }
        #endregion

        #region 其它
        /// <summary>
        /// 获取嵌入的资源文件
        /// </summary>
        internal string GetResourceURL(string url)
        {
            return this.Page.ClientScript.GetWebResourceUrl(this.GetType(), url);
        }

        /// <summary>
        /// 说明信息
        /// </summary>
        private static string GetRemark
        {
            get
            {
                StringBuilder str = new StringBuilder();
                Assembly asm = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                str.AppendFormat("<!--*****************{0}  {1}，{2} *****************-->", fvi.FileDescription, fvi.FileVersion, fvi.LegalCopyright);
                return str.ToString();
            }
        }
        #endregion
    }
}
