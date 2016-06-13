
using Microsoft.Practices.Unity;
using Sabio.Data;
using Sabio.Web.Classes.Processes.EAV;
using Sabio.Web.Domain;
using Sabio.Web.Domain.EVA;
using Sabio.Web.Domain.Index;
using Sabio.Web.Enums;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Requests.EVA;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services.EVA
{
    public class RecordServices : BaseService, IRecordServices
    {
        [Dependency]
        public IEntityServices _EntityServices { get; set; }

        [Dependency]
        public IAttributeServices _AttributeServices { get; set; }

        [Dependency]
        public IValueServices _ValueServices { get; set; }

        [Dependency]
        public IMediaService _MediaServices { get; set; }

        [Dependency]
        public IDealerIndexService _DealerIndexService { get; set; }

        // create
        public int Insert(RecordRequestModel model)
        {
            int recordID = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Record_Insert_two"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@EntityID", model.EntityId);
                   paramCollection.AddWithValue("@WebsiteId", model.WebsiteId);
                   paramCollection.AddWithValue("@AttributeId", model.AttributeId);
                   foreach (ValueRequestModel item in model.Values)
                   {
                       if (item.AttributeId == 136)
                       {
                           paramCollection.AddWithValue("@Value", item.ValueString);
                           break;
                       }
                   }

                   SqlParameter p = new SqlParameter("@OID", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@OID"].Value.ToString(), out recordID);
               }
               );

            if (recordID > 0)
            {
                //ParseValues _ParseValues = new ParseValues();
                //_ParseValues.ParseEAVValues(model, recordID);
                //ParseRecordMedia _ParseRecordMedia = new ParseRecordMedia();
                //_ParseRecordMedia.ParseMedia(model, recordID);

                foreach (ValueRequestModel item in model.Values)
                {
                    _ValueServices.Insert(item, recordID);
                   
                }

                if (model.Medias != null)
                {
                    var x = 0;
                    foreach (MediaRequestModel media in model.Medias)
                    {
                        int MediaID = _MediaServices.InsertMedia(media);
                        RecordMediaRequestModel RMR = new RecordMediaRequestModel();
                        if (x == 0)
                        {
                            RMR.IsCoverPhoto = true;
                        }
                        else
                        {
                            RMR.IsCoverPhoto = false;
                        }

                        RMR.RecordID = recordID;
                        RMR.MediaID = MediaID;
                        InsertRecordMedia(RMR);
                        x++;
                    }
                }

                DealerIndex indexModel = new DealerIndex();
                indexModel.name = "TEST DEALER";
                indexModel.dealer_id = 123456;
                indexModel.zip_code = 92805;

                _DealerIndexService.IndexDealer(indexModel);
            }

            return recordID;
        }

        // update
        public int Update(RecordRequestModel model, int ID)
        {
            int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Record_Update"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@RecordID", ID);
                   paramCollection.AddWithValue("@WebsiteId", model.WebsiteId);

               });

            foreach (ValueRequestModel item in model.Values)
            {
                _ValueServices.Update(item, ID);
            }

            return uid;
        }

        // delete Record by Id
        public void DeleteRecordById(int Id)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Record_Delete"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@ID", Id);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );

        }

        // new function that calls two services
        public List<EVA_Record> ListRecordByEntity(int id, PaginateListRequestModel model)//for other one only id as param
        {
            List<EVA_Record> myList = null;



            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_List_Records_By_Entity"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@EntityID", id);
                   paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                   paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);

               }, map: delegate (IDataReader reader, short set)
               {


                   if (set == 0)
                   {
                       EVA_Record p = new EVA_Record();
                       int startingIndex = 0; //startingOrdinal


                       p.ID = reader.GetSafeInt32(startingIndex++);
                       p.EntityID = reader.GetSafeInt32(startingIndex++);
                       p.WebsiteId = reader.GetSafeInt32(startingIndex++);
                       p.Value = new List<Value>();

                       if (myList == null)
                       {
                           myList = new List<EVA_Record>();
                       }

                       myList.Add(p);

                   }
                   else if (set == 1)
                   {
                       Value x = new Value();
                       int startingIndex = 0; //startingOrdinal

                       x.RecordID = reader.GetSafeInt32(startingIndex++);
                       x.AttributeID = reader.GetSafeInt32(startingIndex++);
                       x.ValueString = reader.GetSafeString(startingIndex++);
                       x.ValueInt = reader.GetSafeInt32(startingIndex++);
                       x.ValueDecimal = reader.GetSafeDecimal(startingIndex++);
                       x.ValueText = reader.GetSafeString(startingIndex++);
                       x.ValueGeo = reader.GetSafeDecimal(startingIndex++);
                       x.AttributeSlug = reader.GetSafeString(startingIndex++);
                       x.AttributeName = reader.GetSafeString(startingIndex++);

                       //insert loop here
                       for (int i = 0; i < myList.Count; i++)
                       {
                           if (myList[i].ID == x.RecordID)
                           {
                               myList[i].Value.Add(x);
                               break;
                           }
                       }

                       // within the loop  p.Value.Add(x);
                   }


               }
               );

            return myList;
        }

        // Records pagination count
        public int RecordsUserCount(int id)
        {
            int count = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Count_Records_By_Entity"
                   , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                   {
                       paramCollection.AddWithValue("@EntityID", id);
                   }, map: delegate (IDataReader reader, short set)
                   {
                       int startingIndex = 0; //startingOrdinal
                       count = reader.GetSafeInt32(startingIndex++);
                   });

            return count;
        }

        // Get Values by Record ID
        public EVA_Record ListValuesByRecordID(int ID)
        {
            EVA_Record p = null;


            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Values_Select_By_RecordID"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@ID", ID);

               }, map: delegate (IDataReader reader, short set)
               {


                   if (set == 0)
                   {
                       p = new EVA_Record();
                       int startingIndex = 0; //startingOrdinal


                       p.ID = reader.GetSafeInt32(startingIndex++);
                       p.EntityID = reader.GetSafeInt32(startingIndex++);
                       p.WebsiteId = reader.GetSafeInt32(startingIndex++);
                       p.Value = new List<Value>();


                   }
                   else if (set == 1)
                   {
                       Value x = new Value();
                       int startingIndex = 0; //startingOrdinal

                       x.RecordID = reader.GetSafeInt32(startingIndex++);
                       x.AttributeID = reader.GetSafeInt32(startingIndex++);
                       x.ValueString = reader.GetSafeString(startingIndex++);
                       x.ValueInt = reader.GetSafeInt32(startingIndex++);
                       x.ValueDecimal = reader.GetSafeDecimal(startingIndex++);
                       x.ValueText = reader.GetSafeString(startingIndex++);
                       x.ValueGeo = reader.GetSafeDecimal(startingIndex++);
                       x.AttributeSlug = reader.GetSafeString(startingIndex++);
                       x.AttributeName = reader.GetSafeString(startingIndex++);
                       p.Value.Add(x);

                   }


               }
               );

            return p;
        }
        // Insert record Media
        public int InsertRecordMedia(RecordMediaRequestModel model)
        {
            int recordMediaID = 0;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Record_Medias_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@RecordID", model.RecordID);
                   paramCollection.AddWithValue("@MediaID", model.MediaID);
                   paramCollection.AddWithValue("@IsCoverPhoto", model.IsCoverPhoto);

                   SqlParameter p = new SqlParameter("@OID", System.Data.SqlDbType.Int);
                   p.Direction = System.Data.ParameterDirection.Output;

                   paramCollection.Add(p);

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   int.TryParse(param["@OID"].Value.ToString(), out recordMediaID);
               }
               );

            return recordMediaID;
        }

        // Function to List all Records with a common Value
        public List<EVA_Record> RecordsSearchByValue(int EntityId, RecordByValueRequestModel model)
        {
            List<EVA_Record> myList = null;



            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Records_Search_By_Value"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@EntityID", EntityId);
                   paramCollection.AddWithValue("@AttributeID", model.AttributeId);
                   paramCollection.AddWithValue("@ValueString", model.ValueString);
                   paramCollection.AddWithValue("@CurrentPage", model.CurrentPage);
                   paramCollection.AddWithValue("@ItemsPerPage", model.ItemsPerPage);

               }, map: delegate (IDataReader reader, short set)
               {


                   if (set == 0)
                   {
                       EVA_Record p = new EVA_Record();
                       int startingIndex = 0; //startingOrdinal


                       p.ID = reader.GetSafeInt32(startingIndex++);
                       p.EntityID = reader.GetSafeInt32(startingIndex++);
                       p.WebsiteId = reader.GetSafeInt32(startingIndex++);
                       p.Value = new List<Value>();

                       if (myList == null)
                       {
                           myList = new List<EVA_Record>();
                       }

                       myList.Add(p);

                   }
                   else if (set == 1)
                   {
                       Value x = new Value();
                       int startingIndex = 0; //startingOrdinal

                       x.RecordID = reader.GetSafeInt32(startingIndex++);
                       x.AttributeID = reader.GetSafeInt32(startingIndex++);
                       x.ValueString = reader.GetSafeString(startingIndex++);
                       x.ValueInt = reader.GetSafeInt32(startingIndex++);
                       x.ValueDecimal = reader.GetSafeDecimal(startingIndex++);
                       x.ValueText = reader.GetSafeString(startingIndex++);
                       x.ValueGeo = reader.GetSafeDecimal(startingIndex++);

                       //insert loop here
                       for (int i = 0; i < myList.Count; i++)
                       {
                           if (myList[i].ID == x.RecordID)
                           {
                               myList[i].Value.Add(x);
                               break;
                           }
                       }

                       // within the loop  p.Value.Add(x);
                   }


               }
               );

            return myList;
        }


    }
}