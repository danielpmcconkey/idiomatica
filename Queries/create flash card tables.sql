ALTER TABLE [Idioma].ParagraphTranslation  ADD CONSTRAINT PK_ParagraphTranslation_Id PRIMARY KEY CLUSTERED (Id);
GO


create table Idioma.FlashCard (
	Id int IDENTITY(1,1) NOT NULL,
	WordUserId int not null,
	[Status] int
);
GO
ALTER TABLE [Idioma].[FlashCard]  ADD CONSTRAINT PK_FlashCard_Id PRIMARY KEY CLUSTERED (Id);
GO
ALTER TABLE [Idioma].FlashCard  WITH CHECK ADD  CONSTRAINT [FK_FlashCard_WordUser_WordUserId] FOREIGN KEY(WordUserId)
REFERENCES [Idioma].WordUser ([Id])
ON DELETE CASCADE
GO

create table Idioma.FlashCardAttempt (
	Id int IDENTITY(1,1) NOT NULL,
	FlashCardId int not null,
	[Status] int,
	AttemptedWhen datetime
);
GO
ALTER TABLE [Idioma].[FlashCardAttempt]  ADD CONSTRAINT PK_FlashCardAttempt_Id PRIMARY KEY CLUSTERED (Id);
GO

ALTER TABLE [Idioma].FlashCardAttempt  WITH CHECK ADD  CONSTRAINT [FK_FlashCardAttempt_FlashCard_FlashCardId] FOREIGN KEY(FlashCardId)
REFERENCES [Idioma].FlashCard ([Id])
ON DELETE CASCADE
GO

create table Idioma.FlashCardParagraphTranslationBridge (
	Id int IDENTITY(1,1) NOT NULL,
	FlashCardId int not null,
	ParagraphTranslationId int not null
);
GO
ALTER TABLE [Idioma].FlashCardParagraphTranslationBridge  ADD CONSTRAINT PK_FlashCardParagraphTranslationBridge_Id PRIMARY KEY CLUSTERED (Id);
GO
ALTER TABLE [Idioma].FlashCardParagraphTranslationBridge  WITH CHECK ADD  CONSTRAINT [FK_FlashCardParagraphTranslationBridge_FlashCard_FlashCardId] FOREIGN KEY(FlashCardId)
REFERENCES [Idioma].FlashCard ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [Idioma].FlashCardParagraphTranslationBridge  WITH CHECK ADD  CONSTRAINT [FK_FlashCardParagraphTranslationBridge_ParagraphTranslation_ParagraphTranslationId] FOREIGN KEY(ParagraphTranslationId)
REFERENCES [Idioma].ParagraphTranslation ([Id])
ON DELETE NO ACTION
GO

