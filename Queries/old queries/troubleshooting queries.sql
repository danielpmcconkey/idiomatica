select
	b.Id as BookId
	--,b.Title
	--, p.Id as PageId
	--,p.Ordinal as PageOrd
	--, p.OriginalText as PageText
	--, bu.Id as BookUserId
	--, bu.CurrentPageID
	--, pu.Id as PageUserId
	--, pu.ReadDate
	--, pp.Id as PPId
	--, pp.Ordinal as PPOrd
	--, s.Id as SentenceId
	--, s.Text as SentenceText
	--, s.Ordinal as SentenceOrd
	--, t.Id as TokenId
	--, t.Display
	--, t.Ordinal as TokenOrdinal
	, w.Id
	, w.TextLowerCase
	, wu.Id as WordUserId
	, wu.Translation
	, wu.Status
	----,count(*)
from [Idioma].[Page] p
left join [Idioma].[Book] b on p.BookId = b.Id
left join [Idioma].[Language] l on b.LanguageId = l.Id
left join [Idioma].[LanguageUser] lu on l.Id = lu.LanguageId
left join [Idioma].[Paragraph] pp on p.Id = pp.PageId
left join [Idioma].[Sentence] s on pp.Id = s.ParagraphId
left join [Idioma].[Token] t on s.Id = t.SentenceId
left join [Idioma].[Word] w on t.WordId = w.Id
--left join [Idioma].[BookUser] bu on b.Id = bu.BookId and lu.Id = bu.LanguageUserId
--left join [Idioma].[PageUser] pu on bu.Id =  pu.BookUserId and p.Id = pu.PageId
left join [Idioma].[WordUser] wu on w.Id = wu.WordId and wu.LanguageUserId = lu.Id
where 1=1
and p.BookId = 6
--and lu.UserId = 3216
--and p.Ordinal = 2
--and pu.Id is not null
and lu.Id = 1
and p.Id = 66
--and w.Id = 39
--and pp.Id = 9114
--and s.Id = 14187
--group by b.Id, wu.Id
order by p.Ordinal, pp.Ordinal, s.Ordinal, t.Ordinal

--select * from [Idioma].Book where Id = 1944
--select * from [Idioma].[vw_BookListRow] where BookId = 1234 or UserId = 1948
--select 
--b.Title,p.Ordinal,p.OriginalText
--,
--delete
--pu
--from [Idioma].[Page] p
--left join [Idioma].[Book] b on p.BookId = b.Id
--left join [Idioma].[Language] l on b.LanguageId = l.Id
--left join [Idioma].[LanguageUser] lu on l.Id = lu.LanguageId
--left join [Idioma].[BookUser] bu on b.Id = bu.BookId and lu.Id = bu.LanguageUserId
--left join [Idioma].[PageUser] pu on bu.Id =  pu.BookUserId and p.Id = pu.PageId
--where b.Id = 10
--and bu.LanguageUserId = 1
--order by p.Ordinal


--update [Idioma].[BookUser] set CurrentPageID = 275 where Id = 10