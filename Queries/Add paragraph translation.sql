USE [Idiomatica]
GO

/****** Object:  Table [Idioma].[BookUser]    Script Date: 5/6/2024 7:27:41 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE TABLE [Idioma].ParagraphTranslation(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	ParagraphId [int] NOT NULL,
	LanguageCode varchar(25) NOT NULL,
	TranslationText varchar(8000)
	);

ALTER TABLE [Idioma].ParagraphTranslation  WITH CHECK ADD  CONSTRAINT [FK_ParagraphTranslation_Paragraph_ParagraphId] FOREIGN KEY(ParagraphId)
REFERENCES [Idioma].Paragraph ([Id])
ON DELETE CASCADE
GO

ALTER TABLE [Idioma].ParagraphTranslation CHECK CONSTRAINT [FK_ParagraphTranslation_Paragraph_ParagraphId]
GO

ALTER TABLE [Idioma].ParagraphTranslation  WITH CHECK ADD  CONSTRAINT [FK_ParagraphTranslation_LanguageCode_LanguageCode] FOREIGN KEY(LanguageCode)
REFERENCES [Idioma].LanguageCode (Code)
ON DELETE CASCADE
GO

ALTER TABLE [Idioma].ParagraphTranslation CHECK CONSTRAINT [FK_ParagraphTranslation_LanguageCode_LanguageCode]
GO


