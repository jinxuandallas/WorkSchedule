using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.IO;
using System.Data;
using System.Web;
using System.Text.RegularExpressions;
using ICSharpCode.SharpZipLib.Zip.Compression;

namespace Core
{
    public class Tools : Database
    {
        public int year;
        public Tools()
        {
            year = DateTime.Now.Year;
        }

        public Tools(int pyear)
        {
            year = pyear;
        }
        /// <summary>
        /// 生成工作人员表
        /// </summary>
        /// <param name="workTable"></param>
        public void AddStaff(string workTable)
        {
            List<string> staff = new List<string>();

            using (SqlDataReader getStaff = GetDataReader("select distinct(责任领导) from " + workTable))
            {
                string s;
                while (getStaff.Read())
                {
                    s = getStaff[0].ToString();
                    if (s.Contains("\n"))
                    {
                        string[] duoren = s.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string ds in duoren)
                            if (!staff.Contains(ds))
                                staff.Add(ds);
                    }
                    else
                        staff.Add(s);
                }
            }

            DataView dv = GetDataSet("select 姓名 from 工作人员").Tables[0].DefaultView;
            dv.Sort = "姓名";
            foreach (string ins in staff)
                if (dv.Find(ins) == -1)
                    ExecuteSql(@"insert 工作人员(姓名) values (@姓名)", new SqlParameter[] { new SqlParameter("@姓名", ins) });
            //List<string> distinctStaff = staff.Distinct<string>().ToList<string>();
            //int i = 3;
        }

        /// <summary>
        /// 导入工作任务
        /// </summary>
        /// <param name="workTable"></param>
        public void ImportTasks(string workTable)
        {
            ExecuteSql("insert 工作 (序号,目标名称,目标内容,年份,备注,工作类别) select 序号,目标名称,目标内容,year(getdate()),备注,工作类别 from " + workTable);
        }

        /*
        public void BuildWorksRelevantTable()
        {

        }
        */

        /// <summary>
        /// 构建工作责任领导表
        /// </summary>
        /// <param name="workTable"></param>
        public void BuildWorkLeader(string workTable)
        {
            Guid taskID;
            int staffID;
            List<WorkLeader> l = new List<WorkLeader>();

            ///此处会产生SqlDataReader嵌套，需用两个connection
            ///或者用泛型列表，用完一个再用下一个
            using (SqlDataReader getLeader = GetDataReader("select 序号,责任领导 from " + workTable))
            {
                string leaderName;
                int SN;
                while (getLeader.Read())
                {

                    leaderName = getLeader[1].ToString();
                    SN = int.Parse(getLeader[0].ToString());

                    if (leaderName.Contains("\n"))
                    {
                        string[] duoren = leaderName.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string ds in duoren)
                            l.Add(new WorkLeader(SN, ds));
                    }
                    else
                        l.Add(new WorkLeader(SN, leaderName));
                }

            }

