  
CREATE Procedure [dbo].[SG2_usp_Customer_ProfileUpdate]  
  @iCustomerId int,  
  @rvcUserName Nvarchar(50),  
  @rvcFirstName Nvarchar(50),  
  @rvcSurName Nvarchar(50),  
  @rvcPhoneNumber Nvarchar(64),  
  @rvcPhoneCode nvarchar(5)  
As    
Begin  
  
 Update  [dbo].[Customer]   
 set   
  FirstName=@rvcFirstName,  
  SurName=@rvcSurName,    
  UserName=@rvcUserName   
 where CustomerId=@iCustomerId;  
  
If not exists(Select 1 From dbo.Customer_ContactDetail where CustomerId = @iCustomerId)   
 Begin  
  Insert into dbo.Customer_ContactDetail  
  (  
   [CustomerId]         
   ,[PhoneNumber]          
   ,PhoneCode  
   ,[GUID]  
  )  
  Values (  
   @iCustomerId,  
   @rvcPhoneNumber,  
   @rvcPhoneCode,  
   NEWID()  
  )  
 END  
 Else   
 begin  
  Update dbo.Customer_ContactDetail  
   set   
    PhoneNumber = @rvcPhoneNumber,  
    PhoneCode = @rvcPhoneCode   
   where   
    CustomerId=@iCustomerId  
 End;  
  
exec [dbo].[SG2_usp_Customers_Get] @iCustomerId  
           
End  
  