
ALTER PROC [dbo].[EVA_Entity_Delete]
			
			@ID int
AS

BEGIN

DELETE FROM [dbo].[EVA_Entities]
		WHERE [ID] = @ID

END