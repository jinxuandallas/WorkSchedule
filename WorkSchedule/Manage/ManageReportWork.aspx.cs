using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;

namespace WorkSchedule.Manage
{
    public partial class ManageReportWork : System.Web.UI.Page
    {
        protected ManageClass mc;
        //protected Core.Tools t;
        protected WorkClass wc;
        protected UserClass uc;
        protected ScheduleClass sc;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UserID"] == null || string.IsNullOrWhiteSpace(Session["UserID"].ToString()))
                Response.Redirect("~/Account/Login.aspx");

            mc = new ManageClass();
            //t = new Core.Tools();
            wc = new WorkClass();
            uc = new UserClass();
            sc = new ScheduleClass();

            if (uc.GetUserType(int.Parse(Session["UserID"].ToString())) != 1)
                Response.Redirect("~/Account/Login.aspx");

            if (!IsPostBack)
            {
                SqlDataSource1.SelectParameters["年份"].DefaultValue = DateTime.Now.Year.ToString();
                DropDownListWork.DataBind();
                //TextBoxSchedule.Text = @"3月：沟通台商和青房意见，加快启动筹备； 
                //                4月 - 5月：台方进入青岛,项目开办公司成立。
                //6月：确定业态、主题定位、设计方案，招商签约发包。
                //7 - 8月：青房完成综合验收，具备交付条件。双方就租赁面积等合作细节、装修等达成一致。
                //9 - 10月：签订合同,启动装修，研究税收扶持政策。
                //11月：落实燃气管道、扶梯、货梯等物业条件。
                //12月：围绕文创、休闲、旅游、美食、购物等招商。
                //";

            }
        }

        protected void DropDownListWork_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextBoxContent.Text = wc.GetWorkContent(Guid.Parse(DropDownListWork.SelectedValue), "绿色建设科技城");
        }

        protected void ButtonRecognize_Click(object sender, EventArgs e)
        {
            Panel1.Visible = true;
            LabelResult.Text = string.Empty;
            foreach (string s in sc.GetMonthScheduleFormTxt(TextBoxSchedule.Text))
                LabelResult.Text += s + "<br/>";
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            bool result;
            Panel1.Visible = false;
            if (TextBoxContent.Text.Trim() != "")
                mc.UpdateWorkContent(Guid.Parse(DropDownListWork.SelectedValue), TextBoxContent.Text, "绿色建设科技城");
            result = mc.BatchUpdateMonthScheduleFormTxt(TextBoxSchedule.Text, Guid.Parse(DropDownListWork.SelectedValue));

            if (result)
                Response.Write("更新成功<br/>");
        }
    }

}