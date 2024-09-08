-- remove FK relationships
ALTER TABLE [Idioma].[Book] DROP CONSTRAINT [FK_Book_Language_LanguageId];
ALTER TABLE [Idioma].[BookStat] DROP CONSTRAINT [FK_BookStat_Book_BookId];
ALTER TABLE [Idioma].[BookTag] DROP CONSTRAINT [FK_BookTag_Book_BookId];
ALTER TABLE [Idioma].[BookTag] DROP CONSTRAINT [FK_BookTag_User_UserId];
ALTER TABLE [Idioma].[BookUser] DROP CONSTRAINT [FK_BookUser_Book_BookId];
ALTER TABLE [Idioma].[BookUser] DROP CONSTRAINT [FK_BookUser_LanguageUser_LanguageUserId];
ALTER TABLE [Idioma].[BookUserStat] DROP CONSTRAINT [FK_BookUserStat_Book_BookId];
ALTER TABLE [Idioma].[BookUserStat] DROP CONSTRAINT [FK_BookUserStat_LanguageUser_LanguageUserId];
ALTER TABLE [Idioma].[FlashCard] DROP CONSTRAINT [FK_FlashCard_WordUser_WordUserId];
ALTER TABLE [Idioma].[FlashCardAttempt] DROP CONSTRAINT [FK_FlashCardAttempt_FlashCard_FlashCardId];
ALTER TABLE [Idioma].[FlashCardParagraphTranslationBridge] DROP CONSTRAINT [FK_FlashCardParagraphTranslationBridge_FlashCard_FlashCardId];
ALTER TABLE [Idioma].[FlashCardParagraphTranslationBridge] DROP CONSTRAINT [FK_FlashCardParagraphTranslationBridge_ParagraphTranslation_ParagraphTranslationId];
ALTER TABLE [Idioma].[Language] DROP CONSTRAINT [FK_Language_LanguageCode_LanguageCode];
ALTER TABLE [Idioma].[LanguageUser] DROP CONSTRAINT [FK_LanguageUser_Language_LanguageId];
ALTER TABLE [Idioma].[LanguageUser] DROP CONSTRAINT [FK_LanguageUser_User_UserId];
ALTER TABLE [Idioma].[Page] DROP CONSTRAINT [FK_Page_Book_BookId];
ALTER TABLE [Idioma].[PageUser] DROP CONSTRAINT [FK_PageUser_BookUser_BookUserId];
ALTER TABLE [Idioma].[PageUser] DROP CONSTRAINT [FK_PageUser_Page_PageId];
ALTER TABLE [Idioma].[Paragraph] DROP CONSTRAINT [FK_Paragraph_Page_PageId];
ALTER TABLE [Idioma].[ParagraphTranslation] DROP CONSTRAINT [FK_ParagraphTranslation_LanguageCode_LanguageCode];
ALTER TABLE [Idioma].[ParagraphTranslation] DROP CONSTRAINT [FK_ParagraphTranslation_Paragraph_ParagraphId];
ALTER TABLE [Idioma].[Sentence] DROP CONSTRAINT [FK_Sentence_Paragraph_ParagraphId];
ALTER TABLE [Idioma].[Token] DROP CONSTRAINT [FK_Token_Sentence_SentenceId];
ALTER TABLE [Idioma].[Token] DROP CONSTRAINT [FK_Token_Word_WordId];
ALTER TABLE [Idioma].[User] DROP CONSTRAINT [FK_User_LanguageCode_LanguageCode];
ALTER TABLE [Idioma].[UserBreadCrumb] DROP CONSTRAINT [FK_UserBreadCrumb_Page_PageId];
ALTER TABLE [Idioma].[UserBreadCrumb] DROP CONSTRAINT [FK_UserBreadCrumb_User_UserId];
ALTER TABLE [Idioma].[UserSetting] DROP CONSTRAINT [FK_UserSetting_User_UserId];
ALTER TABLE [Idioma].[Verb] DROP CONSTRAINT [FK_Verb_Language_LanguageId];
ALTER TABLE [Idioma].[Word] DROP CONSTRAINT [FK_Word_Language_LanguageId];
ALTER TABLE [Idioma].[WordRank] DROP CONSTRAINT [FK_WordRank_Language_LanguageId];
ALTER TABLE [Idioma].[WordRank] DROP CONSTRAINT [FK_WordRank_Word_WordId];
ALTER TABLE [Idioma].[WordTranslation] DROP CONSTRAINT [FK_WordTranslation_Language_LanguageToId];
ALTER TABLE [Idioma].[WordTranslation] DROP CONSTRAINT [FK_WordTranslation_Verb_VerbKey];
ALTER TABLE [Idioma].[WordTranslation] DROP CONSTRAINT [FK_WordTranslation_Word_WordId];
ALTER TABLE [Idioma].[WordUser] DROP CONSTRAINT [FK_WordUser_LanguageUser_LanguageUserId];
ALTER TABLE [Idioma].[WordUser] DROP CONSTRAINT [FK_WordUser_Word_WordId];


-- drop primary keys
ALTER TABLE [Idioma].[PageUser] DROP CONSTRAINT [PK_PageUser] ;
ALTER TABLE [Idioma].[Sentence] DROP CONSTRAINT [PK_Sentence] ;
ALTER TABLE [Idioma].[Token] DROP CONSTRAINT [PK_Token] ;
ALTER TABLE [Idioma].[BookTag] DROP CONSTRAINT [PK_BookTag_Id] ;
ALTER TABLE [Idioma].[FlashCardAttempt] DROP CONSTRAINT [PK_FlashCardAttempt_Id] ;
ALTER TABLE [Idioma].[FlashCard] DROP CONSTRAINT [PK_FlashCard_Id] ;
ALTER TABLE [Idioma].[FlashCardParagraphTranslationBridge] DROP CONSTRAINT [PK_FlashCardParagraphTranslationBridge_Id] ;
ALTER TABLE [Idioma].[ParagraphTranslation] DROP CONSTRAINT [PK_ParagraphTranslation_Id] ;
ALTER TABLE [Idioma].[Language] DROP CONSTRAINT [PK_Language] ;
ALTER TABLE [Idioma].[User] DROP CONSTRAINT [PK_User] ;
ALTER TABLE [Idioma].[Book] DROP CONSTRAINT [PK_Book] ;
ALTER TABLE [Idioma].[Word] DROP CONSTRAINT [PK_Word] ;
ALTER TABLE [Idioma].[LanguageUser] DROP CONSTRAINT [PK_LanguageUser] ;
ALTER TABLE [Idioma].[UserSetting] DROP CONSTRAINT [PK_UserSetting] ;
ALTER TABLE [Idioma].[BookStat] DROP CONSTRAINT [PK_BookStat] ;
ALTER TABLE [Idioma].[Page] DROP CONSTRAINT [PK_Page] ;
ALTER TABLE [Idioma].[BookUser] DROP CONSTRAINT [PK_BookUser] ;
ALTER TABLE [Idioma].[WordUser] DROP CONSTRAINT [PK_WordUser] ;
ALTER TABLE [Idioma].[BookUserStat] DROP CONSTRAINT [PK_BookUserStat] ;
ALTER TABLE [Idioma].[Paragraph] DROP CONSTRAINT [PK_Paragraph] ;
GO
-- add guids to tables that don't already have one (that also need one)
ALTER TABLE [Idioma].[Language] ADD UniqueKey uniqueidentifier NULL;
GO

