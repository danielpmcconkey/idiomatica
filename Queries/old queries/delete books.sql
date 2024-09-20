delete
  FROM [Idioma].[BookUserStat]
  where BookId in (42, 43,44,45,46,47,48)
  ;
delete
  FROM [Idiomatica].[Idioma].[Book]
  where Id in (42, 43,44,45,46,47,48);


select * from [Idioma].[BookUser] where BookId in (42, 43,44,45,46,47,48)

select * from [Idioma].Page where BookId in (42, 43,44,45,46,47,48)