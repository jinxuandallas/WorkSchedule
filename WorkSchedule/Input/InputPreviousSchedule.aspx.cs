using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using System.Data;


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
        protected int editMonth;
        protected Guid editWorkID;

        public Core.Tools tool;
        public ShowScheduleClass ss;
        public Core.AddScheduleClass asc;
        protected void Page_Load(object sender, EventArgs e)
        {
            tool = new Core.Tools();
            ss = new ShowScheduleClass();
            asc = new AddScheduleClass();


            //Session["UserID"] = 4;

            //if (Request.QueryString["id"] != null)
            //    Session["UserID"] = Request.QueryString["UserID"];

            if (Session["UserID"] == null || string.IsNullOrWhiteSpace(Session["UserID"].ToString()))
                Response.Redirect("~/default.aspx");

            userID = int.Parse(Session["UserID"].ToString());

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

            userWorkID = tool.GetAllWorkID();
            int[] existTaskMonths;
            existMonths = new Dictionary<Guid, int[]>();
            existWeeks = new Dictionary<Guid, Dictionary<int, int>>();
            foreach (Guid wid in userWorkID)
            {
                existTaskMonths = tool.GetExistTaskMonths(wid);
                //判断此项工作是否有月节点计划
                if (existTaskMonths != null)
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
            //return;
            int weekOfYear = 0;

            Table table = (Table)e.Item.FindControl("ScheduleTable");
            if (table == null)
                return;

            Guid workID = Guid.Parse((((DataRowView)e.Item.DataItem)["ID"].ToString()));

            TableRow tr = table.Rows[0];
            //任务中的一行表格
            tr.Style.Value = "border-collapse:collapse;border-spacing:0px;padding: 0px; margin: 0px;";
            for (int i = 1; i <= (DateTime.Now.Month == 12 ? 12 : DateTime.Now.Month + 1); i++)
            {

                TableCell tc = new TableCell();
                tc.Style.Value = "border-collapse: collapse; border-spacing: 0px; border: 0px;padding: 0px; margin: 0px;border: 1px solid #000000;";
                //月表格中的嵌套表格
                Table t = new Table();


                t.Style.Value = "border-collapse: collapse; border-spacing: 0px;width:" + weeksOfMonth[i - 1] * 22 + "px; text-align: center;";
                t.Rows.Add(new TableRow());
                t.Rows[0].Cells.Add(new TableCell());

                if (existMonths.ContainsKey(workID) && Array.IndexOf(existMonths[workID], i) != -1)
                {
                    t.Rows[0].Cells[0].ColumnSpan = weeksOfMonth[i - 1];

                    //LinkButton lb = new LinkButton();
                    //lb.Text = i + "月";
                    //lb.Font.Underline = false;
                    //lb.CommandName = "monthLinkButton";
                    //lb.CommandArgument = workID + "$" + i.ToString();

                    t.Rows[0].Cells[0].Controls.Add(ss.GetLinkButton(workID,i));

                    //t.Rows[0].Cells[0].Style.Value = " border-style: solid; border-width: 1px 1px 1px 1px; border-color: #000000;";

                    for (int w = 1; w <= weeksOfMonth[i - 1]; w++)
                    {
                        weekOfYear++;
                        //添加第二行
                        t.Rows.Add(new TableRow());
                        TableCell wtc = new TableCell();
                        wtc.Text = weekOfYear.ToString("00");


                        wtc.Style.Value = "padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 1px 0px; border-color: #000000;width:25px";

                        if (existWeeks[workID].ContainsKey(weekOfYear))
                        {
                            switch (existWeeks[workID][weekOfYear])
                            //switch (ss.GetWeekState(workID, weekOfYear))
                            {
                                case 0:
                                case 1:
                                    break;
                                case 2:
                                    wtc.Style.Value += "; background-color: #FF6600;";
                                    break;
                                case 4:
                                    wtc.Style.Value += "; background-color: #D04242";
                                    break;
                                case 3:
                                    wtc.Style.Value += "; background-color: #3399FF;";
                                    break;

                            }
                        }
                        t.Rows[1].Cells.Add(wtc);
                    }
                }
                else
                {
                    weekOfYear += weeksOfMonth[i - 1];
                    tc.Style.Value = tc.Style.Value.Replace("border: 1px solid #000000;", "border-width:0px");
                }
                tc.Controls.Add(t);
                tr.Cells.Add(tc);
            }
        }

        protected void RepeaterSchedule_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "monthLinkButton")
            {
                Panel p = (Panel)e.Item.FindControl("monthPanel");
                p.Visible = true;
                string[] arg = e.CommandArgument.ToString().Split(new char[] { '$' });

                editWorkID = Guid.Parse(arg[0]);
                editMonth = int.Parse(arg[1]);

                string monthDeail = ss.GetMonthSchedule(editWorkID, editMonth);
                ((Label)e.Item.FindControl("monthLabel")).Text = monthDeail;

                ViewState["editWorkID"] = editWorkID;
                ViewState["editMonth"] = editMonth;

                ((Repeater)e.Item.FindControl("RepeaterWeekSchedule")).DataBind();
            }

            //if (e.CommandName == "SubmitWeekSchedule")
            //{
            //    Response.Write("xx");
            //}
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            string weekSchedule, weekExecution;
            int state, weekOfYear;

            editWorkID = Guid.Parse(ViewState["editWorkID"].ToString());
            editMonth = int.Parse(ViewState["editMonth"].ToString());

            Guid monnthTaskID = tool.GetMonthID(editWorkID, editMonth);
            bool succeed;
            foreach (RepeaterItem ri in ((Repeater)((Control)sender).Parent.Parent).Items)
            {
                //Response.Write(ri.FindControl("TextBoxWeekSchedule") + "<br/>");
                //Response.Write(ri.FindControl("TextBoxWeekExecution") + "<br/>");
                weekOfYear = int.Parse(((Label)ri.FindControl("lbWeek")).Text);
                weekSchedule = ((TextBox)ri.FindControl("TextBoxWeekSchedule")).Text;
                weekExecution = ((TextBox)ri.FindControl("TextBoxWeekExecution")).Text;
                if (weekExecution.Trim() == "")
                    state = weekSchedule.Trim() == "" ? 0 : 1;
                else
                    state = ((CheckBox)ri.FindControl("CheckBoxState")).Checked ? 3 : 2;

                succeed = asc.InputWeekSchedule(monnthTaskID, weekOfYear, weekSchedule, weekExecution, state);

                if (succeed)
                {
                    PreLoadData();
                    RepeaterSchedule.DataBind();
                }
                //Response.Write(weekOfYear + "++" + weekSchedule + "-" + weekExecution + ":" + state + "<br/>");
            }
            //search(((Control)sender).Parent.Parent);
        }

        protected void search(Control c)
        {
            foreach (Control cc in c.Controls)
            {
                Response.Write(cc + "<br/>");
                search(cc);
            }

            Response.Write("<br/>");
        }
    }
}