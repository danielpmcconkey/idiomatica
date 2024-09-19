using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.DAL;
using Logic.Telemetry;
using Microsoft.EntityFrameworkCore;
using Model.Enums;

namespace Logic.Services.API
{
    public static class BookStatApi
    {
        public static void BookStatsCreateAndSave(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId)
        {
            var context = dbContextFactory.CreateDbContext();
            int numRows = context.Database.ExecuteSql($"""
            with allPages as (
            	SELECT b.Id as BookId, p.Id as PageId, p.Ordinal as PageOrdinal
            	FROM [Idioma].[Book] b
            	left join [Idioma].[Page] p on p.BookId = b.Id
            ), allParagraphs as (
            	select BookId, p.PageId, PageOrdinal, pp.Id as ParagraphId, pp.Ordinal as ParagraphOrdinal
            	from [Idioma].[Paragraph] pp
            	left join allPages p on pp.PageId = p.PageId
            ), allSentences as (
            	select BookId, PageId, PageOrdinal, pp.ParagraphId, ParagraphOrdinal, s.Id as SentenceId, s.Ordinal as SentenceOrdinal
            	from allParagraphs pp
            	join [Idioma].[Sentence] s on s.ParagraphId = pp.ParagraphId
            ), allTokens as (
            	select BookId, PageId, PageOrdinal, ParagraphId, ParagraphOrdinal, s.SentenceId, SentenceOrdinal, t.Id as TokenId, t.Ordinal as TokenOrdinal, t.WordId as WordId
            	from allSentences s
            	left join [Idioma].[Token] t on t.SentenceId = s.SentenceId
            ), allWords as (
            	select BookId, PageId, PageOrdinal, ParagraphId, ParagraphOrdinal, SentenceId, SentenceOrdinal, t.TokenId, TokenOrdinal, w.TextLowerCase as WordText
            	from allTokens t
            	left join [Idioma].[Word] w on t.WordId = w.Id
            ), distinctWords as (
            	select BookId, WordText, count(*) as numInstances
            	from allWords
            	group by BookId, WordText
            ), totalPageCount as (
            	select 
                      BookId as BookId
                    , {(int)AvailableBookStat.TOTALPAGES} as [Key]
                    , FORMAT(count(*),'#') as [Value]
            	from allPages
            	group by BookId
            ), totalWordCount as (
            	select 
                      BookId as BookId
                    , {(int)AvailableBookStat.TOTALWORDCOUNT} as [Key]
                    , FORMAT (sum(numInstances),'#') as [Value]
            	from distinctWords
            	group by BookId
            ), distinctWordCount as (
            	select 
                      BookId as BookId
                    , {(int)AvailableBookStat.DISTINCTWORDCOUNT} as [Key]
                    , FORMAT (count(WordText),'#') as [Value]
            	from distinctWords
            	group by BookId
            ), difficultyScore as (
                select
            	    b.Id as BookId
            	    , {(int)AvailableBookStat.DIFFICULTYSCORE} as [Key]
            	    , FORMAT (avg(case when wr.WordId is null then 65000 else wr.[DifficultyScore] end) / 650, '##.##') as [Value]
                from [Idioma].[Page] p
                left join [Idioma].[Book] b on p.BookId = b.Id
                left join [Idioma].[Language] l on b.LanguageId = l.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageId
                left join [Idioma].[Sentence] s on pp.Id = s.ParagraphId
                left join [Idioma].[Token] t on s.Id = t.SentenceId
                left join [Idioma].[Word] w on t.WordId = w.Id
                left join [Idioma].[WordRank] wr on l.Id = wr.[LanguageId] and w.Id = wr.[WordId]
                where b.Id = {bookId}
                group by b.Id
            ), bookStatQueries as (
            	select * from totalPageCount
            	union all
            	select * from totalWordCount
            	union all
            	select * from distinctWordCount
                union all
            	select * from difficultyScore
            )
            insert into [Idioma].[BookStat](BookId, [Key], [Value])
            select * from bookStatQueries
            where BookId = {bookId}
            """);
            if (numRows < 1)
            {
                ErrorHandler.LogAndThrow();
            }
        }
        public static async Task BookStatsCreateAndSaveAsync(IDbContextFactory<IdiomaticaContext> dbContextFactory, Guid bookId)
        {
            await Task.Run(() => BookStatsCreateAndSave(dbContextFactory, bookId));
        }
    }
}
