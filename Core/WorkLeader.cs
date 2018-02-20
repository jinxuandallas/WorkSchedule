using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    class WorkLeader
    {
        public int SN { get; set; }
        public string LeaderName { get; set; }

        public WorkLeader(int sn,string leaderName)
        {
            SN = sn;
            LeaderName = leaderName;
        }

    }
}
