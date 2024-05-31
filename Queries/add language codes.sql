-- already run in prod: true

SELECT  *
  FROM [Idiomatica].[Idioma].[LanguageCode]


alter table [Idiomatica].[Idioma].[LanguageCode] add  IsImplementedForLearning bit default 0;
alter table [Idiomatica].[Idioma].[LanguageCode] add  IsImplementedForUI bit default 0;
alter table [Idiomatica].[Idioma].[LanguageCode] add  IsImplementedForTranslation bit default 0;

update [Idiomatica].[Idioma].[LanguageCode] set IsImplementedForLearning = 1, IsImplementedForUI = 0, IsImplementedForTranslation = 0 where Code = 'ES';
update [Idiomatica].[Idioma].[LanguageCode] set IsImplementedForLearning = 1, IsImplementedForUI = 1, IsImplementedForTranslation = 1 where Code = 'EN-US';


INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('BG','Bulgarian',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('CS','Czech',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('DA','Danish',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('DE','German',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('EL','Greek',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('EN-GB','English (British)',0,0,0);
--INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('EN-US','English (American)',0,1,1);
--INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('ES','Spanisg',1,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('ET','Estonian',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('FI','Finish',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('FR','French',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('HU','Hungarian',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('ID','Indonesian',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('IT','Italian',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('JA','Japanese',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('KO','Korean',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('LT','Lithuanian',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('LV','Latvian',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('NB','Norwegian (Bokmål)',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('NL','Dutch',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('PL','Polish',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('PT_BR','Portuguese (Brazilian)',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('PT_PT','Portuguese (European)',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('RO','Romanian',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('RU','Russian',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('SK','Slovak',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('SL','Slovenian',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('SV','Swedish',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('TR','Turkish',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('UK','Ukrainian',0,0,0);
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('ZH','Chinese (simplified)',0,0,0);

update [Idiomatica].[Idioma].[LanguageCode] set Code = 'ENG-GB' where Code = 'EN-GB';
INSERT INTO Idioma.LanguageCode(Code,LanguageName,IsImplementedForLearning,IsImplementedForUI,IsImplementedForTranslation) VALUES('ENG-US','English (American)',0,1,1);
update [Idiomatica].[Idioma].[User] set LanguageCode = 'ENG-US' where LanguageCode = 'EN-US';
update [Idiomatica].[Idioma].Language set LanguageCode = 'ENG-US' where LanguageCode = 'EN-US';
delete from [Idiomatica].[Idioma].[LanguageCode] where Code = 'EN-US';


update [Idiomatica].[Idioma].[LanguageCode] set IsImplementedForLearning = 1, IsImplementedForUI = 1, IsImplementedForTranslation = 1 where Code = 'ENG-US';