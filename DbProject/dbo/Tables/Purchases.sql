CREATE TABLE [dbo].[Purchases] (
    [PurchaseId] INT            IDENTITY (1, 1) NOT NULL,
    [Person]     NVARCHAR (MAX) NULL,
    [Address]    NVARCHAR (MAX) NULL,
    [BookId]     INT            NOT NULL,
    [Date]       DATETIME       NOT NULL,
    CONSTRAINT [PK_dbo.Purchases] PRIMARY KEY CLUSTERED ([PurchaseId] ASC)
);

