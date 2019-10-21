﻿CREATE TABLE [dbo].[Customer] (
    [CustomerId]                    INT            IDENTITY (1, 1) NOT NULL,
    [GUID]                          NVARCHAR (36)  NOT NULL,
    [FirstName]                     NVARCHAR (20)  NOT NULL,
    [SurName]                       NVARCHAR (20)  NULL,
    [EmailAddress]                  NVARCHAR (50)  NOT NULL,
    [Password]                      NVARCHAR (64)  NOT NULL,
    [CreatedOn]                     DATETIME       NOT NULL,
    [CreatedBy]                     NVARCHAR (50)  NOT NULL,
    [UpdatedOn]                     DATETIME       NOT NULL,
    [UpdatedBy]                     NVARCHAR (50)  NOT NULL,
    [StatusId]                      SMALLINT       NOT NULL,
    [LastLoginDate]                 DATETIME       NOT NULL,
    [LoginAttempts]                 TINYINT        NOT NULL,
    [LastLoginIP]                   NVARCHAR (20)  NOT NULL,
    [Tocken]                        NVARCHAR (128) NOT NULL,
    [StripeCustomerId]              NVARCHAR (MAX) NULL,
    [UserName]                      NVARCHAR (50)  NULL,
    [Source]                        NVARCHAR (50)  NULL,
    [Register]                      NVARCHAR (50)  NULL,
    [ResponsibleTeamMemberId]       INT            NULL,
    [AvailableToEveryOne]           BIT            NULL,
    [Comment]                       NVARCHAR (MAX) NULL,
    [CancelledDate]                 DATETIME       NULL,
    [IsOptedEducationalEmailSeries] BIT            NULL,
    [IsOptedMarketingEmail]         BIT            NULL,
    [Title]                         NVARCHAR (10)  NULL,
    CONSTRAINT [PK_SG2_Customer] PRIMARY KEY CLUSTERED ([CustomerId] ASC)
);

