using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Core
{
    public class ShowSchedule:Database
    {
        protected Tools t;
        public ShowSchedule()
        {
            t = new Tools();
        }

        /// <summary>
        /// 获取当前周的周状态
        /// </summary>
        /// <param name="monthID"></param>
        /// <param name="weekOfYear"></param>
        /// <returns></returns>
        public int GetWeekState(Guid workID,int weekOfYear)
        {
            int state=0;
            using (SqlDataReader sdr = GetDataReader("select 周状态 from 周节点视图 where 工作ID=@工作ID and 周数=@周数", new SqlParameter[] {
                new SqlParameter("@工作ID",workID),
                 new SqlParameter("@周数",weekOfYear)
            }))
                if (sdr.Read())
                    state = int.Parse(sdr[0].ToString());
            return state;
        }

        /// <summary>
        /// 此处取出所有工作的存在月份以提高速度
        /// </summary>
        //public 

        public string GetWorkLeaders(Guid workID)
        {
            string workLeaders = string.Empty;
            using (SqlDataReader sdr = GetDataReader("select 责任领导 from 工作责任领导视图 where 工作ID=@工作ID",new SqlParameter[] { new SqlParameter("@工作ID",workID) }))
                while (sdr.Read())
                    workLeaders += " " + sdr[0].ToString();

            return workLeaders;
        }
    }

    
}