-- update tables with new guid fields to have new values
update [Idioma].[Language] set UniqueKey = NEWID();

GO
-- add guid relationship fields
ALTER TABLE [Idioma].BookStat ADD BookKey uniqueidentifier NULL;
ALTER TABLE [Idioma].BookTag ADD BookKey uniqueidentifier NULL;
ALTER TABLE [Idioma].BookUser ADD BookKey uniqueidentifier NULL;
ALTER TABLE [Idioma].BookUserStat ADD BookKey uniqueidentifier NULL;
ALTER TABLE [Idioma].Page ADD BookKey uniqueidentifier NULL;
ALTER TABLE [Idioma].PageUser ADD BookUserKey uniqueidentifier NULL;
ALTER TABLE [Idioma].FlashCardAttempt ADD FlashCardKey uniqueidentifier NULL;
ALTER TABLE [Idioma].FlashCardParagraphTranslationBridge ADD FlashCardKey uniqueidentifier NULL;
ALTER TABLE [Idioma].Book ADD LanguageKey uniqueidentifier NULL;
ALTER TABLE [Idioma].LanguageUser ADD LanguageKey uniqueidentifier NULL;
ALTER TABLE [Idioma].Verb ADD LanguageKey uniqueidentifier NULL;
ALTER TABLE [Idioma].Word ADD LanguageKey uniqueidentifier NULL;
--ALTER TABLE [Idioma].Language ADD LanguageCodeKey uniqueidentifier NULL;
--ALTER TABLE [Idioma].ParagraphTranslation ADD LanguageCodeKey uniqueidentifier NULL;
--ALTER TABLE [Idioma].WordTranslation ADD LanguageToKey uniqueidentifier NULL;
ALTER TABLE [Idioma].BookUser ADD LanguageUserKey uniqueidentifier NULL;
ALTER TABLE [Idioma].BookUser ADD CurrentPageKey uniqueidentifier NULL;
ALTER TABLE [Idioma].BookUserStat ADD LanguageUserKey uniqueidentifier NULL;
ALTER TABLE [Idioma].WordUser ADD LanguageUserKey uniqueidentifier NULL;
ALTER TABLE [Idioma].PageUser ADD PageKey uniqueidentifier NULL;
ALTER TABLE [Idioma].Paragraph ADD PageKey uniqueidentifier NULL;
ALTER TABLE [Idioma].UserBreadCrumb ADD PageKey uniqueidentifier NULL;
ALTER TABLE [Idioma].ParagraphTranslation ADD ParagraphKey uniqueidentifier NULL;
ALTER TABLE [Idioma].Sentence ADD ParagraphKey uniqueidentifier NULL;
ALTER TABLE [Idioma].FlashCardParagraphTranslationBridge ADD ParagraphTranslationKey uniqueidentifier NULL;
ALTER TABLE [Idioma].Token ADD SentenceKey uniqueidentifier NULL;
ALTER TABLE [Idioma].BookTag ADD UserKey uniqueidentifier NULL;
ALTER TABLE [Idioma].LanguageUser ADD UserKey uniqueidentifier NULL;
ALTER TABLE [Idioma].UserBreadCrumb ADD UserKey uniqueidentifier NULL;
ALTER TABLE [Idioma].UserSetting ADD UserKey uniqueidentifier NULL;
ALTER TABLE [Idioma].WordTranslation ADD VerbKey uniqueidentifier NULL;
ALTER TABLE [Idioma].Token ADD WordKey uniqueidentifier NULL;
--ALTER TABLE [Idioma].WordTranslation ADD WordKey uniqueidentifier NULL;
ALTER TABLE [Idioma].WordUser ADD WordKey uniqueidentifier NULL;
ALTER TABLE [Idioma].FlashCard ADD WordUserKey uniqueidentifier NULL;


--ALTER TABLE [Idioma].Language drop column LanguageCodeKey ;
--ALTER TABLE [Idioma].ParagraphTranslation drop column LanguageCodeKey;

GO

-- recreate relationships using GUIDs instead

	UPDATE [BookStat] SET [BookStat].[BookKey] = [Book].UniqueKey FROM [Idioma].[BookStat] AS [BookStat] INNER JOIN [Idioma].[Book] AS [Book] ON [BookStat].[BookId] = [Book].Id;
	UPDATE [BookTag] SET [BookTag].[BookKey] = [Book].UniqueKey FROM [Idioma].[BookTag] AS [BookTag] INNER JOIN [Idioma].[Book] AS [Book] ON [BookTag].[BookId] = [Book].Id;
	UPDATE [BookUser] SET [BookUser].[BookKey] = [Book].UniqueKey FROM [Idioma].[BookUser] AS [BookUser] INNER JOIN [Idioma].[Book] AS [Book] ON [BookUser].[BookId] = [Book].Id;
	UPDATE [BookUserStat] SET [BookUserStat].[BookKey] = [Book].UniqueKey FROM [Idioma].[BookUserStat] AS [BookUserStat] INNER JOIN [Idioma].[Book] AS [Book] ON [BookUserStat].[BookId] = [Book].Id;
	UPDATE [Page] SET [Page].[BookKey] = [Book].UniqueKey FROM [Idioma].[Page] AS [Page] INNER JOIN [Idioma].[Book] AS [Book] ON [Page].[BookId] = [Book].Id;
	UPDATE [PageUser] SET [PageUser].[BookUserKey] = [BookUser].UniqueKey FROM [Idioma].[PageUser] AS [PageUser] INNER JOIN [Idioma].[BookUser] AS [BookUser] ON [PageUser].[BookUserId] = [BookUser].Id;
	UPDATE [FlashCardAttempt] SET [FlashCardAttempt].[FlashCardKey] = [FlashCard].UniqueKey FROM [Idioma].[FlashCardAttempt] AS [FlashCardAttempt] INNER JOIN [Idioma].[FlashCard] AS [FlashCard] ON [FlashCardAttempt].[FlashCardId] = [FlashCard].Id;
	UPDATE [FlashCardParagraphTranslationBridge] SET [FlashCardParagraphTranslationBridge].[FlashCardKey] = [FlashCard].UniqueKey FROM [Idioma].[FlashCardParagraphTranslationBridge] AS [FlashCardParagraphTranslationBridge] INNER JOIN [Idioma].[FlashCard] AS [FlashCard] ON [FlashCardParagraphTranslationBridge].[FlashCardId] = [FlashCard].Id;
	UPDATE [Book] SET [Book].[LanguageKey] = [Language].UniqueKey FROM [Idioma].[Book] AS [Book] INNER JOIN [Idioma].[Language] AS [Language] ON [Book].[LanguageId] = [Language].Id;
	UPDATE [LanguageUser] SET [LanguageUser].[LanguageKey] = [Language].UniqueKey FROM [Idioma].[LanguageUser] AS [LanguageUser] INNER JOIN [Idioma].[Language] AS [Language] ON [LanguageUser].[LanguageId] = [Language].Id;
	UPDATE [Verb] SET [Verb].[LanguageKey] = [Language].UniqueKey FROM [Idioma].[Verb] AS [Verb] INNER JOIN [Idioma].[Language] AS [Language] ON [Verb].[LanguageId] = [Language].Id;
	UPDATE [Word] SET [Word].[LanguageKey] = [Language].UniqueKey FROM [Idioma].[Word] AS [Word] INNER JOIN [Idioma].[Language] AS [Language] ON [Word].[LanguageId] = [Language].Id;
