using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;//还需引用System.Configuration.dll文件
using Core;
using System.Diagnostics;
namespace Core
{
    /// <summary>
    /// DataBase 的摘要说明。
    /// </summary>

    public abstract class Database
    {
        /// <summary>
        /// DataBase连接对象。
        /// </summary>
        protected SqlConnection Connection;
        private string connectionString;
        protected string sql;
        //		private SqlDataReader dr;

        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public Database()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString1"].ToString();
            Connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// 连接字串只读访问器。
        /// </summary>
        protected string ConnectionString
        {
            get
            {
                return connectionString;
            }
        }

        /*
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <param name="newConnection"></param>
        /// <returns></returns>
        private SqlCommand BuildCommand(string Sql, IDataParameter[] parameters, SqlConnection newConnection)
        {
            SqlCommand command;
            command = new SqlCommand(Sql, newConnection);

            if (parameters == null) return command;

            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;
        }
        */


        /// <summary>
        /// 内部函数，构造含有参数的command实体。
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private SqlCommand BuildCommand(string Sql, IDataParameter[] parameters)
        {
            SqlCommand command = new SqlCommand(Sql, Connection);

            if (parameters == null) return command;

            foreach (SqlParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return command;

        }

        private SqlCommand BuildCommand(string Sql)
        {
            SqlCommand command = new SqlCommand(Sql, Connection);

            return command;

        }
        
        /*
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="Sql"></param>
        /// <param name="parameters"></param>
        /// <param name="isNewConnection"></param>
        /// <returns></returns>
        protected SqlDataReader GetDataReader(string Sql, IDataParameter[] parameters, bool isNewConnection)
        {
            SqlDataReader returnReader;
            SqlCommand command;
            if (isNewConnection)
            {
                SqlConnection newConnection = new SqlConnection(connectionString);
                newConnection.Open();
                command = BuildCommand(Sql, parameters, newConnection);
                returnReader = command.ExecuteReader();
                newConnection.Close();
            }
            else
            {
                if (Connection.State == ConnectionState.Closed) Connection.Open();
                command = BuildCommand(Sql, parameters);
                returnReader = command.ExecuteReader();
            }
            
            //Connection.Close();
            return returnReader;
        }
        */

        /// <summary>
        /// 返回含有结果的SqlDataReader实体
        /// 注：Connection对象未关闭。
        /// </summary>
        /// <param name="Sql">要执行的Sql语句</param>
        /// <param name="parameters">参数列表（无参数时请传入“null”）</param>
        /// <returns>含有结果的SqlDataReader实体</returns>
        protected SqlDataReader GetDataReader(string Sql, IDataParameter[] parameters)
        {
            SqlDataReader returnReader;

            if (Connection.State == ConnectionState.Closed) Connection.Open();
            SqlCommand command = BuildCommand(Sql, parameters);

            returnReader = command.ExecuteReader();
            
            //Connection.Close();
            return returnReader;
        }


        /// <summary>
        /// 返回含有结果的SqlDataReader实体
        /// 注：Connection对象未关闭。
        /// </summary>
        /// <param name="Sql">要执行的Sql语句</param>
        /// <returns>含有结果的SqlDataReader实体</returns>
        protected SqlDataReader GetDataReader(string Sql)
        {
            SqlDataReader returnReader;

            if (Connection.State == ConnectionState.Closed) Connection.Open();
            SqlCommand command = BuildCommand(Sql);

            returnReader = command.ExecuteReader();
            //Connection.Close();
            return returnReader;
        }

        /// <summary>
        /// 运行Sql语句，并生成含有结果的DataSet实体
        /// </summary>
        /// <param name="Sql">要执行的Sql语句</param>
        /// <param name="parameters">参数列表（无参数时请传入“null”）</param>
        /// <param name="tableName">要填充的表名</param>
        /// <returns>含有结果的DataSet实体</returns>
        protected DataSet GetDataSet(string Sql, IDataParameter[] parameters, string tableName)
        {
            DataSet dataSet = new DataSet();
            if (Connection.State == ConnectionState.Closed) Connection.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildCommand(Sql, parameters);
            sqlDA.Fill(dataSet, tableName);
            Connection.Close();

            return dataSet;
        }

        /// <summary>
        /// 运行Sql语句，并生成含有结果的DataSet实体
        /// </summary>
        /// <param name="Sql">要执行的Sql语句</param>
        /// <param name="parameters">参数列表（无参数时请传入“null”）</param>
        /// <returns>含有结果的DataSet实体</returns>
        protected DataSet GetDataSet(string Sql, IDataParameter[] parameters)
        {
            DataSet dataSet = new DataSet();
            if (Connection.State == ConnectionState.Closed) Connection.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildCommand(Sql, parameters);
            sqlDA.Fill(dataSet);
            Connection.Close();

            return dataSet;
        }

        /// <summary>
        /// 运行Sql语句，并生成含有结果的DataSet实体
        /// </summary>
        /// <param name="Sql">要执行的Sql语句</param>
        /// <returns>含有结果的DataSet实体</returns>
        protected DataSet GetDataSet(string Sql)
        {
            DataSet dataSet = new DataSet();
            if (Connection.State == ConnectionState.Closed) Connection.Open();
            SqlDataAdapter sqlDA = new SqlDataAdapter();
            sqlDA.SelectCommand = BuildCommand(Sql);
            sqlDA.Fill(dataSet);
            Connection.Close();

            return dataSet;
        }

        //		/// <summary>
        //		/// 运行Sql语句，并填充已存在的DataSet的相应表
        //		/// </summary>
        //		/// <param name="Sql">要执行的Sql语句</param>
        //		/// <param name="parameters">参数列表（无参数时请传入“null”）</param>
        //		/// <param name="dataSet">已存在的DataSet</param>
        //		/// <param name="tableName">要填充的表名</param>
        //		/// <returns></returns>
        //		protected void FillDataSet(string Sql, IDataParameter[] parameters, DataSet dataSet, string tableName )
        //		{
        //			if (Connection.State==ConnectionState.Closed) Connection.Open();
        //			SqlDataAdapter sqlDA = new SqlDataAdapter();
        //			sqlDA.SelectCommand = BuildCommand( Sql, parameters );
        //			sqlDA.Fill( dataSet, tableName );
        //			Connection.Close();			
        //		}


        /// <summary>
        /// 运行Sql语句，只返回受影响的行数
        /// </summary>
        /// <param name="Sql">要执行的Sql语句</param>
        /// <returns>影响的行数</returns>
        protected int ExecuteSql(string Sql)
        {
            if (Connection.State == ConnectionState.Closed) Connection.Open();
            SqlCommand com;
            com = BuildCommand(Sql, null);

            com.CommandText = Sql;
            int rtn = -1;
            try
            {
                rtn = com.ExecuteNonQuery();
            }

            catch (Exception e)
            {
                deal(e);
            }
            Connection.Close();
            return rtn;
        }

        /// <summary>
        /// 运行Sql语句，只返回受影响的行数(支持参数的重载方法)
        /// </summary>
        /// <param name="Sql">要执行的Sql语句</param>
        /// <param name="parameters">参数列表</param>
        /// <returns>影响的行数</returns>
        protected int ExecuteSql(string Sql, IDataParameter[] parameters)
        {
            if (Connection.State == ConnectionState.Closed) Connection.Open();
            SqlCommand com;
            com = BuildCommand(Sql, parameters);

            com.CommandText = Sql;
            int rtn = -1;
            try
            {
                rtn = com.ExecuteNonQuery();
            }

            catch (Exception e)
            {
                deal(e);
            }
            Connection.Close();
            return rtn;
        }

        /// <summary>
        /// 运行Sql语句，只返回受影响的行数(支持事务的重载方法)，(完成时不关闭Connection对象)。注：不支持并行事务。
        /// </summary>
        /// <param name="Sql">要执行的Sql语句</param>
        /// <param name="parameters">参数列表（无参数时请传入“null”）</param>
        /// <param name="transaction">事务SqlTransaction</param>
        /// <returns>影响的行数</returns>
        protected int ExecuteSql(string Sql, IDataParameter[] parameters, SqlTransaction transaction)
        {
            if (Connection.State == ConnectionState.Closed) Connection.Open();

            SqlCommand com;

            com = BuildCommand(Sql, parameters);
            com.Transaction = transaction;

            //			com.CommandText=Sql;

            if (transaction != null) com.Transaction = transaction;
            int rtn = -1;
            try
            {
                rtn = com.ExecuteNonQuery();
                transaction.Commit();
            }

            catch (Exception e)
            {
                transaction.Rollback();
                deal(e);
            }
            return rtn;
        }


        /// <summary>
        /// 执行一组Sql语句，并启用事务
        /// </summary>
        /// <param name="sqlStrings">Sql语句组</param>
        /// <returns>是否成功</returns>
        protected bool ExecuteTranSQL(String[] sqlStrings)
        {
            if (Connection.State == ConnectionState.Closed) Connection.Open();

            SqlTransaction trans = Connection.BeginTransaction();//SQL Server 数据库中处理的 Transact-SQL 事务
            SqlCommand cmd = new SqlCommand();
            try
            {
                cmd.Connection = Connection;
                cmd.Transaction = trans;

                foreach (string str in sqlStrings)
                {
                    cmd.CommandText = str;
                    cmd.ExecuteNonQuery();
                }
                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();//从挂起状态回滚事务。
                deal(ex);
                //				ErrorEventLog log = new ErrorEventLog("Database Option");
                //				log.Log(EventLogEntryType.Error,ex.ToString());
                //				log.Log(EventLogEntryType.Error,sqlStrings[0]);
                //				foreach (String str in sqlStrings)
                //				{
                //					log.Log(EventLogEntryType.Information,str);
                //				}
                return false;
            }

        }

        protected bool ExecuteTranSQL(String sql, IDataParameter[] parameters)
        {
            if (Connection.State == ConnectionState.Closed) Connection.Open();

            SqlCommand cmd;

            cmd = BuildCommand(sql, parameters);

            SqlTransaction trans = Connection.BeginTransaction();//SQL Server 数据库中处理的 Transact-SQL 事务
            try
            {
                cmd.Transaction = trans;

                cmd.ExecuteNonQuery();

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();//从挂起状态回滚事务。
                deal(ex);
                return false;
            }
        }



        /// <summary>
        /// 插入记录并返回插入的记录的标识列
        /// </summary>
        /// <param name="sql">要插入的Sql语句<</param>
        /// <returns>刚插入的记录的标识列</returns>
        protected int InsertReturnID(string sql)
        {
            int ID = -1;


            sql += " select @@identity";
            if (Connection.State == ConnectionState.Closed) Connection.Open();

            SqlCommand com;

            com = BuildCommand(sql);

            //com.CommandText=sql;


            try
            {
                SqlDataReader Reader;
                Reader = com.ExecuteReader();
                if (Reader.Read())
                    ID = int.Parse(Reader[0].ToString());
                Reader.Close();
            }
            catch (Exception e)
            {
                deal(e);
            }


            return ID;
        }

        /// <summary>
        ///  插入记录并返回插入的记录的标识列
        /// </summary>
        /// <param name="sql">要插入的Sql语句</param>
        /// <param name="parameters">插入Sql语句的参数列表</param>
        /// <returns>>刚插入的记录的标识列</returns>
        protected int InsertReturnID(string sql, IDataParameter[] parameters)
        {
            int ID = -1;


            sql += " select @@identity";
            if (Connection.State == ConnectionState.Closed) Connection.Open();

            SqlCommand com;

            com = BuildCommand(sql, parameters);

            //com.CommandText = sql;


            try
            {
                SqlDataReader Reader;
                Reader = com.ExecuteReader();
                if (Reader.Read())
                    ID = int.Parse(Reader[0].ToString());
                Reader.Close();
            }
            catch (Exception e)
            {
                deal(e);
            }


            return ID;
        }

        /*
        protected int Insert(string sql, IDataParameter[] parameters)
        {
            int rtn = -1;
            if (Connection.State == ConnectionState.Closed) Connection.Open();

            SqlCommand com;

            com = BuildCommand(sql, parameters);

            rtn=com.ExecuteNonQuery();

            //com.CommandText = sql;

            return rtn;
        }
        */

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="e"></param>
        public void deal(Exception e)
        {
            throw e;
        }

    }

}
