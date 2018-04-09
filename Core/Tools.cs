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
        protected ManageClass mc = new ManageClass();
        //protected ScheduleClass sc;
        protected WorkClass wc = new WorkClass();
        protected UserClass uc = new UserClass();

        public Tools()
        {
            year = DateTime.Now.Year;
            //mc = new ManageClass();
            //wc = new WorkClass();
            //uc = new UserClass();
        }

        public Tools(int pyear)
        {
            year = pyear;
            //mc = new ManageClass(pyear);
            //wc = new WorkClass();
            //uc = new UserClass();
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
                wc = new WorkClass();
                uc = new UserClass();
                taskID = wc.GetTaskIDBySN(wl.SN);
                if (taskID == Guid.Empty)
                    break;
                staffID = uc.GetStaffIDByName(wl.LeaderName);
                if (staffID == 0)
                    break;

                AddWorkLeader(taskID, staffID);

            }

        }

        

        

        public void AddWorkLeader(Guid taskID, int staffID)
        {
            //return;
            ExecuteSql("insert 工作责任领导(工作ID,工作人员ID) values(@工作ID,@工作人员ID)", new SqlParameter[] {
                new SqlParameter("@工作ID", taskID),
                new SqlParameter("@工作人员ID", staffID)
            });
        }

        public DataTable DealMonthSchedule(string tableName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable();
            dt.Columns.Add("目标名称", Type.GetType("System.String"));
            dt.Columns.Add("目标节点", Type.GetType("System.String"));
            string scheduleName;
            using (SqlDataReader sdr = GetDataReader("select 目标名称,目标节点或完成时限 from "+ tableName))
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
            mc = new ManageClass();

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
                    mc.AddMonthSchedule(workID, task, startMonth, endMonth, true);
                }

                //匹配8月：
                else if ((r = Regex.Match(task, @"^\d*月：").Value) != "")
                {
                    startMonth = int.Parse(Regex.Match(r, @"^\d*").Value);
                    ExecuteSql("update 临时目标节点 set 识别=2 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                    mc.AddMonthSchedule(workID, task, startMonth, 0, true);
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

                    mc.AddMonthSchedule(workID, task, startMonth, endMonth, true);
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
                    mc.AddMonthSchedule(workID, task, startMonth, endMonth, true);
                }

                //匹配7月
                else if ((r = Regex.Match(task, @"\d+月").Value) != "")
                {
                    startMonth = int.Parse(Regex.Match(r, @"\d+").Value);
                    ExecuteSql("update 临时目标节点 set 识别=6 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", int.Parse(dr[2].ToString())) });
                    mc.AddMonthSchedule(workID, task, startMonth, 0, true);
                }
                //task = task.Substring(task.i)
            }

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

        //public bool ArrangeDatabase(string argument)
        //{
        //    if (argument == "目标节点")
        //    {
        //        //ExecuteSql("update 重点工作 set 目标节点=")
        //    }
        //    return true;
        //}


        
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
