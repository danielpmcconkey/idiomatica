using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Model.DAL
{
    public static partial class DataCache
    {

        private static ConcurrentDictionary<int, List<BookListRow>> BookListRowsByUserId = new ConcurrentDictionary<int, List<BookListRow>>();




        #region read
        public static async Task<List<BookListRow>> BookListRowsByUserIdReadAsync(
            int key, IdiomaticaContext context, bool shouldFetchFreshValue = false)
        {
            //var lc = await DataCache.LanguageCodeByCodeReadAsync("EN-US", context);
            var test = await BookListRowsPowerQueryAsync(key, 50, 0, false, null, null, null, AvailableBookListSortProperties.TITLE, false, context);
            // check cache
            if (BookListRowsByUserId.ContainsKey(key) && !shouldFetchFreshValue)
            {
                return BookListRowsByUserId[key];
            }
            // read DB
            var value = context.BookListRows.Where(x => x.UserId == key && x.IsArchived != true)
                .ToList();

            // write to cache
            BookListRowsByUserId[key] = value;
            return value;
        }
        private static async Task<List<BookListRow>> BookListRowsPowerQueryAsync(
            int userId, int numRecords, int skip, bool shouldShowOnlyInShelf, string? tagsFilter,
            LanguageCode? lcFilter, string? titleFilter, AvailableBookListSortProperties? orderBy, bool sortAscending,
            IdiomaticaContext context)
        {
            /*
             * note: none of the string fields are safe. and, since we don't 
             * use parameters, due to the way we're building the query, you must 
             * sanitize everything. No little Bobby Tables.
            /* 
             * you are here. you want to finish building out the power query 
             * then use it in the method above then implement it for the browse
             * page, which you want to make as a subset of the booklist page
             * */
            Func<string, string> sanitizeString = (input) =>
            {
                try
                {
                    return Regex.Replace(input, @"[^\w\.@ -]", "",
                                         RegexOptions.None, TimeSpan.FromSeconds(1.5));
                }
                // If we timeout when replacing invalid characters,
                // we should return Empty.
                catch (RegexMatchTimeoutException)
                {
                    return String.Empty;
                }
            };
            
            string showShelfWhereClause = shouldShowOnlyInShelf ? "and bu.Id is not null and bu.IsArchived <> 1" : "";
            string tagsWhereClause1 = string.Empty; // pull filtered tags before you do the string ag function
            string tagsWhereClause2 = string.Empty; // only pull rows where the above tags is not null
            if (tagsFilter != null && tagsFilter != string.Empty)
            {
                string tagsFilterSanitized = tagsFilter;
                tagsWhereClause1 = $"and bt.Tag like '%{sanitizeString(tagsFilter)}%'";
                tagsWhereClause2 = "and at.Tag is not null";
            }
            string languageFilterWhereClause = string.Empty;
            if (lcFilter is not null)
            {
                languageFilterWhereClause = $"and l.LanguageCode = '{lcFilter.Code}'";
            }
            string titleFilterWhereClause = string.Empty;
            if (titleFilter is not null)
            {
                titleFilterWhereClause = $"and b.Title like '%{sanitizeString(titleFilter)}%'";
            }
            string sortDirection = sortAscending ? "asc" : "desc";
            string orderByClause = $"order by b.Id {sortDirection}";
            if (orderBy is not null)
            {
                switch (orderBy)
                {
                    case AvailableBookListSortProperties.BOOKID:
                        orderByClause = $"order by b.Id {sortDirection}";
                        break;
                    case AvailableBookListSortProperties.LANGUAGENAME:
                        orderByClause = $"order by l.[Name] {sortDirection}";
                        break;
                    case AvailableBookListSortProperties.ISCOMPLETE:
                        orderByClause = $"order by bus_ISCOMPLETE.ValueString {sortDirection}";
                        break;
                    case AvailableBookListSortProperties.TITLE:
                        orderByClause = $"order by b.Title {sortDirection}";
                        break;
                    case AvailableBookListSortProperties.TOTALPAGES:
                        orderByClause = $"order by cast(bsTotalPages.[Value] as int) {sortDirection}";
                        break;
                    case AvailableBookListSortProperties.TOTALWORDCOUNT:
                        orderByClause = $"order by cast(bsTotalWordCount.[Value] as int) {sortDirection}";
                        break;
                    case AvailableBookListSortProperties.DISTINCTWORDCOUNT:
                        orderByClause = $"order by cast(bsDistinctWordCount.[Value] as int) {sortDirection}";
                        break;

                }
            }
            
            var value = context.Database.SqlQueryRaw<BookListRow>($"""
                
                    with AllTags as (
                        select 
                            b.Id as BookId
                            , bt.Tag
                        from Idioma.Book b
                        left join Idioma.BookTag bt on b.Id = bt.BookId
                        where 1 = 1
                        {tagsWhereClause1}
                        group by b.Id, bt.Tag
                    ), AllResults as (
                        select
                            ROW_NUMBER() over ({orderByClause}) as RowNumber
                            , case when bu.Id is null or bu.IsArchived = 1 then cast(0 as bit) else cast (1 as bit) end as IsInShelf
                            , lu.UserId
                            , b.Id as BookId
                            , l.[Name] as LanguageName
                            , bus_ISCOMPLETE.ValueString as IsComplete
                            , b.Title
                            , cast(bsTotalPages.[Value] as int) as TotalPages
                            , bus_PROGRESS.ValueString as Progress
                            , bus_PROGRESSPERCENT.ValueNumeric as ProgressPercent
                            , cast(bsTotalWordCount.[Value] as int) as TotalWordCount
                            , cast(bsDistinctWordCount.[Value] as int) as DistinctWordCount
                            , bus_DISTINCTKNOWNPERCENT.ValueNumeric as DistinctKnownPercent
                            , bu.IsArchived
                            , STRING_AGG(at.Tag, ',') AS Tags
                        from Idioma.Book b
                        left join Idioma.Language l on b.LanguageId = l.Id
                        left join Idioma.BookStat bsTotalPages on bsTotalPages.BookId = b.Id and bsTotalPages.[Key] = 1
                        left join Idioma.BookStat bsTotalWordCount on bsTotalWordCount.BookId = b.Id and bsTotalWordCount.[Key] = 2
                        left join Idioma.BookStat bsDistinctWordCount on bsDistinctWordCount.BookId = b.Id and bsDistinctWordCount.[Key] = 3
                        left join AllTags at on at.BookId = b.Id
                        left join Idioma.LanguageUser lu on lu.LanguageId = b.LanguageId and lu.UserId = {userId}
                        left join Idioma.BookUser bu on bu.BookId = b.Id and bu.LanguageUserId = lu.Id
                        left join [Idioma].[BookUserStat] bus_ISCOMPLETE on bus_ISCOMPLETE.BookId = b.Id and bus_ISCOMPLETE.LanguageUserId = lu.Id and bus_ISCOMPLETE.[Key] = 1 --AvailableBookUserStat.ISCOMPLETE
                        left join [Idioma].[BookUserStat] bus_PROGRESS on bus_PROGRESS.BookId = b.Id and bus_PROGRESS.LanguageUserId = lu.Id and bus_PROGRESS.[Key] = 3 --AvailableBookUserStat.PROGRESS
                        left join [Idioma].[BookUserStat] bus_PROGRESSPERCENT on bus_PROGRESSPERCENT.BookId = b.Id and bus_PROGRESSPERCENT.LanguageUserId = lu.Id and bus_PROGRESSPERCENT.[Key] = 4 --AvailableBookUserStat.PROGRESSPERCENT
                        left join [Idioma].[BookUserStat] bus_TOTALWORDCOUNT on bus_TOTALWORDCOUNT.BookId = b.Id and bus_TOTALWORDCOUNT.LanguageUserId = lu.Id and bus_TOTALWORDCOUNT.[Key] = 8 --AvailableBookUserStat.TOTALWORDCOUNT
                        left join [Idioma].[BookUserStat] bus_TOTALKNOWNPERCENT on bus_TOTALKNOWNPERCENT.BookId = b.Id and bus_TOTALKNOWNPERCENT.LanguageUserId = lu.Id and bus_TOTALKNOWNPERCENT.[Key] = 6 --AvailableBookUserStat.TOTALKNOWNPERCENT
                        left join [Idioma].[BookUserStat] bus_DISTINCTWORDCOUNT on bus_DISTINCTWORDCOUNT.BookId = b.Id and bus_DISTINCTWORDCOUNT.LanguageUserId = lu.Id and bus_DISTINCTWORDCOUNT.[Key] = 7 --AvailableBookUserStat.DISTINCTWORDCOUNT
                        left join [Idioma].[BookUserStat] bus_DISTINCTKNOWNPERCENT on bus_DISTINCTKNOWNPERCENT.BookId = b.Id and bus_DISTINCTKNOWNPERCENT.LanguageUserId = lu.Id and bus_DISTINCTKNOWNPERCENT.[Key] = 5 --AvailableBookUserStat.DISTINCTKNOWNPERCENT
                        where 1=1
                        {showShelfWhereClause}
                        {tagsWhereClause2}
                        {languageFilterWhereClause}
                        {titleFilterWhereClause}
                        group by
                              b.Id
                            , bu.Id
                            , lu.UserId
                            , l.[Name]
                            , bus_ISCOMPLETE.ValueString
                            , b.Title
                            , bsTotalPages.[Value]
                            , bus_PROGRESS.ValueString
                            , bus_PROGRESSPERCENT.ValueNumeric
                            , bsTotalWordCount.[Value]
                            , bsDistinctWordCount.[Value]
                            , bus_DISTINCTKNOWNPERCENT.ValueNumeric
                            , bu.IsArchived
                    )
                    select * from AllResults
                    where RowNumber > {skip} 
                    and RowNumber <= {skip} + {numRecords}



                
                """);
            return value.ToList();
        }
        #endregion

        #region delete
        /*
        * since BookListRow is just a view, delete methods 
        * will only delete the cache
        * */
        public static void BookListRowsByUserIdDeleteAsync( int key, IdiomaticaContext context)
        {
            BookListRowsByUserId.TryRemove(key, out var deletedRow);
        }
        #endregion


    }
}
