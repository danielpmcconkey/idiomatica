using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        #region create
        public static UserBreadCrumb? UserBreadCrumbCreate(UserBreadCrumb value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            // no sense caching this; it's too transient
            int numRows = context.Database.ExecuteSql($"""
                
                INSERT INTO [Idioma].UserBreadCrumb
                           ([Id]
                           ,[UserId]
                           ,[PageId]
                           ,[ActionDateTime]
                           )
                     VALUES
                           ({value.Id}
                           ,{value.UserId}
                           ,{value.PageId}
                           ,{value.ActionDateTime}
                           )
        
                """);
            if (numRows < 1) throw new InvalidDataException("creating UserBreadCrumb affected 0 rows");
            return value;
        }
        public static async Task<UserBreadCrumb?> UserCreateAsync(UserBreadCrumb value, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return UserBreadCrumbCreate(value, dbContextFactory); });
        }
        #endregion

        #region read
        public static List<UserBreadCrumb> UserBreadCrumbReadByFilter(
            Expression<Func<UserBreadCrumb, bool>> filter, int take, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            var context = dbContextFactory.CreateDbContext();

            return context.UserBreadCrumbs
                .Where(filter)
                .OrderByDescending(x => x.ActionDateTime)
                .Take(take)
                .ToList();
        }
        public static async Task<List<UserBreadCrumb>> UserBreadCrumbReadByFilterAsync(
            Expression<Func<UserBreadCrumb, bool>> filter, int take, IDbContextFactory<IdiomaticaContext> dbContextFactory)
        {
            return await Task.Run(() => { return UserBreadCrumbReadByFilter(filter, take, dbContextFactory); });
        }
        #endregion
    }
}
