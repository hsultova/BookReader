CREATE TABLE [dbo].[Users](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](100) NOT NULL,
    [Firstname] [nvarchar](100) NOT NULL,
    [Lastname] [nvarchar](100) NOT NULL,
	[IsFirstTimeLoggedIn] [bit] NOT NULL,
	[RoleId] [int]  NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON), 
    CONSTRAINT [CK_Users_Roles] CHECK (1 = 1), 
    CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id])
) ON [PRIMARY]
