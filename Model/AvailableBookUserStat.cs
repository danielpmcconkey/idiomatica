using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum AvailableBookUserStat
    {
        DISTINCTIGNOREDCOUNT = 1,
        DISTINCTKNOWNPERCENT = 2,
        DISTINCTLEARNEDCOUNT = 3,
        DISTINCTLEARNING3COUNT = 4,
        DISTINCTLEARNING4COUNT = 5,
        DISTINCTNEW1COUNT = 6,
        DISTINCTNEW2COUNT = 7,
        DISTINCTUNKNOWNCOUNT = 8,
        DISTINCTWELLKNOWNCOUNT = 9,
        DISTINCTWORDCOUNT = 10,
        TOTALIGNOREDCOUNT = 11,
        TOTALKNOWNPERCENT = 12,
        TOTALLEARNEDCOUNT = 13,
        TOTALLEARNING3COUNT = 14,
        TOTALLEARNING4COUNT = 15,
        TOTALNEW1COUNT = 16,
        TOTALNEW2COUNT = 17,
        TOTALUNKNOWNCOUNT = 18,
        TOTALWELLKNOWNCOUNT = 19,
        TOTALWORDCOUNT = 20, // delete this one
        ISCOMPLETE = 21,
        LASTPAGEREAD = 22,
        PROGRESS = 23,
        PROGRESSPERCENT = 24,
    }
}
