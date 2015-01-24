using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace XCLNetSearch2Web.DBHelper
{
    public class DB
    {
        private static readonly string connectionString = System.Configuration.ConfigurationManager.AppSettings["ConnectionString"];

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.SqlClient.SqlException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行带参数的SQL查询语句
        /// </summary>
        /// <param name="SQLString">带参数的SQL查询语句</param>
        /// <param name="paras">参数集合</param>
        /// <param name="ct">执行类型</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, SqlParameter[] paras, CommandType ct)
        {
            DataTable dt = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(SQLString, connection);
                cmd.Parameters.AddRange(paras);
                cmd.CommandType = ct;
                using (SqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    dt.Load(sdr);
                }
            }
            DataSet ds = null;
            if (null != dt)
            {
                ds = new DataSet();
                ds.Tables.Add(dt);
            }
            return ds;
        }
    }
}