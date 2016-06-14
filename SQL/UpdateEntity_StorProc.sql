
ALTER proc [dbo].[EVA_Entity_Update]
		@ID int,
		@Name nvarchar(255),
		@Slug nvarchar(255),
		@WebsiteId int

as

BEGIN
UPDATE [dbo].[EVA_Entities]
   SET [Name] = @Name
      ,[Slug] = @Slug
	 
      ,[ModifiedDate] = getutcdate()
	 
	  ,[WebsiteId] = @WebsiteId
 WHERE ID = @ID


END
		

