select --distinct
	b.Id as BookId
	,b.Title
	--, p.Id as PageId
	--,p.Ordinal as PageOrd
	--, p.OriginalText as PageText
	--,l.[Name] as LanguageName
	--, lu.Id as LanguageUserId
	--, bu.Id as BookUserId
	--, bu.CurrentPageID
	--, pu.Id as PageUserId
	--, pu.ReadDate
	--, pu.Id as PageUserId
	--, pp.Id as PPId
	--, pp.Ordinal as PPOrd
	--, s.Id as SentenceId
	--, s.Text as SentenceText
	--, s.Ordinal as SentenceOrd
	--, t.Id as TokenId
	, t.Display
	--, t.Ordinal as TokenOrdinal
	--, w.Id
	, w.TextLowerCase
	--, w.Text
	--, wu.Id as WordUserId
	, wu.Translation
	, wu.Status
	, u.Name
	----,count(*)
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
left join [Idioma].[User] u on lu.UserId = u.Id
where 1=1
--and p.BookId = '7e1f1169-ae64-4fa7-86f3-b302caebcb8b'
--and bu.Id = '2924de98-6c00-4615-84bc-e9ce8609aecf'
and b.Title = 'El cofre'
--and lu.UserId ='58784a41-e1e7-4e91-808f-81a9f1e0f459'
and p.Ordinal = 1
--and pu.Id is not null
--and lu.Id = 1
--and p.Id = 378
--and w.Id = 39
--and pp.Id = 14594
--and s.Id = 24380
--and t.Id = 94322
and u.Name = 'Dan McConkey'
--group by b.Id, wu.Id
order by p.Ordinal, pp.Ordinal, s.Ordinal, t.Ordinal
--order by w.TextLowerCase

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




--select * from [Idioma].[Word] where Id = 39
--select * from [Idioma].[WordUser] where Id = 1545

--select * from [Idioma].[Word] where TextLowerCase = 'vivían'
--select * from Idioma.WordUser where WordId = 1545

--USE [IdiomaticaFresh]
--GO

--INSERT INTO [Idioma].[PageUser]
--           ([Id]
--           ,[BookUserId]
--           ,[PageId]
--           ,[ReadDate])
--     VALUES
--           (NEWID()
--           ,'2924de98-6c00-4615-84bc-e9ce8609aecf'
--           ,'79241A79-E172-4B26-86F6-534F9C30231E'
--           ,null)
--GO

