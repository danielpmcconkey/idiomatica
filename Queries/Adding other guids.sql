ALTER TABLE Idioma.BookTag ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.BookTag set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].BookTag
ADD CONSTRAINT UC_BookTag_UniqueKey UNIQUE (UniqueKey); 

GO


/**********************************************************************/

ALTER TABLE Idioma.LanguageUser ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.LanguageUser set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].LanguageUser
ADD CONSTRAINT UC_LanguageUser_UniqueKey UNIQUE (UniqueKey); 

GO


/**********************************************************************/

ALTER TABLE Idioma.BookUser ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.BookUser set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].BookUser
ADD CONSTRAINT UC_BookUser_UniqueKey UNIQUE (UniqueKey); 

GO


/**********************************************************************/

ALTER TABLE Idioma.FlashCard ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.FlashCard set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].FlashCard
ADD CONSTRAINT UC_FlashCard_UniqueKey UNIQUE (UniqueKey); 

GO

/**********************************************************************/

ALTER TABLE Idioma.FlashCardAttempt ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.FlashCardAttempt set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].FlashCardAttempt
ADD CONSTRAINT UC_FlashCardAttempt_UniqueKey UNIQUE (UniqueKey); 

GO


/**********************************************************************/

ALTER TABLE Idioma.FlashCardParagraphTranslationBridge ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.FlashCardParagraphTranslationBridge set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].FlashCardParagraphTranslationBridge
ADD CONSTRAINT UC_FlashCardParagraphTranslationBridge_UniqueKey UNIQUE (UniqueKey); 

GO


/**********************************************************************/

ALTER TABLE Idioma.Page ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.Page set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].Page
ADD CONSTRAINT UC_Page_UniqueKey UNIQUE (UniqueKey); 

GO


/**********************************************************************/

ALTER TABLE Idioma.PageUser ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.PageUser set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].PageUser
ADD CONSTRAINT UC_PageUser_UniqueKey UNIQUE (UniqueKey); 

GO


/**********************************************************************/

ALTER TABLE Idioma.Paragraph ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.Paragraph set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].Paragraph
ADD CONSTRAINT UC_Paragraph_UniqueKey UNIQUE (UniqueKey); 

GO


/**********************************************************************/


ALTER TABLE Idioma.ParagraphTranslation ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.ParagraphTranslation set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].ParagraphTranslation
ADD CONSTRAINT UC_ParagraphTranslation_UniqueKey UNIQUE (UniqueKey); 

GO


/**********************************************************************/

ALTER TABLE Idioma.Sentence ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.Sentence set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].Sentence
ADD CONSTRAINT UC_Sentence_UniqueKey UNIQUE (UniqueKey); 

GO

/**********************************************************************/

ALTER TABLE Idioma.Token ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.Token set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].Token
ADD CONSTRAINT UC_Token_UniqueKey UNIQUE (UniqueKey); 

GO

/**********************************************************************/


ALTER TABLE Idioma.[User] ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.[User] set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].[User]
ADD CONSTRAINT UC_User_UniqueKey UNIQUE (UniqueKey); 

GO

/**********************************************************************/


ALTER TABLE Idioma.Word ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.Word set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].Word
ADD CONSTRAINT UC_Word_UniqueKey UNIQUE (UniqueKey); 

GO

/**********************************************************************/


ALTER TABLE Idioma.WordUser ADD
	UniqueKey uniqueidentifier NULL;
GO
update Idioma.WordUser set UniqueKey = NEWID();
GO

ALTER TABLE [Idioma].WordUser
ADD CONSTRAINT UC_WordUser_UniqueKey UNIQUE (UniqueKey); 

GO

/**********************************************************************/
