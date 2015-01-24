using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
namespace XCLNetSearch2Web.DAL
{
    /// <summary>
    /// 数据访问类:UserInfo
    /// </summary>
    public partial class UserInfo
    {
        public UserInfo()
        {
            
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public XCLNetSearch2Web.Model.UserInfo DataRowToModel(DataRow row)
        {
            XCLNetSearch2Web.Model.UserInfo model = new XCLNetSearch2Web.Model.UserInfo();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = long.Parse(row["ID"].ToString());
                }
                if (row["Type"] != null && row["Type"].ToString() != "")
                {
                    model.Type = int.Parse(row["Type"].ToString());
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["Area"] != null && row["Area"].ToString() != "")
                {
                    model.Area = int.Parse(row["Area"].ToString());
                }
                if (row["Birth"] != null && row["Birth"].ToString() != "")
                {
                    model.Birth = DateTime.Parse(row["Birth"].ToString());
                }
                if (row["EntryMonth"] != null && row["EntryMonth"].ToString() != "")
                {
                    model.EntryMonth = DateTime.Parse(row["EntryMonth"].ToString());
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(XCLNetSearch.Search search,XCLNetSearch.Model.SearchModel searchModel)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  FROM UserInfo ");

            search.SearchModel = searchModel;

            if (!string.IsNullOrEmpty(search.SearchModel.SQLWhere))
            {
                strSql.Append(" where " + search.SearchModel.SQLWhere);
            }
            return XCLNetSearch2Web.DBHelper.DB.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得数据列表(带参数)
        /// </summary>
        public DataSet GetListWithParam(XCLNetSearch.Search search, XCLNetSearch.Model.SearchModel searchModel)
        {
            System.Data.SqlClient.SqlParameter[] ps={};
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  FROM UserInfo ");

            search.SearchModel = searchModel;
            if (!string.IsNullOrEmpty(search.SearchModel.NewWhere))
            {
                strSql.AppendFormat(" where {0}",search.SearchModel.NewWhere);
            }
            if (null != search.SearchModel.NewParameters && search.SearchModel.NewParameters.Length > 0)
            {
                ps = search.SearchModel.NewParameters;
            }
            return XCLNetSearch2Web.DBHelper.DB.Query(strSql.ToString(), ps, CommandType.Text);
        }
    }
}

