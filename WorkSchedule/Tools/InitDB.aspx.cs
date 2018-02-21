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
        protected void Page_Load(object sender, EventArgs e)
        {
            string tableName = "重点工作2018";
            Core.Tools t = new Core.Tools();
            //t.AddStaff(tableName);
            //t.ImportWorks(tableName);
            //t.BuildWorkLeader(tableName);
            t.BuildTempMonthTable(tableName);
        }
    }
}