CREATE TABLE [dbo].[SocialProfile_Notification] (
    [Id]              BIGINT         IDENTITY (1, 1) NOT NULL,
    [Notification]    NVARCHAR (250) NOT NULL,
    [StatusId]        SMALLINT       NOT NULL,
    [SocialProfileId] INT            NOT NULL,
    [CreatedOn]       DATETIME       NOT NULL,
    [CreatedBy]       NVARCHAR (50)  NOT NULL,
    [UpdateOn]        DATETIME       NOT NULL,
    [UpdatedBy]       NVARCHAR (50)  NOT NULL,
    [Mode]            NVARCHAR (20)  DEFAULT ('Auto') NOT NULL,
    CONSTRAINT [PK_SG2_SocialProfile_Notification] PRIMARY KEY CLUSTERED ([Id] ASC)
);

