SET IDENTITY_INSERT Idiomatica.Idioma.[User] on
	INSERT INTO Idiomatica.Idioma.[User]
           (Id, [Name])
     select Id, [Name] from Idiomatica.dbo.[UserOld]
SET IDENTITY_INSERT Idiomatica.Idioma.[User] off




SET IDENTITY_INSERT Idiomatica.Idioma.[Language] on
	INSERT INTO Idiomatica.Idioma.[Language]
           (Id, 
		   [Name]
           ,[Dict1URI]
           ,[Dict2URI]
           ,[GoogleTranslateURI]
           ,[CharacterSubstitutions]
           ,[RegexpSplitSentences]
           ,[ExceptionsSplitSentences]
           ,[RegexpWordCharacters]
           ,[RemoveSpaces]
           ,[SplitEachChar]
           ,[RightToLeft]
           ,[ShowRomanization]
           ,[ParserType])
     select 
			Id, 
		   [Name]
           ,[Dict1URI]
           ,[Dict2URI]
           ,[GoogleTranslateURI]
           ,[CharacterSubstitutions]
           ,[RegexpSplitSentences]
           ,[ExceptionsSplitSentences]
           ,[RegexpWordCharacters]
           ,[RemoveSpaces]
           ,[SplitEachChar]
           ,[RightToLeft]
           ,[ShowRomanization]
           ,[ParserType]
		from Idiomatica.dbo.[Language]
SET IDENTITY_INSERT Idiomatica.Idioma.[User] off



SET IDENTITY_INSERT Idiomatica.Idioma.[LanguageUser] on
	INSERT INTO [Idioma].[LanguageUser]
           (id, [LanguageId]
           ,[UserId]
           ,[TotalWordsRead])
     select
		id, [LanguageId]
           ,[UserId]
           ,[TotalWordsRead]
		   from dbo.LanguageUser
SET IDENTITY_INSERT Idiomatica.Idioma.[LanguageUser] off
go


SET IDENTITY_INSERT Idiomatica.Idioma.Book on
INSERT INTO [Idioma].[Book]
           (Id, [LanguageId]
           ,[Title]
           ,[SourceURI]
           ,[AudioFilename])
     select 
           Id, [LanguageUserId]
           ,[Title]
           ,[SourceURI]
           ,[AudioFilename]
	 from dbo.Book
SET IDENTITY_INSERT Idiomatica.Idioma.Book off
GO


USE [Idiomatica]
GO
SET IDENTITY_INSERT Idiomatica.Idioma.[BookUser] on
INSERT INTO [Idioma].[BookUser]
           (id,[BookId]
           ,[LanguageUserId]
           ,[IsArchived]
           ,[CurrentPageID]
           ,[AudioCurrentPos]
           ,[AudioBookmarks])
     select 
           Id, Id,[LanguageUserId]
           ,[IsArchived]
           ,[CurrentPageID]
           ,[AudioCurrentPos]
           ,[AudioBookmarks]
	 from dbo.Book
SET IDENTITY_INSERT Idiomatica.Idioma.[BookUser] off
GO



USE [Idiomatica]
GO

SET IDENTITY_INSERT Idiomatica.Idioma.[Page] on
	INSERT INTO [Idioma].[Page]
           (Id, [BookId]
           ,[Ordinal]
           ,[OriginalText])
     select
           Id, [BookId]
           ,[Ordinal]
           ,[OriginalText]
	from dbo.Page
SET IDENTITY_INSERT Idiomatica.Idioma.[Page] off
GO


USE [Idiomatica]
GO
SET IDENTITY_INSERT Idiomatica.Idioma.[PageUser] on
	INSERT INTO [Idioma].[PageUser]
           (Id,[BookUserId]
           ,[PageId]
           ,[ReadDate])
     select 
		Id,[BookId]
           ,[Id]
           ,[ReadDate]
	from dbo.Page
SET IDENTITY_INSERT Idiomatica.Idioma.[PageUser] off
GO

USE [Idiomatica]
GO
SET IDENTITY_INSERT Idiomatica.Idioma.[Paragraph] on
	INSERT INTO [Idioma].[Paragraph]
           (id, [PageId]
           ,[Ordinal])
     select id, [PageId]
           ,[Ordinal] 
	from dbo.Paragraph
SET IDENTITY_INSERT Idiomatica.Idioma.[Paragraph] off
GO


USE [Idiomatica]
GO
SET IDENTITY_INSERT Idiomatica.Idioma.[Sentence] on
INSERT INTO [Idioma].[Sentence]
           (Id, [ParagraphId]
           ,[Ordinal]
           ,[Text])
     select Id, [ParagraphId]
           ,[Ordinal]
           ,[Text]
		from dbo.Sentence
SET IDENTITY_INSERT Idiomatica.Idioma.[Sentence] off
GO


USE [Idiomatica]
GO
SET IDENTITY_INSERT Idiomatica.Idioma.[Word] on
INSERT INTO [Idioma].[Word]
           (Id, [LanguageId]
           ,[Text]
           ,[TextLowerCase]
           ,[Romanization]
           ,[TokenCount])
     select Id, [LanguageUserId]
           ,[Text]
           ,[TextLowerCase]
           ,[Romanization]
           ,[TokenCount]
		from dbo.Word
SET IDENTITY_INSERT Idiomatica.Idioma.[Word] off
GO


USE [Idiomatica]
GO
SET IDENTITY_INSERT Idiomatica.Idioma.[WordUser] on
INSERT INTO [Idioma].[WordUser]
           (Id, [WordId]
           ,[LanguageUserId]
           ,[Translation]
           ,[Status]
           ,[Created]
           ,[StatusChanged])
     select
		Id, [Id]
           ,[LanguageUserId]
           ,[Translation]
           ,[Status]
           ,[Created]
           ,[StatusChanged]
		from dbo.Word
SET IDENTITY_INSERT Idiomatica.Idioma.[WordUser] off
GO

USE [Idiomatica]
GO
SET IDENTITY_INSERT Idiomatica.Idioma.[Token] on
INSERT INTO [Idioma].[Token]
           (id, [WordId]
           ,[SentenceId]
           ,[Display]
           ,[Ordinal])
     select
           id, [WordId]
           ,[SentenceId]
           ,[Display]
           ,[Ordinal]
		from dbo.Token
SET IDENTITY_INSERT Idiomatica.Idioma.[Token] off
GO



















--bookstat
--bookuserstat
--usersetting