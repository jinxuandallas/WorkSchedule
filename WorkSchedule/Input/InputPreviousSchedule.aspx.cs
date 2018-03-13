using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;

namespace WorkSchedule.Input
{
    public partial class InputPreviousSchedule : System.Web.UI.Page
    {

        protected int[] weeksOfMonth;

        /// <summary>
        /// 本年度所有工作的ID
        /// </summary>
        protected Guid[] userWorkID;

        protected Dictionary<Guid, int[]> existMonths;
        protected Dictionary<Guid, Dictionary<int, int>> existWeeks;
        protected int userID;

        public Core.Tools tool;
        public ShowScheduleClass ss;
        protected void Page_Load(object sender, EventArgs e)
        {
            tool = new Core.Tools();
            ss = new ShowScheduleClass();

            Session["ID"] = 4;

            if (Session["ID"] == null || string.IsNullOrWhiteSpace(Session["ID"].ToString()))
                Response.Redirect("~/default.aspx");

            userID = int.Parse(Session["ID"].ToString());

            if (IsPostBack)
            {
                weeksOfMonth = (int[])ViewState["weeksOfMonth"];
                userWorkID = (Guid[])ViewState["userWorkID"];
                existMonths = (Dictionary<Guid, int[]>)ViewState["existMonths"];
                existWeeks = (Dictionary<Guid, Dictionary<int, int>>)ViewState["existWeeks"];
            }
            else
                PreLoadData();

            SqlDataSource1.SelectParameters["year"].DefaultValue = tool.year.ToString();
            RepeaterSchedule.DataBind();
        }

        protected void PreLoadData()
        {
            //获取一年中所有月份的每个月包含的周数量
            weeksOfMonth = tool.GetWeeksOfAllMonth();

            userWorkID = tool.GetUserWorkID(userID);

            existMonths = new Dictionary<Guid, int[]>();
            existWeeks = new Dictionary<Guid, Dictionary<int, int>>();
            foreach (Guid wid in userWorkID)
            {
                existMonths.Add(wid, tool.GetExistTaskMonths(wid));
                existWeeks.Add(wid, tool.GetExistTaskWeeksAndState(wid, true));
            }

            ViewState["weeksOfMonth"] = weeksOfMonth;
            ViewState["userWorkID"] = userWorkID;
            ViewState["existMonths"] = existMonths;
            ViewState["existWeeks"] = existWeeks;
        }

        protected void RepeaterSchedule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

        }

        protected void RepeaterSchedule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
    }
}