using System;
using System.Data;
using System.Collections.Generic;
using XCLNetSearch2Web.Model;
namespace XCLNetSearch2Web.BLL
{
    /// <summary>
    /// UserInfo
    /// </summary>
    public partial class UserInfo
    {
        private readonly XCLNetSearch2Web.DAL.UserInfo dal = new XCLNetSearch2Web.DAL.UserInfo();
        public UserInfo()
        { }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<XCLNetSearch2Web.Model.UserInfo> GetModelList(XCLNetSearch.Search search, XCLNetSearch.Model.SearchModel searchModel)
        {
            DataSet ds = dal.GetList(search, searchModel);
            return DataTableToList(ds.Tables[0]);
        }

        /// <summary>
        /// 获得数据列表(带参数)
        /// </summary>
        public List<XCLNetSearch2Web.Model.UserInfo> GetModelListWithParam(XCLNetSearch.Search search, XCLNetSearch.Model.SearchModel searchModel)
        {
            DataSet ds = dal.GetListWithParam(search, searchModel);
            return DataTableToList(ds.Tables[0]);
        }


        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<XCLNetSearch2Web.Model.UserInfo> DataTableToList(DataTable dt)
        {
            List<XCLNetSearch2Web.Model.UserInfo> modelList = new List<XCLNetSearch2Web.Model.UserInfo>();
            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                XCLNetSearch2Web.Model.UserInfo model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = dal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }

    }
}

