drop table Idioma.BookTag;
go
create table Idioma.BookTag (
	Id int IDENTITY(1,1) NOT NULL,
	BookId int not null,
	UserId int not null,
	Tag nvarchar(250),
	Created DateTimeOffset
);
GO
ALTER TABLE [Idioma].BookTag  ADD CONSTRAINT PK_BookTag_Id PRIMARY KEY CLUSTERED (Id);
GO
ALTER TABLE [Idioma].BookTag  WITH CHECK ADD  CONSTRAINT [FK_BookTag_Book_BookId] FOREIGN KEY(BookId)
REFERENCES [Idioma].Book ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [Idioma].BookTag  WITH CHECK ADD  CONSTRAINT [FK_BookTag_User_UserId] FOREIGN KEY(UserId)
REFERENCES [Idioma].[User] ([Id])
ON DELETE CASCADE
GO