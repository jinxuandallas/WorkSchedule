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
        protected Core.AddScheduleClass asc;

        protected void Page_Load(object sender, EventArgs e)
        {

            //此页面不成功
            return;

            Session["UserID"] = 12;

            tool = new Core.Tools();
            ss = new ShowScheduleClass();
            asc = new AddScheduleClass();

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
                Guid id = Guid.Parse(e.CommandArgument.ToString());
                bool result;
                Label l = e.Item.FindControl("LabelStatus") as Label;

                weekSchedule = (e.Item.FindControl("TextBoxWeekSchedule") as TextBox).Text;
                weekExecution = (e.Item.FindControl("TextBoxWeekExecution") as TextBox).Text;
                if (weekExecution.Trim() == "")
                    status = weekSchedule.Trim() == "" ? 0 : 1;
                else
                    status = (e.Item.FindControl("CheckBoxStatus") as CheckBox).Checked ? 3 : 2;

                //更新本周计划落实情况
                result=asc.InputWeekSchedule(id, thisWeek, weekSchedule, weekExecution, status);
                if (!result)
                {
                    l.Text = "更新不成功";
                    return;
                }


                //更新下周计划落实情况
                if ((e.Item.FindControl("PlaceHolderWeek53") as PlaceHolder).Visible == true)
                {
                    nextWeekSchedule = (e.Item.FindControl("TextBoxNextWeekSchedule") as TextBox).Text;
                    result=asc.InputWeekSchedule(id, thisWeek + 1, nextWeekSchedule,string.Empty, 1);
                    if (!result)
                    {
                        l.Text = "更新不成功";
                        return;
                    }
                }

                //Label l = e.Item.FindControl("LabelStatus") as Label;
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
                    result = asc.InputWeekSchedule(id, thisWeek + 1, weekSchedule, weekExecution, status);
                    if (!result)
                    {
                        l.Text = "更新不成功";
                        return;
                    }

                    l.Text += "<br/>" + e.CommandArgument.ToString();
                    l.Text += "<br/>" + weekSchedule;
                    l.Text += "<br/>" + weekExecution;
                    l.Text += "<br/>" + status + "<br/>";
                }

                l.Text = "更新成功";

            }
        }


        protected void CheckBoxModify_CheckedChanged(object sender, EventArgs e)
        {
            (((Control)sender).Parent.FindControl("PlaceHolderModify") as PlaceHolder).Visible = ((sender) as CheckBox).Checked;
            //(((Control)sender).Parent.FindControl("LabelStatus") as Label).Text = "xx"+(((Control)sender).Parent.FindControl("ButtonSubmit") as Button).CommandArgument;

        }
    }
}