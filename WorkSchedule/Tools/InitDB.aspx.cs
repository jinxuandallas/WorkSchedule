using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;

namespace WorkSchedule.Tools
{
    public partial class InitDB : System.Web.UI.Page
    {
        public string tableName;
        public Core.Tools t;
        protected void Page_Load(object sender, EventArgs e)
        {
            tableName = "重点工作2018";
            t = new Core.Tools();
            
            
            //t.BuildTempMonthTable();

            //t.BuildMonthSchedule();
        }

        protected void AddStaf_Click(object sender, EventArgs e)
        {
            t.AddStaff(tableName);
            Response.Write("执行成功");
        }

        protected void ImportWorks_Click(object sender, EventArgs e)
        {
            t.ImportTasks(tableName);
            Response.Write("执行成功");
        }

        protected void BuildWorkLeader_Click(object sender, EventArgs e)
        {
            t.BuildWorkLeader(tableName);
            Response.Write("执行成功");
        }

        protected void BuildTempMonthTable_Click(object sender, EventArgs e)
        {
            t.BuildTempMonthTable(tableName);
            Response.Write("执行成功");
        }

        protected void BuildMonthSchedule_Click(object sender, EventArgs e)
        {
            t.BuildMonthSchedule();
            Response.Write("执行成功");
        }
    }
}