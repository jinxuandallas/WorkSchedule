using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WorkSchedule
{
    public partial class ScheduleTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            generateTable();
        }

        private void generateTable()
        {
            foreach (TableCell tc in ScheduleTable.Rows[0].Cells)
            {
                tc.Style.Value = "border-collapse: collapse; border-spacing: 0px; border: 0px";
                Table t = new Table();
                /*
                 * TableStyle ts = new TableStyle();
                
                ts.CellPadding = 0;
                ts.CellSpacing = 0;
                t.ApplyStyle(ts);
                */
                t.Style.Value = "border-collapse: collapse; border-spacing: 0px;border: 1px solid #000000;";
                t.Rows.Add(new TableRow());
                t.Rows[0].Cells.Add(new TableCell());
                t.Rows[0].Cells[0].Text = "aaaa";
                t.Rows[0].Cells[0].ColumnSpan = 4;
                //t.Rows[0].Cells[0].Style.Value = " border-style: solid; border-width: 1px 1px 1px 1px; border-color: #000000;";
                
                for (int i = 0; i < 4; i++)
                {
                    t.Rows.Add(new TableRow());
                    TableCell wtc = new TableCell();
                    wtc.Text = i.ToString();
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
            }
        }
    }
}