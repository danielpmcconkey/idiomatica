using Microsoft.IdentityModel.Protocols.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public enum AvailableFlashCardAttemptStatus
    {
        WRONG = 1,
        HARD = 2,
        GOOD = 3,
        EASY = 4,
        STOP = 5
    }
}
