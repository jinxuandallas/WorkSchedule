using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Core
{
    public class ShowScheduleClass : Database
    {
        protected Tools t;
        public ShowScheduleClass()
        {
            t = new Tools();
        }

        

        /// <summary>
        /// 此处取出所有工作的存在月份以提高速度
        /// </summary>
        //public 

        public string GetWorkLeaders(Guid workID)
        {
            string workLeaders = string.Empty;
            using (SqlDataReader sdr = GetDataReader("select 责任领导 from 工作责任领导视图 where 工作ID=@工作ID",new SqlParameter[] { new SqlParameter("@工作ID",workID) }))
                while (sdr.Read())
                    workLeaders += " " + sdr[0].ToString();

            return workLeaders;
        }
        
        /// <summary>
        /// 为了生成的统一LinkButton样式
        /// </summary>
        /// <param name="workID"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public LinkButton GetLinkButton(Guid workID, int month)
        {
            LinkButton lb = new LinkButton();

            lb.Text = month + "月";
            lb.Font.Underline = false;
            lb.CommandName = "monthLinkButton";
            lb.CommandArgument = workID + "$" + month.ToString();
            lb.Width = Unit.Percentage(100);
            lb.Height = Unit.Percentage(100);
            lb.BackColor = System.Drawing.Color.Lavender;

            return lb;
        }
        public string GetMonthSchedule(Guid workID, int month)
        {
            string monthSchedule= month + "月目标节点："; 
            using (SqlDataReader sdr = GetDataReader("select 目标节点 from 月节点 where 工作ID=@工作ID and datepart(mm,日期)=@月份", new SqlParameter[] {
                new SqlParameter("@工作ID",workID),
                new SqlParameter("@月份",month)
            }))
                if (sdr.Read())
                    monthSchedule += "“" + sdr[0] + "”<br/>";
            return monthSchedule;
        }

        /// <summary>
        /// 获取月节点详细信息（包括周计划）
        /// </summary>
        /// <param name="workID"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public string GetMonthScheduleDetail(Guid workID,int month)
        {
            string monthScheduleDeatil= "<br/>" + month+"月目标节点：";
            using (SqlDataReader sdr = GetDataReader("select 目标节点 from 月节点 where 工作ID=@工作ID and datepart(mm,日期)=@月份", new SqlParameter[] {
                new SqlParameter("@工作ID",workID),
                new SqlParameter("@月份",month)
            }))
                if (sdr.Read())
                    monthScheduleDeatil += "“"+sdr[0]+ "”<br/><br/>";
            DataSet weekDetail = GetDataSet("select 月节点日期,周数,周计划,周完成,状态,开始日期,结束日期 from 周节点视图 where 周状态!=0 and 工作ID=@工作ID and datepart(mm,月节点日期)=@月份",new SqlParameter[] {
                new SqlParameter("@工作ID",workID),
                new SqlParameter("@月份",month)
            });

            //此处也许需要判断weekDetail中是否有数据
            if (weekDetail.Tables[0].Rows.Count > 0)
            {
                //monthScheduleDeatil += "<br/>";
                foreach (DataRow dr in weekDetail.Tables[0].Rows)
                    //(dr["状态"].ToString()=="2"|| dr["状态"].ToString() == "4" ? @"<p style=""color:red""> "+ dr["状态"] + "</p>":dr["状态"])
                    monthScheduleDeatil += "第" + dr["周数"] + "周（" + DateTime.Parse(dr["开始日期"].ToString()).ToString("M月d日") + "—" + DateTime.Parse(dr["结束日期"].ToString()).ToString("M月d日") + "）：" + (dr["状态"].ToString() == "计划未完成" ? @"<span style=""color:red""> " + dr["状态"] + "</span>" : dr["状态"]) + "<br/>" + "周工作计划：" + dr["周计划"] + "<br/>周落实情况：" + dr["周完成"] + "<br/><br/>";
            }
            //monthScheduleDeatil += "<br/>";
            return monthScheduleDeatil;
        }


    }

    
}
