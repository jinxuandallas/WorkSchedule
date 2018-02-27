using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Core;

namespace WorkSchedule.Tools
{
    public partial class AddOtherWorkMonthSchedule : System.Web.UI.Page
    {
        Core.Tools t;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                GridView1.DataBind();

            t = new Core.Tools();
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //Response.Write(((TextBox)GridView1.Rows[1].Cells[5].Controls[0]).Text);
            //Response.Write((((TextBox)GridView1.Rows[1].Cells[5].Controls[1]).Text));
            foreach (GridViewRow gvr in GridView1.Rows)
                //    Response.Write(gvr.Cells[1].Text+ "&nbsp;&nbsp;"+ gvr.Cells[2].Text + "&nbsp;&nbsp;" + ((TextBox)gvr.Cells[3].Controls[1]).Text+ "-"+((TextBox)gvr.Cells[4].Controls[1]).Text+"<br>");

                //t.AddMonthSchedule(int.Parse(gvr.Cells[0].Text), Guid.Empty, null, 0, 0);
                t.AddMonthSchedule(Guid.Parse(gvr.Cells[1].Text), gvr.Cells[2].Text, int.Parse(((TextBox)gvr.Cells[3].Controls[1]).Text), int.Parse(((TextBox)gvr.Cells[4].Controls[1]).Text));
            Response.Write("添加成功");
        }
    }
}