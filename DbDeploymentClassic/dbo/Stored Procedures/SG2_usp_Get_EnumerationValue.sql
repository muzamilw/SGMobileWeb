
CREATE Procedure [dbo].[SG2_usp_Get_EnumerationValue]

As
Begin
 

 Select E.[Name] as Enumeration , EV.EnumerationValueId , EV.[Name]  
 
 
  FROM  Enumeration E inner join EnumerationValue EV
						ON E.EnumerationId=EV.EnumerationId
  WHERE EV.IsVisible = 1
  order by Enumeration, SequenceNo 
End


