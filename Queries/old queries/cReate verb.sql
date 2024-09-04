--drop table [Idioma].Verb;
create table [Idioma].Verb(
	UniqueKey uniqueidentifier not null primary key,
	LanguageId int not null,
	Conjugator nvarchar(2000) not null,
	Infinitive nvarchar(2000) not null,
	Core1 nvarchar(2000) not null,
	Core2 nvarchar(2000) not null,
	Core3 nvarchar(2000) not null,
	Core4 nvarchar(2000) not null,
	Gerund nvarchar(2000) not null,
	PastParticiple nvarchar(2000) not null,
);
go

ALTER TABLE [Idioma].Verb  WITH CHECK ADD  CONSTRAINT [FK_Verb_Language_LanguageId] FOREIGN KEY(LanguageId)
REFERENCES [Idioma].Language ([Id])
ON DELETE CASCADE
GO

  