--	UPDATE [Language] SET [Language].[LanguageCodeKey] = [LanguageCode].UniqueKey FROM [Idioma].[Language] AS [Language] INNER JOIN [Idioma].[LanguageCode] AS [LanguageCode] ON [Language].[LanguageCodeId] = [LanguageCode].Id;
--	UPDATE [ParagraphTranslation] SET [ParagraphTranslation].[LanguageCodeKey] = [LanguageCode].UniqueKey FROM [Idioma].[ParagraphTranslation] AS [ParagraphTranslation] INNER JOIN [Idioma].[LanguageCode] AS [LanguageCode] ON [ParagraphTranslation].[LanguageCodeId] = [LanguageCode].Id;
	UPDATE [WordTranslation] SET [WordTranslation].[LanguageToKey] = [Language].UniqueKey FROM [Idioma].[WordTranslation] AS [WordTranslation] INNER JOIN [Idioma].[Language] AS [Language] ON [WordTranslation].[LanguageToId] = [Language].Id;
	UPDATE [BookUser] SET [BookUser].[LanguageUserKey] = [LanguageUser].UniqueKey FROM [Idioma].[BookUser] AS [BookUser] INNER JOIN [Idioma].[LanguageUser] AS [LanguageUser] ON [BookUser].[LanguageUserId] = [LanguageUser].Id;
	UPDATE [BookUser] SET [BookUser].[CurrentPageKey] = [Page].UniqueKey FROM [Idioma].[BookUser] AS [BookUser] INNER JOIN [Idioma].[Page] AS [Page] ON [BookUser].[CurrentPageID] = [Page].Id;
	UPDATE [BookUserStat] SET [BookUserStat].[LanguageUserKey] = [LanguageUser].UniqueKey FROM [Idioma].[BookUserStat] AS [BookUserStat] INNER JOIN [Idioma].[LanguageUser] AS [LanguageUser] ON [BookUserStat].[LanguageUserId] = [LanguageUser].Id;
	UPDATE [WordUser] SET [WordUser].[LanguageUserKey] = [LanguageUser].UniqueKey FROM [Idioma].[WordUser] AS [WordUser] INNER JOIN [Idioma].[LanguageUser] AS [LanguageUser] ON [WordUser].[LanguageUserId] = [LanguageUser].Id;
	UPDATE [PageUser] SET [PageUser].[PageKey] = [Page].UniqueKey FROM [Idioma].[PageUser] AS [PageUser] INNER JOIN [Idioma].[Page] AS [Page] ON [PageUser].[PageId] = [Page].Id;
	UPDATE [Paragraph] SET [Paragraph].[PageKey] = [Page].UniqueKey FROM [Idioma].[Paragraph] AS [Paragraph] INNER JOIN [Idioma].[Page] AS [Page] ON [Paragraph].[PageId] = [Page].Id;
	UPDATE [UserBreadCrumb] SET [UserBreadCrumb].[PageKey] = [Page].UniqueKey FROM [Idioma].[UserBreadCrumb] AS [UserBreadCrumb] INNER JOIN [Idioma].[Page] AS [Page] ON [UserBreadCrumb].[PageId] = [Page].Id;
	UPDATE [ParagraphTranslation] SET [ParagraphTranslation].[ParagraphKey] = [Paragraph].UniqueKey FROM [Idioma].[ParagraphTranslation] AS [ParagraphTranslation] INNER JOIN [Idioma].[Paragraph] AS [Paragraph] ON [ParagraphTranslation].[ParagraphId] = [Paragraph].Id;
	UPDATE [Sentence] SET [Sentence].[ParagraphKey] = [Paragraph].UniqueKey FROM [Idioma].[Sentence] AS [Sentence] INNER JOIN [Idioma].[Paragraph] AS [Paragraph] ON [Sentence].[ParagraphId] = [Paragraph].Id;
	UPDATE [FlashCardParagraphTranslationBridge] SET [FlashCardParagraphTranslationBridge].[ParagraphTranslationKey] = [ParagraphTranslation].UniqueKey FROM [Idioma].[FlashCardParagraphTranslationBridge] AS [FlashCardParagraphTranslationBridge] INNER JOIN [Idioma].[ParagraphTranslation] AS [ParagraphTranslation] ON [FlashCardParagraphTranslationBridge].[ParagraphTranslationId] = [ParagraphTranslation].Id;
	UPDATE [Token] SET [Token].[SentenceKey] = [Sentence].UniqueKey FROM [Idioma].[Token] AS [Token] INNER JOIN [Idioma].[Sentence] AS [Sentence] ON [Token].[SentenceId] = [Sentence].Id;
	UPDATE [BookTag] SET [BookTag].[UserKey] = [User].UniqueKey FROM [Idioma].[BookTag] AS [BookTag] INNER JOIN [Idioma].[User] AS [User] ON [BookTag].[UserId] = [User].Id;
	UPDATE [LanguageUser] SET [LanguageUser].[UserKey] = [User].UniqueKey FROM [Idioma].[LanguageUser] AS [LanguageUser] INNER JOIN [Idioma].[User] AS [User] ON [LanguageUser].[UserId] = [User].Id;
	UPDATE [UserBreadCrumb] SET [UserBreadCrumb].[UserKey] = [User].UniqueKey FROM [Idioma].[UserBreadCrumb] AS [UserBreadCrumb] INNER JOIN [Idioma].[User] AS [User] ON [UserBreadCrumb].[UserId] = [User].Id;
	UPDATE [UserSetting] SET [UserSetting].[UserKey] = [User].UniqueKey FROM [Idioma].[UserSetting] AS [UserSetting] INNER JOIN [Idioma].[User] AS [User] ON [UserSetting].[UserId] = [User].Id;
