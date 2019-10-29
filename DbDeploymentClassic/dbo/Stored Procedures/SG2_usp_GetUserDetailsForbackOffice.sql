CREATE Procedure [dbo].[SG2_usp_GetUserDetailsForbackOffice]  
  @rsSearchCrite Nvarchar(MAX),  
  @riPageNumber Int,  
  @riPageSize varchar(8),  
  @riStatusId int=null,  
  @riProductId varchar(250)=null,  
  @riJVStatus varchar(250)=null,  
  @riSubscription int=null  
  
As  
Begin  
   
 -- Searches for Products based on given parameters    
 Declare @iFirstRow Int      
 Declare @iLastRow Int   
  
 Declare @xmlSearchCriteria Xml  
   
 IF @riStatusId=''  
 SET @riStatusId=NULL  
  
 IF @riJVStatus=''  
 SET @riJVStatus=NULL  
 --Set @xmlSearchCriteria = dbo.udf_UTL_UpdateWildcardInput(@rsSearchCriteria)  
  
 Set @iFirstRow  = (@riPageNumber-1) * @riPageSize + 1      
 Set @iLastRow = @riPageSize + @iFirstRow - 1   
   
 Declare  @tbResult table(  
 RowNumber     int ,  
 InstaName     nvarchar(250),  
 UserName      nvarchar(250),  
 CustomerId    int,  
 Products      nvarchar(250),  
 ProxyIPNumber nvarchar(15),  
 BoxName       nvarchar(250),  
 [Status]      nvarchar(100),  
 JVBoxStatus nvarchar(100),  
 SocialProfileName  nvarchar(250),  
 SocialProfileId   int,  
 CustomerEmail    nvarchar(250)  
  
 )   
  
 ;With CTE As  
 (   
  -- Get all Product information to create index  
    
  Select Distinct  
   SP.[SocialUsername] as SocialAccountName,  
   SP.[SocialProfileName] as SocialProfileName,  
   SP.SocialProfileId,  
   ISNULL(Customer.FirstName,'') + ' ' + ISNULL(Customer.SurName,'')  as [Name],  
   Customer.CustomerId as CustomerId,  
   SubS.Name as Products,  
  
   EV.[Name] as [Status],  
   Customer.updatedon,  
  
   customer.[EmailAddress] as EmailAddress ,  
            ROW_NUMBER() OVER (PARTITION BY SP.[SocialProfileId] ORDER BY SubS.StartDate desc) AS RankId  
  From dbo.Customer Customer With (Nolock)   
  Inner join [dbo].[SocialProfile] SP ON SP.CustomerId=Customer.CustomerId  

 left join [dbo].[SocialProfile_Payments] SubS   
  ON SubS.[SocialProfileId]=SP.[SocialProfileId]   
    --AND SubS.StatusId=25  
 left join [dbo].[SocialProfile_Instagram_TargetingInformation] TI   
  On TI.[SocialProfileId]=SP.[SocialProfileId]  
 Left join dbo.EnumerationValue EV   
  ON EV.EnumerationValueId=SP.StatusId --And EV.EnumerationId=4  
 Where (  
   ((@riStatusId is null) or Customer.StatusId = @riStatusId)  
  
   AND   
     (((@riProductId is null) or @riProductId ='' ) or  SubS.[StripePlanId] = @riProductId)  
  AND ((@riSubscription is null) or SubS.StatusId= @riSubscription)  
  And (   
    (@rsSearchCrite is null or @rsSearchCrite = '')   
    or (SP.[SocialProfileName] like '%' +@rsSearchCrite +'%'   
    or Customer.UserName like '%' +@rsSearchCrite +'%'  
     or SubS.Name like '%' +@rsSearchCrite +'%'   
   
     or EV.Name like '%' +@rsSearchCrite +'%'   
     or Customer.FirstName like '%' +@rsSearchCrite +'%'  
     or Customer.SurName like '%' +@rsSearchCrite +'%'  
     or Customer.[EmailAddress] like '%' +@rsSearchCrite +'%'  
     or SP.[SocialUsername] like '%' +@rsSearchCrite +'%'   
       
      )  
   )  
 )  
       )  
    
Insert into @tbResult(  
RowNumber  ,    
InstaName  ,    
UserName   ,    
CustomerId ,    
Products   ,    

[Status]   ,  

SocialProfileName,  
SocialProfileId,  
CustomerEmail  
  
)  
SELECT Distinct  
 ROW_NUMBER() Over (    
           Order By UpdatedOn desc  
            ) As RowNumber,  
SocialAccountName,[Name],CustomerId,Products,[Status],SocialProfileName,  
SocialProfileId,EmailAddress  
 FROM CTE  where RankId=1  
  
 Select UserName,    
  InstaName,  
  CustomerId,  
        Products,  
     ProxyIPNumber,  
     BoxName,  
     [Status],  
  JVBoxStatus,  
  SocialProfileName,  
  SocialProfileId,  
  CustomerEmail,  
     (Select Count(1) From @tbResult) TotalRecord  
 From @tbResult   
 Where RowNumber Between @iFirstRow And @iLastRow  
   
 Return @@Error   
End  
  