using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;

namespace WorkSchedule.Input
{
    public partial class InputSchedule : System.Web.UI.Page
    {
        public Core.Test t;
        public ShowScheduleClass ss;
        protected void Page_Load(object sender, EventArgs e)
        {
            t = new Core.Test();
            ss = new ShowScheduleClass();

            Session["UserID"] = 4;

            if (Session["UserID"] == null || string.IsNullOrWhiteSpace(Session["UserID"].ToString()))
                Response.Redirect("~/default.aspx");
        }
    }
}