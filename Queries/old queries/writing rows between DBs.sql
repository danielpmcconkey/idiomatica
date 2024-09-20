SELECT 
--TOP (1000) 
W.TextLowerCase,
wu.*
--[Id]
--      ,[WordId]
--      ,[LanguageUserId]
--      ,[Translation]
--      ,[Status]
--      ,[Created]
--      ,[StatusChanged]
--      ,[UniqueKey]
  FROM [Idiomatica_dev].[Idioma].[WordUser] wu
  left join [Idiomatica_dev].[Idioma].[Word] w on wu.WordId = w.Id
  where Created >= '2024-09-05 00:00'
  order by Created desc




select  * from [Idiomatica_dev].[Idioma].[Word] where Id > 20746


select 'SET IDENTITY_INSERT [Idioma].Word ON'
union all
SELECT --TOP (10) 

id,
'INSERT INTO [Idioma].Word(Id,LanguageId,Text,TextLowerCase,TokenCount,UniqueKey)values('
+        case when Id is null then 'NULL' else  cast(Id as nvarchar(100)) + ' '	end
+ ', ' + case when LanguageId is null then 'NULL' else  cast(LanguageId as nvarchar(100)) + ' '	end
+ ', ''' + case when Text is null then 'NULL' else  cast(Text as nvarchar(100)) + ''' '	end
+ ', ''' + case when TextLowerCase is null then 'NULL' else  cast(TextLowerCase as nvarchar(100)) + ''' '	end
+ ', ' + case when TokenCount is null then 'NULL' else  cast(TokenCount as nvarchar(100)) + ' '	end
+ ', ''' + case when UniqueKey is null then 'NULL' else  cast(UniqueKey as nvarchar(100)) + ''' '	end
+ ');'


from [Idiomatica_dev].[Idioma].[Word] where Id > 20746

union all
select 'SET IDENTITY_INSERT [Idioma].Word OFF'
union all
select 'GO'