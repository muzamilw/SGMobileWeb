CREATE TABLE [dbo].[SystemConfig] (
    [ConfigId]     SMALLINT       IDENTITY (1, 1) NOT NULL,
    [ConfigKey]    NVARCHAR (50)  NOT NULL,
    [ConfigValue]  NVARCHAR (250) NOT NULL,
    [ConfigValue2] NVARCHAR (250) NULL,
    [CreatedOn]    DATETIME       NOT NULL,
    [CreatedBy]    NVARCHAR (50)  NOT NULL,
    [ModifiedOn]   DATETIME       NOT NULL,
    [ModifiedBy]   NVARCHAR (50)  NOT NULL,
    CONSTRAINT [PK_SG2_SystemConfig] PRIMARY KEY CLUSTERED ([ConfigId] ASC)
);

