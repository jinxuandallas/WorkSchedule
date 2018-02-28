using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WorkSchedule.Test
{
    public partial class RepeaterTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Repeater1.DataSource = SqlDataSource1;
            //Repeater1.DataMember = "ID";
            //DataBind();
        }

        protected void Repeater1_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Response.Write(e.Item.DataItem + "<br>");
            //return;
            //if (e.Item.DataItem)
            //Table tt = (Table)e.Item.FindControl("ScheduleTable");
            TableRow tr = ((Table)e.Item.FindControl("ScheduleTable")).Rows[0];
            tr.Style.Value = "border-collapse:collapse;border-spacing:0px;padding: 0px; margin: 0px; ";
            for (int i = 1; i <= 12; i++)
            {
                TableCell tc = new TableCell();
                tc.Style.Value = "border-collapse: collapse; border-spacing: 0px; border: 0px;padding: 0px; margin: 0px;border: 1px solid #000000";
                Table t = new Table();
                /*
                 * TableStyle ts = new TableStyle();
                
                ts.CellPadding = 0;
                ts.CellSpacing = 0;
                t.ApplyStyle(ts);
                */
                t.Style.Value = "border-collapse: collapse; border-spacing: 0px;";
                t.Rows.Add(new TableRow());
                t.Rows[0].Cells.Add(new TableCell());
                t.Rows[0].Cells[0].Text = "aaaa";
                t.Rows[0].Cells[0].ColumnSpan = 4;
                //t.Rows[0].Cells[0].Style.Value = " border-style: solid; border-width: 1px 1px 1px 1px; border-color: #000000;";

                for (int j = 0; j < 4; j++)
                {
                    t.Rows.Add(new TableRow());
                    TableCell wtc = new TableCell();
                    wtc.Text = j.ToString();
                    /*
                     * TableItemStyle tis = new TableItemStyle();
                    
                    tis.BorderStyle = BorderStyle.Solid;
                    tis.BorderWidth = 1;
                    tis.BorderColor = System.Drawing.Color.Black;
                    wtc.ApplyStyle(tis);
                    */
                    wtc.Style.Value = "padding: 0px; margin: 0px; border-style: solid; border-width: 1px 1px 1px 0px; border-color: #000000;";
                    t.Rows[1].Cells.Add(wtc);
                }

                tc.Controls.Add(t);
                tr.Cells.Add(tc);
            }
            //Response.Write("xxx");
            //e.Item.con = tt;
        }
    }
}