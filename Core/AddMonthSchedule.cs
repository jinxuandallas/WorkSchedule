using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.Data.SqlClient;

namespace Core
{
    public class AddMonthSchedule : Database
    {
        public AddMonthSchedule()
        {

        }

        public bool InputWeekSchedule(Guid monthTaskID, int weekOfYear, string weekSchedule, string weekExecution, int state)
        {
            string sql;
            int i;
            using (SqlDataReader sdr = GetDataReader("select ID from 周节点 where 月节点ID=@月节点ID and 周数=@周数", new SqlParameter[] {
                new SqlParameter("@月节点ID",monthTaskID),
                new SqlParameter("@周数",weekOfYear)
            }))
                if (sdr.Read())
                    i = ExecuteSql("update 周节点 set 周计划=@周计划,周完成=@周完成,周状态=@周状态 where ID=@ID", new SqlParameter[]{
                new SqlParameter("@ID",sdr[0].ToString()),
                new SqlParameter("@周计划",weekSchedule),
                new SqlParameter("@周完成",weekExecution),
                new SqlParameter("@周状态",state)
            });
                else
                    i = ExecuteSql("insert 周节点(月节点ID,周数,周计划,周完成,周状态) values(@月节点ID,@周数,@周计划,@周完成,@周状态)", new SqlParameter[]{
                new SqlParameter("@月节点ID",monthTaskID),
                new SqlParameter("@周数",weekOfYear),
                new SqlParameter("@周计划",weekSchedule),
                new SqlParameter("@周完成",weekExecution),
                new SqlParameter("@周状态",state)
            });

            return i == 1 ? true : false;
        }
        /*
        public StringBuilder GetMonthSchedule()
        {
           SqlDataReader works=GetDataReader("select * from 2018重点工作");

        }
        */
    }

}
