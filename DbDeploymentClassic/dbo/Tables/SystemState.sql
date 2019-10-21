CREATE TABLE [dbo].[SystemState] (
    [StateId]   SMALLINT      IDENTITY (1, 1) NOT NULL,
    [CountryId] SMALLINT      NOT NULL,
    [Name]      NVARCHAR (50) NULL,
    [Code]      NVARCHAR (5)  NULL,
    [StatusId]  SMALLINT      NULL,
    CONSTRAINT [PK_SG2_SystemStates] PRIMARY KEY CLUSTERED ([StateId] ASC)
);

