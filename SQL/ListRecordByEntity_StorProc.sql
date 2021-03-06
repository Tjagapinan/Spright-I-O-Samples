
ALTER PROC [dbo].[EVA_Record_ListByEntity]
			
           @ID int

AS

BEGIN
SELECT r.[EntityID]
	  ,r.[WebsiteId]
      ,v.[RecordID]
      ,v.[AttributeID]
	  ,v.[ValueString]
      ,v.[ValueInt]
	  ,v.[ValueDecimal]
	  ,v.[ValueText]
	  ,v.[ValueGeo]
	  

  FROM [dbo].EVA_Values AS v 
  inner JOIN [dbo].[EVA_Records] as r 
  on v.RecordID = r.ID 
  WHERE r.EntityID = @ID

END
