﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
namespace WorkSchedule.AddWorkSchedule
{
    public partial class AddScheduleTool : System.Web.UI.Page
    {
        //protected Core.Tools t;
        protected ScheduleClass sc;
        protected ManageClass mc;
        protected WorkClass wc;

        //protected Core.AddScheduleClass asc;
        protected void Page_Load(object sender, EventArgs e)
        {
            //t = new Core.Tools();
            mc = new ManageClass();
            //ss = new ShowSchedule();
            //asc = new AddScheduleClass();
            sc = new ScheduleClass();
            wc = new WorkClass();

            if (!IsPostBack)
            {
                //Repeater1.Visible = false;
                DataBind();
            }

        }

        protected void DropDownListWork_SelectedIndexChanged(object sender, EventArgs e)
        {
            LabelWork.Text = wc.GetTaskNameByID(Guid.Parse(DropDownListWork.SelectedValue));
            DropDownListMonth.DataBind();
        }

        protected void DropDownListMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            LabelMonthTask.Text = sc.GetMonthTask(Guid.Parse(DropDownListWork.SelectedValue), int.Parse(DropDownListMonth.SelectedValue));
            Repeater1.Visible = true;
            Repeater1.DataBind();
        }

        //protected void Repeater1_ItemCommand(object source, RepeaterCommandEventArgs e)
        //{
        //    if (e.CommandName == "Submit")
        //    {
        //        Response.Write(((TextBox)e.Item.FindControl("TextBoxWeekSchedule")).Text+"<br>");
        //        Response.Write(((TextBox)e.Item.FindControl("TextBoxWeekExecution")).Text + "<br>");
        //        Response.Write(((CheckBox)e.Item.FindControl("CheckBoxState")).Text + "<br>");
        //        Repeater1.Visible = false;
        //    }
        //}

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            string weekSchedule, weekExecution;
            int state, weekOfYear;
            Guid monnthTaskID = sc.GetMonthID(Guid.Parse(DropDownListWork.SelectedValue), int.Parse(DropDownListMonth.SelectedValue));
            bool succeed = true ;
            foreach (RepeaterItem ri in Repeater1.Items)
            {
                weekOfYear = int.Parse(((Label)ri.FindControl("lbWeek")).Text);
                weekSchedule = ((TextBox)ri.FindControl("TextBoxWeekSchedule")).Text;
                weekExecution = ((TextBox)ri.FindControl("TextBoxWeekExecution")).Text;
                if (weekExecution.Trim() == "")
                    state = weekSchedule.Trim() == "" ? 0 : 1;
                else
                    state = ((CheckBox)ri.FindControl("CheckBoxState")).Checked ? 3 : 2;

                succeed = !mc.InputWeekSchedule(monnthTaskID, weekOfYear, weekSchedule, weekExecution, state);
            }

            string result = succeed ? "成功" : "不成功";
            Response.Write("周计划添加" + result + "<br/><br/>");
            Repeater1.Visible = false;
        }
    }
}