--	UPDATE [WordTranslation] SET [WordTranslation].[VerbKey] = [Verb].UniqueKey FROM [Idioma].[WordTranslation] AS [WordTranslation] INNER JOIN [Idioma].[Verb] AS [Verb] ON [WordTranslation].[VerbId] = [Verb].Id;
	UPDATE [Token] SET [Token].[WordKey] = [Word].UniqueKey FROM [Idioma].[Token] AS [Token] INNER JOIN [Idioma].[Word] AS [Word] ON [Token].[WordId] = [Word].Id;
	UPDATE [WordTranslation] SET [WordTranslation].[WordKey] = [Word].UniqueKey FROM [Idioma].[WordTranslation] AS [WordTranslation] INNER JOIN [Idioma].[Word] AS [Word] ON [WordTranslation].[WordId] = [Word].Id;
	UPDATE [WordUser] SET [WordUser].[WordKey] = [Word].UniqueKey FROM [Idioma].[WordUser] AS [WordUser] INNER JOIN [Idioma].[Word] AS [Word] ON [WordUser].[WordId] = [Word].Id;
	UPDATE [FlashCard] SET [FlashCard].[WordUserKey] = [WordUser].UniqueKey FROM [Idioma].[FlashCard] AS [FlashCard] INNER JOIN [Idioma].[WordUser] AS [WordUser] ON [FlashCard].[WordUserId] = [WordUser].Id;

