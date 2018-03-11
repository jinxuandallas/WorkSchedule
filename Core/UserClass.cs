using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;


namespace Core
{
    public class UserClass:Database
    {
        public UserClass()
        {

        }

        public int ValidateUser(string username,string password)
        {
            using (SqlDataReader sdr = GetDataReader("select ID from 用户 where 用户名=@用户名 and 密码=@密码", new SqlParameter[] { new SqlParameter("@用户名",username),
                new SqlParameter("@密码",password)
            }))
                if (sdr.Read())
                    return int.Parse(sdr[0].ToString());
            return 0;
        }
    }
}
