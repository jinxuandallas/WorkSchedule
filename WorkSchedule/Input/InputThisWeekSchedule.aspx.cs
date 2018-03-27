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
                string weekSchedule, weekExecution, nextWeekSchedule;
                int status;

                weekSchedule = (e.Item.FindControl("TextBoxWeekSchedule") as TextBox).Text;
                weekExecution = (e.Item.FindControl("TextBoxWeekExecution") as TextBox).Text;
                if (weekExecution.Trim() == "")
                    status = weekSchedule.Trim() == "" ? 0 : 1;
                else
                    status = (e.Item.FindControl("CheckBoxStatus") as CheckBox).Checked ? 3 : 2;

                //更新本周计划落实情况

                //更新下周计划落实情况
                if ((e.Item.FindControl("PlaceHolderWeek53") as PlaceHolder).Visible == true)
                {
                    nextWeekSchedule = (e.Item.FindControl("TextBoxNextWeekSchedule") as TextBox).Text;
                }

                Label l = e.Item.FindControl("LabelStatus") as Label;
                l.Text = "<br/>" + e.CommandArgument.ToString();
                l.Text += "<br/>" + weekSchedule;
                l.Text += "<br/>" + weekExecution;
                l.Text += "<br/>" + status;
                //l.Text += "<br/>" + nextWeekSchedule;


                //修改上周计划
                if ((e.Item.FindControl("CheckBoxModify") as CheckBox).Checked)
                {
                    weekSchedule = (e.Item.FindControl("TextBoxLastWeekSchedule") as TextBox).Text;
                    weekExecution = (e.Item.FindControl("TextBoxLastWeekExecution") as TextBox).Text;
                    status = (e.Item.FindControl("CheckBoxLastStatus") as CheckBox).Checked ? 3 : 2;
                    l.Text += "<br/>" + e.CommandArgument.ToString();
                    l.Text += "<br/>" + weekSchedule;
                    l.Text += "<br/>" + weekExecution;
                    l.Text += "<br/>" + status + "<br/>";
                }

                
                
            }
        }

        protected void CheckBoxModify_CheckedChanged(object sender, EventArgs e)
        {
            (((Control)sender).Parent.FindControl("PlaceHolderModify") as PlaceHolder).Visible = ((sender) as CheckBox).Checked;
            //(((Control)sender).Parent.FindControl("LabelStatus") as Label).Text = "xx"+(((Control)sender).Parent.FindControl("ButtonSubmit") as Button).CommandArgument;

        }
    }
}