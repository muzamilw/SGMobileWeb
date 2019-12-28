CREATE TABLE [dbo].[LikeyAccount] (
    [LikeyAccountId] INT           IDENTITY (1, 1) NOT NULL,
    [InstaUserName]  NVARCHAR (50) NOT NULL,
    [InstaPassword]  NVARCHAR (50) NULL,
    [Country]        NVARCHAR (50) NULL,
    [City]           NVARCHAR (50) NOT NULL,
    [Gender]         INT           NOT NULL,
    [HashTag]        NVARCHAR (50) NOT NULL,
    [StatusId]       INT           NOT NULL,
    CONSTRAINT [PK_SG2_LikeyAccount] PRIMARY KEY CLUSTERED ([LikeyAccountId] ASC)
);

