using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Core
{
    public class ManageClass:Database
    {
        public ManageClass()
        {

        }

        public bool DeleteWork(Guid workID)
        {
            return ExecuteTranSQL(@"DELETE FROM dbo.周节点 WHERE 月节点ID in (SELECT id FROM 月节点 WHERE 工作ID=@工作ID)
DELETE FROM dbo.月节点 WHERE 工作ID=@工作ID
DECLARE @no INT
SELECT @no=序号 FROM dbo.工作 WHERE Id=@工作ID
UPDATE dbo.工作 SET 序号=序号-1 WHERE 序号>@no AND 年份 IN (SELECT 年份 FROM dbo.工作 WHERE Id=@工作ID)
DELETE FROM dbo.工作 WHERE Id=@工作ID
", new SqlParameter[] { new SqlParameter("工作ID", workID) });
        }
    }
}
