using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;

namespace WorkSchedule.Manage
{
    public partial class DeleteWork : System.Web.UI.Page
    {
        protected ManageClass mc;
        protected UserClass uc;
        protected WorkClass wc;
        //protected Core.Tools t;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || string.IsNullOrWhiteSpace(Session["UserID"].ToString()))
                Response.Redirect("~/Account/Login.aspx");

            mc = new ManageClass();
            //t = new Core.Tools();
            uc = new UserClass();
            wc = new WorkClass();

            if (uc.GetUserType(int.Parse(Session["UserID"].ToString())) != 1)
                Response.Redirect("~/Account/Login.aspx");

            if (!IsPostBack)
            {
                SqlDataSource1.SelectParameters["年份"].DefaultValue = DateTime.Now.Year.ToString();
                DropDownListWork.DataBind();
            }

            //protected void Button1_Click(object sender, EventArgs e)
            //{
            //    //Response.Write("yy");
            //}
        }

        protected void DropDownListWork_SelectedIndexChanged(object sender, EventArgs e)
        {
            LabelWorkDetail.Text = wc.GetWorkDetail(Guid.Parse(DropDownListWork.SelectedValue));
        }

        protected void Delete_Click(object sender, EventArgs e)
        {
            bool result = mc.DeleteWork(Guid.Parse(DropDownListWork.SelectedValue));
            if (result)
                LabelResult.Text = "第" + (DropDownListWork.SelectedIndex + 1) + "项工作已删除";
            else
                LabelResult.Text = "删除失败";

            DropDownListWork.DataBind();
        }
    }
}