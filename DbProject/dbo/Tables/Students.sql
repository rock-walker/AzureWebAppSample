CREATE TABLE [dbo].[Students] (
    [Id]      INT            IDENTITY (1, 1) NOT NULL,
    [Name]    NVARCHAR (MAX) NULL,
    [Surname] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Students] PRIMARY KEY CLUSTERED ([Id] ASC)
);

