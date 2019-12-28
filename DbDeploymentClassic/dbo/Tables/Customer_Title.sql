CREATE TABLE [dbo].[Customer_Title] (
    [PkTitleId] INT          IDENTITY (1, 1) NOT NULL,
    [TitleName] VARCHAR (50) NULL,
    CONSTRAINT [PK_SG2_Title] PRIMARY KEY CLUSTERED ([PkTitleId] ASC)
);

