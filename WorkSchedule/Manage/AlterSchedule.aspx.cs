﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using System.Data;

namespace WorkSchedule.Manage
{
    public partial class AlterSchedule : System.Web.UI.Page
    {
        protected Core.Tools tool;
        protected int[] weeksOfMonth;
        protected Guid[] projectCategoryLocationID;
        protected int category;
        /// <summary>
        /// 本年度所有工作的ID
        /// </summary>
        protected Guid[] allWorkID;
        protected string[] categoryName;
        protected int editMonth;
        protected Guid editWorkID;

        protected Dictionary<Guid, int[]> existMonths;
        protected Dictionary<Guid, Dictionary<int, int>> existWeeks;
        protected ScheduleClass sc;
        protected WorkClass wc;
        protected ManageClass mc;
        protected UserClass uc;

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["UserID"] = 11;

            if (Session["UserID"] == null || string.IsNullOrWhiteSpace(Session["UserID"].ToString()))
                Response.Redirect("~/Account/Login.aspx");

            tool = new Core.Tools();
            sc = new ScheduleClass();
            wc = new WorkClass();
            mc = new ManageClass();
            uc = new UserClass();

            if (uc.GetUserType(int.Parse(Session["UserID"].ToString())) != 1)
                Response.Redirect("~/Account/Login.aspx");

            category = 0;

            if (IsPostBack)
            {
                weeksOfMonth = (int[])ViewState["weeksOfMonth"];
                allWorkID = (Guid[])ViewState["allWorkID"];
                existMonths = (Dictionary<Guid, int[]>)ViewState["existMonths"];
                existWeeks = (Dictionary<Guid, Dictionary<int, int>>)ViewState["existWeeks"];
                projectCategoryLocationID = ViewState["projectCategoryLocationID"] as Guid[];
                categoryName = ViewState["categoryName"] as string[];

            }
            else
                PreLoadData();

            SqlDataSource1.SelectParameters["year"].DefaultValue = tool.year.ToString();
            //SqlDataSource1.DataBind();

            RepeaterSchedule.DataBind();
        }

        protected void PreLoadData()
        {
            //获取一年中所有月份的每个月包含的周数量
            weeksOfMonth = tool.GetWeeksOfAllMonth();



            allWorkID = wc.GetAllWorkID();
            int[] existTaskMonths;
            existMonths = new Dictionary<Guid, int[]>();
            existWeeks = new Dictionary<Guid, Dictionary<int, int>>();
            foreach (Guid wid in allWorkID)
            {
                existTaskMonths = sc.GetExistTaskMonths(wid);
                //判断此项工作是否有月节点计划
                if (existTaskMonths != null)
                    existMonths.Add(wid, sc.GetExistTaskMonths(wid));
                existWeeks.Add(wid, sc.GetExistTaskWeeksAndState(wid, true));
            }
            projectCategoryLocationID = wc.GetProjectCategoryLocationID();
            categoryName = wc.GetCategoryName();

            ViewState["weeksOfMonth"] = weeksOfMonth;
            ViewState["allWorkID"] = allWorkID;
            ViewState["existMonths"] = existMonths;
            ViewState["existWeeks"] = existWeeks;
            ViewState["projectCategoryLocationID"] = projectCategoryLocationID;
            ViewState["categoryName"] = categoryName;
        }

        protected void RepeaterSchedule_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //return;
            int weekOfYear = 0;

            //if (e.Item.ItemType != ListItemType.Item)
            //    return;

            Table table = (Table)e.Item.FindControl("ScheduleTable");
            if (table == null)
                return;

            Guid workID = Guid.Parse((((DataRowView)e.Item.DataItem)["ID"].ToString()));

            if (category < 5 && projectCategoryLocationID[category] == workID)
            {
                Panel pc = e.Item.FindControl("ProjectCategory") as Panel;
                pc.Visible = true;
                (pc.FindControl("lbCategoryName") as Label).Text = categoryName[category];
                category++;
            }
            TableRow tr = table.Rows[0];
            //任务中的一行表格
            tr.Style.Value = "border-collapse:collapse;border-spacing:0px;padding: 0px; margin: 0px;";
            for (int i = 1; i <= 12; i++)
            {

                TableCell tc = new TableCell();
                tc.Style.Value = "border-collapse: collapse; border-spacing: 0px; border: 0px;padding: 0px; margin: 0px;border: 1px solid #000000;";
                //月表格中的嵌套表格
                Table t = new Table();


                t.Style.Value = "border-collapse: collapse; border-spacing: 0px;width:" + weeksOfMonth[i - 1] * 22 + "px; text-align: center;";
                t.Rows.Add(new TableRow());
                t.Rows[0].Cells.Add(new TableCell());

                //if (tool.HasMonthTask(workID, new DateTime(DateTime.Now.Year, i, 1)))
                //t.Style.Value += "background-color: #FFFF99";
                //existMonths[workID] != null &&

                if (existMonths.ContainsKey(workID) && Array.IndexOf(existMonths[workID], i) != -1)
                {
                    t.Rows[0].Cells[0].ColumnSpan = weeksOfMonth[i - 1];

                    //LinkButton lb = new LinkButton();
                    //lb.Text = i + "月";
                    //lb.Font.Underline = false;
                    //lb.CommandName = "monthLinkButton";
                    //lb.CommandArgument = workID + "$" + i.ToString();
                    //lb.Width = Unit.Percentage(100);
                    //lb.Height = Unit.Percentage(100);
                    //lb.BackColor = System.Drawing.Color.Lavender;

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
            if (e.CommandName == "button")
            {
                ((Label)e.Item.FindControl("Label1")).Text = DateTime.Now.ToString();
            }

            else if (e.CommandName == "monthLinkButton")
            {
                Panel p = (Panel)e.Item.FindControl("monthPanel");
                p.Visible = true;
                string[] arg = e.CommandArgument.ToString().Split(new char[] { '$' });

                editWorkID = Guid.Parse(arg[0]);
                editMonth = int.Parse(arg[1]);

                ((Label)e.Item.FindControl("monthLabel")).Text = sc.GetMonthSchedule(editWorkID, editMonth);

                //通过ViewState将当前编辑的工作ID和月份传给提交按钮事件
                ViewState["editWorkID"] = editWorkID;
                ViewState["editMonth"] = editMonth;

                ((Repeater)e.Item.FindControl("RepeaterWeekSchedule")).DataBind();
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            string weekSchedule, weekExecution;
            int state, weekOfYear;

            editWorkID = Guid.Parse(ViewState["editWorkID"].ToString());
            editMonth = int.Parse(ViewState["editMonth"].ToString());

            Guid monnthTaskID = sc.GetMonthID(editWorkID, editMonth);
            bool succeed = false;
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

            }

            if (succeed)
            {
                PreLoadData();
                RepeaterSchedule.DataBind();
            }
        }
    }
}