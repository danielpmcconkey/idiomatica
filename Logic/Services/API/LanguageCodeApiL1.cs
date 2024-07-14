using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DAL;

namespace Logic.Services.Level1
{
    public static class LanguageCodeApiL1
    {
        public static async Task<Dictionary<string, LanguageCode>> LanguageCodeOptionsReadAsync(
            IdiomaticaContext context, Expression<Func<LanguageCode, bool>> filter)
        {
            var options = await context.LanguageCodes
                .Where(filter).OrderBy(x => x.LanguageName).ToListAsync();
            var returnDict = new Dictionary<string, LanguageCode>();
            if (options is not null)
            {
                foreach (var lc in options)
                {
                    if (lc is null || string.IsNullOrEmpty(lc.Code)) continue;
                    returnDict.Add(lc.Code, lc);
                }
            }
            return returnDict;
        }
    }
}
