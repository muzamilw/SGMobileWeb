CREATE TABLE [dbo].[SocialProfile_Actions] (
    [SPSHId]          BIGINT         IDENTITY (1, 1) NOT NULL,
    [SocialProfileId] INT            NULL,
    [ActionID]        INT            NULL,
    [TargetProfile]   NVARCHAR (100) NULL,
    [ActionDateTime]  DATETIME       NULL,
    CONSTRAINT [PK_SG2_SocialProfile_StatusHistory] PRIMARY KEY CLUSTERED ([SPSHId] ASC),
    CONSTRAINT [FK_SG2_SocialProfile_StatusHistory_SG2_SocialProfile] FOREIGN KEY ([SocialProfileId]) REFERENCES [dbo].[SocialProfile] ([SocialProfileId])
);

