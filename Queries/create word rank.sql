create table [Idioma].WordRank(
	LanguageId int not null,
	WordId int not null,
	Ordinal int not null,
	DifficultyScore numeric(8,2) not null
);
go

ALTER TABLE [Idioma].WordRank  WITH CHECK ADD  CONSTRAINT [FK_WordRank_Language_LanguageId] FOREIGN KEY(LanguageId)
REFERENCES [Idioma].Language ([Id])
ON DELETE CASCADE
GO


ALTER TABLE [Idioma].WordRank  WITH CHECK ADD  CONSTRAINT [FK_WordRank_Word_WordId] FOREIGN KEY(WordId)
REFERENCES [Idioma].Word ([Id])
ON DELETE NO ACTION
GO


ALTER TABLE [Idioma].WordRank  
ADD CONSTRAINT UK_Language_Ordinal UNIQUE (LanguageId, Ordinal);  