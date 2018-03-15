using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Core
{
    public class Debug : Database
    {
        public Debug()
        {

        }

        public string RemoveDuplicateString(string s)
        {
            string s1, s2;
            s1 = s.Substring(0, s.Length / 2);
            s2 = s.Substring(s1.Length, s.Length- s1.Length);
            if (s1.Trim() == s2.Trim())
                return s1.Trim();
            else
                return s.Trim();
        }

        public void RemoveDuplicateMonthTask()
        {
            DataTable dt = GetDataSet("select ID,目标节点 from 月节点").Tables[0];

            foreach(DataRow dr in dt.Rows)
            {
                ExecuteSql("update 月节点 set 目标节点=@目标节点 where ID=@ID", new SqlParameter[] {
                    new SqlParameter("@目标节点", RemoveDuplicateString(dr[1].ToString())),
                    new SqlParameter("@ID", dr[0])
                });
            }
        }
    }
}
