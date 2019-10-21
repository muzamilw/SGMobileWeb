CREATE TABLE [dbo].[SystemCountry] (
    [CountryId] SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Name]      NVARCHAR (50) NOT NULL,
    [Code]      NVARCHAR (5)  NULL,
    [PhoneCode] NVARCHAR (5)  NULL,
    [StatusId]  SMALLINT      NULL,
    CONSTRAINT [PK_SG2_SystemCountry] PRIMARY KEY CLUSTERED ([CountryId] ASC)
);