            foreach (WorkLeader wl in l)
            {
                taskID = GetTaskIDBySN(wl.SN);
                if (taskID == Guid.Empty)
                    break;
                staffID = GetStaffIDByName(wl.LeaderName);
                if (staffID == 0)
                    break;

                AddWorkLeader(taskID, staffID);

            }

        }
        public string GetTaskNameByID(Guid workID)
        {
            using (SqlDataReader sdr = GetDataReader("select 目标名称 from 工作 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", workID) }))
            {
                if (sdr.Read())
                    return sdr[0].ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// 通过序号获取工作ID
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public Guid GetTaskIDBySN(int sn)
        {
            using (SqlDataReader sdr = GetDataReader("select ID from 工作 where 序号=@SN", new SqlParameter[] { new SqlParameter("@SN", sn) }))
            {
                if (sdr.Read())
                    return Guid.Parse(sdr[0].ToString());
            }

            return Guid.Empty;
        }

        public int GetStaffIDByName(string name)
        {
            using (SqlDataReader sdr = GetDataReader("select ID from 工作人员 where 姓名=@Name", new SqlParameter[] { new SqlParameter("@Name", name) }))
            {
                if (sdr.Read())
                    return int.Parse(sdr[0].ToString());
            }

            return 0;
        }

        public void AddWorkLeader(Guid taskID, int staffID)
        {
            //return;
            ExecuteSql("insert 工作责任领导(工作ID,工作人员ID) values(@工作ID,@工作人员ID)", new SqlParameter[] {
                new SqlParameter("@工作ID", taskID),
                new SqlParameter("@工作人员ID", staffID)
            });
        }

        public DataTable DealMonthSchedule()
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("目标名称", Type.GetType("System.String"));
            dt.Columns.Add("目标节点", Type.GetType("System.String"));
            string scheduleName;
            using (SqlDataReader sdr = GetDataReader("select 目标名称,目标节点或完成时限 from 重点工作2018"))
            {
                while (sdr.Read())
                {
                    scheduleName = sdr[0].ToString();
                    foreach (string s in sdr[1].ToString().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
                        dt.Rows.Add(new object[] { scheduleName, s });
                }
            }

            return dt;
        }

        /// <summary>
        /// 构建月节点表
        /// </summary>
        public void BuildMonthSchedule()
        {
            List<string> a = new List<string>();
            List<string> b = new List<string>();

            string task;
            string r;
            Guid workID;
            int startMonth, endMonth;
            DataTable dt = GetDataSet("select 工作ID,目标节点,ID from 临时目标节点").Tables[0];
            foreach (DataRow dr in dt.Rows)
            {
                task = dr[1].ToString();
                workID = Guid.Parse(dr[0].ToString());

                //注意顺序


                //匹配1-8月
                if ((r = Regex.Match(task, @"^\d*-\d*月").Value) != "")
                {
                    startMonth = int.Parse(Regex.Match(Regex.Match(r, @"^\d*-").Value, @"^\d*").Value);

                    //注意此处正则表达式@"\d*月"不能写为@"-\d*月"，否则会把-当成负号，"-4月"理解成-4
                    endMonth = int.Parse(Regex.Match(Regex.Match(r, @"\d*月").Value, @"\d*").Value);
                    ExecuteSql("update 临时目标节点 set 识别=1 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                    AddMonthSchedule(workID, task, startMonth, endMonth, true);
                }

                //匹配8月：
                else if ((r = Regex.Match(task, @"^\d*月：").Value) != "")
                {
                    startMonth = int.Parse(Regex.Match(r, @"^\d*").Value);
                    ExecuteSql("update 临时目标节点 set 识别=2 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                    AddMonthSchedule(workID, task, startMonth, 0, true);
                }

                //匹配8月底前或者8月底之前或者8月底
                else if ((r = Regex.Match(task, @"^\d*月底前").Value) != "" || (r = Regex.Match(task, @"^\d*月底之前").Value) != "" || (r = Regex.Match(task, @"^\d*月底").Value) != "")
                {
                    //startMonth需判断前面几个月有没有目标节点，如果没有则从1月份开始，如果有则续接上个节点目标的月份
                    using (SqlDataReader sdr = GetDataReader("select top 1 month(日期) from 月节点 where 工作ID=@工作ID order by 日期 desc ", new SqlParameter[] { new SqlParameter("@工作ID", workID) }))
                        if (sdr.HasRows)
                        {
                            sdr.Read();
                            startMonth = int.Parse(sdr[0].ToString()) + 1;
                        }
                        else
                            startMonth = 1;

                    endMonth = int.Parse(Regex.Match(r, @"^\d*").Value);

                    ExecuteSql("update 临时目标节点 set 识别=3 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });

                    AddMonthSchedule(workID, task, startMonth, endMonth, true);
                }

                //匹配7、8月
                else if ((r = Regex.Match(task, @"^\d*、\d*月").Value) != "")
                {
                    //startMonth = int.Parse(Regex.Match(r, @"^\d*").Value);
                    //endMonth = int.Parse(Regex.Match(r, @"、\d*").Value.Remove(0, 1));
                    //ExecuteSql("update 临时目标节点 set 识别=4 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                    //AddMonthSchedule(workID, task, startMonth, endMonth);
                }

                else if ((r = Regex.Match(task, @"^\d*-\d*").Value) != "")
                {
                    string[] s = r.Trim().Split(new char[] { '-' });
                    startMonth = int.Parse(s[0]);

                    endMonth = int.Parse(s[1]);
                    ExecuteSql("update 临时目标节点 set 识别=5 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                    AddMonthSchedule(workID, task, startMonth, endMonth, true);
                }

                //匹配7月
                else if ((r = Regex.Match(task, @"\d+月").Value) != "")
                {
                    startMonth = int.Parse(Regex.Match(r, @"\d+").Value);
                    ExecuteSql("update 临时目标节点 set 识别=6 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                    AddMonthSchedule(workID, task, startMonth, 0, true);
                }
                //task = task.Substring(task.i)
            }

        }

        public List<string> GetMonthScheduleFormTxt(string ss)
        {
            List<string> result = new List<string>();
            DataTable dt;
            int startMonth, endMonth = 0;
            foreach (string s in ss.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                dt = RecognizeMonthScheduleFormTxt(s, endMonth);
                startMonth = int.Parse(dt.Rows[0][0].ToString());
                endMonth = int.Parse(dt.Rows[0][1].ToString());

                if (dt.Rows.Count > 0)
                    for (int i = startMonth; i <= endMonth; i++)
                        result.Add(i + "月——“" + s + "”");
            }

            return result;
        }

        public bool BatchUpdateMonthScheduleFormTxt(string ss, Guid workID)
        {
            DataTable dt;
            int startMonth, endMonth = 0;
            foreach (string s in ss.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries))
            {
                dt = RecognizeMonthScheduleFormTxt(s, endMonth);
                startMonth = int.Parse(dt.Rows[0][0].ToString());
                endMonth = int.Parse(dt.Rows[0][1].ToString());

                if (dt.Rows.Count > 0)
                    AddMonthSchedule(workID, s, startMonth, endMonth, false);
            }
            return true;
        }


        public DataTable RecognizeMonthScheduleFormTxt(string task, int lastMonth)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("开始月份");
            dt.Columns.Add("结束月份");

            int startMonth = 0, endMonth = 0;
            string r;

            //注意顺序


            //匹配1-8月
            if ((r = Regex.Match(task, @"^\d*-\d*月").Value) != "")
            {
                startMonth = int.Parse(Regex.Match(Regex.Match(r, @"^\d*-").Value, @"^\d*").Value);

                //注意此处正则表达式@"\d*月"不能写为@"-\d*月"，否则会把-当成负号，"-4月"理解成-4
                endMonth = int.Parse(Regex.Match(Regex.Match(r, @"\d*月").Value, @"\d*").Value);
            }
            //匹配1月-8月
            else if ((r = Regex.Match(task, @"^\d*月-\d*月").Value) != "")
            {
                startMonth = int.Parse(Regex.Match(Regex.Match(r, @"^\d*月-").Value, @"^\d*").Value);

                //注意此处正则表达式@"\d*月"不能写为@"-\d*月"，否则会把-当成负号，"-4月"理解成-4
                endMonth = System.Math.Abs((int.Parse(Regex.Match(Regex.Match(r, @"月-\d*月").Value, @"-\d*").Value)));
            }
            //匹配8月：
            else if ((r = Regex.Match(task, @"^\d*月：").Value) != "")
            {
                startMonth = endMonth = int.Parse(Regex.Match(r, @"^\d*").Value);

            }

            //匹配8月底前或者8月底之前或者8月底
            else if ((r = Regex.Match(task, @"^\d*月底前").Value) != "" || (r = Regex.Match(task, @"^\d*月底之前").Value) != "" || (r = Regex.Match(task, @"^\d*月底").Value) != "")
            {
                //startMonth需判断前面几个月有没有目标节点，如果没有则从1月份开始，如果有则续接上个节点目标的月份

                startMonth = lastMonth + 1;

                endMonth = int.Parse(Regex.Match(r, @"^\d*").Value);

            }

            //匹配7、8月
            else if ((r = Regex.Match(task, @"^\d*、\d*月").Value) != "")
            {
                //startMonth = int.Parse(Regex.Match(r, @"^\d*").Value);
                //endMonth = int.Parse(Regex.Match(r, @"、\d*").Value.Remove(0, 1));
                //ExecuteSql("update 临时目标节点 set 识别=4 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                //AddMonthSchedule(workID, task, startMonth, endMonth);
            }

            else if ((r = Regex.Match(task, @"^\d*-\d*").Value) != "")
            {
                string[] s = r.Trim().Split(new char[] { '-' });
                startMonth = int.Parse(s[0]);

                endMonth = int.Parse(s[1]);
            }

            //匹配7月
            else if ((r = Regex.Match(task, @"\d+月").Value) != "")
            {
                startMonth = endMonth = int.Parse(Regex.Match(r, @"\d+").Value);

            }
            else
            {
                return dt;
            }
            DataRow dr = dt.NewRow();
            dr[0] = startMonth;
            dr[1] = endMonth;
            dt.Rows.Add(dr);

            return dt;
        }
        public bool HasInput(int userId)
        {
            bool hasInput = false;
            using (SqlDataReader sdr = GetDataReader("select id from 工作人员 where 信息管理用户ID=@ID or 用户ID=@ID", new SqlParameter[] { new SqlParameter("@ID", userId) }))
                if (sdr.Read())
                    hasInput = true;
            return hasInput;
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

        public bool UpdateWorkContent(Guid workID, string content)
        {
            ExecuteSql("update 工作 set 目标内容=@目标内容 where ID=@ID", new SqlParameter[] { new SqlParameter("@目标内容",content),
                new SqlParameter("@ID",workID)
            });
            return true;
        }
        public bool UpdateWorkContent(Guid workID, string content,string tableName)
        {
            ExecuteSql("update "+tableName+" set 目标内容=@目标内容 where ID=@ID", new SqlParameter[] { new SqlParameter("@目标内容",content),
                new SqlParameter("@ID",workID)
            });
            return true;
        }
        /// <summary>
        /// 构建临时节点目标表
        /// </summary>
        public void BuildTempMonthTable()
        {
            StreamReader sr = new StreamReader(HttpContext.Current.Server.MapPath(@"\App_Data\目标节点.txt"), Encoding.Default);
            //List<string> l = new List<string>();
            string line = null;
            while ((line = sr.ReadLine()) != null)
            {
                string[] s = line.Split(new string[] { "         " }, StringSplitOptions.RemoveEmptyEntries);
                ExecuteSql("insert 临时目标节点(工作ID,目标节点) select id,@目标节点 from 工作 where 目标名称=@目标名称", new SqlParameter[] {
                    new SqlParameter("@目标名称",s[0]) ,
                    new SqlParameter("@目标节点",s[1])
                });
            }
            sr.Close();
            //return l;
        }

        /// <summary>
        /// 构建临时月节点表
        /// </summary>
        /// <param name="tableName"></param>
        public void BuildTempMonthTable(string tableName)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("目标名称", Type.GetType("System.String"));
            dt.Columns.Add("目标节点", Type.GetType("System.String"));

            using (SqlDataReader sdr = GetDataReader("select 目标名称,目标节点 from " + tableName))
                while (sdr.Read())
                {
                    string[] monthSchedule = sdr[1].ToString().Split(new string[] { "\n", "  " }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string s in monthSchedule)
                    {
                        if (s.Trim() == "")
                            continue;
                        DataRow dr = dt.NewRow();
                        dr[0] = sdr[0];
                        dr[1] = s.Trim();
                        dt.Rows.Add(dr);
                    }
                }
            foreach (DataRow dr in dt.Rows)
                ExecuteSql("insert 临时目标节点(工作ID,目标节点) select id,@目标节点 from 工作 where 目标名称=@目标名称", new SqlParameter[] {
                    new SqlParameter("@目标名称",dr[0]) ,
                    new SqlParameter("@目标节点",dr[1])
                });
            //return;
        }
        public void BuildWeekOfYear()
        {
            BuildWeekOfYear(year);
        }


        /// <summary>
        /// 生成某年的所有月份的周数信息
        /// </summary>
        /// <param name="year"></param>
        public void BuildWeekOfYear(int year)
        {
            int month = 1;
            DateTime weekStart = new DateTime(year, month, 1);
            DateTime monthEnd = weekStart.AddMonths(1).AddDays(-1);
            TimeSpan ts = new TimeSpan(monthEnd.Ticks - weekStart.Ticks);
            //int weeks = (int)Math.Ceiling(ts.TotalDays / 7);
            DateTime weekEnd = weekStart.AddDays(7 - Convert.ToInt16(weekStart.DayOfWeek));
            int i = 1;
            for (int m = 1; m <= 12; m++)
            {
                do
                {
                    ExecuteSql("insert 周数(周数,开始日期,结束日期) values(@周数,@开始日期,@结束日期)", new SqlParameter[] {new SqlParameter("@周数",i),
                        new SqlParameter("@开始日期",weekStart),
                        new SqlParameter("@结束日期",weekEnd),
                    });
                    //Response.Write("第" + i + "周：" + weekStart + " " + weekEnd + "<br>");
                    i++;
                    weekStart = weekEnd.AddDays(1);
                    weekEnd = weekStart.AddDays(6);
                } while (weekStart.Month == m);

            }
        }

        public bool ArrangeDatabase(string argument)
        {
            if (argument == "目标节点")
            {
                //ExecuteSql("update 重点工作 set 目标节点=")
            }
            return true;
        }
        public bool HasMonthTask(Guid workID, DateTime dt)
        {
            bool result;
            using (SqlDataReader sdr = GetDataReader("select ID from 月节点 where 工作ID=@工作ID and 日期=@日期", new SqlParameter[] { new SqlParameter("@工作ID", workID),
                new SqlParameter("@日期", dt.ToString("yyyy/MM/dd")) }))
            {
                sdr.Read();
                result = sdr.HasRows ? true : false;
            }
            return result;
        }

        public int GetWeekCountOfMonth(int month)
        {
            return GetWeekCountOfMonth(year, month);
        }

        /// <summary>
        /// 获取某个月有几个周
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public int GetWeekCountOfMonth(int year, int month)
        {
            int m;
            using (SqlDataReader sdr = GetDataReader("select count(Id) from 周数 where datepart(yyyy,开始日期)=@年份 and datepart(mm,开始日期)=@月份", new SqlParameter[] { new SqlParameter("@年份", year),
            new SqlParameter("@月份", month)
            }))
            {
                sdr.Read();
                m = int.Parse(sdr[0].ToString());
            }

            return m;
        }

        public DataSet GetWeeksOfMonth(int month)
        {
            return GetWeeksOfMonth(year, month);
        }

        /// <summary>
        /// 获取某个月的所有周信息
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns>返回DataSet格式：周数，开始日期，结束日期</returns>
        public DataSet GetWeeksOfMonth(int year, int month)
        {
            return GetDataSet("select 周数,开始日期,结束日期 from 周数 where datepart(yyyy,开始日期)=@年份 and datepart(mm,开始日期)=@月份", new SqlParameter[] { new SqlParameter("@年份", year),
            new SqlParameter("@月份", month)
            });
        }

        /// <summary>
        /// 获取一年所有月份每个月有几个周（为预读数据做准备）
        /// </summary>
        /// <returns></returns>
        public int[] GetWeeksOfAllMonth()
        {
            return GetWeeksOfAllMonth(year);
        }

        /// <summary>
        /// 获取一年所有月份每个月有几个周（为预读数据做准备）
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public int[] GetWeeksOfAllMonth(int year)
        {

            DataSet ds = GetDataSet("select count(ID) from 周数 where datepart(yyyy,开始日期)=@年份 group by datepart(mm,开始日期)", new SqlParameter[] { new SqlParameter("@年份", year) });
            //int[] w = new int[12];
            //for (int i = 0; i < 12; i++)
            //    w[i] = int.Parse(ds.Tables[0].Rows[i][0].ToString());
            return GetIntArrFromDataSet(ds);
        }

        /// <summary>
        /// 获取某年的所有工作的工作ID
        /// </summary>
        /// <returns></returns>
        public Guid[] GetAllWorkID()
        {
            return GetAllWorkID(year);
        }

        /// <summary>
        /// 获取某年的所有工作的工作ID
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public Guid[] GetAllWorkID(int year)
        {
            Guid[] allWorkID;
            DataTable dt = GetDataSet("select ID from 工作 where 年份=@年份", new SqlParameter[] { new SqlParameter("@年份", year) }).Tables[0];

            allWorkID = new Guid[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
                allWorkID[i] = Guid.Parse(dt.Rows[i][0].ToString());

            return allWorkID;
        }

        public Guid[] GetAllWorkID(string tableName)
        {
            Guid[] allWorkID;
            DataTable dt = GetDataSet("select ID from "+tableName+" where 年份=@年份", new SqlParameter[] { new SqlParameter("@年份", year) }).Tables[0];

            allWorkID = new Guid[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
                allWorkID[i] = Guid.Parse(dt.Rows[i][0].ToString());

            return allWorkID;
        }
        /// <summary>
        /// 获取某年某用户所有管理的工作ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Guid[] GetUserWorkID(int id)
        {
            return GetUserWorkID(id, year);
        }
        public int GetUserType(int userID)
        {
            DataTable dt = GetDataSet("select 用户类型 from 用户 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", userID) }).Tables[0];
            return int.Parse(dt.Rows[0][0].ToString());
        }

        /// <summary>
        /// 获取某年某用户所有管理的工作ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public Guid[] GetUserWorkID(int id, int year)
        {
            Guid[] userWorkID;
            DataTable dt = GetDataSet("select 工作ID from 工作责任领导视图 where 年份=@年份 and (信息管理用户ID=@用户ID or 用户ID=@用户ID)", new SqlParameter[] { new SqlParameter("@年份", year),
                new SqlParameter("@用户ID",id)
            }).Tables[0];

            userWorkID = new Guid[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
                userWorkID[i] = Guid.Parse(dt.Rows[i][0].ToString());

            return userWorkID;
        }


        /// <summary>
        /// 获取某项工作的所有存在计划的月份列表
        /// </summary>
        /// <param name="workID"></param>
        /// <returns></returns>
        public int[] GetExistTaskMonths(Guid workID)
        {

            DataSet ds = GetDataSet("select distinct datepart(mm,日期) from 月节点 where 工作ID = @工作ID", new SqlParameter[] { new SqlParameter("@工作ID", workID) });
            return GetIntArrFromDataSet(ds);
        }

        /// <summary>
        /// 获取某项工作的全部有工作任务信息的周
        /// </summary>
        /// <param name="workID"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public Dictionary<int, int> GetExistTaskWeeksAndState(Guid workID, bool dealUnfinishedAgain)
        {
            bool lastWeekUnfinished = false;
            Dictionary<int, int> weekAndState = new Dictionary<int, int>();
            DataSet ds = GetDataSet("select 周数,周状态 from 周节点视图 where 工作ID = @工作ID and 周状态!=0", new SqlParameter[] { new SqlParameter("@工作ID", workID) });

            //判断是否需要处理第二次未完成的周工作状态
            if (dealUnfinishedAgain)
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    //判断本周是否未完成
                    if (int.Parse(dr[1].ToString()) == 2)
                    {
                        //判断上周是否未完成并且本周是否仍然未完成
                        if (lastWeekUnfinished)
                        {
                            weekAndState.Add(int.Parse(dr[0].ToString()), 4);
                            continue;
                        }
                        else
                            //将上周没完成状态设置为true，为下周判断是否第二次未完成做准备
                            lastWeekUnfinished = true;
                    }
                    else
                        //如果本周已完成，将下周未完成状态设置为false
                        lastWeekUnfinished = false;
                    weekAndState.Add(int.Parse(dr[0].ToString()), int.Parse(dr[1].ToString()));
                }
            else
                foreach (DataRow dr in ds.Tables[0].Rows)
                    weekAndState.Add(int.Parse(dr[0].ToString()), int.Parse(dr[1].ToString()));

            return weekAndState;
        }


        public Guid[] GetProjectCategoryLocationID()
        {
            return GetProjectCategoryLocationID(year);
        }

        /// <summary>
        /// 获取工作类别分类所在位置的工作ID
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public Guid[] GetProjectCategoryLocationID(int year)
        {
            Guid[] projectCategoryLocationID;
            DataTable category = GetDataSet("select ID from [工作类别]").Tables[0];
            projectCategoryLocationID = new Guid[category.Rows.Count];

            int i = 0;
            foreach (DataRow dr in category.Rows)
                using (SqlDataReader sdr = GetDataReader("select top 1 ID from 工作 where 工作类别=@工作类别 order by 序号", new SqlParameter[] { new SqlParameter("@工作类别", dr[0]) }))
                {
                    sdr.Read();
                    projectCategoryLocationID[i] = Guid.Parse(sdr[0].ToString());
                    i++;
                }

            return projectCategoryLocationID;
        }


        /// <summary>
        /// 根据工作ID和月份获取月份的ID
        /// </summary>
        /// <param name="workID"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public Guid GetMonthID(Guid workID, int month)
        {
            Guid monthID;
            using (SqlDataReader sdr = GetDataReader("select ID from 月节点 where 工作ID=@工作ID and datepart(mm,日期)=@月份", new SqlParameter[] { new SqlParameter("@工作ID",workID),
            new SqlParameter("@月份",month)
            }))
            {
                sdr.Read();
                monthID = Guid.Parse(sdr[0].ToString());
            }
            return monthID;
        }

        /// <summary>
        /// 根据工作ID和月份获取月份的目标节点
        /// </summary>
        /// <param name="workID"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public string GetMonthTask(Guid workID, int month)
        {
            string task;
            using (SqlDataReader sdr = GetDataReader("select 目标节点 from 月节点 where 工作ID=@工作ID and datepart(mm,日期)=@月份", new SqlParameter[] { new SqlParameter("@工作ID",workID),
            new SqlParameter("@月份",month)
            }))
            {
                sdr.Read();
                task = sdr[0].ToString();
            }

            return task;
        }

        public string[] GetCategoryName()
        {
            return GetStringArrFromDataSet(GetDataSet("select 类别名称 from 工作类别"));
        }

        /// <summary>
        /// 将DataSet转换为int数组
        /// </summary>
        /// <param name="ds">要转换的DataSet</param>
        /// <returns>返回转换后的数组</returns>
        public int[] GetIntArrFromDataSet(DataSet ds)
        {
            int l = ds.Tables[0].Rows.Count;
            if (l == 0)
                return null;
            int[] m = new int[l];
            for (int i = 0; i < l; i++)
                m[i] = int.Parse(ds.Tables[0].Rows[i][0].ToString());
            return m;
        }


        /// <summary>
        /// 将DataSet转换为string数组
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public string[] GetStringArrFromDataSet(DataSet ds)
        {
            int l = ds.Tables[0].Rows.Count;
            if (l == 0)
                return null;
            string[] m = new string[l];
            for (int i = 0; i < l; i++)
                m[i] = ds.Tables[0].Rows[i][0].ToString();
            return m;
        }

        /// <summary>
        /// 压缩字符串
        /// </summary>
        /// <param name="pBytes"></param>
        /// <returns></returns>
        public byte[] Compress(byte[] pBytes)
        {
            MemoryStream mMemory = new MemoryStream();
            Deflater mDeflater = new Deflater(ICSharpCode.SharpZipLib.Zip.Compression.Deflater.BEST_COMPRESSION);
            ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream mStream = new ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream(mMemory, mDeflater, 131072);
            mStream.Write(pBytes, 0, pBytes.Length);
            mStream.Close();
            return mMemory.ToArray();
        }

        /// <summary>
        /// 解压字符串
        /// </summary>
        /// <param name="pBytes"></param>
        /// <returns></returns>
        public byte[] DeCompress(byte[] pBytes)
        {
            ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream mStream = new ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream(new MemoryStream(pBytes));
            MemoryStream mMemory = new MemoryStream();
            Int32 mSize;
            byte[] mWriteData = new byte[4096];
            while (true)
            {
                mSize = mStream.Read(mWriteData, 0, mWriteData.Length);
                if (mSize > 0)
                {
                    mMemory.Write(mWriteData, 0, mSize);
                }
                else
                {
                    break;
                }
            }
            mStream.Close();
            return mMemory.ToArray();
        }

        /// <summary>
        /// 获取工作详细信息
        /// </summary>
        /// <param name="workID"></param>
        /// <returns></returns>
        public string GetWorkDetail(Guid workID)
        {
            string workDetail;
            using (SqlDataReader sdr = GetDataReader("select 序号,目标名称,目标内容,备注,类别名称 from 工作视图 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", workID) }))
                if (sdr.Read())
                {
                    workDetail = "第" + sdr[0] + "项："+sdr[1]+"<br/>";
                    workDetail+="目标名称："+sdr[2] + "<br/>";
                    workDetail += sdr[3].ToString().Trim() == "" ? "" : "备注：" + sdr[3] + "<br/>";
                    workDetail+= "类别名称：" + sdr[4] + "<br/>";
                    return workDetail;
                }
                else
                    return string.Empty;
        }

        /// <summary>
        /// 获取工作目标内容
        /// </summary>
        /// <param name="workID"></param>
        /// <returns></returns>
        public string GetWorkContent(Guid workID)
        {
            using (SqlDataReader sdr = GetDataReader("select 目标内容 from 工作 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", workID) }))
                if (sdr.Read())
                    return sdr[0].ToString();
                else
                    return string.Empty;
        }

        public string GetWorkContent(Guid workID,string tableName)
        {
            using (SqlDataReader sdr = GetDataReader("select 目标内容 from "+tableName+" where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", workID) }))
                if (sdr.Read())
                    return sdr[0].ToString();
                else
                    return string.Empty;
        }

        public string GetWeekSchedule(Guid workID, int weekOfYear)
        {
            string weekSchedule = "";
            using (SqlDataReader sdr = GetDataReader("select 周计划 from 周节点视图 where 工作ID=@工作ID and 周数=@周数", new SqlParameter[] { new SqlParameter("@工作ID",workID),
                 new SqlParameter("@周数",weekOfYear)
            }))
                if (sdr.Read())
                    weekSchedule = sdr[0].ToString();
            return weekSchedule;
        }

        public string GetWeekExecution(Guid workID, int weekOfYear)
        {
            string weekExecution = "";
            using (SqlDataReader sdr = GetDataReader("select 周完成 from 周节点视图 where 工作ID=@工作ID and 周数=@周数", new SqlParameter[] { new SqlParameter("@工作ID",workID),
                 new SqlParameter("@周数",weekOfYear)
            }))
                if (sdr.Read())
                    weekExecution = sdr[0].ToString();
            return weekExecution;
        }

        /// <summary>
        /// 获取当前周的周状态
        /// </summary>
        /// <param name="monthID"></param>
        /// <param name="weekOfYear"></param>
        /// <returns></returns>
        public int GetWeekState(Guid workID, int weekOfYear)
        {
            int state = 0;
            using (SqlDataReader sdr = GetDataReader("select 周状态 from 周节点视图 where 工作ID=@工作ID and 周数=@周数", new SqlParameter[] {
                new SqlParameter("@工作ID",workID),
                 new SqlParameter("@周数",weekOfYear)
            }))
                if (sdr.Read())
                    state = int.Parse(sdr[0].ToString());
            return state;
        }
        /*
        /// <summary>
        /// 转换图片地址，如果地址为空则显示默认无图片的地址
        /// </summary>
        /// <param name="sourceAddress">源地址</param>
        /// <returns>返回处理过的图片地址</returns>
        public string TransformPicAddress(string sourceAddress)
        {
            if (string.IsNullOrWhiteSpace(sourceAddress))
                return "~/Images/noImg.jpg";
            return sourceAddress;
        }

        /// <summary>
        /// 将过长字符串截断并在其后添加“...”
        /// </summary>
        /// <param name="str">要截断的字符串</param>
        /// <param name="len">截断的长度</param>
        /// <returns>返回处理好的字符串</returns>
        public string cutStr(string str,int len)
        {
            if (str.Length > len)
                return str.Substring(0, len) + "...";
            return str;
        }

        /// <summary>
        /// 将长整型转换成带单位的注册资本数
        /// </summary>
        /// <param name="longStr">长整型的字符串形式</param>
        /// <returns>返回处理好的注册资本字符串</returns>
        public string LongStr2CapitalStr(string longStr)
        {
            if (string.IsNullOrEmpty(longStr))
                return "";
            long capital = Convert.ToInt64(longStr);
            string capitalStr = capital.ToString();
            if (capitalStr.Length > 8)
                return (capital / 100000000).ToString() + "亿";
            if (capitalStr.Length > 4)
                return (capital / 10000).ToString() + "万";
            if (capitalStr.Length > 3)
                return (capital / 1000).ToString() + "千";
            return capitalStr;
        }

    */
    }
}
