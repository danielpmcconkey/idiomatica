select
	b.Id as BookId
	,b.Title
	, p.Id as PageId
	,p.Ordinal as PageOrd
	, p.OriginalText as PageText
	, bu.Id as BookUserId
	, bu.CurrentPageID
	, pu.Id as PageUserId
	, pu.ReadDate
	, pp.Ordinal as PPOrd
	, s.Text as SentenceText
	, t.Id as TokenId
	, t.Display
	, w.TextLowerCase
	--,count(*)
from [Idioma].[Page] p
left join [Idioma].[Book] b on p.BookId = b.Id
left join [Idioma].[Language] l on b.LanguageId = l.Id
left join [Idioma].[LanguageUser] lu on l.Id = lu.LanguageId
left join [Idioma].[Paragraph] pp on p.Id = pp.PageId
left join [Idioma].[Sentence] s on pp.Id = s.ParagraphId
left join [Idioma].[Token] t on s.Id = t.SentenceId
left join [Idioma].[Word] w on t.WordId = w.Id
left join [Idioma].[BookUser] bu on b.Id = bu.BookId and lu.Id = bu.LanguageUserId
left join [Idioma].[PageUser] pu on bu.Id =  pu.BookUserId and p.Id = pu.PageId
left join [Idioma].[WordUser] wu on w.Id = wu.WordId and wu.LanguageUserId = lu.Id
where p.BookId = 2
and lu.UserId = 1
--and p.Ordinal = 2
--group by b.Id, p.Ordinal, w.Id
--order by p.Ordinal, pp.Ordinal --, s.Ordinal, t.Ordinal

select 
b.Title,p.Ordinal,p.OriginalText
,
delete
pu
from [Idioma].[Page] p
left join [Idioma].[Book] b on p.BookId = b.Id
left join [Idioma].[Language] l on b.LanguageId = l.Id
left join [Idioma].[LanguageUser] lu on l.Id = lu.LanguageId
left join [Idioma].[BookUser] bu on b.Id = bu.BookId and lu.Id = bu.LanguageUserId
left join [Idioma].[PageUser] pu on bu.Id =  pu.BookUserId and p.Id = pu.PageId
where b.Id = 10
and bu.LanguageUserId = 1
order by p.Ordinal


update [Idioma].[BookUser] set CurrentPageID = 275 where Id = 10