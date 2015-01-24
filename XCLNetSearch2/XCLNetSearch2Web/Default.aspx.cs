using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace XCLNetSearch2Web
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.InitSearch();
            this.InitData();
        }

        /// <summary>
        /// 查询控件初始化
        /// </summary>
        private void InitSearch()
        {
            #region 查询控件设置
            this.search1.QueryFields = new List<XCLNetSearch.Model.QueryFieldModel>(){
                new XCLNetSearch.Model.QueryFieldModel(){
                    FieldSymbolDataType=XCLNetSearch.EnumTypes.SymbolDataType.Number,
                    FieldDisplayName="系统编号",
                    FieldName="ID",
                    InputControlType=XCLNetSearch.EnumTypes.InputControlTypeEnum.TextBox,
                    CustomInputControlAttributes="rel='customAtt'",
                    InputValueDataType=System.Data.SqlDbType.Int
                },
                new XCLNetSearch.Model.QueryFieldModel(){
                    FieldSymbolDataType=XCLNetSearch.EnumTypes.SymbolDataType.String,
                    FieldDisplayName="姓名",
                    FieldName="Name",
                    InputControlType=XCLNetSearch.EnumTypes.InputControlTypeEnum.TextBox,
                    InputValueDataType=System.Data.SqlDbType.VarChar
                },
                new XCLNetSearch.Model.QueryFieldModel(){
                    FieldSymbolDataType=XCLNetSearch.EnumTypes.SymbolDataType.Number,
                    FieldDisplayName="所在地",
                    FieldName="Area",
                    InputControlInitValue=@"<option value=""0"">杭州</option><option value=""1"">武汉</option><option value=""2"">上海</option>",
                    InputControlType=XCLNetSearch.EnumTypes.InputControlTypeEnum.Select,
                    InputValueDataType=System.Data.SqlDbType.Int
                },
                new XCLNetSearch.Model.QueryFieldModel(){
                    FieldSymbolDataType=XCLNetSearch.EnumTypes.SymbolDataType.DateTime,
                    FieldDisplayName="出生年月",
                    FieldName="Birth",
                    FieldNameFormat="DATEADD(m,DATEDIFF(m,0,{0}),0)",
                    FieldValueFormat="cast({0}+'-01' as datetime)",
                    FieldDateFormat="yyyy-MM",
                    InputValueDataType=System.Data.SqlDbType.VarChar,
                    InputControlType=XCLNetSearch.EnumTypes.InputControlTypeEnum.TextBox
                },
                new XCLNetSearch.Model.QueryFieldModel(){
                    FieldSymbolDataType=XCLNetSearch.EnumTypes.SymbolDataType.Number,
                    FieldDisplayName="登记月份",
                    FieldName="EntryMonth",
                    FieldNameFormat="Month({0})",
                    FieldDateFormat="MM",
                    InputValueDataType=System.Data.SqlDbType.Int,
                    InputControlType=XCLNetSearch.EnumTypes.InputControlTypeEnum.TextBox
                },
                new XCLNetSearch.Model.QueryFieldModel(){
                    FieldSymbolDataType=XCLNetSearch.EnumTypes.SymbolDataType.TextString,
                    FieldDisplayName="备注",
                    FieldName="Remark",
                    InputControlType=XCLNetSearch.EnumTypes.InputControlTypeEnum.TextBox,
                    InputValueDataType=System.Data.SqlDbType.NText
                }
            }; 
            #endregion
        }

        /// <summary>
        /// 数据初始化
        /// </summary>
        private void InitData()
        {
            //1、指定原条件（如果有的话）
            string oldWhere = "Type=0";

            //2、创建一个查询对象,将赋予原始查询参数
            XCLNetSearch.Model.SearchModel searchModel = new XCLNetSearch.Model.SearchModel() { 
                OldWhere=oldWhere
            };
            //3、在获取列表的方法中，传入this.search1和searchModel
            BLL.UserInfo bll = new BLL.UserInfo();
            //List<Model.UserInfo> lst = bll.GetModelList(this.search1, searchModel);
            List<Model.UserInfo> lst = bll.GetModelListWithParam(this.search1, searchModel);
            if (null != lst && lst.Count > 0)
            {
                this.repList.DataSource = lst;
                this.repList.DataBind();
            }

            #region 参数输出
            if (null != this.search1.SearchModel)
            {
                StringBuilder str = new StringBuilder();
                str.Append("原始条件：<br/>");
                str.AppendFormat("{0}<br/>", this.search1.SearchModel.OldWhere);
                str.Append("原始参数：<br/>");
                if (null != this.search1.SearchModel.OldParameters && this.search1.SearchModel.OldParameters.Length > 0)
                {
                    for (int i = 0; i < this.search1.SearchModel.OldParameters.Length; i++)
                    {
                        var m = this.search1.SearchModel.OldParameters[i];
                        str.AppendFormat("{4}、参数名：【{0}】；参数值【{1}】；参数类型：【{2}】；长度：【{3}】<br/>", m.ParameterName, m.Value, m.SqlDbType, m.Size, i);
                    }
                }

                str.Append("<br/>===========================================================<br/>");
                str.Append("方式一、<br/>新条件（不带参数，直接查询）：<br/>");
                str.Append(string.Format("{0}<br/><br/>", this.search1.SearchModel.SQLWhere));


                str.Append("方式二、<br/>新条件：<br/>");
                str.AppendFormat("{0}<br/>", this.search1.SearchModel.NewWhere);
                str.Append("新参数：<br/>");
                if (null != this.search1.SearchModel.NewParameters && this.search1.SearchModel.NewParameters.Length > 0)
                {
                    for (int i = 0; i < this.search1.SearchModel.NewParameters.Length; i++)
                    {
                        var m = this.search1.SearchModel.NewParameters[i];
                        str.AppendFormat("{4}、参数名：【{0}】；参数值【{1}】；参数类型：【{2}】；长度：【{3}】<br/>", m.ParameterName, m.Value, m.SqlDbType, m.Size, i);
                    }
                }

                this.lbSql.Text = str.ToString();
            }
            #endregion
        }
    }
}