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
        public static void BookStatsCreateAndSave(IdiomaticaContext context, Guid bookId)
        {
            int numRows = context.Database.ExecuteSql($"""
            with allPages as (
            	SELECT b.Id as bookKey, p.Id as pageKey, p.Ordinal as pageOrdinal
            	FROM [Idioma].[Book] b
            	left join [Idioma].[Page] p on p.BookId = b.Id
            ), allParagraphs as (
            	select bookKey, p.pageKey, pageOrdinal, pp.Id as paragraphKey, pp.Ordinal as paragraphOrdinal
            	from [Idioma].[Paragraph] pp
            	left join allPages p on pp.PageKey = p.pageKey
            ), allSentences as (
            	select bookKey, pageKey, pageOrdinal, pp.paragraphKey, paragraphOrdinal, s.Id as sentenceKey, s.Ordinal as sentenceOrdinal
            	from allParagraphs pp
            	join [Idioma].[Sentence] s on s.ParagraphKey = pp.paragraphKey
            ), allTokens as (
            	select bookKey, pageKey, pageOrdinal, paragraphKey, paragraphOrdinal, s.sentenceKey, sentenceOrdinal, t.Id as tokenKey, t.Ordinal as tokenOrdinal, t.WordId as wordKey
            	from allSentences s
            	left join [Idioma].[Token] t on t.SentenceKey = s.sentenceKey
            ), allWords as (
            	select bookKey, pageKey, pageOrdinal, paragraphKey, paragraphOrdinal, sentenceKey, sentenceOrdinal, t.tokenKey, tokenOrdinal, w.TextLowerCase as wordText
            	from allTokens t
            	left join [Idioma].[Word] w on t.wordKey = w.Id
            ), distinctWords as (
            	select bookKey, wordText, count(*) as numInstances
            	from allWords
            	group by bookKey, wordText
            ), totalPageCount as (
            	select 
                      bookKey as BookId
                    , {(int)AvailableBookStat.TOTALPAGES} as [Key]
                    , FORMAT(count(*),'#') as [Value]
            	from allPages
            	group by bookKey
            ), totalWordCount as (
            	select 
                      bookKey as BookId
                    , {(int)AvailableBookStat.TOTALWORDCOUNT} as [Key]
                    , FORMAT (sum(numInstances),'#') as [Value]
            	from distinctWords
            	group by bookKey
            ), distinctWordCount as (
            	select 
                      bookKey as BookId
                    , {(int)AvailableBookStat.DISTINCTWORDCOUNT} as [Key]
                    , FORMAT (count(wordText),'#') as [Value]
            	from distinctWords
            	group by bookKey
            ), difficultyScore as (
                select
            	    b.Id as BookId
            	    , {(int)AvailableBookStat.DIFFICULTYSCORE} as [Key]
            	    , FORMAT (avg(case when wr.WordId is null then 65000 else wr.[DifficultyScore] end) / 650, '##.##') as [Value]
                from [Idioma].[Page] p
                left join [Idioma].[Book] b on p.BookId = b.Id
                left join [Idioma].[Language] l on b.LanguageKey = l.Id
                left join [Idioma].[Paragraph] pp on p.Id = pp.PageKey
                left join [Idioma].[Sentence] s on pp.Id = s.ParagraphKey
                left join [Idioma].[Token] t on s.Id = t.SentenceKey
                left join [Idioma].[Word] w on t.WordId = w.Id
                left join [Idioma].[WordRank] wr on l.Id = wr.[LanguageKey] and w.Id = wr.[WordId]
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
        public static async Task BookStatsCreateAndSaveAsync(IdiomaticaContext context, Guid bookId)
        {
            await Task.Run(() => BookStatsCreateAndSave(context, bookId));
        }
    }
}
