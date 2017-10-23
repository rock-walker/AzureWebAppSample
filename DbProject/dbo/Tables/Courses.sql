CREATE TABLE [dbo].[Courses] (
    [Id]   INT            IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_dbo.Courses] PRIMARY KEY CLUSTERED ([Id] ASC)
);

