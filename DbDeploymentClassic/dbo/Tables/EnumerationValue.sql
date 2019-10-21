CREATE TABLE [dbo].[EnumerationValue] (
    [EnumerationValueId] SMALLINT       NOT NULL,
    [EnumerationId]      SMALLINT       NOT NULL,
    [Name]               NVARCHAR (50)  NOT NULL,
    [Description]        NVARCHAR (255) NOT NULL,
    [IsDefault]          BIT            NOT NULL,
    [SequenceNo]         INT            NOT NULL,
    [IsVisible]          BIT            NULL
);

