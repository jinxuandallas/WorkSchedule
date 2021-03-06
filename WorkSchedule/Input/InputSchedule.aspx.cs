﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using System.Data;


namespace WorkSchedule.Input
{
    public partial class InputSchedule : System.Web.UI.Page
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

        /// <summary>
        /// 本周已经编辑过的工作任务
        /// </summary>
        protected List<Guid> editedWorkID;
        //protected string monthDeail;

        public Core.Tools tool;
        public ScheduleClass sc;
        protected WorkClass wc;
        protected ManageClass mc;

        //public Core.AddScheduleClass asc;
        protected void Page_Load(object sender, EventArgs e)
        {
            tool = new Core.Tools();
            sc = new ScheduleClass();
            wc = new WorkClass();
            mc = new ManageClass();
            //asc = new AddScheduleClass();


            //Session["UserID"] = 12;

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
                editedWorkID=ViewState["editedWorkID"] as List<Guid> ;
            }
            else
                PreLoadData();

            SqlDataSource1.SelectParameters["year"].DefaultValue = tool.year.ToString();
            RepeaterSchedule.DataBind();
        }

        protected void PreLoadData()
        {
            //获取已编辑过的任务ID
            editedWorkID = wc.GetThisWeekEditedWorkID(userID);

            //获取一年中所有月份的每个月包含的周数量
            weeksOfMonth = tool.GetWeeksOfAllMonth();

            userWorkID = wc.GetUserWorkID(userID);
            int[] existTaskMonths;
            existMonths = new Dictionary<Guid, int[]>();
            existWeeks = new Dictionary<Guid, Dictionary<int, int>>();
            foreach (Guid wid in userWorkID)
            {
                existTaskMonths = sc.GetExistTaskMonths(wid);
                //判断此项工作是否有月节点计划
                if (existTaskMonths != null)
                    existMonths.Add(wid, sc.GetExistTaskMonths(wid));
                existWeeks.Add(wid, sc.GetExistTaskWeeksAndState(wid, true));
            }

            ViewState["weeksOfMonth"] = weeksOfMonth;
            ViewState["userWorkID"] = userWorkID;
            ViewState["existMonths"] = existMonths;
            ViewState["existWeeks"] = existWeeks;
            ViewState["editedWorkID"] = editedWorkID;
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
            //任务中的一行表格，勾画生成月份与周表格
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

                    t.Rows[0].Cells[0].Controls.Add(sc.GetLinkButton(workID, i));

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
                            //switch (sc.GetWeekState(workID, weekOfYear))
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

                //判断月节点信息是否为当月，如果不是则不能编辑显示内容包括周工作计划等信息
                string monthDeail = editMonth < DateTime.Now.Month-1 ? sc.GetMonthScheduleDetail(editWorkID, editMonth) : sc.GetMonthSchedule(editWorkID, editMonth);
                //string monthDeail = sc.GetMonthSchedule(editWorkID, editMonth);
                ((Label)e.Item.FindControl("monthLabel")).Text = monthDeail;

                //通过ViewState将当前编辑的工作ID和月份传给提交按钮事件
                ViewState["editWorkID"] = editWorkID;
                ViewState["editMonth"] = editMonth;

                //如果编辑的不为当前页则隐藏编辑文本框和repeater控件
                if (editMonth < DateTime.Now.Month-1)
                    ((Repeater)e.Item.FindControl("RepeaterWeekSchedule")).Visible = false;
                else
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

            Guid monnthTaskID = sc.GetMonthID(editWorkID, editMonth);
            bool succeed=false;
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

                succeed = mc.InputWeekSchedule(monnthTaskID, weekOfYear, weekSchedule, weekExecution, state);

                if (!succeed)
                    return;
                //Response.Write(weekOfYear + "++" + weekSchedule + "-" + weekExecution + ":" + state + "<br/>");
            }

            if (succeed)
            {
                PreLoadData();
                RepeaterSchedule.DataBind();
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