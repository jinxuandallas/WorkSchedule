using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Core;
using System.Data;
using System.Text.RegularExpressions;
//using ICSharpCode.SharpZipLib.Zip.Compression;
using System.IO;

namespace WorkSchedule.Test
{
    public partial class Test : System.Web.UI.Page
    {
        public Core.Test t;
        public ShowScheduleClass ss;
        public Core.Tools tool;
        int[] weeksOfMonth;
        Guid[] allWorkID;
        Dictionary<Guid, int[]> existMonths;
        Dictionary<Guid, Dictionary<int, int>> existWeeks;
        protected Dictionary<Guid, List<Dictionary<int, int>>> allMonthWeekInfo;
        protected void Page_Load(object sender, EventArgs e)
        {
            t = new Core.Test();
            ss = new ShowScheduleClass();
            tool = new Core.Tools();
            //if (!IsPostBack)
            //    PreLoadData();
            //Response.Write(existMonths.Count);
            //Response.Write("<br/>" +existMonths[Guid.Parse("e121878e-1615-e811-82f1-b083fe979874")]);
            //Response.Write(ss.GetMonthScheduleDetail(Guid.Parse("c521878e-1615-e811-82f1-b083fe979874"), 9));
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
        protected void PreLoadData()
        {
            //获取一年中所有月份的每个月包含的周数量
            weeksOfMonth = tool.GetWeeksOfAllMonth();

            allWorkID = tool.GetAllWorkID();

            existMonths = new Dictionary<Guid, int[]>();
            existWeeks = new Dictionary<Guid, Dictionary<int, int>>();
            foreach (Guid wid in allWorkID)
            {
                existMonths.Add(wid, tool.GetExistTaskMonths(wid));
                existWeeks.Add(wid, tool.GetExistTaskWeeksAndState(wid,true));
            }

            ViewState["weeksOfMonth"] = weeksOfMonth;
            ViewState["allWorkID"] = allWorkID;
            ViewState["existMonths"] = existMonths;
            ViewState["existWeeks"] = existWeeks;
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

        protected override void SavePageStateToPersistenceMedium(Object pViewState)
        {
            LosFormatter mFormat = new LosFormatter();
            StringWriter mWriter = new StringWriter();
            mFormat.Serialize(mWriter, pViewState);
            String mViewStateStr = mWriter.ToString();
            byte[] pBytes = System.Convert.FromBase64String(mViewStateStr);
            pBytes = tool.Compress(pBytes);
            String vStateStr = System.Convert.ToBase64String(pBytes);
            ClientScript.RegisterHiddenField("__MSPVSTATE", vStateStr);
        }
        protected override Object LoadPageStateFromPersistenceMedium()
        {
            String vState = this.Request.Form.Get("__MSPVSTATE");
            byte[] pBytes = System.Convert.FromBase64String(vState);
            pBytes = tool.DeCompress(pBytes);
            LosFormatter mFormat = new LosFormatter();
            return mFormat.Deserialize(System.Convert.ToBase64String(pBytes));
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
        //public static byte[] Compress(byte[] pBytes)
        //{
        //    MemoryStream mMemory = new MemoryStream();
        //    Deflater mDeflater = new Deflater(ICSharpCode.SharpZipLib.Zip.Compression.Deflater.BEST_COMPRESSION);
        //    ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream mStream = new ICSharpCode.SharpZipLib.Zip.Compression.Streams.DeflaterOutputStream(mMemory, mDeflater, 131072);
        //    mStream.Write(pBytes, 0, pBytes.Length);
        //    mStream.Close();
        //    return mMemory.ToArray();
        //}
        //public static byte[] DeCompress(byte[] pBytes)
        //{
        //    ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream mStream = new ICSharpCode.SharpZipLib.Zip.Compression.Streams.InflaterInputStream(new MemoryStream(pBytes));
        //    MemoryStream mMemory = new MemoryStream();
        //    Int32 mSize;
        //    byte[] mWriteData = new byte[4096];
        //    while (true)
        //    {
        //        mSize = mStream.Read(mWriteData, 0, mWriteData.Length);
        //        if (mSize > 0)
        //        {
        //            mMemory.Write(mWriteData, 0, mSize);
        //        }
        //        else
        //        {
        //            break;
        //        }
        //    }
        //    mStream.Close();
        //    return mMemory.ToArray();
        //}
    }
}
