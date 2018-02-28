using System;
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
        Core.Tools tool;
        protected void Page_Load(object sender, EventArgs e)
        {
            //Repeater1.DataSource = SqlDataSource1;
            //Repeater1.DataMember = "ID";
            //DataBind();
            tool = new Core.Tools();
            //if (!IsPostBack)
            DataBind();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            int weekOfMonth, weekOfYear = 0;

            //Response.Write(e.Item.DataItem + "<br>");
            //return;
            //if (e.Item.DataItem)
            //Table tt = (Table)e.Item.FindControl("ScheduleTable");
            //if (e.Item.ItemType != ListItemType.Item)
            //    return;
            Table table = (Table)e.Item.FindControl("ScheduleTable");
            //table.Width = 53 * 25;
            if (table == null)
                return;

            Guid workID = Guid.Parse((((DataRowView)e.Item.DataItem)["ID"].ToString()));
            TableRow tr = table.Rows[0];
            //任务中的一行表格
            tr.Style.Value = "border-collapse:collapse;border-spacing:0px;padding: 0px; margin: 0px;";
            for (int i = 1; i <= 12; i++)
            {
                weekOfMonth = tool.GetWeeksOfMonth(DateTime.Now.Year, i);
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

                t.Style.Value = "border-collapse: collapse; border-spacing: 0px;width:" + weekOfMonth * 22 + "px; text-align: center;";
                t.Rows.Add(new TableRow());
                t.Rows[0].Cells.Add(new TableCell());
                if (tool.HasMonthTask(workID, new DateTime(DateTime.Now.Year, i, 1)))
                //t.Style.Value += "background-color: #FFFF99";

                {
                    t.Rows[0].Cells[0].Text = i + "月";
                    t.Rows[0].Cells[0].ColumnSpan = weekOfMonth;
                    //t.Rows[0].Cells[0].Style.Value = " border-style: solid; border-width: 1px 1px 1px 1px; border-color: #000000;";

                    for (int w = 1; w <= weekOfMonth; w++)
                    {
                        weekOfYear++;
                        //添加第二行
                        t.Rows.Add(new TableRow());
                        TableCell wtc = new TableCell();
                        LinkButton lb = new LinkButton();
                        lb.Text = weekOfYear.ToString("00");
                        lb.Font.Underline = false;
                        lb.CommandName = "weekLinkButton";
                        lb.CommandArgument = weekOfYear.ToString();
                        wtc.Controls.Add(lb);
                        /*
                         * TableItemStyle tis = new TableItemStyle();

                        tis.BorderStyle = BorderStyle.Solid;
                        tis.BorderWidth = 1;
                        tis.BorderColor = System.Drawing.Color.Black;
                        wtc.ApplyStyle(tis);
                        */
                        wtc.Style.Value = "padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 1px 0px; border-color: #000000;width:25px";
                        t.Rows[1].Cells.Add(wtc);
                    }
                }
                else
                {
                    weekOfYear += weekOfMonth;
                    tc.Style.Value = tc.Style.Value.Replace("border: 1px solid #000000;", "border-width:0px");
                }
                tc.Controls.Add(t);
                tr.Cells.Add(tc);
            }
            //Response.Write("xxx");
            //e.Item.con = tt;
        }

        //protected void LinkButton1_Click(object sender, EventArgs e)
        //{
        //    Panel1.Visible = !Panel1.Visible;
        //    if (Panel1.Visible)
        //        LinkButton1.Text = "折叠";
        //    else
        //        LinkButton1.Text = "详细";
        //}

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    Panel1.ToolTip = DateTime.Now.ToString();
        //    Label1.Text = DateTime.Now.ToString();
        //}

        protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Detail")
            {
                Panel p = (Panel)e.Item.FindControl("Panel1");
                p.Visible = !p.Visible;
                ((LinkButton)e.Item.FindControl("LinkButton1")).Text = p.Visible ? "折叠" : "详细";
            }

            if (e.CommandName == "button")
            {
                ((Label)e.Item.FindControl("Label1")).Text = DateTime.Now.ToString();
            }

            if (e.CommandName == "weekLinkButton")
            {
                Panel p = (Panel)e.Item.FindControl("weekPanel");
                p.Visible = true;
                ((Label)e.Item.FindControl("weekLabel")).Text = e.CommandArgument + "：";
            }
            //DataBind();
        }
    }
}