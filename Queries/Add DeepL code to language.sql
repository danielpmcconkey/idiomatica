-- already run in prod: true

create table Idioma.LanguageCode (
	Code varchar(25) not null primary key,
	LanguageName varchar (250)
	);

insert into Idioma.LanguageCode (Code, LanguageName) values ('EN-US','English (American)');
insert into Idioma.LanguageCode (Code, LanguageName) values ('ES','Spanish');


  alter table [Idiomatica].[Idioma].[Language] add LanguageCode varchar(25);
  update [Idiomatica].[Idioma].[Language] set LanguageCode = 'ES' where id = 1;
   update [Idiomatica].[Idioma].[Language] set LanguageCode = 'EN-US' where id = 2;
select * from [Idioma].[Language]


alter table [Idiomatica].[Idioma].[User] add LanguageCode varchar(25) default 'EN-US';

ALTER TABLE [Idiomatica].[Idioma].[User]  WITH CHECK ADD  CONSTRAINT [FK_User_LanguageCode_LanguageCode] FOREIGN KEY(LanguageCode)
REFERENCES [Idioma].LanguageCode (Code)

ALTER TABLE [Idiomatica].[Idioma].[Language]  WITH CHECK ADD  CONSTRAINT [FK_Language_LanguageCode_LanguageCode] FOREIGN KEY(LanguageCode)
REFERENCES [Idioma].LanguageCode (Code)


  update [Idiomatica].[Idioma].[User] set LanguageCode =  'EN-US';