using Sabio.Web.Classes.EAV;
using Sabio.Web.Domain;
using Sabio.Web.Domain.EVA;
using Sabio.Web.Models.Requests;
using Sabio.Web.Models.Requests.EVA;
using Sabio.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sabio.Web.Services.EVA
{
    public class OutputService : BaseService    //, IOutputService
    {
        public static DynamicDomainModel RequestToModel(RecordRequestModel inputRecord)
        {
            dynamic record = new DynamicDomainModel();

            foreach (ValueRequestModel currentValue in inputRecord.Values)
            {
                string slug = currentValue.AttributeSlug.ToString().Replace("-", "_");

                switch(slug)
                {
                    case "options":
                        record.SetStringArrayMember(slug, currentValue.ValueString.Split('|'));                        
                        break;

                    default:
                        record.SetStringMember(slug, currentValue.ValueString);
                        break;
                }  
            }

            List<string> images = new List<string>();

            foreach (MediaRequestModel media in inputRecord.Medias)
            {
                images.Add(media.FileName);
            }
            record.SetStringArrayMember("images", images.ToArray());

            return record;
        }

        // Function to List all Records with a common Value
        public static DynamicDomainModel DomainToModel(EVA_Record inputRecord)
        {
            // Creating a dynamic dictionary.
            dynamic record = new DynamicDomainModel();

            foreach (Value currentValue in inputRecord.Value)
            {
                record.SetStringMember(currentValue.AttributeSlug.ToString().Replace("-", "_"),  currentValue.ValueString);
            }

            record.Id = inputRecord.ID;

            return record;
        }
    }
}

