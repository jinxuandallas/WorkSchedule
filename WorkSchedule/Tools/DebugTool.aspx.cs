using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;

namespace WorkSchedule.Tools
{
    public partial class DebugTool : System.Web.UI.Page
    {
        protected Debug debug;
        protected void Page_Load(object sender, EventArgs e)
        {
            debug = new Debug();
            //Response.Write(debug.RemoveDuplicateString("2月：土建精装基本完成90%； 2月：土建精装基本完成90%；"));
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string t = TextBox1.Text;
            if (t.Trim() == "")
                t = "2月：土建精装基本完成90%； 2月：土建精装基本完成90%；";
            Response.Write(debug.RemoveDuplicateString(t));
        }

        protected void ButtonRemove_Click(object sender, EventArgs e)
        {
            debug.RemoveDuplicateMonthTask();
        }
    }
}