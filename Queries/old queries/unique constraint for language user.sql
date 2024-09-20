


ALTER TABLE [Idioma].[LanguageUser]
ADD CONSTRAINT UC_LanguageUser_LanguageId_UserId UNIQUE (LanguageId, UserId); 

