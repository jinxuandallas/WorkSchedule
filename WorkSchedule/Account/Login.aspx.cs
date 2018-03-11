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
        }

        protected void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
        {
            int id = uc.ValidateUser(LoginUser.UserName, LoginUser.Password);
            if (id!=0)
            {
                e.Authenticated = true;
                Session["UserID"] = id;
                LoginUser.DestinationPageUrl = @"..\default.aspx";
            }
        }
    }
}