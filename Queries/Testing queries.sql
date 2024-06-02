select
	lu.Id as LanguageUserId
	, u.Id as UserId
	, bu.Id as BookUserId
	, Title
	, bu.CurrentPageID
	, p.Id as PageId
	, p.Ordinal as PageOrdinal
	, pu.ReadDate
from [Idioma].[Book] b
left join [Idioma].[BookUser] bu on bu.BookId = b.Id
left join [Idioma].[LanguageUser] lu on bu.LanguageUserId = lu.Id
left join [Idioma].[User] u on lu.UserId = u.Id
left join [Idioma].[Page] p on p.BookId = b.Id
left join [Idioma].[PageUser] pu on pu.BookUserId = bu.Id and pu.PageId = p.Id
where 1=1 
--and u.Id = 111
--and b.Id = 7
and bu.Id = 233


select
	*
from  [Idioma].[BookUser] 

select * from [Idioma].[LanguageUser]

select * from [Idioma].[WordUser] where LanguageUserId = 14


select * 
from [Idioma].[WordUser] wu 
left join [Idioma].[Word] w on wu.WordId = w.Id
where 1=1
and w.TextLowerCase = 'de'
--and w.Id = 11
and [LanguageUserId] = 96


-- word users in page
select
wu.*
from [Idioma].[Page] p
left join [Idioma].[Paragraph] pp on pp.PageId = p.Id
left join [Idioma].[Sentence] s on pp.Id = s.ParagraphId
left join [Idioma].[Token] t on s.Id = t.SentenceId
left join [Idioma].[Word] w on w.Id = t.WordId
left join [Idioma].[WordUser] wu on w.Id = wu.WordId
left join [Idioma].[LanguageUser] lu on wu.LanguageUserId = lu.Id
where p.Id = 79
and lu.UserId = 101