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
            	SELECT b.id as bookId, p.UniqueKey as pageId, p.Ordinal as pageOrdinal
            	FROM [Idioma].[Book] b
            	left join [Idioma].[Page] p on p.BookKey = b.UniqueKey
            ), allParagraphs as (
            	select bookId, p.pageId, pageOrdinal, pp.UniqueKey as paragraphId, pp.Ordinal as paragraphOrdinal
            	from [Idioma].[Paragraph] pp
            	left join allPages p on pp.PageKey = p.pageId
            ), allSentences as (
            	select bookId, pageId, pageOrdinal, pp.paragraphId, paragraphOrdinal, s.UniqueKey as sentenceId, s.Ordinal as sentenceOrdinal
            	from allParagraphs pp
            	join [Idioma].[Sentence] s on s.ParagraphKey = pp.paragraphId
            ), allTokens as (
            	select bookId, pageId, pageOrdinal, paragraphId, paragraphOrdinal, s.sentenceId, sentenceOrdinal, t.UniqueKey as tokenId, t.Ordinal as tokenOrdinal, t.WordKey as wordId
            	from allSentences s
            	left join [Idioma].[Token] t on t.SentenceKey = s.sentenceId
            ), allWords as (
            	select bookId, pageId, pageOrdinal, paragraphId, paragraphOrdinal, sentenceId, sentenceOrdinal, t.tokenId, tokenOrdinal, w.TextLowerCase as wordText
            	from allTokens t
            	left join [Idioma].[Word] w on t.wordId = w.UniqueKey
            ), distinctWords as (
            	select bookId, wordText, count(*) as numInstances
            	from allWords
            	group by bookId, wordText
            ), totalPageCount as (
            	select 
                      bookId as BookId
                    , {(int)AvailableBookStat.TOTALPAGES} as [Key]
                    , FORMAT(count(*),'#') as [Value]
            	from allPages
            	group by bookId
            ), totalWordCount as (
            	select 
                      bookId as BookId
                    , {(int)AvailableBookStat.TOTALWORDCOUNT} as [Key]
                    , FORMAT (sum(numInstances),'#') as [Value]
            	from distinctWords
            	group by bookId
            ), distinctWordCount as (
            	select 
                      bookId as BookId
                    , {(int)AvailableBookStat.DISTINCTWORDCOUNT} as [Key]
                    , FORMAT (count(wordText),'#') as [Value]
            	from distinctWords
            	group by bookId
            ), difficultyScore as (
                select
            	    b.UniqueKey as BookId
            	    , {(int)AvailableBookStat.DIFFICULTYSCORE} as [Key]
            	    , FORMAT (avg(case when wr.WordKey is null then 65000 else wr.[DifficultyScore] end) / 650, '##.##') as [Value]
                from [Idioma].[Page] p
                left join [Idioma].[Book] b on p.BookKey = b.UniqueKey
                left join [Idioma].[Language] l on b.LanguageKey = l.UniqueKey
                left join [Idioma].[Paragraph] pp on p.UniqueKey = pp.PageKey
                left join [Idioma].[Sentence] s on pp.UniqueKey = s.ParagraphKey
                left join [Idioma].[Token] t on s.UniqueKey = t.SentenceKey
                left join [Idioma].[Word] w on t.WordKey = w.UniqueKey
                left join [Idioma].[WordRank] wr on l.UniqueKey = wr.[LanguageId] and w.UniqueKey = wr.[WordId]
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
            insert into [Idioma].[BookStat](BookId, [Key], [Value])
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
