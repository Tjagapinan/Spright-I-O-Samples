
ALTER PROC [dbo].[EVA_Values_Select_By_RecordID]
			
           @ID int

AS

BEGIN

  SELECT [ID]
	  ,[EntityID]
	  ,[WebsiteId]
	  
FROM [dbo].[EVA_Records]
WHERE ID = @ID
 

SELECT v.[RecordID]
	  ,v.[AttributeID]
	  ,v.[ValueString]
	  ,v.[ValueInt]
	  ,v.[ValueDecimal]
	  ,v.[ValueText]
	  ,v.[ValueGeo] 
	  ,a.[slug]
	  ,a.[name]

FROM [dbo].[EVA_Values] as v INNER JOIN [dbo].[EVA_Attributes] as a
ON v.AttributeID = a.ID
WHERE RecordID = @ID

ORDER BY AttributeID asc

END
