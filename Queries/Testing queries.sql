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
--and u.Id = 15
and b.Id = 7


select
	*
from  [Idioma].[BookUser] 

select * from [Idioma].[LanguageUser]

select * from [Idioma].[WordUser] where LanguageUserId = 14


14,15,118