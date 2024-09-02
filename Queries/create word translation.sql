create table [Idioma].WordTranslation(
	UniqueKey uniqueidentifier not null primary key,
	LanguageToId int not null,
	WordId int not null,
	PartOfSpeech int not null,
	Translation nvarchar(2000) not null
);
go

ALTER TABLE [Idioma].WordTranslation  WITH CHECK ADD  CONSTRAINT [FK_WordTranslation_Language_LanguageToId] FOREIGN KEY(LanguageToId)
REFERENCES [Idioma].Language ([Id])
ON DELETE CASCADE
GO


ALTER TABLE [Idioma].WordTranslation  WITH CHECK ADD  CONSTRAINT [FK_WordTranslation_Word_WordId] FOREIGN KEY(WordId)
REFERENCES [Idioma].Word ([Id])
ON DELETE NO ACTION
GO


  