delete from [Idioma].[Language];
delete from [Idioma].[User];
delete from [dbo].[AspNetUsers];


select count(*) as NumRows, '[Idioma].[BookUserStat]' from [Idioma].[BookUserStat] union all 
select count(*) as NumRows, '[Idioma].[WordUser]' from [Idioma].[WordUser] union all 
select count(*) as NumRows, '[Idioma].[BookUser]' from [Idioma].[BookUser] union all 
select count(*) as NumRows, '[Idioma].[Paragraph]' from [Idioma].[Paragraph] union all 
select count(*) as NumRows, '[Idioma].[UserBreadCrumb]' from [Idioma].[UserBreadCrumb] union all 
select count(*) as NumRows, '[Idioma].[FlashCard]' from [Idioma].[FlashCard] union all 
select count(*) as NumRows, '[Idioma].[PageUser]' from [Idioma].[PageUser] union all 
select count(*) as NumRows, '[Idioma].[ParagraphTranslation]' from [Idioma].[ParagraphTranslation] union all 
select count(*) as NumRows, '[Idioma].[Sentence]' from [Idioma].[Sentence] union all 
select count(*) as NumRows, '[Idioma].[FlashCardAttempt]' from [Idioma].[FlashCardAttempt] union all 
select count(*) as NumRows, '[Idioma].[FlashCardParagraphTranslationBridge]' from [Idioma].[FlashCardParagraphTranslationBridge] union all 
select count(*) as NumRows, '[Idioma].[Token]' from [Idioma].[Token] union all 
select count(*) as NumRows, '[dbo].[LogMessage]' from [dbo].[LogMessage] union all 
select count(*) as NumRows, '[dbo].[AspNetRoles]' from [dbo].[AspNetRoles] union all 
select count(*) as NumRows, '[dbo].[AspNetUsers]' from [dbo].[AspNetUsers] union all 
select count(*) as NumRows, '[Idioma].[Language]' from [Idioma].[Language] union all 
select count(*) as NumRows, '[Idioma].[User]' from [Idioma].[User] union all 
select count(*) as NumRows, '[dbo].[AspNetRoleClaims]' from [dbo].[AspNetRoleClaims] union all 
select count(*) as NumRows, '[dbo].[AspNetUserClaims]' from [dbo].[AspNetUserClaims] union all 
select count(*) as NumRows, '[dbo].[AspNetUserLogins]' from [dbo].[AspNetUserLogins] union all 
select count(*) as NumRows, '[dbo].[AspNetUserRoles]' from [dbo].[AspNetUserRoles] union all 
select count(*) as NumRows, '[dbo].[AspNetUserTokens]' from [dbo].[AspNetUserTokens] union all 
select count(*) as NumRows, '[Idioma].[Book]' from [Idioma].[Book] union all 
select count(*) as NumRows, '[Idioma].[Verb]' from [Idioma].[Verb] union all 
select count(*) as NumRows, '[Idioma].[Word]' from [Idioma].[Word] union all 
select count(*) as NumRows, '[Idioma].[LanguageUser]' from [Idioma].[LanguageUser] union all 
select count(*) as NumRows, '[Idioma].[UserSetting]' from [Idioma].[UserSetting] union all 
select count(*) as NumRows, '[Idioma].[BookStat]' from [Idioma].[BookStat] union all 
select count(*) as NumRows, '[Idioma].[BookTag]' from [Idioma].[BookTag] union all 
select count(*) as NumRows, '[Idioma].[Page]' from [Idioma].[Page] union all 
select count(*) as NumRows, '[Idioma].[WordRank]' from [Idioma].[WordRank] union all 
select count(*) as NumRows, '[Idioma].[WordTranslation]' from [Idioma].[WordTranslation] 


/*
SELECT 
'select count(*) as NumRows, ''[' 
+ TABLE_SCHEMA + '].['
+ TABLE_NAME + ']'' from ['
+ TABLE_SCHEMA + '].['
+ TABLE_NAME + '] union all '
FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE'
*/