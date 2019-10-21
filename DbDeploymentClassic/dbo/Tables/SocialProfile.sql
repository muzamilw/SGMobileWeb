CREATE TABLE [dbo].[SocialProfile] (
    [SocialProfileId]     INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]          INT            NULL,
    [SocialProfileTypeId] INT            NULL,
    [StatusId]            INT            NULL,
    [StripeCustomerId]    INT            NULL,
    [SocialUsername]      NVARCHAR (50)  NULL,
    [SocialPassword]      NVARCHAR (50)  NULL,
    [SocialProfileName]   NVARCHAR (50)  NULL,
    [CreatedOn]           DATETIME       NOT NULL,
    [CreatedBy]           NVARCHAR (50)  NULL,
    [UpdatedOn]           DATETIME       NOT NULL,
    [UpdatedBy]           NVARCHAR (50)  NULL,
    [verificationCode]    NVARCHAR (50)  NULL,
    [IsArchived]          BIT            CONSTRAINT [DF_IsArchived] DEFAULT ((0)) NULL,
    [DeviceIMEI]          NVARCHAR (100) NULL,
    [DeviceStatus]        INT            NULL,
    CONSTRAINT [PK_SG2_CustomerProfile] PRIMARY KEY CLUSTERED ([SocialProfileId] ASC),
    CONSTRAINT [FK_SG2_SocialProfile_SG2_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId])
);

