ALTER TABLE [Idioma].[Word]
ADD CONSTRAINT UC_Word_LanguageId_TextLowerCase UNIQUE (LanguageId, TextLowerCase); 

ALTER TABLE [Idioma].[WordUser]
ADD CONSTRAINT UC_WordUser_WordId_LanguageUserId UNIQUE (WordId, LanguageUserId); 



/* 

this should fail

USE [Idiomatica]
GO

INSERT INTO [Idioma].[Word]
           ([LanguageId]
           ,[Text]
           ,[TextLowerCase]
           ,[Romanization]
           ,[TokenCount])
     VALUES
           (1
           ,'medir'
           ,'medir'
           ,null
           ,0)
GO

-- this should also fail

USE [Idiomatica]
GO

INSERT INTO [Idioma].[WordUser]
           ([WordId]
           ,[LanguageUserId]
           ,[Translation]
           ,[Status]
           ,[Created]
           ,[StatusChanged])
     VALUES
           (2
           ,1
           ,'to measure'
           ,1
           ,CURRENT_TIMESTAMP
           ,CURRENT_TIMESTAMP)
GO

*/