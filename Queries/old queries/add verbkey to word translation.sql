ALTER TABLE [Idioma].[WordTranslation] add VerbKey uniqueidentifier;
go

ALTER TABLE [Idioma].[WordTranslation]  WITH CHECK ADD  CONSTRAINT [FK_WordTranslation_Verb_VerbKey] FOREIGN KEY(VerbKey)
REFERENCES [Idioma].Verb ([UniqueKey])
ON DELETE no action
GO

  