-- verify that join counts would be the same
select 'Book' as TableA, 'BookStat' as TableB, count(*) from [Idioma].[Book] join [Idioma].[BookStat] on [Book].[Id] = [BookStat].[BookId] union all select 'Book' as TableA, 'BookStat' as TableB, count(*) from [Idioma].[Book] join [Idioma].[BookStat] on [Book].[UniqueKey] = [BookStat].[BookKey]
select 'Book' as TableA, 'BookTag' as TableB, count(*) from [Idioma].[Book] join [Idioma].[BookTag] on [Book].[Id] = [BookTag].[BookId] union all select 'Book' as TableA, 'BookTag' as TableB, count(*) from [Idioma].[Book] join [Idioma].[BookTag] on [Book].[UniqueKey] = [BookTag].[BookKey]
select 'Book' as TableA, 'BookUser' as TableB, count(*) from [Idioma].[Book] join [Idioma].[BookUser] on [Book].[Id] = [BookUser].[BookId] union all select 'Book' as TableA, 'BookUser' as TableB, count(*) from [Idioma].[Book] join [Idioma].[BookUser] on [Book].[UniqueKey] = [BookUser].[BookKey]
select 'Book' as TableA, 'BookUserStat' as TableB, count(*) from [Idioma].[Book] join [Idioma].[BookUserStat] on [Book].[Id] = [BookUserStat].[BookId] union all select 'Book' as TableA, 'BookUserStat' as TableB, count(*) from [Idioma].[Book] join [Idioma].[BookUserStat] on [Book].[UniqueKey] = [BookUserStat].[BookKey]
select 'Book' as TableA, 'Page' as TableB, count(*) from [Idioma].[Book] join [Idioma].[Page] on [Book].[Id] = [Page].[BookId] union all select 'Book' as TableA, 'Page' as TableB, count(*) from [Idioma].[Book] join [Idioma].[Page] on [Book].[UniqueKey] = [Page].[BookKey]
select 'BookUser' as TableA, 'PageUser' as TableB, count(*) from [Idioma].[BookUser] join [Idioma].[PageUser] on [BookUser].[Id] = [PageUser].[BookUserId] union all select 'BookUser' as TableA, 'PageUser' as TableB, count(*) from [Idioma].[BookUser] join [Idioma].[PageUser] on [BookUser].[UniqueKey] = [PageUser].[BookUserKey]
select 'FlashCard' as TableA, 'FlashCardAttempt' as TableB, count(*) from [Idioma].[FlashCard] join [Idioma].[FlashCardAttempt] on [FlashCard].[Id] = [FlashCardAttempt].[FlashCardId] union all select 'FlashCard' as TableA, 'FlashCardAttempt' as TableB, count(*) from [Idioma].[FlashCard] join [Idioma].[FlashCardAttempt] on [FlashCard].[UniqueKey] = [FlashCardAttempt].[FlashCardKey]
select 'FlashCard' as TableA, 'FlashCardParagraphTranslationBridge' as TableB, count(*) from [Idioma].[FlashCard] join [Idioma].[FlashCardParagraphTranslationBridge] on [FlashCard].[Id] = [FlashCardParagraphTranslationBridge].[FlashCardId] union all select 'FlashCard' as TableA, 'FlashCardParagraphTranslationBridge' as TableB, count(*) from [Idioma].[FlashCard] join [Idioma].[FlashCardParagraphTranslationBridge] on [FlashCard].[UniqueKey] = [FlashCardParagraphTranslationBridge].[FlashCardKey]
select 'Language' as TableA, 'Book' as TableB, count(*) from [Idioma].[Language] join [Idioma].[Book] on [Language].[Id] = [Book].[LanguageId] union all select 'Language' as TableA, 'Book' as TableB, count(*) from [Idioma].[Language] join [Idioma].[Book] on [Language].[UniqueKey] = [Book].[LanguageKey]
select 'Language' as TableA, 'LanguageUser' as TableB, count(*) from [Idioma].[Language] join [Idioma].[LanguageUser] on [Language].[Id] = [LanguageUser].[LanguageId] union all select 'Language' as TableA, 'LanguageUser' as TableB, count(*) from [Idioma].[Language] join [Idioma].[LanguageUser] on [Language].[UniqueKey] = [LanguageUser].[LanguageKey]
select 'Language' as TableA, 'Verb' as TableB, count(*) from [Idioma].[Language] join [Idioma].[Verb] on [Language].[Id] = [Verb].[LanguageId] union all select 'Language' as TableA, 'Verb' as TableB, count(*) from [Idioma].[Language] join [Idioma].[Verb] on [Language].[UniqueKey] = [Verb].[LanguageKey]
select 'Language' as TableA, 'Word' as TableB, count(*) from [Idioma].[Language] join [Idioma].[Word] on [Language].[Id] = [Word].[LanguageId] union all select 'Language' as TableA, 'Word' as TableB, count(*) from [Idioma].[Language] join [Idioma].[Word] on [Language].[UniqueKey] = [Word].[LanguageKey]
select 'LanguageUser' as TableA, 'BookUser' as TableB, count(*) from [Idioma].[LanguageUser] join [Idioma].[BookUser] on [LanguageUser].[Id] = [BookUser].[LanguageUserId] union all select 'LanguageUser' as TableA, 'BookUser' as TableB, count(*) from [Idioma].[LanguageUser] join [Idioma].[BookUser] on [LanguageUser].[UniqueKey] = [BookUser].[LanguageUserKey]
select 'LanguageUser' as TableA, 'BookUserStat' as TableB, count(*) from [Idioma].[LanguageUser] join [Idioma].[BookUserStat] on [LanguageUser].[Id] = [BookUserStat].[LanguageUserId] union all select 'LanguageUser' as TableA, 'BookUserStat' as TableB, count(*) from [Idioma].[LanguageUser] join [Idioma].[BookUserStat] on [LanguageUser].[UniqueKey] = [BookUserStat].[LanguageUserKey]
select 'LanguageUser' as TableA, 'WordUser' as TableB, count(*) from [Idioma].[LanguageUser] join [Idioma].[WordUser] on [LanguageUser].[Id] = [WordUser].[LanguageUserId] union all select 'LanguageUser' as TableA, 'WordUser' as TableB, count(*) from [Idioma].[LanguageUser] join [Idioma].[WordUser] on [LanguageUser].[UniqueKey] = [WordUser].[LanguageUserKey]
select 'Page' as TableA, 'PageUser' as TableB, count(*) from [Idioma].[Page] join [Idioma].[PageUser] on [Page].[Id] = [PageUser].[PageId] union all select 'Page' as TableA, 'PageUser' as TableB, count(*) from [Idioma].[Page] join [Idioma].[PageUser] on [Page].[UniqueKey] = [PageUser].[PageKey]
select 'Page' as TableA, 'Paragraph' as TableB, count(*) from [Idioma].[Page] join [Idioma].[Paragraph] on [Page].[Id] = [Paragraph].[PageId] union all select 'Page' as TableA, 'Paragraph' as TableB, count(*) from [Idioma].[Page] join [Idioma].[Paragraph] on [Page].[UniqueKey] = [Paragraph].[PageKey]
select 'Page' as TableA, 'UserBreadCrumb' as TableB, count(*) from [Idioma].[Page] join [Idioma].[UserBreadCrumb] on [Page].[Id] = [UserBreadCrumb].[PageId] union all select 'Page' as TableA, 'UserBreadCrumb' as TableB, count(*) from [Idioma].[Page] join [Idioma].[UserBreadCrumb] on [Page].[UniqueKey] = [UserBreadCrumb].[PageKey]
select 'Paragraph' as TableA, 'ParagraphTranslation' as TableB, count(*) from [Idioma].[Paragraph] join [Idioma].[ParagraphTranslation] on [Paragraph].[Id] = [ParagraphTranslation].[ParagraphId] union all select 'Paragraph' as TableA, 'ParagraphTranslation' as TableB, count(*) from [Idioma].[Paragraph] join [Idioma].[ParagraphTranslation] on [Paragraph].[UniqueKey] = [ParagraphTranslation].[ParagraphKey]
select 'Paragraph' as TableA, 'Sentence' as TableB, count(*) from [Idioma].[Paragraph] join [Idioma].[Sentence] on [Paragraph].[Id] = [Sentence].[ParagraphId] union all select 'Paragraph' as TableA, 'Sentence' as TableB, count(*) from [Idioma].[Paragraph] join [Idioma].[Sentence] on [Paragraph].[UniqueKey] = [Sentence].[ParagraphKey]
select 'ParagraphTranslation' as TableA, 'FlashCardParagraphTranslationBridge' as TableB, count(*) from [Idioma].[ParagraphTranslation] join [Idioma].[FlashCardParagraphTranslationBridge] on [ParagraphTranslation].[Id] = [FlashCardParagraphTranslationBridge].[ParagraphTranslationId] union all select 'ParagraphTranslation' as TableA, 'FlashCardParagraphTranslationBridge' as TableB, count(*) from [Idioma].[ParagraphTranslation] join [Idioma].[FlashCardParagraphTranslationBridge] on [ParagraphTranslation].[UniqueKey] = [FlashCardParagraphTranslationBridge].[ParagraphTranslationKey]
select 'Sentence' as TableA, 'Token' as TableB, count(*) from [Idioma].[Sentence] join [Idioma].[Token] on [Sentence].[Id] = [Token].[SentenceId] union all select 'Sentence' as TableA, 'Token' as TableB, count(*) from [Idioma].[Sentence] join [Idioma].[Token] on [Sentence].[UniqueKey] = [Token].[SentenceKey]
select 'User' as TableA, 'BookTag' as TableB, count(*) from [Idioma].[User] join [Idioma].[BookTag] on [User].[Id] = [BookTag].[UserId] union all select 'User' as TableA, 'BookTag' as TableB, count(*) from [Idioma].[User] join [Idioma].[BookTag] on [User].[UniqueKey] = [BookTag].[UserKey]
select 'User' as TableA, 'LanguageUser' as TableB, count(*) from [Idioma].[User] join [Idioma].[LanguageUser] on [User].[Id] = [LanguageUser].[UserId] union all select 'User' as TableA, 'LanguageUser' as TableB, count(*) from [Idioma].[User] join [Idioma].[LanguageUser] on [User].[UniqueKey] = [LanguageUser].[UserKey]
select 'User' as TableA, 'UserBreadCrumb' as TableB, count(*) from [Idioma].[User] join [Idioma].[UserBreadCrumb] on [User].[Id] = [UserBreadCrumb].[UserId] union all select 'User' as TableA, 'UserBreadCrumb' as TableB, count(*) from [Idioma].[User] join [Idioma].[UserBreadCrumb] on [User].[UniqueKey] = [UserBreadCrumb].[UserKey]
select 'User' as TableA, 'UserSetting' as TableB, count(*) from [Idioma].[User] join [Idioma].[UserSetting] on [User].[Id] = [UserSetting].[UserId] union all select 'User' as TableA, 'UserSetting' as TableB, count(*) from [Idioma].[User] join [Idioma].[UserSetting] on [User].[UniqueKey] = [UserSetting].[UserKey]
select 'Word' as TableA, 'Token' as TableB, count(*) from [Idioma].[Word] join [Idioma].[Token] on [Word].[Id] = [Token].[WordId] union all select 'Word' as TableA, 'Token' as TableB, count(*) from [Idioma].[Word] join [Idioma].[Token] on [Word].[UniqueKey] = [Token].[WordKey]
select 'Word' as TableA, 'WordTranslation' as TableB, count(*) from [Idioma].[Word] join [Idioma].[WordTranslation] on [Word].[Id] = [WordTranslation].[WordId] union all select 'Word' as TableA, 'WordTranslation' as TableB, count(*) from [Idioma].[Word] join [Idioma].[WordTranslation] on [Word].[UniqueKey] = [WordTranslation].[WordKey]
select 'Word' as TableA, 'WordUser' as TableB, count(*) from [Idioma].[Word] join [Idioma].[WordUser] on [Word].[Id] = [WordUser].[WordId] union all select 'Word' as TableA, 'WordUser' as TableB, count(*) from [Idioma].[Word] join [Idioma].[WordUser] on [Word].[UniqueKey] = [WordUser].[WordKey]
select 'WordUser' as TableA, 'FlashCard' as TableB, count(*) from [Idioma].[WordUser] join [Idioma].[FlashCard] on [WordUser].[Id] = [FlashCard].[WordUserId] union all select 'WordUser' as TableA, 'FlashCard' as TableB, count(*) from [Idioma].[WordUser] join [Idioma].[FlashCard] on [WordUser].[UniqueKey] = [FlashCard].[WordUserKey]


