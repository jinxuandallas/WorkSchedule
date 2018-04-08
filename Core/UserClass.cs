using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Core
{
    public class UserClass:Tools
    {
        //public Tools t;
        public UserClass()
        {
            //t = new Tools();
        }

        /// <summary>
        /// 检查是否有录入权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool HasInput(int userId)
        {
            bool hasInput = false;
            using (SqlDataReader sdr = GetDataReader("select id from 工作人员 where 信息管理用户ID=@ID or 用户ID=@ID", new SqlParameter[] { new SqlParameter("@ID", userId) }))
                if (sdr.Read())
                    hasInput = true;
            return hasInput;
        }

        /// <summary>
        /// 获取用户名
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string GetUsername(int userID)
        {
            using (SqlDataReader sdr = GetDataReader("select 用户名 from 用户 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", userID) }))
            {
                sdr.Read();
                return sdr[0].ToString();
            }
        }

        /// <summary>
        /// 验证用户名密码
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public int ValidateUser(string username,string password)
        {
            using (SqlDataReader sdr = GetDataReader("select ID from 用户 where 用户名=@用户名 and 密码=@密码", new SqlParameter[] { new SqlParameter("@用户名",username),
                new SqlParameter("@密码",password)
            }))
                if (sdr.Read())
                    return int.Parse(sdr[0].ToString());
            return 0;
        }

        /// <summary>
        /// 获取某个用户应转向的页面地址
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public string RediRedirectUrl(int userID)
        {
            int userType = GetUserType(userID);
            if (userType == 3)
                return "~/Input/InputSchedule.aspx";
            else
                return "~/ShowSchedule/Schedule.aspx";
        }

        /// <summary>
        /// 获取用户类型
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public int GetUserType(int userID)
        {
            DataTable dt = GetDataSet("select 用户类型 from 用户 where ID=@ID", new SqlParameter[] { new SqlParameter("@ID", userID) }).Tables[0];
            return int.Parse(dt.Rows[0][0].ToString());
        }

        /// <summary>
        /// 通过姓名获取工作人员ID
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public int GetStaffIDByName(string name)
        {
            using (SqlDataReader sdr = GetDataReader("select ID from 工作人员 where 姓名=@Name", new SqlParameter[] { new SqlParameter("@Name", name) }))
            {
                if (sdr.Read())
                    return int.Parse(sdr[0].ToString());
            }

            return 0;
        }
    }
}
