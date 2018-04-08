using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
//using System.Web;


namespace Core
{
    public class ManageClass:Database
    {
        public int year;
        protected ScheduleClass sc;

        public ManageClass()
        {
            year = DateTime.Now.Year;
            sc = new ScheduleClass();
        }
        
        public ManageClass(int pyear)
        {
            year = pyear;
            sc = new ScheduleClass();
        }
        /// <summary>
        /// 添加月节点
        /// </summary>
        /// <param name="workID"></param>
        /// <param name="task"></param>
        /// <param name="startMonth"></param>
        /// <param name="endMonth"></param>
        public void AddMonthSchedule(Guid workID, string task, int startMonth, int endMonth, bool append)
        {
            //如果初始月份为0则不添加本条工作计划
            if (startMonth == 0)
                return;

            if (endMonth == 0 || startMonth == endMonth)
                UpdateMonthSchedule(workID, task, startMonth, append);
            else
                for (int i = startMonth; i <= endMonth; i++)
                    UpdateMonthSchedule(workID, task, i, append);
        }

        /// <summary>
        /// 更新月节点
        /// </summary>
        /// <param name="workID"></param>
        /// <param name="task"></param>
        /// <param name="month"></param>
        /// <param name="append"></param>
        public void UpdateMonthSchedule(Guid workID, string task, int month, bool append)
        {
            string date = year + "-" + month + "-1";
            string id = string.Empty;
            //先判断月节点表中有没有此月的节点数据，如果有则用update语句在原数据后面追加新的节点数据，如果没有则用insert添加新节点数据
            using (SqlDataReader sdr = GetDataReader("select 目标节点,ID from 月节点 where 工作ID=@工作ID and 日期=@日期", new SqlParameter[] { new SqlParameter("@工作ID",workID),
                new SqlParameter("@日期",date)
            }))
            {
                string sql = string.Empty;
                if (sdr.HasRows)
                {
                    sdr.Read();

                    //如果为附加模式
                    if (append)
                        task = sdr[0].ToString() + " " + task;

                    sql = "update 月节点 set 目标节点=@目标节点 where ID=@ID";
                    id = sdr[1].ToString();
                }
                sdr.Close();

                //如果此时sdr.HasRows为真则sql不为空，则运行update命令并退出函数，放在后面是因为必须先关闭SqlDataReader才能执行其他sql命令
                if (sql != string.Empty)
                {
                    ExecuteSql(sql, new SqlParameter[] { new SqlParameter("@目标节点",task),
                        new SqlParameter("@ID",id)
                    });
                    return;
                }
            }

            //如果原来没有此月的计划则添加
            ExecuteSql("insert 月节点(工作ID,目标节点,日期) values(@工作ID,@目标节点,@日期)", new SqlParameter[] {new SqlParameter("@工作ID",workID),
                    new SqlParameter("@目标节点",task),
                    new SqlParameter("@日期",date)
                    });
        }

       

        /// <summary>
        /// 读取字符串形式的月目标节点并批量更新
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="workID"></param>
        /// <returns></returns>
        public bool BatchUpdateMonthScheduleFormTxt(string ss, Guid workID)
        {
            DataTable dt;
            int startMonth, endMonth = 0;
            foreach (string s in ss.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                dt = sc.RecognizeMonthScheduleFormTxt(s, endMonth);
                startMonth = int.Parse(dt.Rows[0][0].ToString());
                endMonth = int.Parse(dt.Rows[0][1].ToString());

                if (dt.Rows.Count > 0)
                    AddMonthSchedule(workID, s, startMonth, endMonth, false);
            }
            return true;
        }

        public bool DeleteWork(Guid workID)
        {
            return ExecuteTranSQL(@"DELETE FROM dbo.周节点 WHERE 月节点ID in (SELECT id FROM 月节点 WHERE 工作ID=@工作ID)
DELETE FROM dbo.月节点 WHERE 工作ID=@工作ID
DECLARE @no INT
SELECT @no=序号 FROM dbo.工作 WHERE Id=@工作ID
UPDATE dbo.工作 SET 序号=序号-1 WHERE 序号>@no AND 年份 IN (SELECT 年份 FROM dbo.工作 WHERE Id=@工作ID)
DELETE FROM dbo.工作 WHERE Id=@工作ID
", new SqlParameter[] { new SqlParameter("工作ID", workID) });
        }

        public bool UpdateWorkContent(Guid workID, string content)
        {
            ExecuteSql("update 工作 set 目标内容=@目标内容 where ID=@ID", new SqlParameter[] { new SqlParameter("@目标内容",content),
                new SqlParameter("@ID",workID)
            });
            return true;
        }
        public bool UpdateWorkContent(Guid workID, string content, string tableName)
        {
            ExecuteSql("update " + tableName + " set 目标内容=@目标内容 where ID=@ID", new SqlParameter[] { new SqlParameter("@目标内容",content),
                new SqlParameter("@ID",workID)
            });
            return true;
        }

        /// <summary>
        /// 录入周计划落实情况（包含插入和更新两种情况）
        /// </summary>
        /// <param name="monthTaskID"></param>
        /// <param name="weekOfYear"></param>
        /// <param name="weekSchedule"></param>
        /// <param name="weekExecution"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public bool InputWeekSchedule(Guid monthTaskID, int weekOfYear, string weekSchedule, string weekExecution, int status)
        {
            bool hasData;
            int i;
            Guid id = Guid.Empty;
            using (SqlDataReader sdr = GetDataReader("select ID from 周节点 where 月节点ID=@月节点ID and 周数=@周数", new SqlParameter[] {
                new SqlParameter("@月节点ID",monthTaskID),
                new SqlParameter("@周数",weekOfYear)
            }))
            {
                hasData = sdr.Read();
                if (hasData)
                    id = Guid.Parse(sdr[0].ToString());
            }
            if (hasData)
            {
                i = ExecuteSql("update 周节点 set 周计划=@周计划,周完成=@周完成,周状态=@周状态 where ID=@ID", new SqlParameter[]{
                new SqlParameter("@ID",id),
                new SqlParameter("@周计划",weekSchedule),
                new SqlParameter("@周完成",weekExecution),
                new SqlParameter("@周状态",status)
            });
            }
            else
            {
                //如果原来没有周工作计划且此也没有周计划和周完成则不必更新周计划
                if (status == 0)
                    return true;
                else
                    i = ExecuteSql("insert 周节点(月节点ID,周数,周计划,周完成,周状态) values(@月节点ID,@周数,@周计划,@周完成,@周状态)", new SqlParameter[]{
                new SqlParameter("@月节点ID",monthTaskID),
                new SqlParameter("@周数",weekOfYear),
                new SqlParameter("@周计划",weekSchedule),
                new SqlParameter("@周完成",weekExecution),
                new SqlParameter("@周状态",status)
            });
            }
            return i == 1 ? true : false;
        }
    }
}
