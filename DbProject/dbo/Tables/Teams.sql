CREATE TABLE [dbo].[Teams] (
    [Id]    INT           IDENTITY (1, 1) NOT NULL,
    [Name]  NVARCHAR (50) NOT NULL,
    [Coach] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

