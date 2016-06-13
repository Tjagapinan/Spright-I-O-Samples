using Microsoft.Practices.Unity;
using Sabio.Data;
using Sabio.Web.Domain;
using Sabio.Web.Enums;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Requests.EVA;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Sabio.Web.Services
{
    public class EntityServices : BaseService, IEntityServices
    {
        [Dependency]
        public IAttributeServices AttrService { get; set; }

        // create
        public int Insert(EVA_EntityRequestModel model)
        {
            int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Entity_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@WebsiteId", model.WebsiteId);
                   paramCollection.AddWithValue("@Name", model.Name);
                   paramCollection.AddWithValue("@Slug", model.Slug);

                   SqlParameter p = new SqlParameter("@OID", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@OID"].Value.ToString(), out uid);
               }
               );
            return uid;
        }

        // update
        public void Update(EVA_EntityRequestModel model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Entity_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@ID", model.ID);
                   paramCollection.AddWithValue("@Name", model.Name);
                   paramCollection.AddWithValue("@Slug", model.Slug);
                   paramCollection.AddWithValue("@WebsiteId", model.WebsiteId);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );

        }

        // delete
        public void Delete(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Entity_Delete"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@ID", id);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );

        }

        // list by Website ID
        public List<Domain.EVA_Entity> GetByUserID(int id)
        {
            List<Domain.EVA_Entity> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Entity_SelectByUserID"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@WebsiteId", id);

               }, map: delegate (IDataReader reader, short set)
               {
                   Domain.EVA_Entity p = new Domain.EVA_Entity();
                   int startingIndex = 0; //startingOrdinal

                   p.ID = reader.GetSafeInt32(startingIndex++);
                   p.Name = reader.GetSafeString(startingIndex++);
                   p.Slug = reader.GetSafeString(startingIndex++);
                   p.WebsiteId = reader.GetSafeInt32(startingIndex++);

                   if (list == null)
                   {
                       list = new List<Domain.EVA_Entity>();
                   }

                   list.Add(p);
               }
               );

            return list;
        }

        // list by User Name
        public EVA_Entity GetBySlug(string slug)
        {
            EVA_Entity item = new EVA_Entity();

            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Entity_SelectBySlug"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Slug", slug);

               }, map: delegate (IDataReader reader, short set)
               {
                   int startingIndex = 0; //startingOrdinal

                   item.ID = reader.GetSafeInt32(startingIndex++);
                   item.Name = reader.GetSafeString(startingIndex++);
                   item.Slug = reader.GetSafeString(startingIndex++);
                   item.WebsiteId = reader.GetSafeInt32(startingIndex++);

               }
               );

            return item;
        }

        // list by ID
        public EVA_Entity GetByID(int id)
        {
            EVA_Entity p = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Entity_SelectByID"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@ID", id);

               }, map: delegate (IDataReader reader, short set)
               {


                   if (set == 0)
                   {
                       p = new EVA_Entity();

                       int startingIndex = 0; //startingOrdinal

                       p.ID = reader.GetSafeInt32(startingIndex++);
                       p.Name = reader.GetSafeString(startingIndex++);
                       p.Slug = reader.GetSafeString(startingIndex++);
                       p.WebsiteId = reader.GetSafeInt32(startingIndex++);
                       p.Attributes = new List<EVA_Attribute>();
                   }
                   else if (set == 1)
                   {
                       EVA_Attribute x = new EVA_Attribute();
                       int startingIndex = 0; //startingOrdinal

                       x.ID = reader.GetSafeInt32(startingIndex++);
                       x.Name = reader.GetSafeString(startingIndex++);
                       x.Slug = reader.GetSafeString(startingIndex++);
                       x.Description = reader.GetSafeString(startingIndex++);
                       int TypeID = reader.GetSafeInt32(startingIndex++);
                       x.DataType = (AttributesDataType)TypeID;
                       x.WebsiteId = reader.GetSafeInt32(startingIndex++);
                       x.IsRequired = reader.GetSafeBool(startingIndex++);
                       x.ShowOnIndex = reader.GetSafeBool(startingIndex++);

                       p.Attributes.Add(x);
                   }


               }
               );

            return p;
        }

        // insert entity and attribute relationship
        public int EntityAttributeInsert(EntityAttributeRelationshipRequestModel model)
        {
            int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_EntityAttributes_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                  
                   paramCollection.AddWithValue("@AttributeID", model.attributeID);
                   paramCollection.AddWithValue("@IsRequired", model.isRequired);
                   paramCollection.AddWithValue("@ShowOnIndex", model.showOnIndex);
                   paramCollection.AddWithValue("@EntityID", model.entityID);

                   //SqlParameter p = new SqlParameter("@OID", System.Data.SqlDbType.Int);
                   //p.Direction = System.Data.ParameterDirection.Output;

                   //paramCollection.Add(p);

               }
               );
            return uid;
        }

        public void EntityAttributeUpdate(EntityAttributeRelationshipRequestModel model)
        {

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_EntityAttributes_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

                   paramCollection.AddWithValue("@AttributeID", model.attributeID);
                   paramCollection.AddWithValue("@IsRequired", model.isRequired);
                   paramCollection.AddWithValue("@ShowOnIndex", model.showOnIndex);
                   paramCollection.AddWithValue("@EntityID", model.entityID);
               }
               );
        }

        // delete relationship
        public void DeleteRelationship(int EntityID, int AttributeID)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_EntityAttribute_Delete"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@EntityID", EntityID);
                   paramCollection.AddWithValue("@AttributeID", AttributeID);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );

        }

        //Get Attributes based on Entity called 
        public int AutoCreateEntity(EntityAutoRequestModel model)
        {
            int entityID = 0;

            List<int> attrIds = new List<int>();

            EVA_EntityRequestModel entity = new EVA_EntityRequestModel();
            entity.WebsiteId = model.WebsiteId;
            entity.Name = model.EntityName;
            entity.Slug = UtilityService.camelCaseToDash(model.EntityName);
            entityID = Insert(entity);

            foreach (string Attribute in model.Attributes)
            {
                string Name = Attribute;
                string Slug = UtilityService.camelCaseToDash(Name);

                int AttrId = AttrService.GetAutoAttributes(Name, Slug);

                attrIds.Add(AttrId);
            }

            AttrService.BulkInsertAttrIds(entityID, attrIds.ToArray());


            return entityID;
        }




    }
}
