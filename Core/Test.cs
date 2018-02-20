using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Core
{
    public class Test : Database
    {
        public Test()
        {

        }

        public string TestReader(int sn)
        {
            SqlDataReader sdr = GetDataReader("select 目标名称 from 工作 where 序号=@SN", new SqlParameter[] { new SqlParameter("@SN", sn) });
            if (sdr.Read())
                return sdr[0].ToString();
            return "没有找到工作目标";
        }

        public void OnlyTest()
        {
            string tableName = "重点工作2018";
            ExecuteSql("select * from " + tableName);
        }
    }
}
