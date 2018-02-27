using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;


namespace WorkSchedule.Tools
{
    public partial class BuildWeeks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //BuildWeek();
            Core.Tools t = new Core.Tools();
            //t.BuildWeekOfYear(DateTime.Now.Year);

            for (int i = 1; i <= 12; i++)
                Response.Write(i+"月"+t.GetWeeksOfMonth(DateTime.Now.Year,i)+"个周<br>");
            Response.Write("执行成功");
        }

        public void BuildWeek()
        {
            int year = DateTime.Now.Year;
            int month = 1;
            DateTime weekStart = new DateTime(year, month, 1);
            DateTime monthEnd = weekStart.AddMonths(1).AddDays(-1);
            TimeSpan ts = new TimeSpan(monthEnd.Ticks- weekStart.Ticks);
            //int weeks = (int)Math.Ceiling(ts.TotalDays / 7);
            DateTime weekEnd = weekStart.AddDays(7 - Convert.ToInt16(weekStart.DayOfWeek));
            int i = 1;
            for (int m = 1; m <= 12; m++)
            {
                Response.Write(m + "月：<br>");
                do
                {
                    Response.Write("第" + i + "周：" + weekStart + " " + weekEnd + "<br>");
                    i++;
                    weekStart = weekEnd.AddDays(1);
                    weekEnd = weekStart.AddDays(6);
                } while (weekStart.Month == m);

                Response.Write("<br>");
            }

            /*
            for (int i = 1; i <= weeks; i++)
            {
                Response.Write("第"+i+"周："+weekStart + " " + weekEnd+ "<br>");
                weekStart = weekEnd.AddDays(1);
                weekEnd = weekStart.AddDays(6);
            }
            Response.Write(weekStart + " " + monthEnd);
            */
        }
    }
}