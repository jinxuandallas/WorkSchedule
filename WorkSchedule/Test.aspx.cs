using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;

namespace WorkSchedule
{
    public partial class Test : System.Web.UI.Page
    {
        public Core.Test t;
        protected void Page_Load(object sender, EventArgs e)
        {
            t = new Core.Test();
            t.OnlyTest();

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            Response.Write(t.TestReader(int.Parse(TextBox_SN.Text.ToString())));
        }
    }
}