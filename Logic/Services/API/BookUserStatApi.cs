﻿using Logic.Telemetry;
using Model.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Model.Enums;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class BookUserStatApi
    {
        public static List<BookUserStat>? BookUserStatsRead(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            return DataCache.BookUserStatsByBookIdAndUserIdRead(
                    (bookId, userId), dbContextFactory);
        }
        public static async Task<List<BookUserStat>?> BookUserStatsReadAsync(
            IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId, Guid userId)
        {
            return await DataCache.BookUserStatsByBookIdAndUserIdReadAsync(
                    (bookId, userId), dbContextFactory);
        }


        public static void BookUserStatsUpdateByBookUserId(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookUserId)
        {
            var bookUser = DataCache.BookUserByIdRead(bookUserId, dbContextFactory);

            if (bookUser == null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            var languageUser = DataCache.LanguageUserByIdRead((Guid)bookUser.LanguageUserId, dbContextFactory);
            if (languageUser is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }
            
            // delete the booklistrows cache for this user
            DataCache.BookListRowsByUserIdDelete((Guid)languageUser.Id, dbContextFactory);
            // delete the actual stats from teh database and bookuserstats cache
            DataCache.BookUserStatsByBookIdAndUserIdDelete(((Guid)bookUser.BookId, (Guid)languageUser.UserId), dbContextFactory);

            var allWordsInBook = DataCache.WordsByBookIdRead((Guid)bookUser.BookId, dbContextFactory);
           
            var allWordUsersInBook = DataCache.WordUsersByBookIdAndLanguageUserIdRead(
                ((Guid)bookUser.BookId, (Guid)bookUser.LanguageUserId), dbContextFactory, true);
            var pageUsers = DataCache.PageUsersByBookUserIdRead(bookUserId, dbContextFactory, true);
            var pages = DataCache.PagesByBookIdRead((Guid)bookUser.BookId, dbContextFactory);
            var bookStats = DataCache.BookStatsByBookIdRead((Guid)bookUser.BookId, dbContextFactory);
            if (bookStats is null)
            {
                ErrorHandler.LogAndThrow();
                return;
            }

            var totalPagesStatline = bookStats.Where(x => x.Key == AvailableBookStat.TOTALPAGES).FirstOrDefault();
            var totalWordsStatline = bookStats.Where(x => x.Key == AvailableBookStat.TOTALWORDCOUNT).FirstOrDefault();
            var totalDistinctWordsStatline = bookStats.Where(x => x.Key == AvailableBookStat.DISTINCTWORDCOUNT).FirstOrDefault();
            var readPages = (from pu in pageUsers
                             join p in pages on pu.PageId equals p.Id
                             where pu.ReadDate != null
                             orderby p.Ordinal descending
                             select p)
                            .ToList();

            //pageUsers.Where(x => x.ReadDate != null).OrderByDescending(x => x.or);
            int totalPages = 0;
            bool isComplete = false;

            // is complete
            if (totalPagesStatline != null)
            {
                int.TryParse(totalPagesStatline.Value, out totalPages);
                isComplete = (readPages.Count() == totalPages) ? true : false;
            }

            // last page read
            int lastPageRead = 0;
            var latestReadPage = readPages.FirstOrDefault();
            if ((int)readPages.Count > 0 && latestReadPage != null && latestReadPage.Ordinal > 0)
                lastPageRead = (int)latestReadPage.Ordinal;

            // progress
            string progress = $"{lastPageRead} / {totalPages}";

            // progress percent
            decimal progressPercent = (totalPages == 0) ? 0 : lastPageRead / (decimal)totalPages;

            // AvailableBookUserStat.TOTALWORDCOUNT
            int totalWordCount = 0;
            if (totalWordsStatline != null)
            {
                int.TryParse(totalWordsStatline.Value, out totalWordCount);
            }

            // AvailableBookUserStat.DISTINCTWORDCOUNT
            int distinctWordCount = allWordsInBook.Count;

            // AvailableBookUserStat.DISTINCTKNOWNPERCENT
            int knownDistinct = allWordUsersInBook
                .Where(x =>
                    x.Status == AvailableWordUserStatus.WELLKNOWN ||
                    x.Status == AvailableWordUserStatus.LEARNED ||
                    x.Status == AvailableWordUserStatus.IGNORED)
                .Count();

            decimal distinctKnownPercent = knownDistinct / (decimal)distinctWordCount;


            // AvailableBookUserStat.TOTALKNOWNPERCENT
            List<BookUserStat> stats = new List<BookUserStat>();
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.ISCOMPLETE,
                Book = bookUser.Book,
                LanguageUser = bookUser.LanguageUser,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueString = isComplete.ToString(),
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.LASTPAGEREAD,
                Book = bookUser.Book,
                LanguageUser = bookUser.LanguageUser,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = lastPageRead,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.PROGRESS,
                Book = bookUser.Book,
                LanguageUser = bookUser.LanguageUser,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueString = progress,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.PROGRESSPERCENT,
                Book = bookUser.Book,
                LanguageUser = bookUser.LanguageUser,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = progressPercent,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.DISTINCTKNOWNPERCENT,
                Book = bookUser.Book,
                LanguageUser = bookUser.LanguageUser,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = distinctKnownPercent,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.DISTINCTWORDCOUNT,
                Book = bookUser.Book,
                LanguageUser = bookUser.LanguageUser,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = distinctWordCount,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.TOTALWORDCOUNT,
                Book = bookUser.Book,
                LanguageUser = bookUser.LanguageUser,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueNumeric = totalWordCount,
            });
            stats.Add(new BookUserStat()
            {
                Key = AvailableBookUserStat.TOTALKNOWNPERCENT,
                Book = bookUser.Book,
                LanguageUser = bookUser.LanguageUser,
                BookId = bookUser.BookId,
                LanguageUserId = bookUser.LanguageUserId,
                ValueString = null,
                ValueNumeric = null,
            });

            
            // add the new stats
            DataCache.BookUserStatsByBookIdAndUserIdCreate(
                ((Guid)bookUser.BookId, (Guid)languageUser.UserId), stats, dbContextFactory);
            

        }
        public static async Task BookUserStatsUpdateByBookUserIdAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookUserId)
        {
            await Task.Run(() =>
            {
                BookUserStatsUpdateByBookUserId(dbContextFactory, bookUserId);
            });
        }
    }
}
