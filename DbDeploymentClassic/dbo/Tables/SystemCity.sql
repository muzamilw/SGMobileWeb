CREATE TABLE [dbo].[SystemCity] (
    [CityId]    INT           IDENTITY (1, 1) NOT NULL,
    [CountryId] SMALLINT      NOT NULL,
    [StateId]   SMALLINT      NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [Code]      NVARCHAR (5)  NULL,
    [StatusId]  SMALLINT      NULL,
    CONSTRAINT [PK_SG2_SystemCity] PRIMARY KEY CLUSTERED ([CityId] ASC)
);

