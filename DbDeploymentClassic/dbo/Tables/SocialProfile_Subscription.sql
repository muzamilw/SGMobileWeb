CREATE TABLE [dbo].[SocialProfile_Subscription] (
    [SubscriptionId]       INT             IDENTITY (1, 1) NOT NULL,
    [Name]                 NVARCHAR (255)  NOT NULL,
    [Description]          NVARCHAR (255)  NOT NULL,
    [SubscriptionType]     NVARCHAR (255)  NULL,
    [Price]                DECIMAL (18, 2) NULL,
    [StartDate]            DATETIME        NOT NULL,
    [EndDate]              DATETIME        NOT NULL,
    [SocialProfileId]      INT             NOT NULL,
    [StripeSubscriptionId] NVARCHAR (255)  NULL,
    [StatusId]             INT             NULL,
    [StripePlanId]         NVARCHAR (255)  NULL,
    [PaymentPlanId]        INT             NULL,
    [StripeInvoiceId]      NVARCHAR (255)  NULL,
    CONSTRAINT [PK_SG2_Subscription] PRIMARY KEY CLUSTERED ([SubscriptionId] ASC),
    CONSTRAINT [FK_SG2_SocialProfile_Subscription_SG2_SocialProfile] FOREIGN KEY ([SocialProfileId]) REFERENCES [dbo].[SocialProfile] ([SocialProfileId]),
    CONSTRAINT [FK_SocialProfile_Subscription_SocialProfile_PaymentPlan] FOREIGN KEY ([PaymentPlanId]) REFERENCES [dbo].[PaymentPlan] ([PaymentPlanId])
);

