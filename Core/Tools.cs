using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Core
{
    public class Tools : Database
    {
        public Tools()
        {

        }

        public void AddStaff()
        {
            List<string> staff = new List<string>();

            using (SqlDataReader getStaff = GetDataReader("select distinct(责任领导) from [重点工作2018]"))
            {
                string s;
                while (getStaff.Read())
                {
                    s = getStaff[0].ToString();
                    if (s.Contains("\n"))
                    {
                        string[] duoren = s.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string ds in duoren)
                            if (!staff.Contains(ds))
                                staff.Add(ds);
                    }
                    else
                        staff.Add(s);
                }
            }

            foreach (string ins in staff)
                ExecuteSql(@"insert 工作人员(姓名,所属部门,人员类型) values (@姓名,1,1)",new SqlParameter[] { new SqlParameter("@姓名",ins)});
            //List<string> distinctStaff = staff.Distinct<string>().ToList<string>();
            //int i = 3;
        }
        /*
        /// <summary>
        /// 将注册资本从文本格式传唤成长整型格式
        /// </summary>
        /// <param name="num">要转换的数字</param>
        /// <param name="unit">单位（亿、万、千）</param>
        /// <returns>转换后的长整型注册资本数</returns>
        public long CapitalStr2Long (string num,string unit)
        {
            int capitalNum;
            long capital;
            if (num.Length > 7 || !int.TryParse(num, out capitalNum)) return 0;
            switch(unit)
            {
                case "千":
                    capital = capitalNum * 1000;
                    break;
                case "万":
                    capital = capitalNum * 10000;
                    break;
                case "亿":
                    capital = capitalNum * 100000000;
                    break;
                default:
                    capital = 0;
                    break;
            }
            return capital;
        }

        /// <summary>
        /// 转换图片地址，如果地址为空则显示默认无图片的地址
        /// </summary>
        /// <param name="sourceAddress">源地址</param>
        /// <returns>返回处理过的图片地址</returns>
        public string TransformPicAddress(string sourceAddress)
        {
            if (string.IsNullOrWhiteSpace(sourceAddress))
                return "~/Images/noImg.jpg";
            return sourceAddress;
        }

        /// <summary>
        /// 将过长字符串截断并在其后添加“...”
        /// </summary>
        /// <param name="str">要截断的字符串</param>
        /// <param name="len">截断的长度</param>
        /// <returns>返回处理好的字符串</returns>
        public string cutStr(string str,int len)
        {
            if (str.Length > len)
                return str.Substring(0, len) + "...";
            return str;
        }

        /// <summary>
        /// 将长整型转换成带单位的注册资本数
        /// </summary>
        /// <param name="longStr">长整型的字符串形式</param>
        /// <returns>返回处理好的注册资本字符串</returns>
        public string LongStr2CapitalStr(string longStr)
        {
            if (string.IsNullOrEmpty(longStr))
                return "";
            long capital = Convert.ToInt64(longStr);
            string capitalStr = capital.ToString();
            if (capitalStr.Length > 8)
                return (capital / 100000000).ToString() + "亿";
            if (capitalStr.Length > 4)
                return (capital / 10000).ToString() + "万";
            if (capitalStr.Length > 3)
                return (capital / 1000).ToString() + "千";
            return capitalStr;
        }

    */
    }
}
