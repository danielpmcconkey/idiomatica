//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using Logic.Telemetry;
//using Microsoft.EntityFrameworkCore;
//using Model;
//using Model.DAL;
//using static System.Net.Mime.MediaTypeNames;

//namespace Logic.Services.API
//{
//    public static class LanguageCodeApi
//    {
//        public static LanguageCode? LanguageCodeReadByCode(IDbContextFactory<IdiomaticaContext> dbContextFactory, string code)
//        {
//            if (string.IsNullOrEmpty(code)) ErrorHandler.LogAndThrow();
//            return DataCache.LanguageCodeByCodeRead(code, dbContextFactory);
//        }
//        public static async Task<LanguageCode?> LanguageCodeReadByCodeAsync(
//            IDbContextFactory<IdiomaticaContext> dbContextFactory, string code)
//        {
//            if (string.IsNullOrEmpty(code)) ErrorHandler.LogAndThrow();
//            return await DataCache.LanguageCodeByCodeReadAsync(code, dbContextFactory);
//        }


//        public static Dictionary<string, LanguageCode> LanguageCodeOptionsRead(
//            IDbContextFactory<IdiomaticaContext> dbContextFactory, Expression<Func<LanguageCode, bool>> filter)
//        {
//            var options = context.LanguageCodes
//                .Where(filter).OrderBy(x => x.LanguageName).ToList();
//            var returnDict = new Dictionary<string, LanguageCode>();
//            if (options is not null)
//            {
//                foreach (var lc in options)
//                {
//                    if (lc is null || string.IsNullOrEmpty(lc.Code)) continue;
//                    returnDict.Add(lc.Code, lc);
//                }
//            }
//            return returnDict;
//        }
//        public static async Task<Dictionary<string, LanguageCode>> LanguageCodeOptionsReadAsync(
//            IDbContextFactory<IdiomaticaContext> dbContextFactory, Expression<Func<LanguageCode, bool>> filter)
//        {
//            return await Task<Dictionary<string, LanguageCode>>.Run(() =>
//            {
//                return LanguageCodeOptionsRead(dbContextFactory, filter);
//            });

//        }

//        public static LanguageCode? LanguageCodeUserInterfacePreferenceReadByUserId(
//            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
//        {




//            // replace with call to DataCache.UserSettingUiLanguageByUserIdRead




//            var languageCode = DataCache.LanguageCodeUserInterfacePreferenceByUserIdRead(userId, dbContextFactory);
//            if (languageCode is null)
//            {
//                return LanguageCodeReadByCode(dbContextFactory, "EN-US");
//            }
//            return languageCode;
//        }
//        public static async Task<LanguageCode?> LanguageCodeUserInterfacePreferenceReadByUserIdAsync(
//            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid userId)
//        {
//            return await Task<LanguageCode?>.Run(() =>
//            {
//                return LanguageCodeUserInterfacePreferenceReadByUserId(dbContextFactory, userId);
//            });
//        }
//    }
//}
