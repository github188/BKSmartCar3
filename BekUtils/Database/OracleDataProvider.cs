﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OracleClient;
using System.Data;

namespace BekUtils.Database
{
    internal class OracleDataProvider : IDataProvider
    {
        private OracleConnection connection;
        private OracleCommand command;
        private string connectionString;
        public OracleDataProvider() : this(null)
        {
            //
            // TODO: 在此处添加构造函数逻辑
            //
        }

        public OracleDataProvider(string connectionString)
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

        /// <summary>
        /// Oracle 连接字符串 "User Id=southfence;Data Source=FENCEORA;Password=southfence;Persist Security Info=true;"    
        /// </summary>
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
        /// 返回一个带有连接字符串的Oracle Connection.
        /// </summary>
        /// <returns>OracleConnection</returns>
        private OracleConnection GetOracleConnection()
        {
            try
            {
                return new OracleConnection(this.connectionString);
            }
            catch (Exception ex)
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
            using (connection = this.GetOracleConnection())
            {
                if (connection == null)
                    return -1;
                int rv = -1;
                try
                {
                    if (connection.State == System.Data.ConnectionState.Closed)
                        connection.Open();

                    command = new OracleCommand();
                    command.Connection = connection;
                    command.CommandType = CommandType.Text;
                    command.CommandText = sql;
                    rv = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //oracleTransaction.Rollback();
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
            if (sql == null || sql == string.Empty)
            {
                return null;
            }
            using (connection = this.GetOracleConnection())
            {
                if (connection == null)
                    return null;
                using (OracleDataAdapter da = new OracleDataAdapter(sql, connection))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds);
                    }
                    catch (Exception ex)
                    {
                    }
                    return ds;
                }
            }
        }

        public void Dispose()
        {
            this.connectionString = null;
            this.command.Dispose();
            this.connection.Dispose();
        }
    }
}
