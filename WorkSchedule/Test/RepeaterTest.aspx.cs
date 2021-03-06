﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using System.Data;

namespace WorkSchedule.Test
{
    public partial class RepeaterTest : System.Web.UI.Page
    {
        protected Core.Tools tool;
        int[] weeksOfMonth;

        /// <summary>
        /// 本年度所有工作的ID
        /// </summary>
        Guid[] allWorkID;
        protected ScheduleClass sc;
        protected WorkClass wc;
        protected Dictionary<Guid, List<Dictionary<int,int>>> allMonthWeekInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            tool = new Core.Tools();
            sc = new ScheduleClass();
            wc = new WorkClass();
            PreLoadData();
            
            //if (!IsPostBack)
            //{
            Repeater1.DataBind();
            //    ViewState["repeater"] = Repeater1.ItemTemplate;
            //}
            //else
            //    Repeater1.ItemTemplate = (ITemplate )ViewState["repeater"];
        }
        
        /// <summary>
        /// 预读repeater控件中所需的数据以提高效率
        /// </summary>
        protected void PreLoadData()
        {
            //获取一年中所有月份的每个月包含的周数量
            weeksOfMonth = tool.GetWeeksOfAllMonth();

            allWorkID = wc.GetAllWorkID();

            //foreach(Guid wid in allWorkID)


            
        }
        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //return;
            int weekOfYear = 0;


            //if (e.Item.ItemType != ListItemType.Item)
            //    return;
            Table table = (Table)e.Item.FindControl("ScheduleTable");
            if (table == null)
                return;

            Guid workID = Guid.Parse((((DataRowView)e.Item.DataItem)["ID"].ToString()));

            TableRow tr = table.Rows[0];
            //任务中的一行表格
            tr.Style.Value = "border-collapse:collapse;border-spacing:0px;padding: 0px; margin: 0px;";
            for (int i = 1; i <= 12; i++)
            {
                int[] existMonth = sc.GetExistTaskMonths(workID);
                //int[] existMonth = { 1, 2, 3, 4, 5, 7, 8, 9, 10, 11, 12 };
                //月表格
                TableCell tc = new TableCell();
                tc.Style.Value = "border-collapse: collapse; border-spacing: 0px; border: 0px;padding: 0px; margin: 0px;border: 1px solid #000000;";
                //月表格中的嵌套表格
                Table t = new Table();
                /*
                 * TableStyle ts = new TableStyle();
                
                ts.CellPadding = 0;
                ts.CellSpacing = 0;
                t.ApplyStyle(ts);
                */

                t.Style.Value = "border-collapse: collapse; border-spacing: 0px;width:" + weeksOfMonth[i - 1] * 22 + "px; text-align: center;";
                t.Rows.Add(new TableRow());
                t.Rows[0].Cells.Add(new TableCell());
                //if (tool.HasMonthTask(workID, new DateTime(DateTime.Now.Year, i, 1)))
                //t.Style.Value += "background-color: #FFFF99";
                if (Array.IndexOf(existMonth, i) != -1)
                {
                    t.Rows[0].Cells[0].ColumnSpan = weeksOfMonth[i - 1];

                    LinkButton lb = new LinkButton();
                    lb.Text = i + "月";
                    lb.Font.Underline = false;
                    lb.CommandName = "monthLinkButton";
                    lb.CommandArgument =workID+"$"+ i.ToString();
                    t.Rows[0].Cells[0].Controls.Add(lb);

                    //t.Rows[0].Cells[0].Style.Value = " border-style: solid; border-width: 1px 1px 1px 1px; border-color: #000000;";

                    for (int w = 1; w <= weeksOfMonth[i - 1]; w++)
                    {
                        weekOfYear++;
                        //添加第二行
                        t.Rows.Add(new TableRow());
                        TableCell wtc = new TableCell();
                        wtc.Text = weekOfYear.ToString("00");


                        /*
                         * TableItemStyle tis = new TableItemStyle();

                        tis.BorderStyle = BorderStyle.Solid;
                        tis.BorderWidth = 1;
                        tis.BorderColor = System.Drawing.Color.Black;
                        wtc.ApplyStyle(tis);
                        */
                        wtc.Style.Value = "padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 1px 0px; border-color: #000000;width:25px";
                        switch (sc.GetWeekState(workID, weekOfYear))
                        {
                            case 0:
                                break;
                            case 1:
                                wtc.Style.Value += "; background-color: #FFFF99;";
                                break;
                            case 2:
                                wtc.Style.Value += "; background-color: #D04242";
                                break;
                            case 3:
                                wtc.Style.Value += "; background-color: #3399FF;";
                                break;

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
            //Response.Write("xxx");
            //e.Item.con = tt;
        }


        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                Panel p = (Panel)e.Item.FindControl("Panel1");
                p.Visible = !p.Visible;
                ((LinkButton)e.Item.FindControl("LinkButton1")).Text = p.Visible ? "折叠" : "详细";
            }

            else if (e.CommandName == "button")
            {
                ((Label)e.Item.FindControl("Label1")).Text = DateTime.Now.ToString();
            }

            else if (e.CommandName == "monthLinkButton")
            {
                Panel p = (Panel)e.Item.FindControl("monthPanel");
                p.Visible = true;
                string[] arg = e.CommandArgument.ToString().Split(new char[] { '$'});
                string monthDeail = sc.GetMonthScheduleDetail(Guid.Parse(arg[0]), int.Parse(arg[1]));
                ((Label)e.Item.FindControl("monthLabel")).Text = monthDeail;
            }
            //Repeater1.DataBind();
        }
    }
}