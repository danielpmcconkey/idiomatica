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

        private static ConcurrentDictionary<Guid, List<BookListRow>> BookListRowsByUserId = [];
        private static ConcurrentDictionary<(Guid bookId, Guid userId), BookListRow?> BookListRowByBookIdAndUserId = [];



        #region read
        public static BookListRow? BookListRowByBookIdAndUserIdRead(
            (Guid bookId, Guid userId) key, IdiomaticaContext context, bool shouldOverrideCache = false)
        {
            // check cache
            if (BookListRowByBookIdAndUserId.ContainsKey(key) && !shouldOverrideCache)
            {
                return BookListRowByBookIdAndUserId[key];
            }

            // read DB
            var value = context.BookListRows.Where(x => x.BookKey == key.bookId && x.UserKey == key.userId)
                .FirstOrDefault();
            if (value == null) return null;
            // write to cache
            BookListRowByBookIdAndUserId[key] = value;
            return value;
        }
        public static async Task<BookListRow?> BookListRowByBookIdAndUserIdReadAsync(
            (Guid bookId, Guid userId) key, IdiomaticaContext context, bool shouldOverrideCache = false)
        {
            return await Task<BookListRow?>.Run(() =>
            {
                return BookListRowByBookIdAndUserIdRead(key, context);
            });
        }


        public static (long count, List<BookListRow> results) BookListRowsPowerQuery(
            Guid userId, int numRecords, int skip, bool shouldShowOnlyInShelf, string? tagsFilter,
            LanguageCode? lcFilter, string? titleFilter, AvailableBookListSortProperties? orderBy,
            bool sortAscending, IdiomaticaContext context, Guid? bookIdOverride = null)
        {
            /*
             * note: none of the string fields are safe. and, since we don't 
             * use parameters, due to the way we're building the query, you must 
             * sanitize everything. No little Bobby Tables.
             * */
            

            bool useTags = (tagsFilter != null && tagsFilter != string.Empty) ? true : false;
            int skipVal = (bookIdOverride is null) ? skip : 0;
            int numRecordsVal = (bookIdOverride is null) ? numRecords : 1;


            string sortDirection = sortAscending ? "asc" : "desc";
            string orderByClause = $"order by b.UniqueKey {sortDirection}";
            if (orderBy is not null)
            {
                switch (orderBy)
                {
                    case AvailableBookListSortProperties.DIFFICULTY:
                        orderByClause = $"order by cast(bsDifficultyScore.[Value] as numeric(5,2)) {sortDirection}";
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

            StringBuilder sb = new StringBuilder();

            sb.Append($"""
                    with
                """);
            if (useTags && tagsFilter is not null && bookIdOverride is null)
            {
                sb.Append($"""

                     AllTags as (
                        select 
                            b.UniqueKey as BookId
                            , bt.Tag
                        from Idioma.Book b
                        left join Idioma.BookTag bt on b.UniqueKey = bt.BookKey
                        where bt.Tag like '%{DALUtilities.SanitizeString(tagsFilter)}%'
                        group by b.UniqueKey, bt.Tag
                    ),
                    """);
            }
            sb.Append($"""

                    AllResults as (
                        select
                """);
            if (numRecords > 0)
            {
                sb.Append($"""

                            ROW_NUMBER() over ({orderByClause}) as RowNumber
                    """);
            }
            else
            {
                sb.Append($"""

                            cast(1 as bigint) as RowNumber
                    """);
            }
            sb.Append($"""

                            , case when bu.UniqueKey is null or bu.IsArchived = 1 then cast(0 as bit) else cast (1 as bit) end as IsInShelf
                            , lu.UserKey
                            , b.UniqueKey as BookId
                            , l.[Name] as LanguageName
                            , bus_ISCOMPLETE.ValueString as IsComplete
                            , b.Title
                            , cast(bsTotalPages.[Value] as int) as TotalPages
                            , bus_PROGRESS.ValueString as Progress
                            , bus_PROGRESSPERCENT.ValueNumeric as ProgressPercent
                            , cast(bsTotalWordCount.[Value] as int) as TotalWordCount
                            , cast(bsDistinctWordCount.[Value] as int) as DistinctWordCount
                            , bus_DISTINCTKNOWNPERCENT.ValueNumeric as DistinctKnownPercent
                            , cast(bsDifficultyScore.[Value] as numeric(5,2)) as DifficultyScore
                            , bu.IsArchived
                """);
            if (useTags && bookIdOverride is null)
            {
                sb.Append($"""

                            , STRING_AGG(at.Tag, ',') AS Tags
                    """);
            }
            else
            {
                sb.Append($"""

                            , null AS Tags
                    """);
            }
            sb.Append($"""

                        from Idioma.Book b
                        left join Idioma.Language l on b.LanguageKey = l.UniqueKey
                        left join Idioma.BookStat bsTotalPages on bsTotalPages.BookKey = b.UniqueKey and bsTotalPages.[Key] = {(int)AvailableBookStat.TOTALPAGES}
                        left join Idioma.BookStat bsTotalWordCount on bsTotalWordCount.BookKey = b.UniqueKey and bsTotalWordCount.[Key] = {(int)AvailableBookStat.TOTALWORDCOUNT}
                        left join Idioma.BookStat bsDistinctWordCount on bsDistinctWordCount.BookKey = b.UniqueKey and bsDistinctWordCount.[Key] = {(int)AvailableBookStat.DISTINCTWORDCOUNT}
                        left join Idioma.BookStat bsDifficultyScore on bsDifficultyScore.BookKey = b.UniqueKey and bsDifficultyScore.[Key] = {(int)AvailableBookStat.DIFFICULTYSCORE}
                        left join Idioma.LanguageUser lu on lu.LanguageKey = b.LanguageKey and lu.UserKey = {userId}
                        left join Idioma.BookUser bu on bu.BookKey = b.UniqueKey and bu.LanguageUserKey = lu.UniqueKey
                        left join [Idioma].[BookUserStat] bus_ISCOMPLETE on bus_ISCOMPLETE.BookKey = b.UniqueKey and bus_ISCOMPLETE.LanguageUserKey = lu.UniqueKey and bus_ISCOMPLETE.[Key] = {(int)AvailableBookUserStat.ISCOMPLETE}
                        left join [Idioma].[BookUserStat] bus_PROGRESS on bus_PROGRESS.BookKey = b.UniqueKey and bus_PROGRESS.LanguageUserKey = lu.UniqueKey and bus_PROGRESS.[Key] = {(int)AvailableBookUserStat.PROGRESS}
                        left join [Idioma].[BookUserStat] bus_PROGRESSPERCENT on bus_PROGRESSPERCENT.BookKey = b.UniqueKey and bus_PROGRESSPERCENT.LanguageUserKey = lu.UniqueKey and bus_PROGRESSPERCENT.[Key] = {(int)AvailableBookUserStat.PROGRESSPERCENT}
                        left join [Idioma].[BookUserStat] bus_TOTALWORDCOUNT on bus_TOTALWORDCOUNT.BookKey = b.UniqueKey and bus_TOTALWORDCOUNT.LanguageUserKey = lu.UniqueKey and bus_TOTALWORDCOUNT.[Key] = {(int)AvailableBookUserStat.TOTALWORDCOUNT}
                        left join [Idioma].[BookUserStat] bus_TOTALKNOWNPERCENT on bus_TOTALKNOWNPERCENT.BookKey = b.UniqueKey and bus_TOTALKNOWNPERCENT.LanguageUserKey = lu.UniqueKey and bus_TOTALKNOWNPERCENT.[Key] = {(int)AvailableBookUserStat.TOTALKNOWNPERCENT}
                        left join [Idioma].[BookUserStat] bus_DISTINCTWORDCOUNT on bus_DISTINCTWORDCOUNT.BookKey = b.UniqueKey and bus_DISTINCTWORDCOUNT.LanguageUserKey = lu.UniqueKey and bus_DISTINCTWORDCOUNT.[Key] = {(int)AvailableBookUserStat.DISTINCTWORDCOUNT}
                        left join [Idioma].[BookUserStat] bus_DISTINCTKNOWNPERCENT on bus_DISTINCTKNOWNPERCENT.BookKey = b.UniqueKey and bus_DISTINCTKNOWNPERCENT.LanguageUserKey = lu.UniqueKey and bus_DISTINCTKNOWNPERCENT.[Key] = {(int)AvailableBookUserStat.DISTINCTKNOWNPERCENT}
                """);
            if (useTags && bookIdOverride is null)
            {
                sb.Append($"""

                        left join AllTags at on at.BookKey = b.UniqueKey
                    """);
            }
             sb.Append($"""

                        where 1=1
                """);
            if (bookIdOverride is not null)
            {
                sb.Append($"""

                        and b.UniqueKey = {bookIdOverride}
                    """);
            }
            if (shouldShowOnlyInShelf && bookIdOverride is null)
            {
                sb.Append($"""

                        and bu.UniqueKey is not null and bu.IsArchived <> 1
                    """);
            }
            if (useTags && bookIdOverride is null)
            {
                sb.Append($"""

                        and at.Tag is not null
                    """);
            }
            if (lcFilter is not null && bookIdOverride is null)
            {
                sb.Append($"""

                        and l.LanguageCode = '{lcFilter.Code}'
                    """);
            }
            if (titleFilter is not null && bookIdOverride is null)
            {
                sb.Append($"""

                        and b.Title like '%{DALUtilities.SanitizeString(titleFilter)}%'
                    """);
            }
            if (useTags && bookIdOverride is null) // we only aggregate when bringing in tags
            {
                sb.Append($"""

                        group by
                              b.UniqueKey
                            , bu.UniqueKey
                            , lu.UserKey
                            , l.[Name]
                            , bus_ISCOMPLETE.ValueString
                            , b.Title
                            , bsTotalPages.[Value]
                            , bus_PROGRESS.ValueString
                            , bus_PROGRESSPERCENT.ValueNumeric
                            , bsTotalWordCount.[Value]
                            , bsDistinctWordCount.[Value]
                            , bus_DISTINCTKNOWNPERCENT.ValueNumeric
                            , bsDifficultyScore.[Value]
                            , bu.IsArchived
                    """);
            }
            sb.Append($"""

                    )
            """);
            // add in the "leading" record to let consumers know how many results there would be if we didn't paginate
            sb.Append($"""

                    select
                        count(*)  as RowNumber
                        , null as IsInShelf
                        , null as UserId
                        , null as BookId
                        , null as LanguageName
                        , null as IsComplete
                        , null as Title
                        , null as TotalPages
                        , null as Progress
                        , null as ProgressPercent
                        , null as TotalWordCount
                        , null as DistinctWordCount
                        , null as DistinctKnownPercent
                        , null as DifficultyScore
                        , null as IsArchived
                        , null as Tags
                    from AllResults
                    union all
                """);
            sb.Append($"""

                    select * from AllResults
                    """);
            if (numRecords > 0)
            {
                sb.Append($"""

                    where RowNumber > {skipVal} 
                    and RowNumber <= {skipVal} + {numRecordsVal}
                    """);
            }
            var dbValue = context.Database.SqlQueryRaw<BookListRow>(sb.ToString()).ToList();
            if (dbValue.Count > 0) {
                var leadingRecord = dbValue.Where(
                    x => x.BookKey == null && x.Title == null && x.LanguageName == null)
                    .FirstOrDefault();
                if (leadingRecord != null && leadingRecord.RowNumber != null)
                {
                    long count = (long)leadingRecord.RowNumber;
                    var realRows = dbValue.Where(
                    x => x.BookKey != null && x.Title != null && x.LanguageName != null)
                        .OrderBy(x => x.RowNumber)
                        .ToList();
                    return (count,  realRows);
                }
            }
            return (0L, new List<BookListRow>());
        }
        public static async Task<(long count, List<BookListRow> results)> BookListRowsPowerQueryAsync(
            Guid userId, int numRecords, int skip, bool shouldShowOnlyInShelf, string? tagsFilter,
            LanguageCode? lcFilter, string? titleFilter, AvailableBookListSortProperties? orderBy,
            bool sortAscending, IdiomaticaContext context, Guid? bookIdOverride = null)
        {
            return await Task<(long count, List<BookListRow> results)>.Run(() =>
            {
                return BookListRowsPowerQuery(userId, numRecords, skip, shouldShowOnlyInShelf, tagsFilter,
                    lcFilter, titleFilter, orderBy, sortAscending, context, bookIdOverride);
            });
        }
        #endregion

        #region delete
        /*
        * since BookListRow is just a view, delete methods 
        * will only delete the cache
        * */
        public static void BookListRowsByUserIdDelete( Guid key, IdiomaticaContext context)
        {
            BookListRowsByUserId.TryRemove(key, out var deletedRow);
        }
        #endregion


    }
}
