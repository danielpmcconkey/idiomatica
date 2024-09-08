using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Model.DAL;
using Logic.Telemetry;
using Microsoft.EntityFrameworkCore;

namespace Logic.Services.API
{
    public static class BookStatApi
    {
        public static void BookStatsCreateAndSave(IdiomaticaContext context, Guid bookId)
        {
            int numRows = context.Database.ExecuteSql($"""
            with allPages as (
            	SELECT b.UniqueKey as bookKey, p.UniqueKey as pageKey, p.Ordinal as pageOrdinal
            	FROM [Idioma].[Book] b
            	left join [Idioma].[Page] p on p.BookKey = b.UniqueKey
            ), allParagraphs as (
            	select bookKey, p.pageKey, pageOrdinal, pp.UniqueKey as paragraphKey, pp.Ordinal as paragraphOrdinal
            	from [Idioma].[Paragraph] pp
            	left join allPages p on pp.PageKey = p.pageKey
            ), allSentences as (
            	select bookKey, pageKey, pageOrdinal, pp.paragraphKey, paragraphOrdinal, s.UniqueKey as sentenceKey, s.Ordinal as sentenceOrdinal
            	from allParagraphs pp
            	join [Idioma].[Sentence] s on s.ParagraphKey = pp.paragraphKey
            ), allTokens as (
            	select bookKey, pageKey, pageOrdinal, paragraphKey, paragraphOrdinal, s.sentenceKey, sentenceOrdinal, t.UniqueKey as tokenKey, t.Ordinal as tokenOrdinal, t.WordKey as wordKey
            	from allSentences s
            	left join [Idioma].[Token] t on t.SentenceKey = s.sentenceKey
            ), allWords as (
            	select bookKey, pageKey, pageOrdinal, paragraphKey, paragraphOrdinal, sentenceKey, sentenceOrdinal, t.tokenKey, tokenOrdinal, w.TextLowerCase as wordText
            	from allTokens t
            	left join [Idioma].[Word] w on t.wordKey = w.UniqueKey
            ), distinctWords as (
            	select bookKey, wordText, count(*) as numInstances
            	from allWords
            	group by bookKey, wordText
            ), totalPageCount as (
            	select 
                      bookKey as BookKey
                    , {(int)AvailableBookStat.TOTALPAGES} as [Key]
                    , FORMAT(count(*),'#') as [Value]
            	from allPages
            	group by bookKey
            ), totalWordCount as (
            	select 
                      bookKey as BookKey
                    , {(int)AvailableBookStat.TOTALWORDCOUNT} as [Key]
                    , FORMAT (sum(numInstances),'#') as [Value]
            	from distinctWords
            	group by bookKey
            ), distinctWordCount as (
            	select 
                      bookKey as BookKey
                    , {(int)AvailableBookStat.DISTINCTWORDCOUNT} as [Key]
                    , FORMAT (count(wordText),'#') as [Value]
            	from distinctWords
            	group by bookKey
            ), difficultyScore as (
                select
            	    b.UniqueKey as BookKey
            	    , {(int)AvailableBookStat.DIFFICULTYSCORE} as [Key]
            	    , FORMAT (avg(case when wr.WordKey is null then 65000 else wr.[DifficultyScore] end) / 650, '##.##') as [Value]
                from [Idioma].[Page] p
                left join [Idioma].[Book] b on p.BookKey = b.UniqueKey
                left join [Idioma].[Language] l on b.LanguageKey = l.UniqueKey
                left join [Idioma].[Paragraph] pp on p.UniqueKey = pp.PageKey
                left join [Idioma].[Sentence] s on pp.UniqueKey = s.ParagraphKey
                left join [Idioma].[Token] t on s.UniqueKey = t.SentenceKey
                left join [Idioma].[Word] w on t.WordKey = w.UniqueKey
                left join [Idioma].[WordRank] wr on l.UniqueKey = wr.[LanguageKey] and w.UniqueKey = wr.[WordKey]
                where b.UniqueKey = {bookId}
                group by b.UniqueKey
            ), bookStatQueries as (
            	select * from totalPageCount
            	union all
            	select * from totalWordCount
            	union all
            	select * from distinctWordCount
                union all
            	select * from difficultyScore
            )
            insert into [Idioma].[BookStat](BookKey, [Key], [Value])
            select * from bookStatQueries
            where BookKey = {bookId}
            """);
            if (numRows < 1)
            {
                ErrorHandler.LogAndThrow(2110);
            }
        }
        public static async Task BookStatsCreateAndSaveAsync(IdiomaticaContext context, Guid bookId)
        {
            await Task.Run(() => BookStatsCreateAndSave(context, bookId));
        }
    }
}
