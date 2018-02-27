using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using System.Data;
using System.Text.RegularExpressions;

namespace WorkSchedule.Test
{
    public partial class Test : System.Web.UI.Page
    {
        public Core.Test t;
        protected void Page_Load(object sender, EventArgs e)
        {
            t = new Core.Test();
            //t.OnlyTest();
            //DataTable dt = t.DealMonthSchedule();
            //foreach (DataRow dr in dt.Rows)
            //    Response.Write(dr[0] + "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + dr[1] + "<br>");

            //Response.Write(System.IO.File.Exists(Server.MapPath(@"\App_Data\目标节点.txt")));
            //string s = "xxxx";
            //Response.Write((s => int.Parse(s)));

            //string s = "拜泉路6月份完工通车。 清江支路清江之路丰华地块3月份签订收地协议， 4月份完成清江之路丰华地块收地.后续施工根据收地情况开展";
            //string st = Regex.Match(s, @"\d*月").Value;
            //Response.Write(st);

            //for (int i = 3; i <= 6; i++)
            //    Response.Write(i + "<br>");


        }

        private void TestRegex()
        {
            string s = "5-6月：底板施工完成；";
            string st;
            //Response.Write(DealString(s));
            if ((st = Regex.Match(s, @"\d*-\d月").Value) != "")
                Response.Write(st);
            else
                Response.Write("未匹配");
        }


        public string DealString(string s)
        {
            return Regex.Match(s, @"\d*-\d月").Value;
        }

        protected void Button1_Click1(object sender, EventArgs e)
        {
            Response.Write(t.TestReader(int.Parse(TextBox_SN.Text.ToString())));
        }
    }
}
