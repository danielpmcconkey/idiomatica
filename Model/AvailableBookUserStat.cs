using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
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

/*
 * 
with params as (
	select 
	  AvailableWordUserStatusUNKNOWN = 8
, AvailableWordUserStatusLEARNED = 5
, AvailableWordUserStatusIGNORED = 6
, AvailableWordUserStatusWELLKNOWN = 7
, AvailableBookUserStatISCOMPLETE = 1
, AvailableBookUserStatLASTPAGEREAD = 2
, AvailableBookUserStatPROGRESS = 3
, AvailableBookUserStatPROGRESSPERCENT = 4
, AvailableBookUserStatDISTINCTKNOWNPERCENT = 5
, AvailableBookUserStatTOTALKNOWNPERCENT = 6
, AvailableBookUserStatDISTINCTWORDCOUNT = 7
, AvailableBookUserStatTOTALWORDCOUNT = 8

 * */
