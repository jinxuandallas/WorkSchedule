using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using System.Globalization;

namespace WorkSchedule.Input
{
    public partial class InputThisWeekSchedule : System.Web.UI.Page
    {
        protected ShowScheduleClass ss;
        protected Core.Tools tool;
        protected int thisWeek;

        protected void Page_Load(object sender, EventArgs e)
        {
            Session["UserID"] = 12;

            tool = new Core.Tools();
            ss = new ShowScheduleClass();
            thisWeek = (new GregorianCalendar()).GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstDay, DayOfWeek.Monday);

            //thisWeek = 1;

            if (!IsPostBack)
            {
                SqlDataSource1.SelectParameters["year"].DefaultValue = tool.year.ToString();
                RepeaterThisWeek.DataBind();
            }
        }

        protected void RepeaterThisWeek_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "SubmitSchedule")
            {
                (e.Item.FindControl("LabelStatus") as Label).Text = e.CommandArgument.ToString();
            }
        }

        protected void CheckBoxModify_CheckedChanged(object sender, EventArgs e)
        {
            (((Control)sender).Parent.FindControl("PlaceHolderModify") as PlaceHolder).Visible = ((sender) as CheckBox).Checked;
            //(((Control)sender).Parent.FindControl("LabelStatus") as Label).Text = "xx"+(((Control)sender).Parent.FindControl("ButtonSubmit") as Button).CommandArgument;

        }
    }
}