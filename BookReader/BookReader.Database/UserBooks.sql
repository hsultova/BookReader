﻿CREATE TABLE [dbo].[UserBooks]
(
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Status] [nvarchar](100) NOT NULL,
	[UserId] [int] NOT NULL,
	[BookId] [int] NOT NULL,
 CONSTRAINT [PK_UserBooks] PRIMARY KEY CLUSTERED 
(
[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON), 
    CONSTRAINT [CK_UserBooks_Users] CHECK (1 = 1), 
    CONSTRAINT [FK_UserBooks_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
	CONSTRAINT [CK_UserBooks_Books] CHECK (1 = 1), 
    CONSTRAINT [FK_UserBooks_Books] FOREIGN KEY ([BookId]) REFERENCES [Books]([Id])
) ON [PRIMARY]
