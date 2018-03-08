using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Core
{
    public class ShowScheduleClass : Database
    {
        protected Tools t;
        public ShowScheduleClass()
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

        public string GetMonthScheduleDetail(Guid workID,int month)
        {
            string monthScheduleDeatil= "<br/>&nbsp;&nbsp;&nbsp;&nbsp;" + month+"月目标节点：";
            using (SqlDataReader sdr = GetDataReader("select 目标节点 from 月节点 where 工作ID=@工作ID and datepart(mm,日期)=@月份", new SqlParameter[] {
                new SqlParameter("@工作ID",workID),
                new SqlParameter("@月份",month)
            }))
                if (sdr.Read())
                    monthScheduleDeatil += "“"+sdr[0]+"”<br/>";
            DataSet weekDetail = GetDataSet("select 月节点日期,周数,周计划,周完成,状态 from 周节点视图 where 周状态!=0 and 工作ID=@工作ID and datepart(mm,月节点日期)=@月份",new SqlParameter[] {
                new SqlParameter("@工作ID",workID),
                new SqlParameter("@月份",month)
            });

            //此处也许需要判断weekDetail中是否有数据
            if(weekDetail.Tables[0].Rows.Count>0)
                foreach (DataRow dr in weekDetail.Tables[0].Rows)
                    monthScheduleDeatil += "&nbsp;&nbsp;&nbsp;&nbsp;第" + dr["周数"] + "周：" + dr["状态"] + "<br/>&nbsp;&nbsp;&nbsp;&nbsp;" + "周工作计划：" + dr["周计划"] + "&nbsp;&nbsp;&nbsp;&nbsp;周落实情况：" + dr["周完成"] + "<br/>";

            monthScheduleDeatil += "<br/>";
            return monthScheduleDeatil;
        }
    }

    
}
