-- already run in prod: true

drop table if exists Idioma.BookUserStat;

create table Idioma.BookUserStat (
	LanguageUserId int not null,
	BookId int not null,
	[Key] int not null,
	ValueString nvarchar(500),
	ValueNumeric numeric(10,4)
);


ALTER TABLE Idioma.BookUserStat
   ADD CONSTRAINT PK_BookUserStat PRIMARY KEY CLUSTERED (LanguageUserId, BookId, [Key]);

ALTER TABLE [Idioma].BookUserStat  WITH CHECK ADD  CONSTRAINT [FK_BookUserStat_LanguageUser_LanguageUserId] FOREIGN KEY(LanguageUserId)
REFERENCES [Idioma].LanguageUser ([Id])
ON DELETE CASCADE
;

ALTER TABLE [Idioma].BookUserStat  WITH CHECK ADD  CONSTRAINT [FK_BookUserStat_Book_BookId] FOREIGN KEY(BookId)
REFERENCES [Idioma].Book ([Id])
ON DELETE  NO ACTION
;

INSERT INTO [Idioma].[BookUserStat]
           ([LanguageUserId]
           ,[BookId]
           ,[Key]
           ,[ValueString])
    select [LanguageUserId]
           ,[BookId]
           ,[Key]
           ,[Value]
	from Idioma.vw_BookUserStat
GO

update [Idioma].[BookUserStat] set [ValueNumeric] = cast(ValueString as numeric(10,4)) where [Key] in (2,4,5,6,7,8);
update [Idioma].[BookUserStat] set [ValueString] = null where [Key] in (2,4,5,6,7,8);

drop view if exists Idioma.vw_BookListRow;
create view Idioma.vw_BookListRow
as

select
	  b.Id as BookId
	, u.Id as UserId
	, l.[Name] as LanguageName
	, bus_ISCOMPLETE.ValueString as IsComplete
	, b.Title
	, bus_PROGRESS.ValueString as Progress
	, bus_PROGRESSPERCENT.ValueNumeric as ProgressPercent
	, bus_TOTALWORDCOUNT.ValueNumeric as TotalWordCount
	, bus_TOTALKNOWNPERCENT.ValueNumeric as TotalKnownPercent
	, bus_DISTINCTWORDCOUNT.ValueNumeric as DistinctWordCount
	, bus_DISTINCTKNOWNPERCENT.ValueNumeric as DistinctKnownPercent
from Idioma.BookUser bu
left join  Idioma.LanguageUser lu on bu.LanguageUserId = lu.Id
left join Idioma.[User] u on lu.UserId = u.Id
left join  Idioma.Language l on lu.LanguageId = l.Id
left join Idioma.Book b on bu.BookId = b.Id
left join [Idioma].[BookUserStat] bus_ISCOMPLETE on bus_ISCOMPLETE.BookId = b.Id and bus_ISCOMPLETE.LanguageUserId = lu.Id and bus_ISCOMPLETE.[Key] = 1 --AvailableBookUserStat.ISCOMPLETE
left join [Idioma].[BookUserStat] bus_PROGRESS on bus_PROGRESS.BookId = b.Id and bus_PROGRESS.LanguageUserId = lu.Id and bus_PROGRESS.[Key] = 3 --AvailableBookUserStat.PROGRESS
left join [Idioma].[BookUserStat] bus_PROGRESSPERCENT on bus_PROGRESSPERCENT.BookId = b.Id and bus_PROGRESSPERCENT.LanguageUserId = lu.Id and bus_PROGRESSPERCENT.[Key] = 4 --AvailableBookUserStat.PROGRESSPERCENT
left join [Idioma].[BookUserStat] bus_TOTALWORDCOUNT on bus_TOTALWORDCOUNT.BookId = b.Id and bus_TOTALWORDCOUNT.LanguageUserId = lu.Id and bus_TOTALWORDCOUNT.[Key] = 8 --AvailableBookUserStat.TOTALWORDCOUNT
left join [Idioma].[BookUserStat] bus_TOTALKNOWNPERCENT on bus_TOTALKNOWNPERCENT.BookId = b.Id and bus_TOTALKNOWNPERCENT.LanguageUserId = lu.Id and bus_TOTALKNOWNPERCENT.[Key] = 6 --AvailableBookUserStat.TOTALKNOWNPERCENT
left join [Idioma].[BookUserStat] bus_DISTINCTWORDCOUNT on bus_DISTINCTWORDCOUNT.BookId = b.Id and bus_DISTINCTWORDCOUNT.LanguageUserId = lu.Id and bus_DISTINCTWORDCOUNT.[Key] = 7 --AvailableBookUserStat.DISTINCTWORDCOUNT
left join [Idioma].[BookUserStat] bus_DISTINCTKNOWNPERCENT on bus_DISTINCTKNOWNPERCENT.BookId = b.Id and bus_DISTINCTKNOWNPERCENT.LanguageUserId = lu.Id and bus_DISTINCTKNOWNPERCENT.[Key] = 5 --AvailableBookUserStat.DISTINCTKNOWNPERCENT

;