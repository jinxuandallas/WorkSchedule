using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;

namespace WorkSchedule.MasterPage
{
    public partial class MainSite : System.Web.UI.MasterPage
    {
        protected Core.UserClass muc;
        protected int userID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || string.IsNullOrWhiteSpace(Session["UserID"].ToString()))
                Response.Redirect("~/Account/Login.aspx");

            Core.Tools mt = new Core.Tools();
            muc = new UserClass();
            userID = int.Parse(Session["UserID"].ToString());
            
            lbUsername.Text = muc.GetUsername(userID);

            if (!mt.HasInput(userID)|| muc.GetUserType(userID) == 3)
                lbFunc.Visible = false;
            else
            {
                string pageName = System.IO.Path.GetFileName(Request.Path.ToLower());

                if (pageName == "inputpreviousschedule.aspx" && muc.GetUserType(userID) == 2)
                {
                    lbFunc.Text = "查看";
                    MenuManage.Visible = true;
                }
                if (pageName == "/schedule.aspx")
                {
                    lbFunc.Text = "管理";
                    MenuManage.Visible = false;
                }
            }
        }

        protected void lbStaus_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Cookies["UserID"].Expires = DateTime.Now.AddDays(-1);
            Response.Redirect("~/Account/Login.aspx");
        }

        protected void lbFunc_Click(object sender, EventArgs e)
        {
            if (lbFunc.Text == "管理")
                Response.Redirect("~/Input/InputPreviousSchedule.aspx");

            if (lbFunc.Text == "查看")
                Response.Redirect("~/ShowSchedule/Schedule.aspx");
        }
    }
}