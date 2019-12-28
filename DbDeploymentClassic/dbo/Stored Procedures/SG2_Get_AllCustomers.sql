
CREATE Procedure [dbo].[SG2_Get_AllCustomers]


 
As  
Begin

 -- Searches for Customers based on given parameters  

SELECT Distinct CustomerId, ISnull(FirstName,'') + ' ' + Isnull(SurName,'') as CustomerName  FROM SG2_Customer
 
 
    
End

