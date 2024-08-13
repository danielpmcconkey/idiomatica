USE [Idiomatica]
GO

drop table [Idioma].UserBreadCrumb;
GO

/****** Object:  Table [Idioma].[Page]    Script Date: 8/12/2024 11:59:58 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [Idioma].UserBreadCrumb(
	UniqueKey uniqueidentifier not NULL primary key,
	[UserId] [int] NOT NULL,
	[PageId] [int] NOT NULL,
	[ActionDateTime] DateTime not null
)
	

GO

ALTER TABLE [Idioma].UserBreadCrumb  WITH CHECK ADD  CONSTRAINT [FK_UserBreadCrumb_User_UserId] FOREIGN KEY(UserId)
REFERENCES [Idioma].[User] ([Id])
ON DELETE CASCADE
GO


ALTER TABLE [Idioma].UserBreadCrumb  WITH CHECK ADD  CONSTRAINT [FK_UserBreadCrumb_Page_PageId] FOREIGN KEY(PageId)
REFERENCES [Idioma].[Page] ([Id])
ON DELETE CASCADE
GO





