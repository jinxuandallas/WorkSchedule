using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;//��������System.Configuration.dll�ļ�
using Core;
using System.Diagnostics;
namespace Core
{
    /// <summary>
    /// DataBase ��ժҪ˵����
    /// </summary>

    public abstract class Database
    {
        /// <summary>
        /// DataBase���Ӷ���
        /// </summary>
        protected SqlConnection Connection;
        private string connectionString;
        protected string sql;
        //		private SqlDataReader dr;

        /// <summary>
        /// Ĭ�Ϲ��캯����
        /// </summary>
        public Database()
        {
            connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
            Connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// �����ִ�ֻ����������
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
        /// ʧ��
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
        /// �ڲ����������캬�в�����commandʵ�塣
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
        /// ʧ��
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
        /// ���غ��н����SqlDataReaderʵ��
        /// ע��Connection����δ�رա�
        /// </summary>
        /// <param name="Sql">Ҫִ�е�Sql���</param>
        /// <param name="parameters">�����б��޲���ʱ�봫�롰null����</param>
        /// <returns>���н����SqlDataReaderʵ��</returns>
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
        /// ���غ��н����SqlDataReaderʵ��
        /// ע��Connection����δ�رա�
        /// </summary>
        /// <param name="Sql">Ҫִ�е�Sql���</param>
        /// <returns>���н����SqlDataReaderʵ��</returns>
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
        /// ����Sql��䣬�����ɺ��н����DataSetʵ��
        /// </summary>
        /// <param name="Sql">Ҫִ�е�Sql���</param>
        /// <param name="parameters">�����б��޲���ʱ�봫�롰null����</param>
        /// <param name="tableName">Ҫ���ı���</param>
        /// <returns>���н����DataSetʵ��</returns>
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
        /// ����Sql��䣬�����ɺ��н����DataSetʵ��
        /// </summary>
        /// <param name="Sql">Ҫִ�е�Sql���</param>
        /// <param name="parameters">�����б��޲���ʱ�봫�롰null����</param>
        /// <returns>���н����DataSetʵ��</returns>
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
        /// ����Sql��䣬�����ɺ��н����DataSetʵ��
        /// </summary>
        /// <param name="Sql">Ҫִ�е�Sql���</param>
        /// <returns>���н����DataSetʵ��</returns>
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
        //		/// ����Sql��䣬������Ѵ��ڵ�DataSet����Ӧ��
        //		/// </summary>
        //		/// <param name="Sql">Ҫִ�е�Sql���</param>
        //		/// <param name="parameters">�����б��޲���ʱ�봫�롰null����</param>
        //		/// <param name="dataSet">�Ѵ��ڵ�DataSet</param>
        //		/// <param name="tableName">Ҫ���ı���</param>
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
        /// ����Sql��䣬ֻ������Ӱ�������
        /// </summary>
        /// <param name="Sql">Ҫִ�е�Sql���</param>
        /// <returns>Ӱ�������</returns>
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
        /// ����Sql��䣬ֻ������Ӱ�������(֧�ֲ��������ط���)
        /// </summary>
        /// <param name="Sql">Ҫִ�е�Sql���</param>
        /// <param name="parameters">�����б�</param>
        /// <returns>Ӱ�������</returns>
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
        /// ����Sql��䣬ֻ������Ӱ�������(֧����������ط���)��(���ʱ���ر�Connection����)��ע����֧�ֲ�������
        /// </summary>
        /// <param name="Sql">Ҫִ�е�Sql���</param>
        /// <param name="parameters">�����б��޲���ʱ�봫�롰null����</param>
        /// <param name="transaction">����SqlTransaction</param>
        /// <returns>Ӱ�������</returns>
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
        /// ִ��һ��Sql��䣬����������
        /// </summary>
        /// <param name="sqlStrings">Sql�����</param>
        /// <returns>�Ƿ�ɹ�</returns>
        protected bool ExecuteTranSQL(String[] sqlStrings)
        {
            if (Connection.State == ConnectionState.Closed) Connection.Open();

            SqlTransaction trans = Connection.BeginTransaction();//SQL Server ���ݿ��д���� Transact-SQL ����
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
                trans.Rollback();//�ӹ���״̬�ع�����
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

            SqlTransaction trans = Connection.BeginTransaction();//SQL Server ���ݿ��д���� Transact-SQL ����
            try
            {
                cmd.Transaction = trans;

                cmd.ExecuteNonQuery();

                trans.Commit();
                return true;
            }
            catch (Exception ex)
            {
                trans.Rollback();//�ӹ���״̬�ع�����
                deal(ex);
                return false;
            }
        }



        /// <summary>
        /// �����¼�����ز���ļ�¼�ı�ʶ��
        /// </summary>
        /// <param name="sql">Ҫ�����Sql���<</param>
        /// <returns>�ղ���ļ�¼�ı�ʶ��</returns>
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
        ///  �����¼�����ز���ļ�¼�ı�ʶ��
        /// </summary>
        /// <param name="sql">Ҫ�����Sql���</param>
        /// <param name="parameters">����Sql���Ĳ����б�</param>
        /// <returns>>�ղ���ļ�¼�ı�ʶ��</returns>
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
        /// �쳣����
        /// </summary>
        /// <param name="e"></param>
        public void deal(Exception e)
        {
            throw e;
        }

    }

}