-- add your PKs back
ALTER TABLE [Idioma].[Book] DROP CONSTRAINT  [UC_Book_UniqueKey];
GO
ALTER TABLE [Idioma].[Book] ALTER COLUMN [UniqueKey] uniqueidentifier not null;
GO
ALTER TABLE [Idioma].[Book] ADD CONSTRAINT PK_Book_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO


ALTER TABLE [Idioma].[BookStat] ALTER COLUMN [BookKey] uniqueidentifier not null;
GO
ALTER TABLE [Idioma].[BookStat] ADD CONSTRAINT PK_BookStat_BookKey_Key PRIMARY KEY CLUSTERED ([BookKey],[Key]);
GO

ALTER TABLE [Idioma].[BookTag] DROP CONSTRAINT  [UC_BookTag_UniqueKey];
GO
ALTER TABLE [Idioma].[BookTag] ALTER COLUMN [UniqueKey] uniqueidentifier not null;
GO
ALTER TABLE [Idioma].[BookTag] ADD CONSTRAINT PK_BookTag_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].[BookUser] DROP CONSTRAINT  [UC_BookUser_UniqueKey];
GO
ALTER TABLE [Idioma].[BookUser] ALTER COLUMN [UniqueKey] uniqueidentifier not null;
GO
ALTER TABLE [Idioma].[BookUser] ADD CONSTRAINT PK_BookUser_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO
ALTER TABLE [Idioma].[BookUser] DROP CONSTRAINT  [UK_BookUser_BookId_LanguageUserId];
GO
ALTER TABLE [Idioma].[BookUser] ADD CONSTRAINT  [UK_BookUser_BookKey_LanguageUserKey]  UNIQUE (BookKey, LanguageUserKey);
GO

ALTER TABLE [Idioma].[BookUserStat] ALTER COLUMN [BookKey] uniqueidentifier not null;
GO
ALTER TABLE [Idioma].[BookUserStat] ALTER COLUMN [LanguageUserKey] uniqueidentifier not null;
GO
ALTER TABLE [Idioma].[BookUserStat] ADD CONSTRAINT PK_BookUserStat_BookKey_LanguageUserKey_Key PRIMARY KEY CLUSTERED ([BookKey],[LanguageUserKey],[Key]);
GO

ALTER TABLE [Idioma].[FlashCard] DROP CONSTRAINT  [UC_FlashCard_UniqueKey];
GO
ALTER TABLE [Idioma].[FlashCard] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[FlashCard] ADD CONSTRAINT PK_FlashCard_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].[FlashCardAttempt] DROP CONSTRAINT  [UC_FlashCardAttempt_UniqueKey];
GO
ALTER TABLE [Idioma].[FlashCardAttempt] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[FlashCardAttempt] ADD CONSTRAINT PK_FlashCardAttempt_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].[FlashCardParagraphTranslationBridge] DROP CONSTRAINT  [UC_FlashCardParagraphTranslationBridge_UniqueKey];
GO
ALTER TABLE [Idioma].FlashCardParagraphTranslationBridge ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].FlashCardParagraphTranslationBridge ADD CONSTRAINT PK_FlashCardParagraphTranslationBridge_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].[FlashCardAttempt] DROP CONSTRAINT  [UC_FlashCardAttempt_UniqueKey];
GO
ALTER TABLE [Idioma].[FlashCardAttempt] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[FlashCardAttempt] ADD CONSTRAINT PK_FlashCardAttempt_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].Language ALTER COLUMN [LanguageCode] varchar(25) not null ;
GO
alter table [Idioma].[Language] ADD CONSTRAINT PK_Language_LanguageCode PRIMARY KEY CLUSTERED (LanguageCode);
GO

ALTER TABLE [Idioma].[LanguageUser] DROP CONSTRAINT  [UC_LanguageUser_UniqueKey];
GO
ALTER TABLE [Idioma].[LanguageUser] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[LanguageUser] ADD CONSTRAINT PK_LanguageUser_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO
ALTER TABLE [Idioma].[LanguageUser] ADD CONSTRAINT UK_LanguageUser_LanguageKey_UserKey_Key Unique ([LanguageKey],[UserKey]);
GO
ALTER TABLE [Idioma].[LanguageUser] DROP CONSTRAINT [UC_LanguageUser_LanguageId_UserId] ;
GO

