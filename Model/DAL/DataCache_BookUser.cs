﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {
        private static ConcurrentDictionary<int, BookUser> BookUserById = new ConcurrentDictionary<int, BookUser>();
        private static ConcurrentDictionary<(int bookId, int userId), BookUser> BookUserByBookIdAndUserId = new ConcurrentDictionary<(int bookId, int userId), BookUser>();

        #region create

        public static BookUser? BookUserCreate(BookUser bookUser, IdiomaticaContext context)
        {
            Guid guid = Guid.NewGuid();
            int numRows = context.Database.ExecuteSql($"""
                        INSERT INTO [Idioma].[BookUser]
                              ([BookId]
                              ,[LanguageUserId]
                              ,[IsArchived]
                              ,[CurrentPageID]
                              ,[AudioBookmarks]
                              ,[AudioCurrentPos]
                              ,[UniqueKey])
                        VALUES
                              ({bookUser.BookId}
                              ,{bookUser.LanguageUserId}
                              ,{bookUser.IsArchived}
                              ,{bookUser.CurrentPageID}
                              ,{bookUser.AudioBookmarks}
                              ,{bookUser.AudioCurrentPos}
                              ,{guid})
                        """);
            if (numRows < 1) throw new InvalidDataException("creating BookUser affected 0 rows");


            var newEntity = context.BookUsers.Where(x => x.UniqueKey == guid).FirstOrDefault();
            if (newEntity is null || newEntity.Id is null || newEntity.Id < 1)
            {
                throw new InvalidDataException("newEntity is null in BookUserCreate");
            }


            // add it to cache
            BookUserById[(int)newEntity.Id] = newEntity;
            return newEntity;
        }
        public static async Task<BookUser?> BookUserCreateAsync(BookUser value, IdiomaticaContext context)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserCreate(value, context);
            });
        }

        #endregion

        #region read
        public static BookUser? BookUserByIdRead(int key, IdiomaticaContext context)
        {
            // check cache
            if (BookUserById.TryGetValue(key, out BookUser? value))
            {
                return value;
            }

            // read DB
            value = context.BookUsers.Where(x => x.Id == key)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            BookUserById[key] = value;
            return value;
        }
        public static async Task<BookUser?> BookUserByIdReadAsync(int key, IdiomaticaContext context)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserByIdRead(key, context);
            });
        }
        public static BookUser? BookUserByBookIdAndUserIdRead(
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            // check cache
            if (BookUserByBookIdAndUserId.ContainsKey(key))
            {
                return BookUserByBookIdAndUserId[key];
            }

            // read DB
            var value = context.BookUsers
                .Where(x => x.LanguageUser != null &&
                    x.LanguageUser.UserId == key.userId &&
                    x.BookId == key.bookId)
                .FirstOrDefault();
            if (value is null || value.Id is null) return null;
            // write to cache
            BookUserByBookIdAndUserId[key] = value;
            BookUserById[(int)value.Id] = value;
            return value;
        }
        public static async Task<BookUser?> BookUserByBookIdAndUserIdReadAsync(
            (int bookId, int userId) key, IdiomaticaContext context)
        {
            return await Task<BookUser?>.Run(() =>
            {
                return BookUserByBookIdAndUserIdRead(key, context);
            });
        }
        #endregion

        #region update
        public static void BookUserUpdate(BookUser bookUser, IdiomaticaContext context)
        {
            if (bookUser.Id == null || bookUser.Id < 1) 
                throw new ArgumentException("ID cannot be null or 0 when updating");
            
            int numRows = context.Database.ExecuteSql($"""
                        update [Idioma].[BookUser]
                              set [BookId] = {bookUser.BookId}
                              ,[LanguageUserId] = {bookUser.LanguageUserId}
                              ,[IsArchived] = {bookUser.IsArchived}
                              ,[CurrentPageID] = {bookUser.CurrentPageID}
                              ,[AudioBookmarks] = {bookUser.AudioBookmarks}
                              ,[AudioCurrentPos] = {bookUser.AudioCurrentPos}
                        where Id = {bookUser.Id}
                        ;
                        """);
            if (numRows < 1)
            {
                throw new InvalidDataException("BookUser update affected 0 rows");
            };
            // now update the cache
            BookUserById[(int)bookUser.Id] = bookUser;

            var cachedItem1 = BookUserByBookIdAndUserId.Where(x => x.Value.Id == bookUser.Id).FirstOrDefault();
            if (cachedItem1.Value != null)
            {
                BookUserByBookIdAndUserId[cachedItem1.Key] = bookUser;
            }
            return;
        }
        public static async Task BookUserUpdateAsync(BookUser value, IdiomaticaContext context)
        {
            await Task.Run(() =>
            {
                BookUserUpdate(value, context);
            });
        }
        #endregion
    }
}
