using Sabio.Web.Models.Requests;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Sabio.Data;
using Sabio.Web.Domain.EVA;
using Sabio.Web.Models.Requests.EVA;
using System.Data;

namespace Sabio.Web.Services.EVA
{
    public class DealerRecordService:BaseService, IDealerRecordService
    {
        public int InsertRecord(DealerRecordAddRequest model)
        {
            int uid = 0;

            DataProvider.ExecuteNonQuery(GetConnection, "DealerRecord_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@DealerID", model.DealerID);
                   paramCollection.AddWithValue("@RecordID", model.RecordID);
                  
               }, returnParameters: delegate (SqlParameterCollection param)
               {
               }
               );

            return uid;
        }
        // list dealer records
        public List<EVA_Record> ListVehicleRecordByDealerID(int DealerID, PaginateListRequestModel model)//for other one only id as param
        {
            List<EVA_Record> myList = null;

            
            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Records_Select_By_DealerID"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@DealerID", DealerID);
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
                       p.Pictures = new List<Domain.Media>(); //this line only give a empty [];
                       

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
                  else if (set == 2)
                   {
                       Domain.Media w = new Domain.Media();
                       int startingIndex = 0; //startingOrdinal
                       w.ID= reader.GetSafeInt32(startingIndex++);
                       w.MediaType= reader.GetSafeString(startingIndex++);
                       w.Path= reader.GetSafeString(startingIndex++);
                       w.FileName = reader.GetSafeString(startingIndex++);
                       w.FileType= reader.GetSafeString(startingIndex++);
                       w.Title= reader.GetSafeString(startingIndex++);
                       w.Description= reader.GetSafeString(startingIndex++);
                       w.UserID= reader.GetSafeString(startingIndex++);
                       w.ThumbnailPath= reader.GetSafeString(startingIndex++);
                       w.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                       w.ModifiedDate= reader.GetSafeDateTime(startingIndex++);
                       w.MediaId = reader.GetSafeInt32(startingIndex++);
                       w.RecordId = reader.GetSafeInt32(startingIndex++);
                       w.DealerId = reader.GetSafeInt32(startingIndex++);
                       w.IsCoverPhoto = reader.GetSafeBool(startingIndex++);
                       //insert loop here
                       for (int y = 0; y < myList.Count; y++)
                       {
                           if (myList[y].ID == w.RecordId)
                           {
                               myList[y].Pictures.Add(w);
                               break;
                           }
                       }




                   }



               }
               );

            return myList;
        }

        // GET dealer VEHICLES  COUNT VEHICLES BY dealerid  //pagination COUNT 

        public int GetVehicleRecord_Count(int DealerID)
        {

            int count = 0;
            DataProvider.ExecuteCmd(GetConnection, "dbo.EVA_Records_Select_By_DealerID_Count"
                   , inputParamMapper: delegate (SqlParameterCollection paramCollection)
                   {
                       paramCollection.AddWithValue("@DealerID", DealerID);


                   }, map: delegate (IDataReader reader, short set)
                   {
                       int startingIndex = 0; //startingOrdinal
                       count = reader.GetSafeInt32(startingIndex++);
                   });

            return count;
        }

        // select media by recordID 
        //**=== Get Media By VehicleId for Dealer Vehicle Media(image) ==**// 
        public List<Domain.EVA.DealerRecordMedia> GetMediasByRecordId(int recordID)
        {
            List<Domain.EVA.DealerRecordMedia> list = null;

            DataProvider.ExecuteCmd(GetConnection, "dbo.Medias_SelectByRecordID"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@RecordID", recordID);

               }, map: delegate (IDataReader reader, short set)
               {
                   Domain.EVA.DealerRecordMedia p = new Domain.EVA.DealerRecordMedia();
                   int startingIndex = 0; //startingOrdinal

                   p.ID = reader.GetSafeInt32(startingIndex++);
                   p.MediaType = reader.GetSafeString(startingIndex++);
                   p.Path = reader.GetSafeString(startingIndex++);
                   p.FileName = reader.GetSafeString(startingIndex++);
                   p.FileType = reader.GetSafeString(startingIndex++);
                   p.Title = reader.GetSafeString(startingIndex++);
                   p.Description = reader.GetSafeString(startingIndex++);
                   p.UserID = reader.GetSafeString(startingIndex++);
                   p.ThumbnailPath = reader.GetSafeString(startingIndex++);
                   p.CreatedDate = reader.GetSafeDateTime(startingIndex++);
                   p.ModifiedDate = reader.GetSafeDateTime(startingIndex++);

                   // Added two files into Domain model: 
                   p.MediaID = reader.GetSafeInt32(startingIndex++);
                   p.RecordID= reader.GetSafeInt32(startingIndex++);
                   p.IsCoverPhoto = reader.GetSafeBool(startingIndex++);

                   // DOMAIN MODLE IS LIKE A PLACEHODER FOR THE VALUE GET OUT FORM DB 
                   if (list == null)
                   {
                       list = new List<Domain.EVA.DealerRecordMedia>();
                   }

                   list.Add(p);
               }
               );

            return list;
        }

        // Insert dealer record Media
        public int InsertDealerRecordMedia(DealerRecordMediaRequest model)
        {
            int uid = 0;
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Dealer_Record_Medias_Insert"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@DealerId", model.DealerID);
                   paramCollection.AddWithValue("@RecordId", model.RecordID);
                   paramCollection.AddWithValue("@MediaId", model.MediaID);
                   paramCollection.AddWithValue("@IsCoverPhoto", model.IsCoverPhoto);
                 

               }, returnParameters: delegate (SqlParameterCollection param)
               {
                   
               }
           );

            return uid;
        }

        //**=== Get Media By RecordID for Set as main photo ==**// 

        public void setMainPhoto(RecordMediaSetMainPhoto model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.RecordMedia_SetMainPhoto"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@RecordID", model.RecordID);
                   paramCollection.AddWithValue("@MediaID", model.MediaID);

               }
               );
        }

        //Delete a Record by dealerID and recordID
        public void DeleteByID(int DealerID, int RecordID)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.DealerRecord1_Delete"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@DealerID", DealerID);
                   paramCollection.AddWithValue("@RecordID", RecordID);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );
        }

        //Delete a media by  recordID and mediaid
        public void DeleteMediaByID(DealerRecordMediaRequest model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Dealer_Record_Medias_Delete"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@RecordID", model.RecordID);
                   paramCollection.AddWithValue("@MediaID", model.MediaID);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );
        }

        //Delete a Video by  recordID and mediaid
        public void DeleteVideoByID(DealerRecordMediaRequest model)
        {
            DataProvider.ExecuteNonQuery(GetConnection, "dbo.EVA_Dealer_Record_Medias_Video_Delete"
               , inputParamMapper: delegate (SqlParameterCollection paramCollection)
               {
                   paramCollection.AddWithValue("@RecordID", model.RecordID);
                   paramCollection.AddWithValue("@MediaID", model.MediaID);

               }, returnParameters: delegate (SqlParameterCollection param)
               {

               }
               );
        }



    } //end of class 
}