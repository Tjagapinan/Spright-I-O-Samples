using Spright.Data;
using Spright.Web.Domain.EVA;
using Spright.Web.Models.Requests.EVA;
using Spright.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Spright.Web.Services.EVA
{
    public class ValueServices : BaseService, IValueServices
    {
         // create
        public int Insert(ValueRequestModel model, int recordID)
        {
            int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "EVA_Value_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@RecordID", recordID);
                   paramCollection.AddWithValue("@AttributeID", model.AttributeId);
                   paramCollection.AddWithValue("@ValueString", model.ValueString);
                   paramCollection.AddWithValue("@ValueInt", model.ValueInt);
                   paramCollection.AddWithValue("@ValueDecimal", model.ValueDecimal);
                   paramCollection.AddWithValue("@ValueText", model.ValueText);
                   paramCollection.AddWithValue("@ValueGeo", model.ValueGeo);

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
        public void Update(ValueRequestModel model, int recordID)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Value_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@RecordID", recordID);
                   paramCollection.AddWithValue("@AttributeID", model.AttributeId);
                   paramCollection.AddWithValue("@ValueString", model.ValueString);
                   paramCollection.AddWithValue("@ValueInt", model.ValueInt);
                   paramCollection.AddWithValue("@ValueDecimal", model.ValueDecimal);
                   paramCollection.AddWithValue("@ValueText", model.ValueText);
                   paramCollection.AddWithValue("@ValueGeo", model.ValueGeo);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );

        }


        // list by ID
        public List<Value> GetByID(int id)
        {

            List<Value> item = new List<Value>();
            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Value_SelectByID"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@ID", id);

               }, map: delegate (IDataReader reader, short set)
               {
                   Value p = new Value();
                   int startingIndex = 0; //startingOrdinal

                   p.RecordID = reader.GetSafeInt32(startingIndex++);
                   p.AttributeID = reader.GetSafeInt32(startingIndex++);
                   p.ValueString = reader.GetSafeString(startingIndex++);
                   p.ValueInt = reader.GetSafeInt32(startingIndex++);
                   p.ValueDecimal = reader.GetSafeDecimal(startingIndex++);
                   p.ValueText = reader.GetSafeString(startingIndex++);
                   p.ValueGeo = reader.GetSafeDecimal(startingIndex++);
                   //p.EntityID = reader.GetSafeInt32(startingIndex++);
                   //p.WebsiteID = reader.GetSafeInt32(startingIndex++);

                   item.Add(p);
               }
               );

            return item;
        }

        // list records&values by entity ID
        public List<Value> GetRecValbyEntityId (int id)
        {

            List<Value> item = new List<Value>();
            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Record_ListByEntity"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@ID", id);

               }, map: delegate (IDataReader reader, short set)
               {
                   Value p = new Value();
                   int startingIndex = 0; //startingOrdinal

                   p.EntityID = reader.GetSafeInt32(startingIndex++);
                   p.WebsiteID = reader.GetSafeInt32(startingIndex++);
                   p.RecordID = reader.GetSafeInt32(startingIndex++);
                   p.AttributeID = reader.GetSafeInt32(startingIndex++);
                   p.ValueString = reader.GetSafeString(startingIndex++);
                   p.ValueInt = reader.GetSafeInt32(startingIndex++);
                   p.ValueDecimal = reader.GetSafeDecimal(startingIndex++);
                   p.ValueText = reader.GetSafeString(startingIndex++);
                   p.ValueGeo = reader.GetSafeDecimal(startingIndex++);

                   item.Add(p);
               }
               );

            return item;
        }


    }
}
