INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('EN-US','English (American)',0,1,1);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('EN-GB','English (British)',0,0,0);



update [Idiomatica].[Idioma].[User] set LanguageCode = 'EN-US' where LanguageCode = 'ENG-US';
update [Idiomatica].[Idioma].Language set LanguageCode = 'EN-US' where LanguageCode = 'ENG-US';
delete from [Idiomatica].[Idioma].[LanguageCode] where Code = 'ENG-US';
delete from [Idiomatica].[Idioma].[LanguageCode] where Code = 'ENG-GB';
