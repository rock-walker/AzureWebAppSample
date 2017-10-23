CREATE TABLE [dbo].[Books] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Name]   NVARCHAR (MAX) NULL,
    [Author] NVARCHAR (MAX) NULL,
    [Price]  INT            NOT NULL,
    CONSTRAINT [PK_dbo.Books] PRIMARY KEY CLUSTERED ([Id] ASC)
);
