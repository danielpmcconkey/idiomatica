

  alter table [Idiomatica].[Idioma].[Language] add [DeepLCode] varchar(25);
  update [Idiomatica].[Idioma].[Language] set [DeepLCode] = 'ES' where id = 1;
   update [Idiomatica].[Idioma].[Language] set [DeepLCode] = 'EN-US' where id = 2;
select * from [Idioma].[Language]