-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SG2_usp_SocialProfile_GetNotificationsByStatus]
	@StatusId smallint
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select 
		nt.[Id], 
		nt.[Notification], 
		nt.StatusId, 
		nt.SocialProfileId,
		coalesce(sp.SocialUsername, [EmailAddress]) SocialUsername,
		nt.CreatedOn,
		nt.CreatedBy,
		nt.UpdateOn,
		nt.Updatedby,
		nt.Mode
	from [dbo].[SocialProfile_Notification] nt
		inner join [dbo].[SocialProfile] sp
			on nt.socialprofileid = sp.socialprofileid
		Inner Join [dbo].[Customer] c
			on c.customerid = sp.customerid
	where nt.StatusId  in (51,52)
		order by nt.CreatedOn desc
END
