--alter table [Idioma].[oldBook] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldBookStat] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldBookUser] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldBookUserStat] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldFlashCard] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldFlashCardAttempt] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldFlashCardParagraphTranslationBridge] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldLanguage] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldLanguageCode] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldLanguageUser] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldPage] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldPageUser] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldParagraph] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldParagraphTranslation] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldSentence] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldToken] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldUser] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldUserBreadCrumb] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldUserSetting] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldWord] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldWordUser] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldWordTranslation] add [new_id] uniqueidentifier;
--alter table [Idioma].[oldWord] add [new_language_id] uniqueidentifier;

--spanish 'D87BE460-344B-4442-895C-597143CB9270'
--english '0CEAAAEA-5D56-4E99-B388-EAD7ED94A9E4'
-- Dan 'E8CA7570-4B48-4AF1-9A7B-FD9DA4DB735F'
-- Dan LU key 83F46697-FFD6-4A1F-B981-05C88590315A

/**************************************************************************************************************
language
**************************************************************************************************************/
--update [Idioma].[oldLanguage] set new_id = 'D87BE460-344B-4442-895C-597143CB9270' where Id = 1; 
--update [Idioma].[oldLanguage] set new_id = '0CEAAAEA-5D56-4E99-B388-EAD7ED94A9E4' where Id = 2;

/**************************************************************************************************************
word
**************************************************************************************************************/
--update [Idioma].[oldWord] set [new_language_id] = 'D87BE460-344B-4442-895C-597143CB9270' where LanguageId = 1;
--update [Idioma].[oldWord] set [new_language_id] = '0CEAAAEA-5D56-4E99-B388-EAD7ED94A9E4' where LanguageId = 2;

--select 
--top 100 
--oldTable.*
--, newTable.*
--from [Idioma].[oldWord] oldTable
--left join [Idioma].[Word] newTable
--on oldTable.TextLowerCase = newTable.TextLowerCase and oldTable.new_language_id = newTable.LanguageId
----where newTable.Id is not null;

--update oldTable
--set new_id = newTable.Id
--from [Idioma].[oldWord] oldTable
--left join [Idioma].[Word] newTable
--on oldTable.TextLowerCase = newTable.TextLowerCase and oldTable.new_language_id = newTable.LanguageId
--where newTable.Id is not null;


--INSERT INTO [Idioma].[Word]
--           ([Id]
--           ,[LanguageId]
--           ,[TextLowerCase]
--           ,[Text]
--           ,[Romanization])
--select 
--		NEWID(),
--		oldTable.new_language_id,
--		oldTable.TextLowerCase,
--		oldTable.Text,
--		oldTable.Romanization
--from [Idioma].[oldWord] oldTable
--left join [Idioma].[Word] newTable
--on oldTable.TextLowerCase = newTable.TextLowerCase and oldTable.new_language_id = newTable.LanguageId
--where newTable.Id is null



/**************************************************************************************************************
user
**************************************************************************************************************/


--INSERT INTO [Idioma].[User]
--           ([Id]
--           ,[ApplicationUserId]
--           ,[Name])
--SELECT  UniqueKey
--      ,[ApplicationUserId]
--      ,[Name]
--  FROM [Idioma].[oldUser]


/**************************************************************************************************************
usersetting
**************************************************************************************************************/


--select * from [Idioma].[UserSetting]
--insert into [Idioma].[UserSetting] ([Key], UserId, [Value]) values (1, 'E8CA7570-4B48-4AF1-9A7B-FD9DA4DB735F', '0ceaaaea-5d56-4e99-b388-ead7ed94a9e4');
--insert into [Idioma].[UserSetting] ([Key], UserId, [Value]) values (2, 'E8CA7570-4B48-4AF1-9A7B-FD9DA4DB735F', 'd87be460-344b-4442-895c-597143cb9270');



/**************************************************************************************************************
languageUser
**************************************************************************************************************/



--INSERT INTO [Idioma].[LanguageUser]
--           ([Id]
--           ,[LanguageId]
--           ,[UserId]
--           ,[TotalWordsRead])
--     VALUES
--           ('83F46697-FFD6-4A1F-B981-05C88590315A'
--           ,'D87BE460-344B-4442-895C-597143CB9270'
--           ,'E8CA7570-4B48-4AF1-9A7B-FD9DA4DB735F'
--           ,0)
--GO
--spanish 'D87BE460-344B-4442-895C-597143CB9270'
--english '0CEAAAEA-5D56-4E99-B388-EAD7ED94A9E4'
-- Dan 'E8CA7570-4B48-4AF1-9A7B-FD9DA4DB735F'
-- Dan LU key 83F46697-FFD6-4A1F-B981-05C88590315A





/**************************************************************************************************************
wordUser
**************************************************************************************************************/


--alter table [Idioma].[oldWordUser] add [new_word_id] uniqueidentifier;



--INSERT INTO [Idioma].[WordUser]
--           ([Id]
--           ,[WordId]
--           ,[LanguageUserId]
--           ,[Status]
--           ,[Created]
--           ,[StatusChanged]
--           ,[Translation])
--select --top 100
--	newId(),
--	ow.new_id,
--	'83F46697-FFD6-4A1F-B981-05C88590315A',
--	owu.[Status],
--	owu.Created,
--	owu.StatusChanged,
--	owu.Translation
--from [Idioma].[oldWordUser] owu
--Left join [Idioma].[oldWord] ow on owu.WordId = ow.Id


