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


namespace Core
{
    public class Tools : Database
    {
        public Tools()
        {

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

            foreach (string ins in staff)
                ExecuteSql(@"insert 工作人员(姓名,所属部门,人员类型) values (@姓名,1,1)", new SqlParameter[] { new SqlParameter("@姓名", ins) });
            //List<string> distinctStaff = staff.Distinct<string>().ToList<string>();
            //int i = 3;
        }


        public void ImportTasts(string workTable)
        {
            ExecuteSql("insert 工作 (序号,目标名称,目标内容,年份,备注) select 序号,目标名称,目标内容,year(getdate()),备注 from " + workTable);
        }

        /*
        public void BuildWorksRelevantTable()
        {

        }
        */

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

                //匹配1-8月
                if ((r = Regex.Match(task, @"^\d*-\d*月").Value) != "")
                {
                    startMonth = int.Parse(Regex.Match(Regex.Match(r, @"^\d*-").Value, @"^\d*").Value);

                    //注意此处正则表达式@"\d*月"不能写为@"-\d*月"，否则会把-当成负号，"-4月"理解成-4
                    endMonth = int.Parse(Regex.Match(Regex.Match(r, @"\d*月").Value, @"\d*").Value);
                    //ExecuteSql("update 临时目标节点 set 识别=1 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                    AddMonthSchedule(workID, task, startMonth, endMonth);
                }

                //匹配8月：
                else if ((r = Regex.Match(task, @"^\d*月：").Value) != "")
                {
                    startMonth = int.Parse(Regex.Match(r, @"^\d*").Value);
                    //ExecuteSql("update 临时目标节点 set 识别=2 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                    AddMonthSchedule(workID, task, startMonth, 0);
                }

                //匹配8月底前或者8月底之前或者8月底
                else if ((r = Regex.Match(task, @"^\d*月底前").Value) != "" || (r = Regex.Match(task, @"^\d*月底之前").Value) != "" || (r = Regex.Match(task, @"^\d*月底").Value) != "")
                {
                    //startMonth需判断前面几个月有没有目标节点，如果没有则从1月份开始，如果有则续接上个节点目标的月份
                    using (SqlDataReader sdr = GetDataReader("select top 1 month(年份) from 月节点 where 工作ID=@工作ID order by 年份 desc ", new SqlParameter[] { new SqlParameter("@工作ID", workID) }))
                        if (sdr.HasRows)
                        {
                            sdr.Read();
                            startMonth = int.Parse(sdr[0].ToString()) + 1;
                        }
                        else
                            startMonth = 1;

                    endMonth = int.Parse(Regex.Match(r, @"^\d*").Value);

                    //ExecuteSql("update 临时目标节点 set 识别=3 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });

                    AddMonthSchedule(workID, task, startMonth, endMonth);
                }

                //匹配7、8月
                else if ((r = Regex.Match(task, @"^\d*、\d*月").Value) != "")
                {
                    startMonth = int.Parse(Regex.Match(r, @"^\d*").Value);
                    endMonth = int.Parse(Regex.Match(r, @"、\d*").Value.Remove(0, 1));
                    //ExecuteSql("update 临时目标节点 set 识别=4 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                    AddMonthSchedule(workID, task, startMonth, endMonth);
                }

                //匹配7月
                else if ((r = Regex.Match(task, @"\d+月").Value) != "")
                {
                    startMonth = int.Parse(Regex.Match(r, @"\d+").Value);
                    ExecuteSql("update 临时目标节点 set 识别=5 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                    AddMonthSchedule(workID, task, startMonth, 0);
                }
                //task = task.Substring(task.i)
            }

        }

        public void AddMonthSchedule(Guid workID, string task, int startMonth, int endMonth)
        {
            //如果初始月份为0则不添加本条工作计划
            if (startMonth == 0)
                return;

            if (endMonth == 0 || startMonth == endMonth)
                UpdateMonthSchedule(workID, task, startMonth);
            else
                for (int i = startMonth; i <= endMonth; i++)
                    UpdateMonthSchedule(workID, task, i);
        }

        public void UpdateMonthSchedule(Guid workID, string task, int month)
        {
            string date = DateTime.Now.Year + "-" + month + "-1";
            string id = string.Empty;
            //先判断月节点表中有没有此月的节点数据，如果有则用update语句在原数据后面追加新的节点数据，如果没有则用insert添加新节点数据
            using (SqlDataReader sdr = GetDataReader("select 目标节点,ID from 月节点 where 工作ID=@工作ID and 年份=@年份", new SqlParameter[] { new SqlParameter("@工作ID",workID),
                new SqlParameter("@年份",date)
            }))
            {
                string sql = string.Empty;
                if (sdr.HasRows)
                {
                    sdr.Read();
                    task = sdr[0].ToString() + " " + task;
                    sql = "update 月节点 set 目标节点=@目标节点 where ID=@ID";
                    id = sdr[1].ToString();
                }
                sdr.Close();

                if (sql != string.Empty)
                {
                    ExecuteSql(sql, new SqlParameter[] { new SqlParameter("@目标节点",task),
                        new SqlParameter("@ID",id)
                    });
                    return;
                }
            }

            ExecuteSql("insert 月节点(工作ID,目标节点,年份) values(@工作ID,@目标节点,@年份)", new SqlParameter[] {new SqlParameter("@工作ID",workID),
                    new SqlParameter("@目标节点",task),
                    new SqlParameter("@年份",date)
                    });
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
                    ExecuteSql("insert 周(周数,开始日期,结束日期) values(@周数,@开始日期,@结束日期)", new SqlParameter[] {new SqlParameter("@周数",i),
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

        public bool HasMonthTask(Guid workID, DateTime dt)
        {
            bool result;
            using (SqlDataReader sdr = GetDataReader("select ID from 月节点 where 工作ID=@工作ID and 年份=@年份", new SqlParameter[] { new SqlParameter("@工作ID", workID),
                new SqlParameter("@年份", dt.ToString("yyyy/MM/dd")) }))
            {
                sdr.Read();
                result = sdr.HasRows ? true : false;
            }
            return result;
        }
        public int GetWeeksOfMonth(int year, int month)
        {
            int m;
            using (SqlDataReader sdr = GetDataReader("select count(*) from 周 where datepart(yyyy,开始日期)=@年份 and datepart(mm,开始日期)=@月份", new SqlParameter[] { new SqlParameter("@年份", year),
            new SqlParameter("@月份", month)
            }))
            {
                sdr.Read();
                m = int.Parse(sdr[0].ToString());
            }

            return m;
        }
        /*
        /// <summary>
        /// 将注册资本从文本格式传唤成长整型格式
        /// </summary>
        /// <param name="num">要转换的数字</param>
        /// <param name="unit">单位（亿、万、千）</param>
        /// <returns>转换后的长整型注册资本数</returns>
        public long CapitalStr2Long (string num,string unit)
        {
            int capitalNum;
            long capital;
            if (num.Length > 7 || !int.TryParse(num, out capitalNum)) return 0;
            switch(unit)
            {
                case "千":
                    capital = capitalNum * 1000;
                    break;
                case "万":
                    capital = capitalNum * 10000;
                    break;
                case "亿":
                    capital = capitalNum * 100000000;
                    break;
                default:
                    capital = 0;
                    break;
            }
            return capital;
        }

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
