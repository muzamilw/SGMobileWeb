CREATE TABLE [dbo].[PaymentPlan] (
    [PaymentPlanId]        INT            IDENTITY (1, 1) NOT NULL,
    [NoOfLikes]            INT            NULL,
    [DisplayPrice]         FLOAT (53)     NULL,
    [PlanName]             NVARCHAR (250) NOT NULL,
    [PlanShortDescription] NVARCHAR (250) NOT NULL,
    [IsParentPlan]         BIT            NULL,
    [StripePlanId]         NVARCHAR (50)  NULL,
    [StripePlanPrice]      FLOAT (53)     NULL,
    [NoOfLikesDuration]    INT            NULL,
    [StatusId]             INT            NULL,
    [CreatedOn]            DATETIME       NULL,
    [CreatedBy]            NVARCHAR (50)  NULL,
    [UpdatedOn]            DATETIME       NULL,
    [UpdatedBy]            NVARCHAR (50)  NULL,
    [SortOrder]            SMALLINT       NULL,
    [SocialPlatform]       INT            NULL,
    [IsDefault]            BIT            NULL,
    CONSTRAINT [PK_PlanInformation] PRIMARY KEY CLUSTERED ([PaymentPlanId] ASC)
);