ALTER TABLE [Idioma].[Page] DROP CONSTRAINT  [UC_Page_UniqueKey];
GO
ALTER TABLE [Idioma].[Page] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[Page] ADD CONSTRAINT PK_Page_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].[PageUser] DROP CONSTRAINT  [UC_PageUser_UniqueKey];
GO
ALTER TABLE [Idioma].[PageUser] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[PageUser] ADD CONSTRAINT PK_PageUser_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].[Paragraph] DROP CONSTRAINT  [UC_Paragraph_UniqueKey];
GO
ALTER TABLE [Idioma].[Paragraph] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[Paragraph] ADD CONSTRAINT PK_Paragraph_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].[ParagraphTranslation] DROP CONSTRAINT  [UC_ParagraphTranslation_UniqueKey];
GO
ALTER TABLE [Idioma].[ParagraphTranslation] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[ParagraphTranslation] ADD CONSTRAINT PK_ParagraphTranslation_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].[Sentence] DROP CONSTRAINT  [UC_Sentence_UniqueKey];
GO
ALTER TABLE [Idioma].[Sentence] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[Sentence] ADD CONSTRAINT PK_Sentence_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].[Token] DROP CONSTRAINT  [UC_Token_UniqueKey];
GO
ALTER TABLE [Idioma].[Token] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[Token] ADD CONSTRAINT PK_Token_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].[User] DROP CONSTRAINT  [UC_User_UniqueKey];
GO
ALTER TABLE [Idioma].[User] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[User] ADD CONSTRAINT PK_User_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO

ALTER TABLE [Idioma].[UserSetting] ALTER COLUMN UserKey uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[UserSetting] ADD CONSTRAINT PK_UserSetting_UniqueKey PRIMARY KEY CLUSTERED ([UserKey], [Key]);
GO

ALTER TABLE [Idioma].[Word] DROP CONSTRAINT  [UC_Word_UniqueKey];
GO
ALTER TABLE [Idioma].[Word] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[Word] ADD CONSTRAINT PK_Word_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO
ALTER TABLE [Idioma].[Word] ADD CONSTRAINT UK_Word_LanguageKey_UniqueKey UNIQUE ([TextLowerCase],[LanguageKey]);
GO

ALTER TABLE [Idioma].[WordRank] DROP CONSTRAINT [UK_Language_Ordinal] ;
GO
ALTER TABLE [Idioma].[WordRank] Add WordKey uniqueidentifier not null default newid() ;
GO
UPDATE [WordRank] SET [WordRank].[WordKey] = [Word].UniqueKey FROM [Idioma].[WordRank] AS [WordRank] INNER JOIN [Idioma].[Word] AS [Word] ON [WordRank].[WordId] = [Word].Id;
GO
ALTER TABLE [Idioma].[WordRank] Add LanguageCode varchar(25) not null default '';
GO
UPDATE [WordRank] SET [WordRank].LanguageCode = [Language].[LanguageCode] FROM [Idioma].[WordRank] AS [WordRank] INNER JOIN [Idioma].[Language] AS [Language] ON [WordRank].[LanguageId] = [Language].Id;
GO
ALTER TABLE [Idioma].[WordRank] ADD CONSTRAINT PK_WordRank_LanguageCode_Ordinal PRIMARY KEY CLUSTERED (LanguageCode, Ordinal);
GO

ALTER TABLE [Idioma].[WordUser] DROP CONSTRAINT  [UC_WordUser_UniqueKey];
GO
ALTER TABLE [Idioma].[WordUser] ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
ALTER TABLE [Idioma].[WordUser] ADD CONSTRAINT PK_WordUser_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO
ALTER TABLE [Idioma].[WordUser] DROP CONSTRAINT  [UC_WordUser_WordId_LanguageUserId]
GO
ALTER TABLE [Idioma].[WordUser] ADD CONSTRAINT [UC_WordUser_WordKey_LanguageUserKey] UNIQUE ([WordKey],[LanguageUserKey]);
GO



ALTER TABLE [Idioma].Language ALTER COLUMN [UniqueKey] uniqueidentifier not null ;
GO
alter table [Idioma].[Language] drop CONSTRAINT PK_Language_LanguageCode ;
GO
alter table [Idioma].[Language] ADD CONSTRAINT PK_Language_UniqueKey PRIMARY KEY CLUSTERED ([UniqueKey]);
GO



ALTER TABLE [Idioma].[WordRank] DROP CONSTRAINT [PK_WordRank_LanguageCode_Ordinal];
GO
ALTER TABLE [Idioma].[WordRank] Add LanguageKey uniqueidentifier not null default newid() ;
GO
ALTER TABLE [Idioma].[WordRank] DROP CONSTRAINT DF__WordRank__Langua__0FB750B3;
GO
ALTER TABLE [Idioma].[WordRank] DROP CONSTRAINT [DF__WordRank__Langua__4AD81681];
GO
ALTER TABLE [Idioma].[WordRank] DROP CONSTRAINT [DF__WordRank__WordKe__0EC32C7A];
GO
ALTER TABLE [Idioma].[WordRank] drop column LanguageCode ;
GO
ALTER TABLE [Idioma].[WordRank] add UniqueKey uniqueidentifier not null default newid();
GO
ALTER TABLE [Idioma].[WordRank] ADD CONSTRAINT PK_WordRank_UniqueKey PRIMARY KEY CLUSTERED (UniqueKey);
GO

UPDATE [WordRank] SET [WordRank].LanguageKey = [Language].UniqueKey FROM [Idioma].[WordRank] AS [WordRank] INNER JOIN [Idioma].[Language] AS [Language] ON [WordRank].[LanguageId] = [Language].Id;
GO
-- add the FK relationships now

