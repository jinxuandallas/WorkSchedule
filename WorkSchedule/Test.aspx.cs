using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using System.Data;

namespace WorkSchedule
{
    public partial class Test : System.Web.UI.Page
    {
        public Core.Test t;
        protected void Page_Load(object sender, EventArgs e)
        {
            t = new Core.Test();
            //t.OnlyTest();
            //DataTable dt = t.DealMonthSchedule();
            //foreach (DataRow dr in dt.Rows)
            //    Response.Write(dr[0] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + dr[1] + "<br>");

            Response.Write(System.IO.File.Exists(Server.MapPath(@"\App_Data\目标节点.txt")));

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Write(t.TestReader(int.Parse(TextBox_SN.Text.ToString())));
        }
    }
}