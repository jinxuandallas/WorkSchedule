using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Core
{
    public class WorkClass:Database
    {
        public int year;
        //protected Tools t;
        public WorkClass()
        {
            year = DateTime.Now.Year;
            //t = new Tools();
            //mc = new ManageClass();
        }

        public WorkClass(int pyear)
        {
            year = pyear;
            //t = new Tools(pyear);
            //mc = new ManageClass();
        }

        /// <summary>
        /// 获取目标名称
        /// </summary>
        /// <param name="workID"></param>
        /// <returns></returns>
        public string GetTaskNameByID(Guid workID)
        {
            using (SqlDataReader sdr = GetDataReader("select 目标名称 from 工作 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", workID) }))
            {
                if (sdr.Read())
                    return sdr[0].ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取某年的所有工作的工作ID
        /// </summary>
        /// <returns></returns>
        public Guid[] GetAllWorkID()
        {
            return GetAllWorkID(year);
        }

        /// <summary>
        /// 获取某年的所有工作的工作ID
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public Guid[] GetAllWorkID(int year)
        {
            Guid[] allWorkID;
            DataTable dt = GetDataSet("select ID from 工作 where 年份=@年份", new SqlParameter[] { new SqlParameter("@年份", year) }).Tables[0];

            allWorkID = new Guid[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
                allWorkID[i] = Guid.Parse(dt.Rows[i][0].ToString());

            return allWorkID;
        }

        public Guid[] GetAllWorkID(string tableName)
        {
            Guid[] allWorkID;
            DataTable dt = GetDataSet("select ID from " + tableName + " where 年份=@年份", new SqlParameter[] { new SqlParameter("@年份", year) }).Tables[0];

            allWorkID = new Guid[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
                allWorkID[i] = Guid.Parse(dt.Rows[i][0].ToString());

            return allWorkID;
        }
        /// <summary>
        /// 获取某年某用户所有管理的工作ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Guid[] GetUserWorkID(int id)
        {
            return GetUserWorkID(id, year);
        }
        

        /// <summary>
        /// 获取某年某用户所有管理的工作ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public Guid[] GetUserWorkID(int userId, int year)
        {
            Guid[] userWorkID;
            DataTable dt = GetDataSet("select 工作ID from 工作责任领导视图 where 年份=@年份 and (信息管理用户ID=@用户ID or 用户ID=@用户ID)", new SqlParameter[] { new SqlParameter("@年份", year),
                new SqlParameter("@用户ID",userId)
            }).Tables[0];

            userWorkID = new Guid[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
                userWorkID[i] = Guid.Parse(dt.Rows[i][0].ToString());

            return userWorkID;
        }

        public Guid[] GetProjectCategoryLocationID()
        {
            return GetProjectCategoryLocationID(year);
        }

        /// <summary>
        /// 获取工作类别分类所在位置的工作ID
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public Guid[] GetProjectCategoryLocationID(int year)
        {
            Guid[] projectCategoryLocationID;
            DataTable category = GetDataSet("select ID from [工作类别]").Tables[0];
            projectCategoryLocationID = new Guid[category.Rows.Count];

            int i = 0;
            foreach (DataRow dr in category.Rows)
                using (SqlDataReader sdr = GetDataReader("select top 1 ID from 工作 where 工作类别=@工作类别 order by 序号", new SqlParameter[] { new SqlParameter("@工作类别", dr[0]) }))
                {
                    sdr.Read();
                    projectCategoryLocationID[i] = Guid.Parse(sdr[0].ToString());
                    i++;
                }

            return projectCategoryLocationID;
        }

        /// <summary>
        /// 获取所有类别名称
        /// </summary>
        /// <returns></returns>
        public string[] GetCategoryName()
        {
            return GetStringArrFromDataSet(GetDataSet("select 类别名称 from 工作类别"));
        }

        /// <summary>
        /// 将DataSet转换为string数组
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public string[] GetStringArrFromDataSet(DataSet ds)
        {
            int l = ds.Tables[0].Rows.Count;
            if (l == 0)
                return null;
            string[] m = new string[l];
            for (int i = 0; i < l; i++)
                m[i] = ds.Tables[0].Rows[i][0].ToString();
            return m;
        }

        /// <summary>
        /// 获取工作详细信息
        /// </summary>
        /// <param name="workID"></param>
        /// <returns></returns>
        public string GetWorkDetail(Guid workID)
        {
            string workDetail;
            using (SqlDataReader sdr = GetDataReader("select 序号,目标名称,目标内容,备注,类别名称 from 工作视图 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", workID) }))
                if (sdr.Read())
                {
                    workDetail = "第" + sdr[0] + "项：" + sdr[1] + "<br/>";
                    workDetail += "目标名称：" + sdr[2] + "<br/>";
                    workDetail += sdr[3].ToString().Trim() == "" ? "" : "备注：" + sdr[3] + "<br/>";
                    workDetail += "类别名称：" + sdr[4] + "<br/>";
                    return workDetail;
                }
                else
                    return string.Empty;
        }

        /// <summary>
        /// 获取工作目标内容
        /// </summary>
        /// <param name="workID"></param>
        /// <returns></returns>
        public string GetWorkContent(Guid workID)
        {
            using (SqlDataReader sdr = GetDataReader("select 目标内容 from 工作 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", workID) }))
                if (sdr.Read())
                    return sdr[0].ToString();
                else
                    return string.Empty;
        }

        public string GetWorkContent(Guid workID, string tableName)
        {
            using (SqlDataReader sdr = GetDataReader("select 目标内容 from " + tableName + " where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", workID) }))
                if (sdr.Read())
                    return sdr[0].ToString();
                else
                    return string.Empty;
        }

        /// <summary>
        /// 通过序号获取工作ID
        /// </summary>
        /// <param name="sn"></param>
        /// <returns></returns>
        public Guid GetTaskIDBySN(int sn)
        {
            using (SqlDataReader sdr = GetDataReader("select ID from 工作 where 序号=@SN", new SqlParameter[] { new SqlParameter("@SN", sn) }))
            {
                if (sdr.Read())
                    return Guid.Parse(sdr[0].ToString());
            }

            return Guid.Empty;
        }





    }
}
