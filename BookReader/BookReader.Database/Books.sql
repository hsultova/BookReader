CREATE TABLE [dbo].[Books]
(
	 [Id] [int] IDENTITY(1,1) NOT NULL,
	 [Title] [nvarchar](100) NOT NULL,
	 [Description] [nvarchar](1000) NOT NULL,
	 [Status] [nvarchar](100) NOT NULL,
	 [Date] [datetime2] NOT NULL,
	 [AuthorId] [int] NOT NULL,
	 [GenreId] [int] NOT NULL,
 CONSTRAINT [PK_Books] PRIMARY KEY CLUSTERED 
(
[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON), 
    CONSTRAINT [CK_Books_Genres] CHECK (1 = 1), 
    CONSTRAINT [FK_Books_Genres] FOREIGN KEY ([GenreId]) REFERENCES [Genres]([Id]),
	CONSTRAINT [CK_Books_Authors] CHECK (1 = 1), 
    CONSTRAINT [FK_Books_Authors] FOREIGN KEY ([AuthorId]) REFERENCES [Authors]([Id])
) ON [PRIMARY]

