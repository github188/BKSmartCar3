using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Data;

namespace BekUtils.Database
{
    internal class OleDbDataProvider : IDataProvider
    {
        private OleDbConnection connection;
        private OleDbCommand command;
        private string connectionString;

        public OleDbDataProvider() : this(null)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public OleDbDataProvider(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return;
            }
            else
            {
                this.connectionString = connectionString;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                this.connectionString = value;
            }
        }

        /// <summary>
        /// 返回一个带有连接字符串的SQLSERVER Connection.
        /// </summary>
        /// <returns>OracleConnection</returns>
        private OleDbConnection GetOleDbConnection()
        {
            try
            {
                return new OleDbConnection(this.connectionString);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        /// <summary>
        /// 对于 UPDATE、INSERT 和 DELETE 语句，返回值为该命令所影响的行数。对于其他所有类型的语句，返回值为 -1
        /// </summary>
        /// <param name="Sql">UPDATE、INSERT 和 DELETE 语句</param>
        public int ExecuteNonQuery(string sql)
        {
            using (connection = this.GetOleDbConnection())
            {
                if (connection == null)
                    return -1;
                int rv = -1;

                try
                {
                    if (System.Data.ConnectionState.Closed == connection.State)
                    {
                        connection.Open();
                    }

                    command = new OleDbCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    rv = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    rv = -1;
                }

                return rv;
            }
        }

        /// <summary>
        ///  执行单Sql语句查询，并将查询返回的结果作为一个数据集返回
        /// </summary>
        /// <param name="selectSql">SELECT 语句</param>
        /// <returns>数据集 DataSet</returns>
        public DataSet RetriveDataSet(string sql)
        {
            if (string.IsNullOrEmpty(sql))
            {
                return null;
            }
            using (connection = this.GetOleDbConnection())
            {
                if (connection == null)
                    return null;
                using (OleDbDataAdapter da = new OleDbDataAdapter(sql, connection))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds);
                    }
                    catch (Exception e)
                    {
                    }
                    return ds;
                }
            }
        }

        public void Dispose()
        {
            this.connectionString = null;
            //this.sqlCommand.Dispose();
            this.connection.Dispose();
        }

    }
}
