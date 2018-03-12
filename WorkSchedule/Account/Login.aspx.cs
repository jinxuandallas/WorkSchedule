using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;

namespace WorkSchedule.Account
{
    public partial class Login : System.Web.UI.Page
    {
        protected UserClass uc;
        protected void Page_Load(object sender, EventArgs e)
        {
            uc = new UserClass();

            /*
            if (Request.Cookies["ID"] != null)
            {
                Session["ID"] = Request.Cookies["ID"].Value;
                LoginUser.DestinationPageUrl = @"..\default.aspx";
                Response.Redirect(@"..\default.aspx");
            }

            */
            //Response.Write(Request.Cookies["ID"].Value + "<br/>");
            //foreach (string s in Request.Cookies.AllKeys)
            //    Response.Write(s + "<br/>");
        }

        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            int id = uc.ValidateUser(LoginUser.UserName, LoginUser.Password);
            if (id != 0)
            {
                e.Authenticated = true;
                Session["UserID"] = id;
                if (LoginUser.RememberMeSet)
                {
                    HttpCookie hc = new HttpCookie("ID", id.ToString());
                    hc.Expires = DateTime.MaxValue;
                    Response.Cookies.Add(hc);
                }
                LoginUser.DestinationPageUrl = @"..\default.aspx";
                //Response.Write(LoginUser.RememberMeSet);
            }
        }
    }
}