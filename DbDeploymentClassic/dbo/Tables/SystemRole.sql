CREATE TABLE [dbo].[SystemRole] (
    [RoleId]     SMALLINT      IDENTITY (1, 1) NOT NULL,
    [Name]       NVARCHAR (50) NOT NULL,
    [StatusId]   SMALLINT      NOT NULL,
    [CreatedOn]  DATETIME      NOT NULL,
    [CreatedBy]  NVARCHAR (50) NOT NULL,
    [ModifiedOn] DATETIME      NOT NULL,
    [ModifiedBy] NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SG2_SystemRoles] PRIMARY KEY CLUSTERED ([RoleId] ASC)
);

