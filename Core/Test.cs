using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

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

        public DataTable DealMonthSchedule()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("目标名称", Type.GetType("System.String"));
            dt.Columns.Add("目标节点", Type.GetType("System.String"));
            string scheduleName;
            using (SqlDataReader sdr=GetDataReader("select 目标名称,目标节点或完成时限 from 重点工作2018"))
            {
                while(sdr.Read())
                {
                    scheduleName = sdr[0].ToString();
                    foreach (string s in sdr[1].ToString().Split(new char[] { '\n' },StringSplitOptions.RemoveEmptyEntries))
                        dt.Rows.Add(new object[] { scheduleName, s });
                }
            }

            return dt;
        }

        public int DealString()
        {
            string num = "92302 95612 61011";
            int i  = Regex.Match(num, @"6\d{3}1$").Index;
            return i;
        }
    }
}
