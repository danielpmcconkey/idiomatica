select
	b.Title
	, bu.CurrentPageID
	, p.Id as PageId
	, p.Ordinal as PageOrdinal
	, pu.ReadDate
from [Idioma].[Book] b
left join [Idioma].[BookUser] bu on bu.BookId = b.Id
left join [Idioma].[Page] p on p.BookId = b.Id
left join [Idioma].[PageUser] pu on pu.BookUserId = bu.Id and pu.PageId = p.Id
where b.Id = 6