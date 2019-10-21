CREATE TABLE [dbo].[SystemUser] (
    [SystemUserId] INT           IDENTITY (1, 1) NOT NULL,
    [Title]        NVARCHAR (5)  NULL,
    [FirstName]    NVARCHAR (50) NOT NULL,
    [LastName]     NVARCHAR (50) NOT NULL,
    [Email]        NVARCHAR (50) NOT NULL,
    [SystemRoleId] SMALLINT      NOT NULL,
    [Password]     NVARCHAR (50) NOT NULL,
    [StatusId]     SMALLINT      NOT NULL,
    [CreatedOn]    DATETIME      NOT NULL,
    [CreatedBy]    NVARCHAR (50) NOT NULL,
    [ModifiedOn]   DATETIME      NOT NULL,
    [ModifiedBy]   NVARCHAR (50) NOT NULL,
    [HostUser]     BIT           NOT NULL,
    CONSTRAINT [PK_SG2_SystemUsers] PRIMARY KEY CLUSTERED ([SystemUserId] ASC),
    CONSTRAINT [FK_SG2_SystemUsers_SG2_SystemRoles] FOREIGN KEY ([SystemRoleId]) REFERENCES [dbo].[SystemRole] ([RoleId])
);

