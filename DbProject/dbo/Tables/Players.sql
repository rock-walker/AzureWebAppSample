CREATE TABLE [dbo].[Players] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    [Age]      INT           NOT NULL,
    [Position] NVARCHAR (50) NOT NULL,
    [TeamId]   INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Players_Teams] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Teams] ([Id]) ON DELETE SET NULL
);