ALTER TABLE [Idioma].[LanguageUser]  WITH CHECK ADD CONSTRAINT [FK_LanguageUser_Language] FOREIGN KEY([LanguageKey]) REFERENCES [Idioma].[Language] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[Word]  WITH CHECK ADD CONSTRAINT [FK_Word_Language] FOREIGN KEY([LanguageKey]) REFERENCES [Idioma].[Language] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[Book]  WITH CHECK ADD CONSTRAINT [FK_Book_Language] FOREIGN KEY([LanguageKey]) REFERENCES [Idioma].[Language] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[Page]  WITH CHECK ADD CONSTRAINT [FK_Page_Book] FOREIGN KEY([BookKey]) REFERENCES [Idioma].[Book] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[BookStat]  WITH CHECK ADD CONSTRAINT [FK_BookStat_Book] FOREIGN KEY([BookKey]) REFERENCES [Idioma].[Book] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[BookTag]  WITH CHECK ADD CONSTRAINT [FK_BookTag_Book] FOREIGN KEY([BookKey]) REFERENCES [Idioma].[Book] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[BookUser]  WITH CHECK ADD CONSTRAINT [FK_BookUser_LanguageUser] FOREIGN KEY([LanguageUserKey]) REFERENCES [Idioma].[LanguageUser] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[WordUser]  WITH CHECK ADD CONSTRAINT [FK_WordUser_LanguageUser] FOREIGN KEY([LanguageUserKey]) REFERENCES [Idioma].[LanguageUser] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[PageUser]  WITH CHECK ADD CONSTRAINT [FK_PageUser_BookUser] FOREIGN KEY([BookUserKey]) REFERENCES [Idioma].[BookUser] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[Paragraph]  WITH CHECK ADD CONSTRAINT [FK_Paragraph_Page] FOREIGN KEY([PageKey]) REFERENCES [Idioma].[Page] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[Sentence]  WITH CHECK ADD CONSTRAINT [FK_Sentence_Paragraph] FOREIGN KEY([ParagraphKey]) REFERENCES [Idioma].[Paragraph] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[ParagraphTranslation]  WITH CHECK ADD CONSTRAINT [FK_ParagraphTranslation_Paragraph] FOREIGN KEY([ParagraphKey]) REFERENCES [Idioma].[Paragraph] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[Token]  WITH CHECK ADD CONSTRAINT [FK_Token_Sentence] FOREIGN KEY([SentenceKey]) REFERENCES [Idioma].[Sentence] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[PageUser]  WITH CHECK ADD CONSTRAINT [FK_PageUser_Page] FOREIGN KEY([PageKey]) REFERENCES [Idioma].[Page] ([UniqueKey]) ON DELETE NO ACTION;
ALTER TABLE [Idioma].[Language]  WITH CHECK ADD CONSTRAINT [FK_Language_LanguageCode] FOREIGN KEY([LanguageCode]) REFERENCES [Idioma].[LanguageCode] ([Code]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[LanguageUser]  WITH CHECK ADD CONSTRAINT [FK_LanguageUser_User] FOREIGN KEY([UserKey]) REFERENCES [Idioma].[User] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[BookUser]  WITH CHECK ADD CONSTRAINT [FK_BookUser_Book] FOREIGN KEY([BookKey]) REFERENCES [Idioma].[Book] ([UniqueKey]) ON DELETE NO ACTION;
ALTER TABLE [Idioma].[BookUser]  WITH CHECK ADD CONSTRAINT [FK_BookUser_Page] FOREIGN KEY([CurrentPageKey]) REFERENCES [Idioma].[Page] ([UniqueKey]) ON DELETE NO ACTION;
ALTER TABLE [Idioma].[WordUser]  WITH CHECK ADD CONSTRAINT [FK_WordUser_Word] FOREIGN KEY([WordKey]) REFERENCES [Idioma].[Word] ([UniqueKey]) ON DELETE NO ACTION;
ALTER TABLE [Idioma].[BookTag]  WITH CHECK ADD CONSTRAINT [FK_BookTag_User] FOREIGN KEY([UserKey]) REFERENCES [Idioma].[User] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[BookUserStat]  WITH CHECK ADD CONSTRAINT [FK_BookUserStat_LanguageUser] FOREIGN KEY([LanguageUserKey]) REFERENCES [Idioma].[LanguageUser] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[BookUserStat]  WITH CHECK ADD CONSTRAINT [FK_BookUserStat_Book] FOREIGN KEY([BookKey]) REFERENCES [Idioma].[Book] ([UniqueKey]) ON DELETE NO ACTION;
ALTER TABLE [Idioma].[FlashCard]  WITH CHECK ADD CONSTRAINT [FK_FlashCard_WordUser] FOREIGN KEY([WordUserKey]) REFERENCES [Idioma].[WordUser] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[FlashCardAttempt]  WITH CHECK ADD CONSTRAINT [FK_FlashCardAttempt_FlashCard] FOREIGN KEY([FlashCardKey]) REFERENCES [Idioma].[FlashCard] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[FlashCardParagraphTranslationBridge]  WITH CHECK ADD CONSTRAINT [FK_FlashCardParagraphTranslationBridge_FlashCard] FOREIGN KEY([FlashCardKey]) REFERENCES [Idioma].[FlashCard] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[FlashCardParagraphTranslationBridge]  WITH CHECK ADD CONSTRAINT [FK_FlashCardParagraphTranslationBridge_ParagraphTranslation] FOREIGN KEY([ParagraphTranslationKey]) REFERENCES [Idioma].[ParagraphTranslation] ([UniqueKey]) ON DELETE NO ACTION;
ALTER TABLE [Idioma].[ParagraphTranslation]  WITH CHECK ADD CONSTRAINT [FK_ParagraphTranslation_LanguageCode] FOREIGN KEY([LanguageCode]) REFERENCES [Idioma].[LanguageCode] ([Code]) ON DELETE NO ACTION;
ALTER TABLE [Idioma].[Token]  WITH CHECK ADD CONSTRAINT [FK_Token_Word] FOREIGN KEY([WordKey]) REFERENCES [Idioma].[Word] ([UniqueKey]) ON DELETE NO ACTION;
ALTER TABLE [Idioma].[UserBreadCrumb]  WITH CHECK ADD CONSTRAINT [FK_UserBreadCrumb_User] FOREIGN KEY([UserKey]) REFERENCES [Idioma].[User] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[UserBreadCrumb]  WITH CHECK ADD CONSTRAINT [FK_UserBreadCrumb_Page] FOREIGN KEY([PageKey]) REFERENCES [Idioma].[Page] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[UserSetting]  WITH CHECK ADD CONSTRAINT [FK_UserSetting_User] FOREIGN KEY([UserKey]) REFERENCES [Idioma].[User] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[Verb]  WITH CHECK ADD CONSTRAINT [FK_Verb_Language] FOREIGN KEY([LanguageKey]) REFERENCES [Idioma].[Language] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[WordRank]  WITH CHECK ADD CONSTRAINT [FK_WordRank_Language] FOREIGN KEY([LanguageKey]) REFERENCES [Idioma].[Language] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[WordRank]  WITH CHECK ADD CONSTRAINT [FK_WordRank_Word] FOREIGN KEY([WordKey]) REFERENCES [Idioma].[Word] ([UniqueKey]) ON DELETE NO ACTION;
ALTER TABLE [Idioma].[WordTranslation]  WITH CHECK ADD CONSTRAINT [FK_WordTranslation_Language] FOREIGN KEY([LanguageToKey]) REFERENCES [Idioma].[Language] ([UniqueKey]) ON DELETE CASCADE;
ALTER TABLE [Idioma].[WordTranslation]  WITH CHECK ADD CONSTRAINT [FK_WordTranslation_Verb] FOREIGN KEY([VerbKey]) REFERENCES [Idioma].[Verb] ([UniqueKey]) ON DELETE NO ACTION;
ALTER TABLE [Idioma].[WordTranslation]  WITH CHECK ADD CONSTRAINT [FK_WordTranslation_Word] FOREIGN KEY([WordKey]) REFERENCES [Idioma].[Word] ([UniqueKey]) ON DELETE NO ACTION;















