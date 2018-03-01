using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core;
using System.Data.SqlClient;

namespace Core
{
    public class AddMonthSchedule:Database
    {
        public AddMonthSchedule()
        {

        }

        public bool InsertWeekSchedule(Guid monthTaskID, int weekOfYear,string weekSchedule,string weekExecution,int state)
        {
            int i=ExecuteSql("insert 周节点(月节点ID,周数,周计划,周完成,周状态) values(@月节点ID,@周数,@周计划,@周完成,@周状态)", new SqlParameter[]{
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
