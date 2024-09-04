

INSERT INTO [Idioma].[BookStat]
           ([BookId]
           ,[Key]
           ,[Value])

select
	b.Id as BookId
	, 4 as [Key]
	, FORMAT (avg(case when wr.WordId is null then 65000 else wr.[DifficultyScore] end) / 650, '##.##') as [Value]
from [Idioma].[Page] p
left join [Idioma].[Book] b on p.BookId = b.Id
left join [Idioma].[Language] l on b.LanguageId = l.Id
left join [Idioma].[Paragraph] pp on p.Id = pp.PageId
left join [Idioma].[Sentence] s on pp.Id = s.ParagraphId
left join [Idioma].[Token] t on s.Id = t.SentenceId
left join [Idioma].[Word] w on t.WordId = w.Id
left join [Idioma].[WordRank] wr on l.Id = wr.[LanguageId] and w.Id = wr.[WordId]
where 1=1
and l.Id = 1
group by b.Id, b.Title
order by 3