using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Enums
{
    public enum AvailableBookUserStat
    {
        ISCOMPLETE = 1,
        LASTPAGEREAD = 2,
        PROGRESS = 3,
        PROGRESSPERCENT = 4,
        DISTINCTKNOWNPERCENT = 5,
        TOTALKNOWNPERCENT = 6,
        DISTINCTWORDCOUNT = 7,
        TOTALWORDCOUNT = 8,
    }
}