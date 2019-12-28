
CREATE Procedure [dbo].[SG2_usp_Get_Title]
 
As  
Begin

SELECT Distinct [PkTitleId],[TitleName]   FROM  [dbo].[SG2_Customer_Title]
  
End

