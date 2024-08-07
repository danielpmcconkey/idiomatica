/*
   Friday, July 19, 202411:11:03 AM
   User: 
   Server: DESKTOP-FA2KQPJ
   Database: Idiomatica
   Application: 
*/

/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE Idioma.Book ADD
	UniqueKey uniqueidentifier NULL
GO
ALTER TABLE Idioma.Book SET (LOCK_ESCALATION = TABLE)
GO



update Idioma.Book set UniqueKey = NEWID();


ALTER TABLE [Idioma].Book
ADD CONSTRAINT UC_Book_UniqueKey UNIQUE (UniqueKey); 
 GO

COMMIT
