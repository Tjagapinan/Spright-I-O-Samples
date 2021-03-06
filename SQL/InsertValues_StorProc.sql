
ALTER PROC [dbo].[EVA_Value_Insert]
			
			@RecordID int
           ,@AttributeID int
           ,@ValueString nvarchar(255) = null
		   ,@ValueInt int = null
		   ,@ValueDecimal decimal(9,6) = null
		   ,@ValueText nvarchar(255) = null
		   ,@ValueGeo decimal(9,6) = null

		   ,@OID int output

AS

BEGIN
INSERT INTO [dbo].[EVA_Values]
           
		   (
		    [RecordID]
           ,[AttributeID]
           ,[ValueString]
		   ,[ValueInt]
           ,[ValueDecimal]
           ,[ValueText]
           ,[ValueGeo]     
		   )
     VALUES
           (
		    @RecordID
           ,@AttributeID
           ,@ValueString
		   ,@ValueInt
		   ,@ValueDecimal
		   ,@ValueText
		   ,@ValueGeo
		   )

SET @OID = SCOPE_IDENTITY()
END