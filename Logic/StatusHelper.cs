using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public static class StatusHelper
    {
        public static List<Status> GetStatuses(IdiomaticaContext context, Func<Status, bool> filter)
        {
            return context.Statuses
                .Where(filter)
                .ToList();
        }
    }
}