--update owu 
--set owu.new_id = wu.Id
----select top 10 
----ow.Id as OldWordId,
----w.Id as NewWordId,
----ow.LanguageId as OldWordLanguageId,
----w.LanguageId as NewWordLanguageId,
----ow.TextLowerCase as OldWordText,
----w.TextLowerCase as NewWordText,
----owu.Id as OldWordUserId,
----wu.Id as NewWordUserId,
----owu.LanguageUserId as OldLanguageUserId,
----wu.LanguageUserId as NewLanguageUserId

--from [Idioma].[oldWord] ow
--left join [Idioma].[Word] w on ow.new_id = w.Id
--left join [Idioma].[oldWordUser] owu on ow.Id = owu.WordId
--left join [Idioma].[WordUser] wu on w.Id = wu.WordId
--where 1=1
--and owu.LanguageUserId = 1
--and wu.LanguageUserId = '83F46697-FFD6-4A1F-B981-05C88590315A'
----and w.TextLowerCase = 'de'
----where ow.TextLowerCase <> w.TextLowerCase


--select count(*) from [Idioma].[oldWordUser] where LanguageUserId = 1; -- 15930

/**************************************************************************************************************
book
**************************************************************************************************************/


--select * from [Idioma].[oldBook]
--where LanguageId = 1
--and Id not in (11, 13, 39, 36, 40);

--update [Idioma].[oldBook] set new_id = NEWID();


--INSERT INTO [Idioma].[Book]
--           ([Id]
--           ,[LanguageId]
--           ,[Title]
--           ,[SourceURI])

--select 
--	new_id,
--	'D87BE460-344B-4442-895C-597143CB9270',
--	Title,
--	SourceURI
--from [Idioma].[oldBook]
--where LanguageId = 1
--and Id not in (11, 13, 39, 36, 40);



/**************************************************************************************************************
bookstat
**************************************************************************************************************/

--select * 
--from [Idioma].[oldBookStat] bs
--left join [Idioma].[oldBook] b on bs.BookId = b.Id
--where BookId not in (11, 13, 39, 36, 40);



--INSERT INTO [Idioma].[BookStat]
--           ([BookId]
--           ,[Key]
--           ,[Value])

--select 
--	b.new_id,
--	bs.[Key],
--	bs.[Value]
--from [Idioma].[oldBookStat] bs
--left join [Idioma].[oldBook] b on bs.BookId = b.Id
--left join Idioma.Book newB on b.new_id = newB.Id
--where b.Id not in (11, 13, 39, 36, 40)
--and b.LanguageId = 1;
    

/**************************************************************************************************************
page
**************************************************************************************************************/

--select
--op.*
--from [Idioma].[oldPage] op
--left join [Idioma].[oldBook] ob on op.BookId = ob.Id
--where ob.Id not in (11, 13, 39, 36, 40)
--and ob.LanguageId = 1;

--update [Idioma].[oldPage] set new_id = NEWID();



--INSERT INTO [Idioma].[Page]
--           ([Id]
--           ,[BookId]
--           ,[Ordinal]
--           ,[OriginalText])
--select
--	op.new_id,
--	ob.new_id,
--	Ordinal,
--	OriginalText
--from [Idioma].[oldPage] op
--left join [Idioma].[oldBook] ob on op.BookId = ob.Id
--where ob.Id not in (11, 13, 39, 36, 40)
--and ob.LanguageId = 1;


/**************************************************************************************************************
paragraph
**************************************************************************************************************/

--select 
--opp.*
--from [Idioma].[oldParagraph] opp
--left join [Idioma].[oldPage] op on opp.PageId = op.Id
--left join [Idioma].[oldBook] ob on op.BookId = ob.Id
--where ob.Id not in (11, 13, 39, 36, 40)
--and ob.LanguageId = 1;

----update [Idioma].[oldParagraph] set new_id = NEWID()


--INSERT INTO [Idioma].[Paragraph]
--           ([Id]
--           ,[PageId]
--           ,[Ordinal])


--select 
--	opp.new_id,
--	op.new_id,
--	opp.Ordinal
--from [Idioma].[oldParagraph] opp
--left join [Idioma].[oldPage] op on opp.PageId = op.Id
--left join [Idioma].[oldBook] ob on op.BookId = ob.Id
--where ob.Id not in (11, 13, 39, 36, 40)
--and ob.LanguageId = 1;

/**************************************************************************************************************
sentence
**************************************************************************************************************/

--update Idioma.oldSentence set new_id = NEWID();

--INSERT INTO [Idioma].[Sentence]
--           ([Id]
--           ,[ParagraphId]
--           ,[Ordinal]
--           ,[Text])


--select 
--	os.new_id,
--	opp.new_id,
--	os.Ordinal,
--	os.Text
--from [Idioma].[oldSentence] os
--left join [Idioma].[oldParagraph] opp on os.ParagraphId = opp.Id
--left join [Idioma].[oldPage] op on opp.PageId = op.Id
--left join [Idioma].[oldBook] ob on op.BookId = ob.Id
--where ob.Id not in (11, 13, 39, 36, 40)
--and ob.LanguageId = 1;

--13,569



/**************************************************************************************************************
token
**************************************************************************************************************/

