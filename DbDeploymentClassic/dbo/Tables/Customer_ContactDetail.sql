CREATE TABLE [dbo].[Customer_ContactDetail] (
    [ContactDetailsId] INT            IDENTITY (1, 1) NOT NULL,
    [CustomerId]       INT            NOT NULL,
    [JobTitle]         NVARCHAR (50)  NULL,
    [MobileNumber]     NVARCHAR (50)  NULL,
    [PhoneNumber]      NVARCHAR (50)  NULL,
    [AddressLine1]     NVARCHAR (255) NULL,
    [AddressLine2]     NVARCHAR (255) NULL,
    [City]             NVARCHAR (50)  NULL,
    [Sate]             NVARCHAR (50)  NULL,
    [Country]          NVARCHAR (50)  NULL,
    [PostalCode]       NVARCHAR (50)  NULL,
    [GUID]             NVARCHAR (36)  NULL,
    [PhoneCode]        NVARCHAR (5)   NULL,
    [ScheduleCallDate] DATETIME       NULL,
    [Notes]            NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_SG2_ContactDetails] PRIMARY KEY CLUSTERED ([ContactDetailsId] ASC),
    CONSTRAINT [FK_SG2_ContactDetails_SG2_Customer] FOREIGN KEY ([CustomerId]) REFERENCES [dbo].[Customer] ([CustomerId])
);

