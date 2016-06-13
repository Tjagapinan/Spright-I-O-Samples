using Sabio.Data;
using Sabio.Web.Enums;
using Sabio.Web.Models;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Requests.EVA;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services
{
    public class AttributeServices : BaseService, IAttributeServices
    {

        // create
        public int Insert(EVA_AttributeRequestModel model)
        {
            int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Attribute_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

                   paramCollection.AddWithValue("@Name", model.Name);
                   paramCollection.AddWithValue("@Slug", model.Slug);
                   paramCollection.AddWithValue("@Description", model.Description);
                   paramCollection.AddWithValue("@DataType", model.DataType);

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
        public void Update(EVA_AttributeRequestModel model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Attribute_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@ID", model.ID);
                   paramCollection.AddWithValue("@Name", model.Name);
                   paramCollection.AddWithValue("@Slug", model.Slug);
                   paramCollection.AddWithValue("@Description", model.Description);
                   paramCollection.AddWithValue("@DataType", model.DataType);


               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );

        }


        // delete
        public void Delete(int id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Attribute_Delete"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@ID", id);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );

        }


        // Pagiantion list  
        public List<Domain.EVA_Attribute> GetPaginationList(PaginateListRequestModel model)
        {
            List<Domain.EVA_Attribute> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Attribute_Select"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                   paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);
                   paramCollection.AddWithValue("@Query", model.Query);

               }, map: delegate (IDataReader reader, short set)
               {
                   Domain.EVA_Attribute p = new Domain.EVA_Attribute();
                   int startingIndex = 0; //startingOrdinal


                   p.ID = reader.GetSafeInt32(startingIndex++);
                   p.Name = reader.GetSafeString(startingIndex++);
                   p.Slug = reader.GetSafeString(startingIndex++);
                   p.Description = reader.GetSafeString(startingIndex++);
                   int TypeID = reader.GetSafeInt32(startingIndex++);

                   p.DataType = (AttributesDataType)TypeID;


                   if (list == null)
                   {
                       list = new List<Domain.EVA_Attribute>();
                   }

                   list.Add(p);
               }
               );

            return list;
        }

        //Get the count from Query for the search pagination function on the Attributes page.
        public int AttributeQueryCount(string Query)
        {

            int result = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Attribute_Query_Count"
                   , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                   {
                       paramCollection.AddWithValue("@Query", Query);

                   }, map: delegate (IDataReader reader, short set)
                   {
                       int startingIndex = 0; //startingOrdinal

                       result = reader.GetSafeInt32(startingIndex++);

                   });

            return result;
        }



        public int AttributeCount()
        {

            int count = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Attribute_Count"
                   , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                   {


                   }, map: delegate (IDataReader reader, short set)
                   {
                       int startingIndex = 0; //startingOrdinal

                       count = reader.GetSafeInt32(startingIndex++);

                   });

            return count;
        }



        // list by  ID
        public Domain.EVA_Attribute GetByID(int id)
        {
            Domain.EVA_Attribute item = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Attribute_SelectByID"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@ID", id);

               }, map: delegate (IDataReader reader, short set)
               {
                   Domain.EVA_Attribute p = new Domain.EVA_Attribute();
                   int startingIndex = 0; //startingOrdinal

                   p.ID = reader.GetSafeInt32(startingIndex++);
                   p.Name = reader.GetSafeString(startingIndex++);
                   p.Slug = reader.GetSafeString(startingIndex++);
                   p.Description = reader.GetSafeString(startingIndex++);
                   //p.DataType = reader.GetSafeInt32(startingIndex++);
                   int TypeID = reader.GetSafeInt32(startingIndex++);

                   p.DataType = (AttributesDataType)TypeID;
                   //p.DataType = AttributesDataType.
                   //p.UserID = reader.GetSafeInt32(startingIndex++);

                   item = p;
               }
               );

            return item;
        }

        // list by  Slug
        public Domain.EVA_Attribute GetBySlug(string slug)
        {
            Domain.EVA_Attribute item = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Attribute_SelectBySlug"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Slug", slug);

               }, map: delegate (IDataReader reader, short set)
               {
                   Domain.EVA_Attribute p = new Domain.EVA_Attribute();
                   int startingIndex = 0; //startingOrdinal

                   p.ID = reader.GetSafeInt32(startingIndex++);
                   p.Name = reader.GetSafeString(startingIndex++);
                   p.Slug = reader.GetSafeString(startingIndex++);
                   p.Description = reader.GetSafeString(startingIndex++);
                   //p.DataType = reader.GetSafeInt32(startingIndex++);
                   int TypeID = reader.GetSafeInt32(startingIndex++);

                   p.DataType = (AttributesDataType)TypeID;
                   //p.DataType = AttributesDataType.
                   //p.UserID = reader.GetSafeInt32(startingIndex++);

                   item = p;
               }
               );

            return item;
        }

        // list by  Name
        public Domain.EVA_Attribute GetByName(string name)
        {
            Domain.EVA_Attribute item = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Attribute_SelectByName"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Name", name);

               }, map: delegate (IDataReader reader, short set)
               {
                   Domain.EVA_Attribute p = new Domain.EVA_Attribute();
                   int startingIndex = 0; //startingOrdinal

                   p.ID = reader.GetSafeInt32(startingIndex++);
                   p.Name = reader.GetSafeString(startingIndex++);
                   p.Slug = reader.GetSafeString(startingIndex++);
                   p.Description = reader.GetSafeString(startingIndex++);
                   //p.DataType = reader.GetSafeInt32(startingIndex++);
                   int TypeID = reader.GetSafeInt32(startingIndex++);

                   p.DataType = (AttributesDataType)TypeID;
                   //p.DataType = AttributesDataType.
                   //p.UserID = reader.GetSafeInt32(startingIndex++);

                   item = p;
               }
               );

            return item;
        }

        // list all
        public List<Domain.EVA_Attribute> GetAll()
        {
            List<Domain.EVA_Attribute> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Attribute_SelectAll"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {

               }, map: delegate (IDataReader reader, short set)
               {
                   Domain.EVA_Attribute p = new Domain.EVA_Attribute();
                   int startingIndex = 0; //startingOrdinal

                   p.ID = reader.GetSafeInt32(startingIndex++);
                   p.Name = reader.GetSafeString(startingIndex++);
                   p.Slug = reader.GetSafeString(startingIndex++);
                   p.Description = reader.GetSafeString(startingIndex++);
                   int TypeID = reader.GetSafeInt32(startingIndex++);

                   p.DataType = (AttributesDataType)TypeID;

                   if (list == null)
                   {
                       list = new List<Domain.EVA_Attribute>();
                   }

                   list.Add(p);
               }
               );

            return list;
        }

        // list relationship by entity ID
        public List<Domain.EVA.EntityAttributeRelationship> GetRelationshipByEntityID(int id)
        {
            List<Domain.EVA.EntityAttributeRelationship> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_EntityAttribute_SelectByID"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@EntityID", id);

               }, map: delegate (IDataReader reader, short set)
               {
                   Domain.EVA.EntityAttributeRelationship p = new Domain.EVA.EntityAttributeRelationship();
                   int startingIndex = 0; //startingOrdinal

                   //p.EntityID = reader.GetSafeInt32(startingIndex++);

                   p.AttributeID = reader.GetSafeInt32(startingIndex++);

                   if (list == null)
                   {
                       list = new List<Domain.EVA.EntityAttributeRelationship>();
                   }

                   list.Add(p);
               }
               );

            return list;
        }

        public int GetAutoAttributes(string Name, string Slug)
        {
            int ID = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Attribute_Insert_Select"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@Name", Name);
                   paramCollection.AddWithValue("@Slug", Slug);

                   SqlParameter p = new SqlParameter("@ID", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }
               , returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@ID"].Value.ToString(), out ID);

               });


            return ID;
        }

        // UserWebsite insert method
        public void BulkInsertAttrIds(int entityId, int[] AttributeId)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.Attribute_Bulk_Insert"
          , inputParamMapper: delegate (SqlParameterCollection paramCollection)
          {
              paramCollection.AddWithValue("@EntityID", entityId);
              SqlParameter c = new SqlParameter("@AttributeID", SqlDbType.Structured);
              if (AttributeId != null && AttributeId.Any())
              {
                  c.Value = new IntIdTable(AttributeId);
              }

              paramCollection.Add(c);
          });
        }
    }
}