using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;


namespace Core
{
    public class ScheduleClass : Database
    {
        public int year;
        Core.Tools t;
        public ScheduleClass()
        {
            year = DateTime.Now.Year;
        }

        public ScheduleClass(int pyear)
        {
            year = pyear;
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
        
        /// <summary>
        /// 为了生成的统一LinkButton样式
        /// </summary>
        /// <param name="workID"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public LinkButton GetLinkButton(Guid workID, int month)
        {
            LinkButton lb = new LinkButton();

            lb.Text = month + "月";
            lb.Font.Underline = false;
            lb.CommandName = "monthLinkButton";
            lb.CommandArgument = workID + "$" + month.ToString();
            lb.Width = Unit.Percentage(100);
            lb.Height = Unit.Percentage(100);
            lb.BackColor = System.Drawing.Color.Lavender;

            return lb;
        }
        public string GetMonthSchedule(Guid workID, int month)
        {
            string monthSchedule= month + "月目标节点："; 
            using (SqlDataReader sdr = GetDataReader("select 目标节点 from 月节点 where 工作ID=@工作ID and datepart(mm,日期)=@月份", new SqlParameter[] {
                new SqlParameter("@工作ID",workID),
                new SqlParameter("@月份",month)
            }))
                if (sdr.Read())
                    monthSchedule += "“" + sdr[0] + "”<br/>";
            return monthSchedule;
        }

        /// <summary>
        /// 获取月节点详细信息（包括周计划）
        /// </summary>
        /// <param name="workID"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public string GetMonthScheduleDetail(Guid workID,int month)
        {
            string monthScheduleDeatil= month+"月目标节点：";
            using (SqlDataReader sdr = GetDataReader("select 目标节点 from 月节点 where 工作ID=@工作ID and datepart(mm,日期)=@月份", new SqlParameter[] {
                new SqlParameter("@工作ID",workID),
                new SqlParameter("@月份",month)
            }))
                if (sdr.Read())
                    monthScheduleDeatil += "“"+sdr[0]+ "”<br/><br/>";
            DataSet weekDetail = GetDataSet("select 月节点日期,周数,周计划,周完成,状态,开始日期,结束日期 from 周节点视图 where 周状态!=0 and 工作ID=@工作ID and datepart(mm,月节点日期)=@月份",new SqlParameter[] {
                new SqlParameter("@工作ID",workID),
                new SqlParameter("@月份",month)
            });

            //此处也许需要判断weekDetail中是否有数据
            if (weekDetail.Tables[0].Rows.Count > 0)
            {
                //monthScheduleDeatil += "<br/>";
                foreach (DataRow dr in weekDetail.Tables[0].Rows)
                    //(dr["状态"].ToString()=="2"|| dr["状态"].ToString() == "4" ? @"<p style=""color:red""> "+ dr["状态"] + "</p>":dr["状态"])
                    monthScheduleDeatil += "第" + dr["周数"] + "周（" + DateTime.Parse(dr["开始日期"].ToString()).ToString("M月d日") + "—" + DateTime.Parse(dr["结束日期"].ToString()).ToString("M月d日") + "）：" + (dr["状态"].ToString() == "计划未完成" ? @"<span style=""color:red""> " + dr["状态"] + "</span>" : dr["状态"]) + "<br/>&nbsp;&nbsp;周工作计划：" + dr["周计划"] + "<br/>&nbsp;&nbsp;周落实情况：" + dr["周完成"] + "<br/><br/>";
            }
            //monthScheduleDeatil += "<br/>";
            return monthScheduleDeatil;
        }

        /// <summary>
        /// 读取字符串形式的月目标节点
        /// </summary>
        /// <param name="ss"></param>
        /// <returns>返回每个月的节点形式的泛型</returns>
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

        /// <summary>
        /// 识别字符串中的目标节点，读出月份
        /// </summary>
        /// <param name="task"></param>
        /// <param name="lastMonth"></param>
        /// <returns></returns>
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
        /*
        public StringBuilder GetMonthSchedule()
        {
           SqlDataReader works=GetDataReader("select * from 2018重点工作");

        }
        */

        //public int ConvertLabeltoWeek(string l)
        //{
        //    return int.Parse(Regex.Match(Regex.Match(l, @"第\d*周").Value, @"\d*").Value);
        //}

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
        /// 获取周计划
        /// </summary>
        /// <param name="workID"></param>
        /// <param name="weekOfYear"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 获取周落实情况
        /// </summary>
        /// <param name="workID"></param>
        /// <param name="weekOfYear"></param>
        /// <returns></returns>
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


        /// <summary>
        /// 获取某项工作的所有存在计划的月份列表
        /// </summary>
        /// <param name="workID"></param>
        /// <returns></returns>
        public int[] GetExistTaskMonths(Guid workID)
        {
            t = new Tools();
            DataSet ds = GetDataSet("select distinct datepart(mm,日期) from 月节点 where 工作ID = @工作ID", new SqlParameter[] { new SqlParameter("@工作ID", workID) });
            return t.GetIntArrFromDataSet(ds);
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




    }


    //public bool HasMonthTask(Guid workID, DateTime dt)
    //{
    //    bool result;
    //    using (SqlDataReader sdr = GetDataReader("select ID from 月节点 where 工作ID=@工作ID and 日期=@日期", new SqlParameter[] { new SqlParameter("@工作ID", workID),
    //            new SqlParameter("@日期", dt.ToString("yyyy/MM/dd")) }))
    //    {
    //        sdr.Read();
    //        result = sdr.HasRows ? true : false;
    //    }
    //    return result;
    //}


}

    

